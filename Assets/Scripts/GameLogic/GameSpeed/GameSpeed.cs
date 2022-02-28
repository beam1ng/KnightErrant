using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeed : MonoBehaviour
{
    public static GameSpeed GS;

    public delegate void EventHandler(float newGameSpeed);

    public event EventHandler GameSpeedChangedEvent;

    public float speedPerLevel = 0.2f;

    private float _gameSpeedAmplifier = 1f;


    private void Awake()
    {
        if (GS != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
            GS = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GetComponent<LevelSystem>().LevelChangedEvent += OnLevelChanged;
    }

    private void OnLevelChanged(int newLevel)
    {
        _gameSpeedAmplifier = (1 + speedPerLevel * newLevel);
        GameSpeedChanged();
    }

    protected virtual void GameSpeedChanged()
    {
        GameSpeedChangedEvent?.Invoke(_gameSpeedAmplifier);
    }
}
