using System;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public float horizontalSpeed = 1f;
    private MovementState _ms = MovementState.Static;
    
    private readonly Guid _id = Guid.NewGuid();
    private float _gameSpeedAmplifier = 1f;
    
    private enum MovementState
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
        if (_ms == MovementState.Static) return;
        Move();
    }

    private void OnGameSpeedChanged(float newGameSpeedAmplifier)
    {
        _gameSpeedAmplifier = newGameSpeedAmplifier;
    }
    
    private void OnPlayerLanded(Guid groundHitID)
    {
        if (groundHitID != _id) return;
        _ms = MovementState.Static;
        GetComponent<AudioSource>().Play();
    }
    
    private void Move()
    {
        var deltaTime = Time.deltaTime * _gameSpeedAmplifier;
        switch (_ms)
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

    public Guid GetID()
    {
        return _id;
    }

    public void SetInitialMovement()
    {
        if (RandomHandler.RH.Random.Next(0, 2) == 0)
        {
            transform.position+=Vector3.right*ScreenDimensions.SD.GetScreenWidth()/2;
            _ms = MovementState.MovingLeft;
        }
        else
        {
            transform.position+=Vector3.left*ScreenDimensions.SD.GetScreenWidth()/2;
            _ms = MovementState.MovingRight;
        }
    }

    private void OnOffScreen()
    {
        Destroy(gameObject);
    }
}
