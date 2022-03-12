using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BackgroundMovement : MonoBehaviour
{
    private GameObject _camera;
    public int maxScore=50;
    private float _startingHeight;
    private float _endingHeight;
    private float _cameraStartingY = 0.0f;
    private float _cameraEndingY;

    void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");
        
        var calculatedScale =
            ScreenDimensions.SD.GetScreenWidth() * GetComponent<SpriteRenderer>().sprite.pixelsPerUnit / GetComponent<SpriteRenderer>().sprite.texture.width;
        transform.localScale = new Vector3(calculatedScale, calculatedScale, 1);
        var calculatedWidthInUnits = GetComponent<SpriteRenderer>().sprite.texture.width*transform.localScale.x/GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        var calculatedHeightInUnits = GetComponent<SpriteRenderer>().sprite.texture.height*transform.localScale.y/GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        _startingHeight = (calculatedHeightInUnits / 2 - ScreenDimensions.SD.GetScreenHeight() / 2);
        _endingHeight = (-calculatedHeightInUnits / 2 + ScreenDimensions.SD.GetScreenHeight() / 2);
        transform.localPosition = new Vector3(0, _startingHeight, 1);
        _cameraEndingY = GameObject.FindWithTag("GroundGenerator").GetComponent<GroundGenerator>().interGroundDistance * maxScore-_cameraStartingY;
    }


    void Update()
    {
        if (_camera == null) return;
        var cameraPosition = _camera.transform.position;
        transform.localPosition =
            new Vector3(0,  Math.Max(Math.Min(transform.localPosition.y, GetBackgroundPosition(cameraPosition.y)),_endingHeight), 1-cameraPosition.z);
    }

    private float GetBackgroundPosition(float cameraY)
    {
        return (cameraY - _cameraStartingY) / (_cameraEndingY - _cameraStartingY) * (_endingHeight-_startingHeight) + _startingHeight;
    }
}
