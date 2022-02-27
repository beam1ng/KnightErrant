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
    private float _currentVerticalVelocity = 0f;
    private int _collisionCount = 0;

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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        HandleGravity(_ps);
    }

    void Update()
    {
        HandlePlayerAnimation(_pas);
    }


    private void HandleGravity(PlayerState ps)
    {
        float deltaTime = Time.deltaTime;
        switch (ps)
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
        ContactPoint2D[] contacts = new ContactPoint2D[col.contactCount];
        col.GetContacts(contacts);
        
        _ps = PlayerState.InAir;
        if (!contacts.Any(contact => Vector2.Angle(contact.normal, Vector2.up) < 45f)) return;
        _ps = PlayerState.OnGround;
        _currentVerticalVelocity = 0;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (--_collisionCount == 0)
        {
            _ps = PlayerState.InAir;
        }
    }

    private void HandlePlayerAnimation(PlayerAnimationState pas)
    {
        switch (pas)
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
        Debug.Log("jump");
        _currentVerticalVelocity = jumpVerticalVelocity;
    }
}
