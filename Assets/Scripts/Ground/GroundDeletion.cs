using System;
using UnityEngine;

public class GroundDeletion : MonoBehaviour
{
    [SerializeField] private float baseLifeDuration = 5f;
    
    private GameObject _player;
    private float _gameSpeedAmplifier = 1f;
    private float _lifeTimer;

    void Awake()
    {
        _lifeTimer = baseLifeDuration;
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        GameSpeed.GS.GameSpeedChangedEvent += OnGameSpeedChanged;
        _gameSpeedAmplifier = GameSpeed.GS.GetGameSpeed();
    }
    
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

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject != _player) return;
        if (GetComponent<GroundMovement>().ms == GroundMovement.MovementState.Static)
        {
            Destroy(gameObject);
        }
    }
}
