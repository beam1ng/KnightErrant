using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem LS;

    public delegate void EventHandler(int currentLevel);

    public event EventHandler LevelChangedEvent;

    public int jumpsPerLevel = 5;

    private int _currentLevel = 0;
    
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

    public void OnSuccessfulJump(int successfulJumps)
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
}
