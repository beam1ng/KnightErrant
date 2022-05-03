using System;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem SS;

    private int _successfulJumps;
    public delegate void EventHandler(int successfulJumps);
    public event EventHandler SuccessfulJumpEvent;
    public GameObject initialGround;

    private Guid _lastGroundID;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _currentScore;

    private void Awake()
    {
        if (SS != null)
        {
            Destroy(this);
        }
        else
        {
            SS = this;
        }

        _lastGroundID = initialGround.GetComponent<GroundMovement>().GetID();
    }

    private void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().PlayerLandedEvent += CheckProgress;
        _scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        _currentScore = GameObject.FindWithTag("CurrentScore").GetComponent<TextMeshProUGUI>();
        UpdateScoreText();
    }

    private void CheckProgress(Guid groundHitID)
    {
        if (_lastGroundID == groundHitID) return;
        SuccessfulJump();
        _lastGroundID = groundHitID;
    }
    
    private void SuccessfulJump()
    {
        _successfulJumps++;
        UpdateScoreText();
        SuccessfulJumpEvent?.Invoke(_successfulJumps);
    }

    private void UpdateScoreText()
    {
        _scoreText.text = _successfulJumps.ToString();
        _currentScore.text = _successfulJumps.ToString();
    }
}
