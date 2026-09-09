using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsUI : MonoBehaviour
{
    [SerializeField] private GameObject _level1;
    [SerializeField] private GameObject _level2;
    [SerializeField] private GameObject _level3;

    public static event Action<GameSceneManager.GameScene> OnLevelButtonPressed;

    private Button _level1Button;
    private Button _level2Button;
    private Button _level3Button;

    private TextMeshProUGUI _level1Text;
    private TextMeshProUGUI _level2Text;
    private TextMeshProUGUI _level3Text;

    [SerializeField] private Color32[] _openedLevelButtonsColor; // 0: L1, 1: L2, 2: L3
    [SerializeField] private Color32[] _closedLevelButtonsColor;

    private float _closedLevelTextAlpha = 0.6f;
    private float _maxAlpha = 1.0f;

    private void Awake()
    {
        _level1Button = _level1.GetComponent<Button>();
        _level2Button = _level2.GetComponent<Button>();
        _level3Button = _level3.GetComponent<Button>();

        _level1Text = _level1.GetComponentInChildren<TextMeshProUGUI>();
        _level2Text = _level2.GetComponentInChildren<TextMeshProUGUI>();
        _level3Text = _level3.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        SetButtonsBehaviour();
        SetButtonsColor();
    }

    private void SetButtonsBehaviour()
    {
        int maxAchievedLevel = GameDataManager.Instance.GetAchievedMaxLevel();
        _level1Button.onClick.AddListener(() =>
        {
            OnLevelButtonPressed?.Invoke(GameSceneManager.GameScene.Level1);
        });
        _level2Button.onClick.AddListener(() =>
        {
            if (maxAchievedLevel < 2) return;
            OnLevelButtonPressed?.Invoke(GameSceneManager.GameScene.Level2);
        });
        _level3Button.onClick.AddListener(() =>
        {
            if (maxAchievedLevel < 3) return;
            OnLevelButtonPressed?.Invoke(GameSceneManager.GameScene.Level3);
        });
    }

    private void SetButtonsColor()
    {
        // Level 1 always opened.
        _level1Button.GetComponent<Image>().color = _openedLevelButtonsColor[0];
        _level1Text.alpha = _maxAlpha;

        // Close Level 2 and Level 3
        _level2Button.GetComponent<Image>().color = _closedLevelButtonsColor[1];
        _level2Text.alpha = _closedLevelTextAlpha;
        _level3Button.GetComponent<Image>().color = _closedLevelButtonsColor[2];
        _level3Text.alpha = _closedLevelTextAlpha;

        int maxAchievedLevel = GameDataManager.Instance.GetAchievedMaxLevel();
        if(maxAchievedLevel > 1)
        {
            // Level 2 is achieved already.
            _level2Button.GetComponent<Image>().color = _openedLevelButtonsColor[1];
            _level2Text.alpha = _maxAlpha;
            if(maxAchievedLevel > 2)
            {
                // Level 3 is achieved already.
                _level3Button.GetComponent<Image>().color = _openedLevelButtonsColor[2];
                _level3Text.alpha = _maxAlpha;
            }
        }
    }
}
