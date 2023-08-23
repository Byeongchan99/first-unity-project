using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<Wave> waves; // 여러 웨이브 정보
    public Transform[] spawnPoints; // 스폰 포인트 위치 정보
    private int currentWave = 0; // 현재 웨이브

    [Header("Object Pooling")]
    public int poolSize = 10; // 각 몬스터 별로 준비할 오브젝트 수
    private Dictionary<GameObject, List<GameObject>> objectPools;

    private void Start()
    {
        InitializeObjectPools();
    }

    void InitializeObjectPools()
    {
        objectPools = new Dictionary<GameObject, List<GameObject>>();

        foreach (var wave in waves)
        {
            foreach (var spawnInfo in wave.spawnInfos)
            {
                foreach (var monsterData in spawnInfo.monstersToSpawn)
                {
                    if (!objectPools.ContainsKey(monsterData.monsterPrefab))
                    {
                        objectPools[monsterData.monsterPrefab] = new List<GameObject>();
                        for (int i = 0; i < poolSize; i++)
                        {
                            GameObject instance = Instantiate(monsterData.monsterPrefab);
                            instance.SetActive(false);
                            objectPools[monsterData.monsterPrefab].Add(instance);
                        }
                    }
                }
            }
        }
    }

    public void StartWave()
    {
        if (currentWave < waves.Count)
        {
            foreach (var spawnInfo in waves[currentWave].spawnInfos)
            {
                Transform spawnPoint = spawnPoints[spawnInfo.spawnPointIndex];
                foreach (var monsterData in spawnInfo.monstersToSpawn)
                {
                    for (int i = 0; i < monsterData.count; i++)
                    {
                        SpawnMonster(monsterData.monsterPrefab, spawnPoint.position);
                    }
                }
            }
            currentWave++;
        }
    }

    void SpawnMonster(GameObject monsterPrefab, Vector3 position)
    {
        GameObject monsterToSpawn = null;

        foreach (var monster in objectPools[monsterPrefab])
        {
            if (!monster.activeInHierarchy)
            {
                monsterToSpawn = monster;
                break;
            }
        }

        if (monsterToSpawn != null)
        {
            monsterToSpawn.transform.position = position;
            monsterToSpawn.SetActive(true);
        }
    }
}

[System.Serializable]
public class MonsterSpawnData
{
    public GameObject monsterPrefab; // 소환될 몬스터의 프리팹
    public int count;   // 소환될 몬스터의 개수
}

[System.Serializable]
public class SpawnInfo
{
    public List<MonsterSpawnData> monstersToSpawn;   // 소환될 몬스터의 데이터
    public int spawnPointIndex;   // 스폰 포인트 index
}

[System.Serializable]
public class Wave
{
    public List<SpawnInfo> spawnInfos;   // 이 웨이브에서 소환될 몬스터의 정보
}
