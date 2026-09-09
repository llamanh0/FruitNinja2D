using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    public enum GameScene
    {
        MainMenu,   // 0
        Levels,     // 1
        Level1,     // 2
        Level2,     // 3
        Level3,     // 4
    }
    public GameScene CurrentScene = GameScene.MainMenu;

    #region UNITY LIFECYCLE

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Subscribe necessary Events
        LevelsUI.OnLevelButtonPressed += LoadSpesificLevelScene;
        MainMenuUI.OnPlayButtonPressed += LoadLevelsScene;
        GameDataManager.OnDataSavedBeforeSceneChanged += LoadLevelsSceneWithAnimation;
    }

    private void Update()
    {
        DebugSceneHandler();
    }

    private void OnDestroy()
    {
        // Unsubscribe from Subscribed Evets
        LevelsUI.OnLevelButtonPressed -= LoadSpesificLevelScene;
        MainMenuUI.OnPlayButtonPressed -= LoadLevelsScene;
        GameDataManager.OnDataSavedBeforeSceneChanged -= LoadLevelsSceneWithAnimation;
    }

    #endregion

    #region EVENT FUNCTIONS

    private void LoadSpesificLevelScene(GameSceneManager.GameScene scene) => LoadScene(scene);

    private void LoadLevelsScene() => LoadScene(GameScene.Levels);

    private void LoadLevelsSceneWithAnimation() => StartCoroutine(WaitForSeconds(1.5f));

    IEnumerator WaitForSeconds(float seconds)
    {
        // TODO: Should add Animations
        yield return new WaitForSeconds(seconds);
        LoadLevelsScene();
    }

    #endregion

    #region UTILITES

    private void LoadScene(GameScene newScene)
    {
        CurrentScene = newScene;
        SceneManager.LoadScene((int)newScene);
    }

    private void DebugSceneHandler()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            LoadScene(GameScene.MainMenu);
        }
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            LoadScene(GameScene.Levels);
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            LoadScene(GameScene.Level1);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            LoadScene(GameScene.Level2);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            LoadScene(GameScene.Level3);
        }
    }

    #endregion
}
