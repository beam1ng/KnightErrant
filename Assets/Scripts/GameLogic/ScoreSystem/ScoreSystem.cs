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

    private Guid _lastGroundID;

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

    private void CheckProgress(Guid groundHitID)
    {
        if (_lastGroundID != groundHitID)
        {
            SuccessfulJump();
            _lastGroundID = groundHitID;
        }
    }
    
    protected virtual void SuccessfulJump()
    {
        SuccessfulJumpEvent?.Invoke(++_successfulJumps);
    }
}
