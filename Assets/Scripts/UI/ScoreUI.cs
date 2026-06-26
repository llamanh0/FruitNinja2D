using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _scoreBarFillImage;

    private void Awake()
    {
        ScoreManager.OnScoreChanged += UpdateScoreVisual;
    }

    private void OnDestroy()
    {
        ScoreManager.OnScoreChanged -= UpdateScoreVisual;
    }

    private void UpdateScoreVisual(int currentScore, int requiredScore)
    {
        _scoreBarFillImage.fillAmount = (float)currentScore / (float)requiredScore; ;
        _scoreText.text = currentScore.ToString() + " / " + requiredScore.ToString();
    }
}
