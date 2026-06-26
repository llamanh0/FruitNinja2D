using System.Collections.Generic;
using UnityEngine;

public class LevelPropertiesSO : ScriptableObject
{
    [Header("Level Settings")]
    public GameSceneManager.GameScene scene;
    public int requiredScore;
    public float timeLimit;

    [Header("Object Lists")]
    public List<GameObject> bombList = new List<GameObject>();
    public List<GameObject> fruitList = new List<GameObject>();

    [Header("Throwing Settings")]
    [Range(1f, 100f)]
    public float bombPercentage;
    public float throwIntervalMin;
    public float throwIntervalMax;
}
