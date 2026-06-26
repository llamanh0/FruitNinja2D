using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // Level Properties SO
    private LevelPropertiesSO _levelPropertiesSO;

    // Game Objects
    private List<GameObject> _bombList;
    private List<GameObject> _fruitList;

    // Throwing Properties
    private const float THROW_POWER_VERTICAL = 12f;
    private const float THROW_POWER_HORIZONTAL = 5f;
    private const float MIN_POSITION_X = -10f;
    private const float MAX_POSITION_X = 10f;
    private const float MIN_ANGLE_Z = 30;
    private const float MAX_ANGLE_Z = 60;
    private const float OBJECT_DESTROY_TIME = 3.5f;

    private Vector3 _randomAngle;
    private Vector3 _randomPosition;
    private float _throwIntervalMin;
    private float _throwIntervalMax;
    private float _bombPercentage;


    private void Start()
    {
        if(!GameSceneManager.Instance || !GameDataManager.Instance)
        {
            Debug.LogError("GameSceneManager or GameDataManager does not exist!");
            return;
        }
        GameSceneManager.GameScene currentScene = GameSceneManager.Instance.CurrentScene;
        _levelPropertiesSO = GameDataManager.Instance.GetLevelPropertiesSOFromScene(currentScene);

        if (!_levelPropertiesSO) return;

        _bombList = _levelPropertiesSO.bombList;
        _fruitList = _levelPropertiesSO.fruitList;
        _throwIntervalMax = _levelPropertiesSO.throwIntervalMax;
        _throwIntervalMin = _levelPropertiesSO.throwIntervalMin;
        _bombPercentage = _levelPropertiesSO.bombPercentage;

        StartCoroutine(SpawnRoutine());
    }


    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(_throwIntervalMin, _throwIntervalMax);

            yield return new WaitForSeconds(waitTime);

            SpawnGameObject();
        }
    }

    private void SpawnGameObject()
    {
        GameObject gameObjectToSpawn = null;

        float randomChance = Random.Range(0f, 100f);
        if(randomChance <= _bombPercentage)
        {
            gameObjectToSpawn = GetRandomGameObjectFromList(_bombList);
        }
        else
        {
            gameObjectToSpawn = GetRandomGameObjectFromList(_fruitList);
        }
        RandomThrowPositionAndRotation();

        GameObject spawnedGameObject = null;

        spawnedGameObject = Instantiate(gameObjectToSpawn, _randomPosition, Quaternion.Euler(_randomAngle));
        Destroy(spawnedGameObject, OBJECT_DESTROY_TIME);

        // TODO: Dont use GetComponent for each spawned object (Performance issue!)
        Rigidbody2D rb = spawnedGameObject.gameObject.GetComponent<Rigidbody2D>();
        rb.AddForceX(spawnedGameObject.transform.up.x * THROW_POWER_HORIZONTAL, ForceMode2D.Impulse);
        rb.AddForceY(THROW_POWER_VERTICAL, ForceMode2D.Impulse);
    }

    private void RandomThrowPositionAndRotation()
    {
        float randomPositionX = Random.Range(MIN_POSITION_X, MAX_POSITION_X);
        _randomPosition = new Vector3(randomPositionX, transform.position.y, 0f);

        int angleNegativeModifier = (randomPositionX < 0f) ? -1 : 1; 

        float randomAngleZ = Random.Range(MIN_ANGLE_Z, MAX_ANGLE_Z) * angleNegativeModifier;
        _randomAngle = new Vector3(0, 0, randomAngleZ);
    }

    private GameObject GetRandomGameObjectFromList(List<GameObject> list)
    {
        if (list == null || list.Count == 0) return null;

        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, new Vector3(-MIN_POSITION_X + MAX_POSITION_X, 1f));
    }
#endif
}
