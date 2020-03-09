using System.Collections;
using System.Collections.Generic;
using NetwalkLib;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellClickDetector : MonoBehaviour, IPointerClickHandler
{
    private int _row;
    private int _col;
    private GameObject _background;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetPosition((int Row, int Col) newPosition)
    {
        _row = newPosition.Row;
        _col = newPosition.Col;
    }
    
    public void SetBackground(GameObject background)
    {
        _background = background;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _background.SendMessage("RotateCell", (_row, _col));
    }
}
