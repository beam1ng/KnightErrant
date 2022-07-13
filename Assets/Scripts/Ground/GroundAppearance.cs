using System;
using GameLogic.ThemeSystem;
using UnityEngine;

public class GroundAppearance : MonoBehaviour
{
    private Theme _currentTheme;
    void Start()
    {
        ThemeSystem.TS.ThemeChangedEvent += OnThemeChanged;
        _currentTheme = ThemeSystem.TS.GetCurrentTheme();
        UpdateSprite();
    }

    private void OnDestroy()
    {
        ThemeSystem.TS.ThemeChangedEvent -= OnThemeChanged;
    }

    private void OnThemeChanged(Theme newTheme)
    {
        _currentTheme = newTheme;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = _currentTheme.PlatformSprite;
    }
}
