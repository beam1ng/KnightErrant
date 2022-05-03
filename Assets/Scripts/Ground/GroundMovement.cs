using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundMovement : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 6f;
    [SerializeField] private AudioClip landedOnGroundAudioClip;
    [SerializeField] private float sinusoidalMovementOffset = 4;
    [SerializeField] private float sinusoidalMovementPeriod = 4;
    
    [FormerlySerializedAs("_ms")] public MovementState ms = MovementState.Static;
    private readonly Guid _id = Guid.NewGuid();
    private float _gameSpeedAmplifier = 1f;
    private float _sinusoidalMovementPhase = 0;
    
    public enum MovementState
    {
        Static,MovingLeft,MovingRight,MovingSinusoidally
    }

    private void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().PlayerLandedEvent += OnPlayerLanded;
        GetComponent<OffScreenable>().OffScreenEvent += OnOffScreen;
        GameSpeed.GS.GameSpeedChangedEvent += OnGameSpeedChanged;
        _gameSpeedAmplifier = GameSpeed.GS.GetGameSpeed();
    }

    private void Update()
    {
        Move();
    }

    private void OnGameSpeedChanged(float newGameSpeedAmplifier)
    {
        _gameSpeedAmplifier = newGameSpeedAmplifier;
    }
    
    private void OnPlayerLanded(Guid groundHitID)
    {
        if (groundHitID != _id) return;
        ms = MovementState.Static;
        GetComponent<AudioSource>().PlayOneShot(landedOnGroundAudioClip);
    }
    
    private void Move()
    {
        var deltaTime = Time.deltaTime * _gameSpeedAmplifier;
        switch (ms)
        {
            case MovementState.Static:
                break;
            case MovementState.MovingLeft:
                transform.position+=Vector3.left * (horizontalSpeed * deltaTime);
                break;
            case MovementState.MovingRight:
                transform.position+=Vector3.right * (horizontalSpeed * deltaTime);
                break;
            case MovementState.MovingSinusoidally:
                _sinusoidalMovementPhase += deltaTime;
                var position = transform.position;
                transform.position = new Vector3(sinusoidalMovementOffset * Mathf.Cos(_sinusoidalMovementPhase*2*Mathf.PI /sinusoidalMovementPeriod),position.y,position.z);
                break;
                
        }
    }

    private void OnOffScreen()
    {
        Destroy(gameObject);
    }

    public void SetInitialMovement()
    {
        // if (RandomHandler.RH.Random.Next(0, 2) == 0)
        // {
        //     transform.position+=Vector3.right*ScreenDimensions.SD.GetScreenWidth()/2;
        //     ms = MovementState.MovingLeft;
        //     
        // }
        // else
        // {
        //     transform.position+=Vector3.left*ScreenDimensions.SD.GetScreenWidth()/2;
        //     ms = MovementState.MovingRight;
        // }
        _sinusoidalMovementPhase = (float)RandomHandler.RH.Random.NextDouble()*sinusoidalMovementPeriod;
        ms = MovementState.MovingSinusoidally;
    }

    public Guid GetID()
    {
        return _id;
    }
}
