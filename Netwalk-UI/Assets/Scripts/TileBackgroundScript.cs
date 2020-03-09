using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NetwalkLib;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TileBackgroundScript : MonoBehaviour
{
    private float _backgroundMargin = 0.02f;

    private Camera _mainCamera;
    private GameObject _txtYouWon;
    private GameObject _txtBoardSize;
    private float _backgroundX;
    private float _backgroundY;
    private float _backgroundHeight;
    private float _backgroundWidth;
    private float _cellSpriteHeight;
    private float _cellSpriteWidth;
    private int _nextGameBoardSize;

    private GameConfig _gameConfig;
    private IBoardGenerator _boardGenerator;
    private IGame _game;
    private (GameObject GameObject, bool Active)[,] _cellObjects;

    private Dictionary<string, (GameObject GameObject, SpriteRenderer SpriteRenderer)> _cellPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        LoadSprites();
        ResizeBackgroundSpriteToScreen();
        GenerateLongLivedObjects();
        NewGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void RotateCell((int Row, int Col) data)
    {
        _game.RotateCell(data.Row, data.Col);
        bool[,] newActive = _game.PlayingBoard.GetActive();
        for (var row = 0; row < _gameConfig.Height; row++)
        {
            for (var col = 0; col < _gameConfig.Width; col++)
            {
                if (
                    (row == data.Row && col == data.Col) ||
                    (newActive[row, col] != _cellObjects[row, col].Active))
                {
                    SetCellObject(row, col, newActive[row, col]);
                }
            }
        }
    }

    public void OnNewGameClick()
    {
        NewGame();
    }

    public void OnSliderChange(float newValue)
    {
        SetNextBoardSize(3 + 2 * Convert.ToInt32(newValue + 0.1f));
    }

    private void SetNextBoardSize(int newSize)
    {
        _nextGameBoardSize = newSize;
        if (_txtBoardSize == null)
        {
            _txtBoardSize = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(o => o.name == "txtBoardSize");
        }

        if (_txtBoardSize != null)
        {
            _txtBoardSize.GetComponent<Text>().text = $"{newSize}x{newSize}";
        }
    }
    
    private void NewGame()
    {
        DestroyOldGame();
        ResetGameSize();
        CreateGame();
        LayoutBoardPieces();
    }

    private void DestroyOldGame()
    {
        for (var row = 0; row < _gameConfig.Height; row++)
        {
            for (var col = 0; col < _gameConfig.Width; col++)
            {
                if (_cellObjects[row, col].GameObject != null)
                {
                    Destroy(_cellObjects[row, col].GameObject);
                    _cellObjects[row, col].GameObject = null;
                }
            }
        }
    }

    private void LoadSprites()
    {
        _cellPrefabs = Resources.LoadAll<GameObject>("CellPrefabs").ToDictionary(o => o.name, o => (o, o.GetComponent<SpriteRenderer>()));
    }

    private void GenerateLongLivedObjects()
    {
        if (_nextGameBoardSize == 0)
        {
            _nextGameBoardSize = 5;
        }
        _boardGenerator = new BoardGenerator();
        ResetGameSize();
    }

    private void ResetGameSize()
    {
        _gameConfig = new GameConfig
        {
            Height = _nextGameBoardSize,
            Width = _nextGameBoardSize
        };
        _cellSpriteHeight = _backgroundHeight / _gameConfig.Height;
        _cellSpriteWidth = _backgroundWidth / _gameConfig.Width;
        _cellObjects = new (GameObject, bool)[_gameConfig.Height, _gameConfig.Width];
    }
    
    private void LayoutBoardPieces()
    {
        var active = _game.PlayingBoard.GetActive();

        for (var row = 0; row < _gameConfig.Height; row++)
        {
            for (var col = 0; col < _gameConfig.Width; col++)
            {
                SetCellObject(row, col, active[row, col]);
            }
        }
    }
    
    private void SetCellObject(int row, int col, bool active)
    {
        if (_cellObjects[row, col].GameObject != null)
        {
            Destroy(_cellObjects[row, col].GameObject);
            _cellObjects[row, col].GameObject = null;
        }

        var newCellValue = _game.PlayingBoard.Cells[row, col].ToString();
        var spriteName = newCellValue;
        if (row == _gameConfig.Height / 2 && col == _gameConfig.Width / 2)
        {
            spriteName += "c";
        }
        else if (active)
        {
            spriteName += "a";
        }
        var newCellPrefab = _cellPrefabs[spriteName];
        var newCellGo = Instantiate(newCellPrefab.GameObject);
        var newCellPosition = GetCellPosition(row, col);
        newCellGo.transform.localPosition = newCellPosition;
        var oldSizeSpriteRenderer = newCellPrefab.SpriteRenderer.bounds.size;
        var oldSizeSprite = newCellPrefab.SpriteRenderer.sprite.bounds.size;
        BoxCollider2D boxCollider = newCellGo.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.size = oldSizeSprite;
        }
        newCellGo.transform.localScale = new Vector3(_cellSpriteWidth / oldSizeSpriteRenderer.x, _cellSpriteHeight / oldSizeSpriteRenderer.y, 1f);
        newCellGo.SendMessage("SetPosition", (row, col));
        newCellGo.SendMessage("SetBackground", gameObject);
        _cellObjects[row, col] = (newCellGo, active);
    }

    private Vector3 GetCellPosition(int row, int col)
    {
        var x = _backgroundX - (_backgroundWidth - _cellSpriteWidth) / 2f + (col * _cellSpriteWidth);
        var y = _backgroundY + (_backgroundHeight - _cellSpriteHeight) / 2f - (row * _cellSpriteHeight);
        return new Vector3(x, y, 0);
    }

    private void CreateGame()
    {
        if (_game != null)
        {
            _game.GameWonEvent -= GameWon;
        }
        _game = new Game(_boardGenerator, _gameConfig);
        _game.GameWonEvent += GameWon;
        SetWonActive(false);
        Debug.Log(_game.SolvedBoard.ToString());
    }

    private void SetWonActive(bool active)
    {
        if (_txtYouWon == null)
        {
            _txtYouWon = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(o => o.name == "txtYouWon");
        }

        if (_txtYouWon != null)
        {
            _txtYouWon.SetActive(active);
        }
    }

    private void GameWon(object sender, GameWonEventArgs eventArgs)
    {
        SetWonActive(true);
    }

    private void ResizeBackgroundSpriteToScreen()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;
        var tf = transform;
        if (tf == null) return;

        var sprite = sr.sprite;
        var oldSpriteSize = sprite.bounds.size.y;

        var worldScreenHeight = _mainCamera.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        var newSpriteScale = (float) (worldScreenWidth / oldSpriteSize * (1f - _backgroundMargin));
        _backgroundHeight = oldSpriteSize * newSpriteScale;
        _backgroundWidth = _backgroundHeight;
        _backgroundY = (float) ((worldScreenHeight - _backgroundMargin * _backgroundWidth - _backgroundWidth) / 2f);
        _backgroundX = 0f;

        //tf.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        tf.localScale = new Vector3(newSpriteScale, newSpriteScale, 1f);
        tf.Translate(tf.up * _backgroundY);
    }
}
