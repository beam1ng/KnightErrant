using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDeletion : MonoBehaviour
{
    public float baseLifeDuration = 5f;

    private float _gameSpeedAmplifier = 1f;
    private float _lifeTimer;
    public bool sh = false;
    // Start is called before the first frame update

    void Awake()
    {
        _lifeTimer = baseLifeDuration;
    }

    private void Start()
    {
        _gameSpeedAmplifier = GameSpeed.GS.GetGameSpeed();
        GameSpeed.GS.GameSpeedChangedEvent += OnGameSpeedChanged;
    }

    // Update is called once per frame
    void Update()
    {
        _lifeTimer -= Time.deltaTime * _gameSpeedAmplifier;
        if (_lifeTimer<=0)
        {
            Destroy(gameObject);
        }
    }

    private void OnGameSpeedChanged(float newGameSpeedAmplifier)
    {
        _gameSpeedAmplifier = newGameSpeedAmplifier;
    }

    public float DebugCurrentLifeDuration()
    {
        return _lifeTimer;
    }
}
