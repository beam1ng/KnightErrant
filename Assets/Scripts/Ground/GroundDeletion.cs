using UnityEngine;

public class GroundDeletion : MonoBehaviour
{
    public float baseLifeDuration = 5f;
    public bool sh;

    private float _gameSpeedAmplifier = 1f;
    private float _lifeTimer;

    void Awake()
    {
        _lifeTimer = baseLifeDuration;
    }

    private void Start()
    {
        GameSpeed.GS.GameSpeedChangedEvent += OnGameSpeedChanged;
        _gameSpeedAmplifier = GameSpeed.GS.GetGameSpeed();
    }
    
    void Update()
    {
        _lifeTimer -= Time.deltaTime * _gameSpeedAmplifier;
        if (_lifeTimer<=0)
        {
            Destroy(gameObject);
        }
    }

    private void OnGameSpeedChanged(float newGameSpeedAmplifier)
    {
        _gameSpeedAmplifier = newGameSpeedAmplifier;
    }
}
