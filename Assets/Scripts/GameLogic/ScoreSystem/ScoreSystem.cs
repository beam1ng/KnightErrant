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
    private TextMeshProUGUI _currentScore;

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
        _currentScore = GameObject.FindWithTag("CurrentScore").GetComponent<TextMeshProUGUI>();
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
        successfulJumps++;
        UpdateScoreText();
        SuccessfulJumpEvent?.Invoke(successfulJumps);
    }

    private void UpdateScoreText()
    {
        _scoreText.text = successfulJumps.ToString();
        _currentScore.text = successfulJumps.ToString();
    }
}
