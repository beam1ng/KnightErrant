using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class Highscores : MonoBehaviour
{
    private int[] _scoreArray = new int[5]{0,0,0,0,0};
    private TextMeshProUGUI[] _highscoreText = new TextMeshProUGUI[5];
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI defaultScoreText;

    private GameObject _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _player.GetComponent<PlayerMovement>().GameOverEvent += OnGameOver;
        ScoreSystem.SS.SuccessfulJumpEvent += OnScoreChanged;
        for (int i = 0; i < 4; i++)
        {
            _highscoreText[i] = Instantiate(defaultScoreText,transform);
        }

        _highscoreText[4] = Instantiate(currentScoreText,transform);
        CalculateTextHeights();
        LoadHighScores();
    }

    private void CalculateTextHeights()
    {
        for (int i = 0; i < 5; i++)
        {
            _highscoreText[i].transform.localPosition = Vector3.up*(160-i*80);
        }
    }

    // Update is called once per frame
    void OnScoreChanged(int successfulJumps)
    {
        for (var i = 0; i < _highscoreText.Length; i++)
        {
            _scoreArray[i] = int.Parse(_highscoreText[i].text);
        }

        for (int i = 4; i > 0; i--)
        {
            if (_scoreArray[i] > _scoreArray[i-1])
            {
                (_scoreArray[i], _scoreArray[i - 1]) = (_scoreArray[i - 1], _scoreArray[i]);
                (_highscoreText[i], _highscoreText[i-1]) = (_highscoreText[i-1], _highscoreText[i]);
            }
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
        File.WriteAllText("./Assets/Storage/highscores.json",json);
    }

    private void LoadHighScores()
    {
        int[] scoresData;
        var json = File.ReadAllText("./Assets/Storage/highscores.json");
        scoresData = JsonConvert.DeserializeObject<int[]>(json);
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
}
