using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class FootballUi : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI nextBall;
    private ScoreManager scoreManager;
    private FootballGameManager gameManager;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<FootballGameManager>();
        
        scoreManager.OnScoreChanged += UpdateUi;
        InvokeRepeating(nameof(UpdateUi), 0, .1f);
        UpdateUi();
    }

    private void UpdateUi()
    {
        score.text = $"{scoreManager.RedScore}-{scoreManager.BlueScore}";
        nextBall.text = $"Extra ball in: {gameManager.nextBallTimer:F1}s";
    }
}
