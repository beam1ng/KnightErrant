using TMPro;
using UnityEngine;

public class GameSpeed : MonoBehaviour
{
    public static GameSpeed GS;
    
    [SerializeField] private float speedPerLevel = 0.2f;

    public delegate void EventHandler(float newGameSpeed);
    public event EventHandler GameSpeedChangedEvent;
    
    private float _gameSpeedAmplifier = 1f;
    private TextMeshProUGUI _gameSpeedText;
    
    private void Awake()
    {
        if (GS != null)
        {
            Destroy(this);
        }
        else
        {
            GS = this;
        }
    }

    private void Start()
    {
        _gameSpeedText = GameObject.FindWithTag("GameSpeedText").GetComponent<TextMeshProUGUI>();
        LevelSystem.LS.LevelChangedEvent += OnLevelChanged;
        OnLevelChanged(LevelSystem.LS.GetLevel());
    }

    private void OnLevelChanged(int newLevel)
    {
        _gameSpeedAmplifier = (1 + speedPerLevel * newLevel);
        GameSpeedChanged();
        _gameSpeedText.text = "level " + (newLevel+1);
    }

    protected virtual void GameSpeedChanged()
    {
        GameSpeedChangedEvent?.Invoke(_gameSpeedAmplifier);
    }

    public float GetGameSpeed()
    {
        return _gameSpeedAmplifier;
    }
}
