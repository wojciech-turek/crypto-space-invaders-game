using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves Config", fileName = "New Waves Config")]
public class WavesConfigSO : ScriptableObject
{
    [SerializeField]
    Transform pathPrefab;

    [SerializeField]
    List<GameObject> enemyPrefabs;

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float timeBetweenEnemySpawns = 1f;

    [SerializeField]
    float spawnTimeVariance = 1f;

    [SerializeField]
    float minimumSpawnTime = 0.2f;

    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab)
        {
            waveWaypoints.Add (child);
        }
        return waveWaypoints;
    }

    public Transform GetStartingWaypoint()
    {
        return pathPrefab.GetChild(0);
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public int GetEnemyCount()
    {
        return enemyPrefabs.Count;
    }

    public GameObject GetEnemyPrefab(int index)
    {
        return enemyPrefabs[index];
    }

    public float GetRandomSpawnTime()
    {
        float spawnTime =
            Random
                .Range(timeBetweenEnemySpawns - spawnTimeVariance,
                timeBetweenEnemySpawns + spawnTimeVariance);

        return Mathf.Clamp(spawnTime, minimumSpawnTime, float.MaxValue);
    }
}
