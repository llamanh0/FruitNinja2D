using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Managing Player Data
/// <para>
/// <list type="bullet">
/// <c>
/// <item>LevelProgress</item>
/// <item>Level(1,2,3)Score</item>
/// <item></item>
/// </c>
/// </list>
/// </para>
/// </summary>
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public static event Action OnDataSavedBeforeSceneChanged;

    [SerializeField] private LevelsSO _levelsSO;

    private const string LEVEL_PROGRESS = "LevelProgress";
    private const string LEVEL_1_STATUS = "Level1Data"; // Float Percentage: 1f = %100
    private const string LEVEL_2_STATUS = "Level2Data";
    private const string LEVEL_3_STATUS = "Level3Data";

    private int _levelProgress = 0;

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

        ScoreManager.OnLevelCompleted += OnLevelCompleted;
    }

    private void Start()
    {
        // Get old data or Set base values
        _levelProgress = PlayerPrefs.GetInt(LEVEL_PROGRESS, 1);
        PlayerPrefs.GetFloat(LEVEL_1_STATUS, 0f);
        PlayerPrefs.GetFloat(LEVEL_2_STATUS, 0f);
        PlayerPrefs.GetFloat(LEVEL_3_STATUS, 0f);
    }

    private void Update()
    {
        DebugDataHandler();
    }

    private void OnDestroy()
    {
        ScoreManager.OnLevelCompleted -= OnLevelCompleted;
    }

    private void DebugDataHandler()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(LEVEL_PROGRESS, 1);
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            SetAchievedMaxLevel(2);
        }
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            SetAchievedMaxLevel(3);
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            Debug.Log("Current Achieved Max Level: " + GetAchievedMaxLevel());
        }
    }

    private void SaveData() => PlayerPrefs.Save();

    private void OnLevelCompleted(int levelNumber)
    {
        _levelProgress = _levelProgress > levelNumber ? _levelProgress : levelNumber;

        PlayerPrefs.SetInt(LEVEL_PROGRESS, _levelProgress);
        SaveData();

        OnDataSavedBeforeSceneChanged?.Invoke();
    }

    // PUBLIC FUNCTIONS

    public LevelPropertiesSO GetLevelPropertiesSOFromScene(GameSceneManager.GameScene scene)
    {
        foreach (var levelSO in _levelsSO.levels)
        {
            if(levelSO.scene == scene)
            {
                return levelSO;
            }
        }
        return null;
    }

    public LevelPropertiesSO GetLevelPropertiesSOFromCurrentScene()
    {
        if (!GameSceneManager.Instance) return null;
        return GetLevelPropertiesSOFromScene(GameSceneManager.Instance.CurrentScene);
    }

    /// <summary>
    /// Set and Save the <c>LevelProgess</c> data for Levels Section
    /// </summary>
    public void SetAchievedMaxLevel(int level)
    {
        PlayerPrefs.SetInt(LEVEL_PROGRESS, level);
        SaveData();
    }

    public int GetAchievedMaxLevel()
    {
        int achievedMaxLevel = 1;
        achievedMaxLevel = PlayerPrefs.GetInt(LEVEL_PROGRESS);
        return achievedMaxLevel;
    }
}
