using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static event Action OnPlayButtonPressed;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        _playButton.onClick.AddListener( () =>
        {
            OnPlayButtonPressed?.Invoke();
        });

        _exitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
