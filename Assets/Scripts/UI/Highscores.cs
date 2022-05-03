using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
public class Highscores : MonoBehaviour
{
    private int[] _scoreArray = new int[5]{0,0,0,0,0};
    private readonly TextMeshProUGUI[] _highscoreText = new TextMeshProUGUI[5];
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI defaultScoreText;

    private GameObject _player;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            _highscoreText[i] = Instantiate(defaultScoreText,transform);
        }

        _highscoreText[4] = Instantiate(currentScoreText,transform);
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");

        CalculateTextHeights();
        LoadHighScores();
        
        ScoreSystem.SS.SuccessfulJumpEvent += OnScoreChanged;
        _player.GetComponent<PlayerMovement>().GameOverEvent += OnGameOver;
    }

    private void CalculateTextHeights()
    {
        for (var i = 0; i < 5; i++)
        {
            _highscoreText[i].transform.localPosition = Vector3.up*(160-i*80);
        }
    }

    private void LoadHighScores()
    {
        if (!File.Exists(Application.persistentDataPath + "/highscores.json"))
        {
            File.Create(Application.persistentDataPath + "/highscores.json");
        }
        var json = File.ReadAllText(Application.persistentDataPath+"/highscores.json");
        var scoresData = JsonConvert.DeserializeObject<int[]>(json);
        if (scoresData == null)
        {
            _scoreArray = new[] {0, 0, 0, 0, 0};
        }
        else
        {
            var currentScoreData = new int[5-scoresData.Length];
            _scoreArray = scoresData.Concat(currentScoreData).ToArray();
        }

        for (int i = 0; i < _scoreArray.Length; i++)
        {
            _highscoreText[i].text = _scoreArray[i].ToString();
        }
    }

    private void OnScoreChanged(int successfulJumps)
    {
        for (var i = 0; i < _highscoreText.Length; i++)
        {
            _scoreArray[i] = int.Parse(_highscoreText[i].text);
        }

        for (var i = 4; i > 0; i--)
        {
            if (_scoreArray[i] <= _scoreArray[i - 1]) continue;
            (_scoreArray[i], _scoreArray[i - 1]) = (_scoreArray[i - 1], _scoreArray[i]);
            (_highscoreText[i], _highscoreText[i-1]) = (_highscoreText[i-1], _highscoreText[i]);
        }
        CalculateTextHeights();
    }

    private void OnGameOver()
    {
        SaveHighScores();
    }

    private void SaveHighScores()
    {
        var data = _scoreArray.Take(4).ToArray();
        var json = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.persistentDataPath+"/highscores.json",json);
    }
}
