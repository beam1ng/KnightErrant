using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDeletion : MonoBehaviour
{
    public float baseLifeDuration = 5f;

    private float _gameSpeedAmplifier = 1f;
    private float _lifeTimer;
    // Start is called before the first frame update
    void Start()
    {
        GameSpeed.GS.GameSpeedChangedEvent += OnGameSpeedChanged;
        _lifeTimer = baseLifeDuration;
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
}
