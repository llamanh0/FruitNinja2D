using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        BeforeGameStarts,
        Playing,
        Paused,
        GameOver,
    }
    public GameState State {  get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Subscribe necessary Events
    }

    private void OnDestroy()
    {
        // Unsubscribe from Subscribed Evets
    }

    public void ChangeState(GameState newState)
    {
        State = newState;

        switch (State)
        {
            case GameState.BeforeGameStarts:
                break;
            case GameState.Playing:
                Resume();
                break;
            case GameState.Paused:
                Pause();
                break;
            case GameState.GameOver:
                // Pause();
                break;
            default:
                break;
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
    }
    private void Resume()
    {
        Time.timeScale = 1f;
    }

}
