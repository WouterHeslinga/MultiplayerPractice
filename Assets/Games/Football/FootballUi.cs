using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FootballUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI nextBall;
    private ScoreManager scoreManager;
    private FootballGameManager gameManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<FootballGameManager>();
        
        scoreManager.OnScoreChanged += UpdateUi;
        InvokeRepeating(nameof(UpdateUi), 0, .1f);
        UpdateUi();
    }

    private void UpdateUi()
    {
        score.text = $"{scoreManager.RedScore}-{scoreManager.BlueScore}";
        nextBall.text = $"{gameManager.nextBallTimer:F1}";
    }
}
