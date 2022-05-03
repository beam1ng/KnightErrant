using System.Collections.Generic;
using GameLogic.ThemeSystem;
using UnityEngine;

public class ThemeSystem : MonoBehaviour
{
    public static ThemeSystem TS;
    
    [SerializeField] private Sprite junglePlatform;
    [SerializeField] private Sprite jungleBackground;
    [SerializeField] private Sprite mountainPlatform;
    [SerializeField] private Sprite mountainBackground;
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
        _themeQueue.Enqueue(new Theme("jungleTheme",junglePlatform,jungleBackground,(0,2)));//todo: calculate backgroundScrollingMaxscore somewhere
        _themeQueue.Enqueue(new Theme("mountainTheme",junglePlatform,jungleBackground,(3,50)));
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
