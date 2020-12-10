using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    private ScoreManager scoreManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        scoreManager.OnScoreChanged += UpdateUi;
        UpdateUi();
    }

    private void UpdateUi()
    {
        score.text = $"{scoreManager.LeftScore}-{scoreManager.RightScore}";
    }
}
