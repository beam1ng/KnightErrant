using System.Collections.Generic;
using GameLogic.ThemeSystem;
using UnityEngine;

public class ThemeSystem : MonoBehaviour
{
    public static ThemeSystem TS;
    public Sprite junglePlatform;
    public Sprite jungleBackground;
    public delegate void EventHandler(Theme newTheme);
    public event EventHandler ThemeChangedEvent;

    private readonly Queue<Theme> _themeQueue = new Queue<Theme>();
    private Theme _currentTheme;
    
    private void Awake()
    {
        if (TS != null)
        {
            Destroy(this);
        }
        else
        {
            TS = this;
        }
    }
    
    private void Start()
    {
        _themeQueue.Enqueue(new Theme("jungleTheme",junglePlatform,jungleBackground,(0,4)));//todo: calculate backgroundScrollingMaxscore somewhere
        _currentTheme = _themeQueue.Peek();
        LevelSystem.LS.LevelChangedEvent += OnLevelChanged;
        
        ThemeChanged(_currentTheme);
    }

    private void OnLevelChanged(int newLevel)
    {
        if (!_currentTheme.OutOfLevelBounds(newLevel)) return;
        _themeQueue.Dequeue();
        _currentTheme = _themeQueue.Peek();
        ThemeChanged(_currentTheme);
    }

    protected virtual void ThemeChanged(Theme newTheme)
    {
        ThemeChangedEvent?.Invoke(newTheme);
    }
}
