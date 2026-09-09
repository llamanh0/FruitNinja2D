using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Slider _mainScoreSlider;
    [SerializeField] private Slider _easeScoreSlider;
    
    private float _maxValue = 1f;
    private float _currentValue = 0f;
    private float _oldValue = 0f;
    private float _lerpSpeed = 0.05f;

    private void Update()
    {
        if(_currentValue > _oldValue)
        {
            _mainScoreSlider.value = Mathf.Lerp(_mainScoreSlider.value, _currentValue / _maxValue, _lerpSpeed);
        } 
        else if (_mainScoreSlider.value != _currentValue)
        {
            _mainScoreSlider.value = _currentValue / _maxValue;
        }

        if (_mainScoreSlider.value != _easeScoreSlider.value)
        {
            _easeScoreSlider.value = Mathf.Lerp(_easeScoreSlider.value, _currentValue / _maxValue, _lerpSpeed);
        }
    }

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
        _oldValue = _currentValue;
        _currentValue = (float)currentScore;
        _maxValue = (float)requiredScore;

        _scoreText.text = currentScore.ToString() + " / " + requiredScore.ToString();
    }
}
