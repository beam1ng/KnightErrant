using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float jumpHeight = 3.5f;
    public float gravity = 40;
    private float _jumpVerticalVelocity;

    public delegate void EventHandler(Guid groundHitID);
    public event EventHandler PlayerLandedEvent;

    public delegate void GOEventHandler();

    public event GOEventHandler GameOverEvent;

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
        _jumpVerticalVelocity = (float)Math.Sqrt(2 * gravity * jumpHeight);
        GameSpeed.GS.GameSpeedChangedEvent += GameSpeedChanged;
        _gameSpeed = GameSpeed.GS.GetGameSpeed();
        GetComponent<OffScreenable>().OffScreenEvent += OnOffScreen;
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

        if (_currentVerticalVelocity < 0)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position+Vector3.down* 0.5f, Vector3.down, -1 * _currentVerticalVelocity * deltaTime,~(1<<3));
            if(hit)
            {
                transform.position += Vector3.down * hit.distance;
            }else
            {
                transform.position += Vector3.up * _currentVerticalVelocity * deltaTime;
            }
        }
        else
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position+Vector3.up* 0.5f, Vector3.up, _currentVerticalVelocity * deltaTime,~(1<<3));
            if(hit)
            {
                transform.position += Vector3.up * hit.distance;
            }else
            {
                transform.position += Vector3.up * _currentVerticalVelocity * deltaTime;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _collisionCount++;


        var contacts = new ContactPoint2D[col.contactCount];
        col.GetContacts(contacts);

        var initialPlayerState = _ps;
        
        //todo eject player sideways onGroundSideTouch
        
        _ps = PlayerState.InAir;
        if (contacts.Any(contact => Vector2.Angle(contact.normal, Vector2.down) < 45f))//headbutt
        {
            _currentVerticalVelocity = 0;
        }
        else if (contacts.Any(contact => Vector2.Angle(contact.normal, Vector2.up) < 45f))//land
        {
            _ps = PlayerState.OnGround;
            if (initialPlayerState == PlayerState.InAir && _ps == PlayerState.OnGround)
            {
                OnPlayerLanded(col.gameObject.GetComponent<GroundMovement>().GetID());
            }

            _currentVerticalVelocity = 0;
        }
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
        if (!ctx.started) return;
        if (_ps != PlayerState.OnGround) return;
        _currentVerticalVelocity = _jumpVerticalVelocity;
    }

    protected virtual void OnPlayerLanded(Guid groundHitID)
    {
        PlayerLandedEvent?.Invoke(groundHitID);
    }
    
    private void OnOffScreen()
    {
        OnGameOver();
        Destroy(gameObject);
    }

    protected virtual void OnGameOver()
    {
        GameOverEvent?.Invoke();
    }
}
