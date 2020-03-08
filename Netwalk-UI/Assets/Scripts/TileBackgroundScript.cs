using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using NetwalkLib;

public class TileBackgroundScript : MonoBehaviour
{
    private float _backgroundMargin = 0.02f;

    private Camera _mainCamera;
    private float _backgroundX;
    private float _backgroundY;
    private float _backgroundHeight;
    private float _backgroundWidth;
    private float _cellSpriteHeight;
    private float _cellSpriteWidth;

    private GameConfig _gameConfig;
    private IGame _game;

    private Dictionary<string, (GameObject GameObject, SpriteRenderer SpriteRenderer)> _cellPrefabs;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        LoadSprites();
        ResizeBackgroundSpriteToScreen();
        CreateGame();
        LayoutBoardPieces();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LoadSprites()
    {
        _cellPrefabs = Resources.LoadAll<GameObject>("CellPrefabs").ToDictionary(o => o.name, o => (o, o.GetComponent<SpriteRenderer>()));
    }
    
    private void LayoutBoardPieces()
    {
        var go = gameObject;
        var sc = go.scene;

        for (var row = 0; row < _gameConfig.Height; row++)
        {
            for (var col = 0; col < _gameConfig.Width; col++)
            {
                var newCellValue = _game.PlayingBoard.Cells[row, col].ToString();
                var newCellPrefab = _cellPrefabs[newCellValue];
                var newCellGo = Instantiate(newCellPrefab.GameObject);
                var newCellPosition = GetCellPosition(row, col);
                newCellGo.transform.localPosition = newCellPosition;
                var oldSize = newCellPrefab.SpriteRenderer.bounds.size;
                newCellGo.transform.localScale = new Vector3(_cellSpriteWidth / oldSize.x, _cellSpriteHeight / oldSize.y, 1f);
            }
        }
    }

    private Vector3 GetCellPosition(int row, int col)
    {
        var x = _backgroundX - (_backgroundWidth - _cellSpriteWidth) / 2f + (col * _cellSpriteWidth);
        var y = _backgroundY + (_backgroundHeight - _cellSpriteHeight) / 2f - (row * _cellSpriteHeight);
        return new Vector3(x, y, 0);
    }

    private void CreateGame()
    {
        _gameConfig = new GameConfig
        {
            Height = 5,
            Width = 5
        };
        var boardGenerator = new BoardGenerator();
        _game = new Game(boardGenerator, _gameConfig);
        Debug.Log(_game.SolvedBoard.ToString());
        _cellSpriteHeight = _backgroundHeight / _gameConfig.Height;
        _cellSpriteWidth = _backgroundWidth / _gameConfig.Width;
    }

    private void ResizeBackgroundSpriteToScreen() {
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
        _backgroundY = (float)((worldScreenHeight - _backgroundMargin * _backgroundWidth - _backgroundWidth) / 2f);
        _backgroundX = 0f;
        
        //tf.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        tf.localScale = new Vector3(newSpriteScale, newSpriteScale, 1f);
        tf.Translate(tf.up * _backgroundY);
    }
}
