using System;
using GameLogic.ThemeSystem;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    private GameObject _camera;
    private float _backgroundStartingLocalY;
    private float _backgroundEndingLocalY;
    private float _cameraStartingY;
    private float _cameraEndingY;
    private Sprite _currentSprite;
    private Theme _currentTheme;
    void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");
        ThemeSystem.TS.ThemeChangedEvent += OnThemeChanged;
        _currentSprite = GetComponent<SpriteRenderer>().sprite;
    }


    private void Update()
    {
        if (_camera == null) return;
        var cameraPosition = _camera.transform.position;
        transform.localPosition =
            new Vector3(0,  Math.Max(Math.Min(transform.localPosition.y, CalculateBackgroundLocalPositionY(cameraPosition.y)),_backgroundEndingLocalY), 1-cameraPosition.z);
    }

    private void CalculateBackgroundBounds()
    {
        var calculatedScale = ScreenDimensions.SD.GetScreenWidth() * _currentSprite.pixelsPerUnit / _currentSprite.texture.width;
        var localScale = new Vector3(calculatedScale, calculatedScale, 1);
        transform.localScale = localScale;

        var calculatedWidthInUnits = _currentSprite.texture.width*localScale.x/_currentSprite.pixelsPerUnit;
        var calculatedHeightInUnits = _currentSprite.texture.height*localScale.y/_currentSprite.pixelsPerUnit;
        
        _backgroundStartingLocalY = (calculatedHeightInUnits / 2 - ScreenDimensions.SD.GetScreenHeight() / 2);
        _backgroundEndingLocalY = (-calculatedHeightInUnits / 2 + ScreenDimensions.SD.GetScreenHeight() / 2);

        _cameraStartingY = _camera.transform.position.y;
        _cameraEndingY = GroundGenerator.GG.GetInterGroundDistance() * ((_currentTheme.LevelBounds.maxLevel-_currentTheme.LevelBounds.minLevel+1) * LevelSystem.LS.GetJumpsPerLevel()-1) + _playerMovement.jumpHeight + _cameraStartingY;
        
        transform.localPosition = new Vector3(0, _backgroundStartingLocalY, 1);
    }
    
    private float CalculateBackgroundLocalPositionY(float cameraY)
    {
        return (cameraY - _cameraStartingY) / (_cameraEndingY - _cameraStartingY) * (_backgroundEndingLocalY-_backgroundStartingLocalY) + _backgroundStartingLocalY;
    }
    
    private void OnThemeChanged(Theme newTheme)
    {
        _currentTheme = newTheme;
        GetComponent<SpriteRenderer>().sprite = newTheme.BackgroundSprite;
        CalculateBackgroundBounds();
    }
}
