using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float gravity = 40;
    public float jumpVerticalVelocity = 25;

    public delegate void EventHandler(GameObject ground);
    public event EventHandler PlayerLandedEvent;
    
    private float _currentVerticalVelocity = 0f;
    private int _collisionCount = 0;
    private float _gameSpeed = 1;

    private enum PlayerAnimationState
    {
        Idle,Standing,Running,Jumping,Falling
    }

    private enum PlayerState
    {
        OnGround,InAir
    }
    
    private PlayerState _ps = PlayerState.InAir;
    private PlayerAnimationState _pas = PlayerAnimationState.Idle;

    private void Start()
    {
        GameObject.FindWithTag("GameLogic").GetComponent<GameSpeed>().GameSpeedChangedEvent += GameSpeedChanged;
    }

    private void FixedUpdate()
    {
        HandleGravity();
    }

    void Update()
    {
        HandlePlayerAnimation();
    }

    private void GameSpeedChanged(float newGameSpeed)
    {
        _gameSpeed = newGameSpeed;
    }

    private void HandleGravity()
    {
        float deltaTime = Time.deltaTime * _gameSpeed;
        switch (_ps)
        {
            case PlayerState.OnGround:
                break;
            case PlayerState.InAir:
                _currentVerticalVelocity -= gravity * deltaTime;
                break;

        }
        transform.position+=Vector3.up*_currentVerticalVelocity*deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _collisionCount++;
        var contacts = new ContactPoint2D[col.contactCount];
        col.GetContacts(contacts);

        var initialPlayerState = _ps;
        
        _ps = PlayerState.InAir;
        if (!contacts.Any(contact => Vector2.Angle(contact.normal, Vector2.up) < 45f)) return;
        _ps = PlayerState.OnGround;
        if (initialPlayerState == PlayerState.InAir && _ps == PlayerState.OnGround)
        {
            OnPlayerLanded(col.gameObject);
        }
        _currentVerticalVelocity = 0;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (--_collisionCount == 0)
        {
            _ps = PlayerState.InAir;
        }
    }

    private void HandlePlayerAnimation()
    {
        switch (_pas)
        {
            case PlayerAnimationState.Idle:
                break;
            case PlayerAnimationState.Standing:
                break;
            case PlayerAnimationState.Running:
                break;
            case PlayerAnimationState.Jumping:
                break;
            case PlayerAnimationState.Falling:
                break;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (_ps != PlayerState.OnGround) return;
        if (!ctx.started) return;
        _currentVerticalVelocity = jumpVerticalVelocity;
    }

    protected virtual void OnPlayerLanded(GameObject ground)
    {
        PlayerLandedEvent?.Invoke(ground);
    }
}
