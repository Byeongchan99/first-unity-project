using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static WaveManager Instance { get; private set; }

    [Header("Wave Settings")]
    public List<Wave> waves;   // 여러 웨이브 정보
    public Transform[] spawnPoints;   // 스폰 포인트 위치 정보
    private int currentWave = 0;  // 현재 웨이브
    private int remainingMonsters; // 현재 웨이브의 남은 몬스터 수

    [Header("Object Pooling")]
    public int poolSize = 10; // 각 몬스터 별로 준비할 오브젝트 수
    private Dictionary<GameObject, List<GameObject>> objectPools;

    private void Awake()
    {
        // 싱글톤 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 바뀌어도 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 있다면 현재 인스턴스 파괴
        }
    }

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
            // 초기화
            remainingMonsters = 0;

            foreach (var spawnInfo in waves[currentWave].spawnInfos)
            {
                Transform spawnPoint = spawnPoints[spawnInfo.spawnPointIndex];
                foreach (var monsterData in spawnInfo.monstersToSpawn)
                {
                    for (int i = 0; i < monsterData.count; i++)
                    {
                        SpawnMonster(monsterData.monsterPrefab, spawnPoint.position);
                        remainingMonsters++;
                    }
                }
            }
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

    // 몬스터가 죽었을 때 호출되는 메서드
    public void OnMonsterDeath()
    {
        remainingMonsters--;

        if (remainingMonsters <= 0)
        {
            StartWave();  // 모든 몬스터가 죽었으면 다음 웨이브 시작
        }
    }
}

[System.Serializable]
public class MonsterSpawnData
{
    [Tooltip("몬스터 프리팹")]
    public GameObject monsterPrefab; // 소환될 몬스터의 프리팹

    [Tooltip("소환될 몬스터의 개수")]
    public int count;   // 소환될 몬스터의 개수
}

[System.Serializable]
public class SpawnInfo
{
    [Header("몬스터 소환 데이터")]
    [Tooltip("이 웨이브에서 소환될 몬스터의 데이터")]
    public List<MonsterSpawnData> monstersToSpawn;   // 소환될 몬스터의 데이터

    [Header("스폰 포인트 설정")]
    [Tooltip("몬스터가 소환될 스폰 포인트의 인덱스")]
    public int spawnPointIndex;   // 스폰 포인트 index
}

[System.Serializable]
public class Wave
{
    [Header("웨이브 설정")]
    [Tooltip("각 웨이브에서 소환될 몬스터와 스폰 포인트 정보")]
    public List<SpawnInfo> spawnInfos;   // 이 웨이브에서 소환될 몬스터의 정보
}

