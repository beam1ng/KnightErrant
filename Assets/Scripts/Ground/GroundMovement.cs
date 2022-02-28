using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public float horizontalSpeed;
    private MovementState _ms = MovementState.MovingLeft;
    
    [SerializeField] private readonly Guid _id = Guid.NewGuid();
    
    private enum MovementState
    {
        Static,MovingLeft,MovingRight
    }

    private void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().PlayerLandedEvent += OnPlayerLanded;
    }

    private void Update()
    {
        if (_ms == MovementState.Static) return;
        Move();
    }

    private void OnPlayerLanded(Guid groundHitID)
    {
        if (groundHitID != _id) return;
        _ms = MovementState.Static;
    }
    
    private void Move()
    {
        switch (_ms)
        {
            case MovementState.Static:
                break;
            case MovementState.MovingLeft:
                transform.position+=Vector3.left*horizontalSpeed*Time.deltaTime;
                break;
            case MovementState.MovingRight:
                transform.position+=Vector3.right*horizontalSpeed*Time.deltaTime;
                break;
        }
    }

    public Guid GetID()
    {
        return _id;
    }
}
