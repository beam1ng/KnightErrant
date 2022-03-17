using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundMovement : MonoBehaviour
{
    public float horizontalSpeed = 6f;
    public AudioClip landedOnGroundAudioClip;
    
    [FormerlySerializedAs("_ms")] public MovementState ms = MovementState.Static;
    private readonly Guid _id = Guid.NewGuid();
    private float _gameSpeedAmplifier = 1f;
    
    public enum MovementState
    {
        Static,MovingLeft,MovingRight
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
        }
    }

    private void OnOffScreen()
    {
        Destroy(gameObject);
    }

    public void SetInitialMovement()
    {
        if (RandomHandler.RH.Random.Next(0, 2) == 0)
        {
            transform.position+=Vector3.right*ScreenDimensions.SD.GetScreenWidth()/2;
            ms = MovementState.MovingLeft;
        }
        else
        {
            transform.position+=Vector3.left*ScreenDimensions.SD.GetScreenWidth()/2;
            ms = MovementState.MovingRight;
        }
    }

    public Guid GetID()
    {
        return _id;
    }
}
