using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HighScoreCalculator : MonoBehaviour
{
    [SerializeField] TMP_Text highscore;
    [SerializeField] private TMP_Text _scoreHistoryText;
    
    int score;
    int totalScore;
    int[] savedScores;

    
    void Start()
    {
        ScoreCalculator();
        StartCoroutine(HighscoreCounter());
    }
    

    IEnumerator HighscoreCounter()
    {
        while (score < totalScore)
        {
            score += 2;
            highscore.text = "Score: " + Convert.ToString(score);
            yield return 0;
        }
        PreviousScoreGenerator();
    }

    void ScoreCalculator()
    {
        if (GameManager.Instance != null)
        {
            totalScore = GameManager.Instance.points;
            savedScores = GameManager.Instance.savedScores;
            
            savedScores = savedScores.Concat(new [] {totalScore}).ToArray();
            GameManager.Instance.savedScores = savedScores;
        }
        else
        {
            totalScore = 500;
            savedScores = new []{0, 0, 0, 0, 0};
        }
        
    }

    void PreviousScoreGenerator()
    {
        savedScores = savedScores.Concat(new [] {totalScore}).ToArray();
        Array.Sort(savedScores);
        Array.Reverse(savedScores);
        _scoreHistoryText.text = 
            "1st - " + Convert.ToString(savedScores[0]) + 
            "\n2nd - " + Convert.ToString(savedScores[1]) + 
            "\n3rd - " + Convert.ToString(savedScores[2]) + 
            "\n4th - " + Convert.ToString(savedScores[3]) + 
            "\n5th - " + Convert.ToString(savedScores[4]);
    }
}
