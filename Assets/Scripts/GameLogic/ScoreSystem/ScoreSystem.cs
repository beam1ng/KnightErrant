using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem SS;
    public int successfulJumps = 0;

    public delegate void EventHandler(int successfulJumps);

    public event EventHandler SuccessfulJumpEvent;

    private Guid _lastGroundID;
    private TextMeshProUGUI _scoreText;

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
        _scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        Debug.Log(_scoreText);
        UpdateScoreText();
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
        SuccessfulJumpEvent?.Invoke(++successfulJumps);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = successfulJumps.ToString();
    }
}
