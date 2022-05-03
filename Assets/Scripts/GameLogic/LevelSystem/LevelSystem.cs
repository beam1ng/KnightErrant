using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem LS;
    
    [SerializeField] private int jumpsPerLevel = 5;
    public delegate void EventHandler(int currentLevel);
    public event EventHandler LevelChangedEvent;
    
    private int _currentLevel;
    
    private void Awake()
    {
        if (LS != null)
        {
            Destroy(this);
        }
        else
        {
            LS = this;
        }
    }

    private void Start()
    {
        ScoreSystem.SS.SuccessfulJumpEvent += OnSuccessfulJump;
    }

    private void OnSuccessfulJump(int successfulJumps)
    {
        var newLevel = (int)(successfulJumps/jumpsPerLevel);
        if (newLevel == _currentLevel) return;
        _currentLevel = newLevel;   
        LevelChanged(_currentLevel);
    }

    protected virtual void LevelChanged(int currentLevel)
    {
        LevelChangedEvent?.Invoke(currentLevel);
    }

    public int GetLevel()
    {
        return _currentLevel;
    }

    public int GetJumpsPerLevel()
    {
        return jumpsPerLevel;
    }
}
