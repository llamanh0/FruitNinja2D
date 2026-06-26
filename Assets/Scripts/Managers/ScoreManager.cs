using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int, int> OnScoreChanged; // currentScore, requiredScore

    public static event Action<int> OnLevelCompleted; // levelNumber {1, 2, 3}

    private LevelPropertiesSO _levelPropertiesSO;
    private int _currentScore = 0;
    private int _requiredScore = 1;

    #region Unity Lifecycle

    private void Awake()
    {
        BladeController.OnFruitSliced += OnFruitSliced;
        BladeController.OnBombSliced += OnBombSliced;
    }

    private void Start()
    {
        if (!GameDataManager.Instance)
        {
            Debug.LogError("GameDataManager does not exist!");
            return;
        }

        _levelPropertiesSO = GameDataManager.Instance.GetLevelPropertiesSOFromCurrentScene();
        _requiredScore = _levelPropertiesSO.requiredScore;

        OnScoreChanged?.Invoke(_currentScore, _requiredScore);
    }

    private void OnDestroy()
    {
        BladeController.OnFruitSliced -= OnFruitSliced;
        BladeController.OnBombSliced -= OnBombSliced;
    }

    #endregion

    private void OnFruitSliced(object sender, System.EventArgs e)
    {
        _currentScore += 25;
        UpdateScore();
    }

    private void OnBombSliced(object sender, System.EventArgs e)
    {
        _currentScore -= 10;
        UpdateScore();
    }

    private void UpdateScore()
    {
        OnScoreChanged?.Invoke(_currentScore, _requiredScore);

        if (_currentScore >= _requiredScore && GameSceneManager.Instance != null)
        {
            // TODO: Refactor this shitty code
            OnLevelCompleted?.Invoke((int)GameSceneManager.Instance.CurrentScene);
        }
    }
}
