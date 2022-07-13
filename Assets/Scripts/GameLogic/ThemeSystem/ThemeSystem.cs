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
    [SerializeField] private Sprite spacePlatform;
    [SerializeField] private Sprite spaceBackground;
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
            InitializeThemes();
        }
    }

    private void InitializeThemes()
    {
        _themeQueue.Enqueue(new Theme("jungleTheme",junglePlatform,jungleBackground,(0,0)));
        _themeQueue.Enqueue(new Theme("mountainTheme",mountainPlatform,mountainBackground,(1,1)));
        _themeQueue.Enqueue(new Theme("spaceTheme",spacePlatform,spaceBackground,(2,3)));
        _currentTheme = _themeQueue.Peek();
    }
    
    private void Start()
    {
        LevelSystem.LS.LevelChangedEvent += OnLevelChanged;
        ThemeChanged(_currentTheme);
    }
    
    private void OnLevelChanged(int newLevel)
    {
        if (!_currentTheme.OutOfLevelBounds(newLevel)) return;
        if (_themeQueue.Count == 1) return;
        _themeQueue.Dequeue();
        _currentTheme = _themeQueue.Peek();
        ThemeChanged(_currentTheme);
    }

    protected virtual void ThemeChanged(Theme newTheme)
    {
        ThemeChangedEvent?.Invoke(newTheme);
    }

    public Theme GetCurrentTheme()
    {
        return _currentTheme;
    }
}
