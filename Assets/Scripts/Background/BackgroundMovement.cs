using System;
using GameLogic.ThemeSystem;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BackgroundMovement : MonoBehaviour
{
    public int maxScore=50;
    
    private GameObject _camera;
    private float _backgroundStartingLocalY;
    private float _backgroundEndingLocalY;
    private float _cameraStartingY;
    private float _cameraEndingY;

    void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");
        var groundGenerator = GameObject.FindWithTag("GroundGenerator").GetComponent<GroundGenerator>();
        ThemeSystem.TS.ThemeChangedEvent += OnThemeChanged;
        var sprite = GetComponent<SpriteRenderer>().sprite;
        
        var calculatedScale = ScreenDimensions.SD.GetScreenWidth() * sprite.pixelsPerUnit / sprite.texture.width;
        var localScale = new Vector3(calculatedScale, calculatedScale, 1);
        transform.localScale = localScale;

        var calculatedWidthInUnits = sprite.texture.width*localScale.x/sprite.pixelsPerUnit;
        var calculatedHeightInUnits = sprite.texture.height*localScale.y/sprite.pixelsPerUnit;
        
        _backgroundStartingLocalY = (calculatedHeightInUnits / 2 - ScreenDimensions.SD.GetScreenHeight() / 2);
        _backgroundEndingLocalY = (-calculatedHeightInUnits / 2 + ScreenDimensions.SD.GetScreenHeight() / 2);

        _cameraStartingY = 0.0f;
        _cameraEndingY = groundGenerator.interGroundDistance * maxScore - _cameraStartingY;
        
        transform.localPosition = new Vector3(0, _backgroundStartingLocalY, 1);
    }


    private void Update()
    {
        if (_camera == null) return;
        var cameraPosition = _camera.transform.position;
        transform.localPosition =
            new Vector3(0,  Math.Max(Math.Min(transform.localPosition.y, CalculateBackgroundLocalPositionY(cameraPosition.y)),_backgroundEndingLocalY), 1-cameraPosition.z);
    }

    private float CalculateBackgroundLocalPositionY(float cameraY)
    {
        return (cameraY - _cameraStartingY) / (_cameraEndingY - _cameraStartingY) * (_backgroundEndingLocalY-_backgroundStartingLocalY) + _backgroundStartingLocalY;
    }

    private void OnThemeChanged(Theme newTheme)
    {
        GetComponent<SpriteRenderer>().sprite = newTheme.BackgroundSprite;
    }
}
