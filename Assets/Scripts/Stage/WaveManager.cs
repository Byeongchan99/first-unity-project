using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static WaveManager Instance { get; private set; }

    private int currentStageID;  // 현재 스테이지 ID
    private int currentWave = 0;  // 현재 웨이브 ID
    private int remainingMonsters; // 현재 웨이브의 남은 몬스터 수

    protected GameObject monsterSpawnEffect;   // 몬스터 소환 이펙트

    [System.Serializable]
    public class MonsterPool
    {
        public GameObject monsterPrefab;
        public int poolSize;
    }

    [Header("Object Pooling")]
    public MonsterPool[] monsterPools;

    // 오브젝트 풀링을 위한 Dictionary
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

    public void DestroyInstance()
    {
        Destroy(gameObject);
        Instance = null;
    }

    private void Start()
    {
        InitializeObjectPools();
    }

    // 오브젝트 풀링 초기화
    void InitializeObjectPools()
    {
        objectPools = new Dictionary<GameObject, List<GameObject>>();

        GameObject monstersContainer = new GameObject("MonstersContainer");
        monstersContainer.transform.SetParent(this.transform);

        foreach (var monsterPool in monsterPools)
        {
            GameObject prefab = monsterPool.monsterPrefab;
            int poolSize = monsterPool.poolSize;

            objectPools[prefab] = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newMonster = Instantiate(prefab);
                newMonster.transform.SetParent(monstersContainer.transform);
                newMonster.SetActive(false);

                // 필요한 초기화 코드...

                objectPools[prefab].Add(newMonster);
            }
        }
    }

    // 웨이브 시작 메서드
    public void StartWave()
    {
        // 현재 스테이지와 웨이브의 데이터를 가져옵니다.
        StageData currentStageData = StageManager.Instance.currentStage;
        currentStageID = currentStageData.stageID;

        // 해당 스테이지가 이미 완료되었다면 더 이상 웨이브를 시작하지 않음
        if (StageManager.Instance.IsStageCompleted(currentStageID) || (currentWave >= currentStageData.waves.Count))
        {
            Debug.LogWarning("This stage is already completed!");
            return;
        }     

        WaveData currentWaveData = currentStageData.waves[currentWave]; // 스크립터블 오브젝트 사용

        Vector2[] currentSpawnPointsPositions = currentWaveData.spawnPointsPositions; // 스폰 포인트 위치 정보를 현재 웨이브 데이터에서 가져옴

        // 초기화
        remainingMonsters = 0;
        GameManager.instance.isBattle = true;

        foreach (var spawnInfo in currentWaveData.spawnInfos)
        {
            // Vector2 spawnPointPosition = new Vector2(currentSpawnPointsPositions[spawnInfo.spawnPointIndex].x + currentStageData.startPosition.x, currentSpawnPointsPositions[spawnInfo.spawnPointIndex].y + currentStageData.startPosition.y);
            Vector2 spawnPointPosition = new Vector2(currentSpawnPointsPositions[spawnInfo.spawnPointIndex].x, currentSpawnPointsPositions[spawnInfo.spawnPointIndex].y);
            foreach (var monsterData in spawnInfo.monstersToSpawn)
            {
                for (int i = 0; i < monsterData.count; i++)
                {
                    // 스테이지 타입에 따라 몬스터 소환
                    if (currentStageData.stageType == "battle")
                    {
                        SpawnMonster(monsterData.monsterPrefab, spawnPointPosition);
                    }
                    else if (currentStageData.stageType == "boss")
                    {
                        SpawnBoss(monsterData.monsterPrefab, spawnPointPosition);
                    }
                    remainingMonsters++;
                }
            }
        }
    }

    // 페이드인 효과
    IEnumerator FadeIn(GameObject monster)
    {
        SpriteRenderer renderer = monster.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            // 투명도를 0에서 1까지 점진적으로 변경
            for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime / 1f) // 1초 동안 페이드인
            {
                Color newColor = renderer.color;
                newColor.a = alpha;
                renderer.color = newColor;
                yield return null; // 다음 프레임까지 기다림
            }
        }
    }

    // 몬스터 소환 이펙트
    IEnumerator DeactivateAfterSeconds(GameObject objectToDeactivate, float seconds)
    {
        yield return new WaitForSeconds(seconds); // 지정된 시간만큼 기다림
        objectToDeactivate.SetActive(false); // 객체 비활성화
    }

    public void DeactivateSpawnEffect(GameObject effect)
    {
        // 몬스터 소환 애니메이션 시간
        StartCoroutine(DeactivateAfterSeconds(effect, 0.834f));
    }

    // 몬스터 대기
    IEnumerator StopMonsterMoment(GameObject monster, MonsterBase monsterComponent)
    {
        monsterComponent.rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정  
        yield return new WaitForSeconds(0.5f);
        monsterComponent.rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제
    }


    // 일반 몬스터 소환
    void SpawnMonster(GameObject monsterPrefab, Vector2 spawnPointPosition)
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
            monsterToSpawn.transform.position = spawnPointPosition; // 스폰 포인트 위치 설정
            MonsterBase monsterComponent = monsterToSpawn.GetComponent<MonsterBase>();
            Astar Astar = monsterToSpawn.GetComponent<Astar>();
            if (monsterComponent != null)
            {
                AudioManager.Instance.PlaySound(7);
                GameObject monsterSpawnEffect = GameManager.instance.pool.Get(4);   // 몬스터 소환 애니메이션 활성화
                monsterSpawnEffect.transform.position = spawnPointPosition;
                DeactivateSpawnEffect(monsterSpawnEffect); // 몬스터 소환 애니메이션 비활성화 코루틴 호출

                // 일반 몬스터 소환
                monsterToSpawn.SetActive(true);
                Astar.Initialize(StageManager.Instance.currentStage);   // Astar 알고리즘에 맵 데이터 업데이트
                monsterComponent.ActivateMonster(); // 추가 초기화나 설정이 필요한 경우
                StartCoroutine(StopMonsterMoment(monsterToSpawn, monsterComponent));   // 몬스터 약간 대기

                // 페이드인 효과 시작
                StartCoroutine(FadeIn(monsterToSpawn));
            }
        }
    }

    // 보스 몬스터 소환
    void SpawnBoss(GameObject monsterPrefab, Vector2 spawnPointPosition)
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
            Debug.Log("spwan point position: " + spawnPointPosition + "");
            monsterToSpawn.transform.position = spawnPointPosition;   // 스폰 포인트 위치 설정
            BossMonster bossComponent = monsterToSpawn.GetComponent<BossMonster>();
            if (bossComponent != null)
            {
                monsterToSpawn.SetActive(true);
                bossComponent.ActivateBossMonster(); // 추가 초기화나 설정이 필요한 경우

                // BossHPBar 스크립트에 보스 몬스터 참조 전달
                BossHPBar.Instance.SetBossMonster(bossComponent);
            }        
        }
    }

    // 몬스터가 죽었을 때 호출되는 메서드
    public void OnMonsterDeath()
    {
        Debug.Log("OnMonsterDeath 호출");
        remainingMonsters--;
        PlayerStat.Instance.Kill++;   // 플레이어 킬 수 증가

        if (remainingMonsters <= 0)   // 남은 몬스터가 없다면
        {
            GameManager.instance.isBattle = false;
            StageData currentStageData = StageManager.Instance.currentStage;
            currentWave++;    // 웨이브 시작 전에 currentWave 값을 증가시킵니다.
            
            if (currentWave >= currentStageData.waves.Count)
            {
                // 마지막 웨이브 클리어 후 스테이지 완료 상태 업데이트
                StageManager.Instance.SetStageCompleted(currentStageID, true);
                Debug.LogWarning("Stage completed!");
                currentWave = 0;   // 한 스테이지의 웨이브가 모두 끝났다면 0으로 초기화
            }
            else
            {
                StartCoroutine(StartNextWaveWithDelay(2.0f));  // 2초 후에 다음 웨이브 시작
            }
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