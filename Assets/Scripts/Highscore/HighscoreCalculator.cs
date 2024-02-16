using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HighScoreCalculator : MonoBehaviour
{
    [SerializeField] GameObject highscore; 
    int score;
    int totalScore;

    //Placeholder Example Scores
    int[] savedScores = new int[] {5000, 4000, 3000, 2000, 1000};


    // Start is called before the first frame update
    void Start()
    {
        score = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        //Temporairly bound to space key
        if (Input.GetKeyDown("space"))
        {
            ScoreCalculator();
            StartCoroutine(HighscoreCounter());
        }        
    }

    IEnumerator HighscoreCounter()
    {
        while (score < totalScore)
        {
            score = score+2;
            highscore.GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + Convert.ToString(score);
            yield return 0;
        }
        PreviousScoreGenerator();
    }

    void ScoreCalculator()
    {
        //Placeholder Total Score
        totalScore = 2345;
    }

    void PreviousScoreGenerator()
    {
        savedScores = savedScores.Concat(new int[] {totalScore}).ToArray();
        Array.Sort(savedScores);
        Array.Reverse(savedScores);
        highscore.GetComponent<TMPro.TextMeshProUGUI>().text = highscore.GetComponent<TMPro.TextMeshProUGUI>().text + 
            "\n\n1st - " + Convert.ToString(savedScores[0]) + 
            "\n2nd - " + Convert.ToString(savedScores[1]) + 
            "\n3rd - " + Convert.ToString(savedScores[2]) + 
            "\n4th - " + Convert.ToString(savedScores[3]) + 
            "\n5th - " + Convert.ToString(savedScores[4]);
    }
}
