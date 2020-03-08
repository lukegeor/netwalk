using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class TileBackgroundScript : MonoBehaviour
{
    private float _backgroundMargin = 0.02f;
    
    // Start is called before the first frame update
    void Start()
    {
        ResizeBackgroundSpriteToScreen();
        CreatePuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePuzzle()
    {
    }

    private void ResizeBackgroundSpriteToScreen() {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;
        var tf = transform;
        if (tf == null) return;
        var mc = Camera.main;
        if (mc == null) return;
        
        var sprite = sr.sprite;
        var oldSpriteSize = sprite.bounds.size.y;
     
        var worldScreenHeight = mc.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        var newSpriteScale = (float) (worldScreenWidth / oldSpriteSize * (1f - _backgroundMargin));
        var newSpriteSize = oldSpriteSize * newSpriteScale;
        var newY = (float)((worldScreenHeight - _backgroundMargin * newSpriteSize - newSpriteSize) / 2f);

        tf.localScale = new Vector3(newSpriteScale, newSpriteScale, 1f);
        tf.Translate(tf.up * newY);
    }
}
