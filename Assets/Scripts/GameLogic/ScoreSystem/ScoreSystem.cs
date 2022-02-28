using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem SS;
    private int _successfulJumps = 0;

    public delegate void EventHandler(int successfulJumps);

    public event EventHandler SuccessfulJumpEvent;

    private GameObject _lastGround;

    private void Awake()
    {
        if (SS != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
            SS = this;
        }
    }

    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().PlayerLandedEvent += CheckProgress;
    }

    private void CheckProgress(GameObject ground)
    {
        if (_lastGround == null)
        {
            _lastGround = ground;
        }
        else if (_lastGround != ground && ground.CompareTag("Ground"))
        {
            SuccessfulJump();
        }
    }
    
    protected virtual void SuccessfulJump()
    {
        SuccessfulJumpEvent?.Invoke(++_successfulJumps);
    }
}
