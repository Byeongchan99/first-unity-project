using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static WaveManager Instance { get; private set; }

    [Header("Wave Settings")]
    public List<Wave> waves;   // 여러 웨이브 정보
    public GameObject[] spawnPoints;   // 스폰 포인트 위치 정보   // 스폰 포인트 위치 정보
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

        // MonstersContainer 생성 및 설정
        GameObject monstersContainer = new GameObject("MonstersContainer");
        monstersContainer.transform.SetParent(this.transform);

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
                            instance.transform.SetParent(monstersContainer.transform); // 여기를 변경
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
        // 웨이브의 최대 수를 초과하면 시작하지 않습니다.
        if (currentWave >= waves.Count)
        {
            return;
        }

        // 초기화
        remainingMonsters = 0;

        foreach (var spawnInfo in waves[currentWave].spawnInfos)
        {
            GameObject spawnPoint = spawnPoints[spawnInfo.spawnPointIndex];
            foreach (var monsterData in spawnInfo.monstersToSpawn)
            {
                for (int i = 0; i < monsterData.count; i++)
                {
                    SpawnMonster(monsterData.monsterPrefab, spawnPoint);
                    remainingMonsters++;
                }
            }
        }

        currentWave++;  // 웨이브 시작 후 currentWave 값을 증가시킵니다.
    }

    void SpawnMonster(GameObject monsterPrefab, GameObject spawnPointObj)
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
            monsterToSpawn.transform.position = spawnPointObj.transform.position;
            monsterToSpawn.transform.SetParent(spawnPointObj.transform);  // 몬스터의 부모를 해당 SpawnPoint로 설정
            monsterToSpawn.SetActive(true);
        }
    }

    // 몬스터가 죽었을 때 호출되는 메서드
    public void OnMonsterDeath()
    {
        remainingMonsters--;

        if (remainingMonsters <= 0)
        {
            StartCoroutine(StartNextWaveWithDelay(2.0f));  // 2초 후에 다음 웨이브 시작
        }
    }

    // 일정 시간 대기 후 웨이브 시작
    IEnumerator StartNextWaveWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartWave();
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

