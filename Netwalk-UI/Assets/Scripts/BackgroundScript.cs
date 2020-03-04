using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        // var transformObj = gameObject.transform;
        //
        // var screenWidth = Screen.width;
        // var screenHeight = Screen.height;
        // var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // var currentImageSize = spriteRenderer.sprite.rect.size.y;
        //
        // var margin = 0.02f;
        //
        // var newImageSize = screenWidth * (1.0f - 2.0f * margin);
        // var newY = screenHeight / 2.0f - margin * newImageSize - newImageSize / 2.0f;
        //
        // var scale = newImageSize / currentImageSize;
        //
        // Debug.Log($"Screen size is {screenWidth}, {screenHeight}");
        // Debug.Log($"Old image size was {currentImageSize}");
        // Debug.Log($"New image size is {newImageSize}");
        // Debug.Log($"Scale is {scale}");
        // Debug.Log($"New position is 0.0, {newY}");
        //
        // //transformObj.localScale = new Vector3(.5f, .5f, 1.0f);
        // transformObj.localPosition = new Vector3(0.0f, 3f, 0.0f);
        // //transformObj.localPosition = new Vector3(0.0f, newY, 0.0f);
        // transformObj.localScale = new Vector3(scale, scale, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}