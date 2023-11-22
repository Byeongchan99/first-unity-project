using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static WaveManager Instance { get; private set; }

    private int currentStageID;  // ���� �������� ID
    private int currentWave = 0;  // ���� ���̺� ID
    private int remainingMonsters; // ���� ���̺��� ���� ���� ��

    [System.Serializable]
    public class MonsterPool
    {
        public GameObject monsterPrefab;
        public int poolSize;
    }

    [Header("Object Pooling")]
    public MonsterPool[] monsterPools;

    // ������Ʈ Ǯ���� ���� Dictionary
    private Dictionary<GameObject, List<GameObject>> objectPools;

    private void Awake()
    {
        // �̱��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // ���� �ٲ� �ı����� �ʰ� ����
        }
        else
        {
            Destroy(gameObject);  // �̹� �ν��Ͻ��� �ִٸ� ���� �ν��Ͻ� �ı�
        }
    }

    private void Start()
    {
        InitializeObjectPools();
    }

    // ������Ʈ Ǯ�� �ʱ�ȭ
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

                // �ʿ��� �ʱ�ȭ �ڵ�...

                objectPools[prefab].Add(newMonster);
            }
        }
    }

    // ���̺� ���� �޼���
    public void StartWave()
    {
        // ���� ���������� ���̺��� �����͸� �����ɴϴ�.
        StageData currentStageData = StageManager.Instance.currentStage;
        currentStageID = currentStageData.stageID;

        // �ش� ���������� �̹� �Ϸ�Ǿ��ٸ� �� �̻� ���̺긦 �������� ����
        if (StageManager.Instance.IsStageCompleted(currentStageID))
        {
            Debug.LogWarning("This stage is already completed!");
            return;
        }     

        WaveData currentWaveData = currentStageData.waves[currentWave]; // ��ũ���ͺ� ������Ʈ ���

        Vector2[] currentSpawnPointsPositions = currentWaveData.spawnPointsPositions; // ���� ����Ʈ ��ġ ������ ���� ���̺� �����Ϳ��� ������

        // �ʱ�ȭ
        remainingMonsters = 0;
        GameManager.instance.isBattle = true;

        foreach (var spawnInfo in currentWaveData.spawnInfos)
        {
            Vector2 spawnPointPosition = new Vector2(currentSpawnPointsPositions[spawnInfo.spawnPointIndex].x + currentStageData.startPosition.x, currentSpawnPointsPositions[spawnInfo.spawnPointIndex].y + currentStageData.startPosition.y);
            foreach (var monsterData in spawnInfo.monstersToSpawn)
            {
                for (int i = 0; i < monsterData.count; i++)
                {
                    // �������� Ÿ�Կ� ���� ���� ��ȯ
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

        currentWave++;  // ���̺� ���� �� currentWave ���� ������ŵ�ϴ�.

        // ������ ���̺� Ŭ���� �� �������� �Ϸ� ���� ������Ʈ
        if (currentWave >= currentStageData.waves.Count)
        {
            StageManager.Instance.SetStageCompleted(currentStageID, true); // �������� �Ϸ� ���� ������Ʈ
            Debug.LogWarning("No more waves in the current stage!");
            currentWave = 0;   // �� ���������� ���̺갡 ��� �����ٸ� 0���� �ʱ�ȭ
            return;
        }
    }

    // �Ϲ� ���� ��ȯ
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
            monsterToSpawn.transform.position = spawnPointPosition; // ���� ����Ʈ ��ġ ����
            MonsterBase monsterComponent = monsterToSpawn.GetComponent<MonsterBase>();
            Astar Astar = monsterToSpawn.GetComponent<Astar>();
            if (monsterComponent != null)
            {
                monsterToSpawn.SetActive(true);
                Astar.Initialize(StageManager.Instance.currentStage);   // Astar �˰��� �� ������ ������Ʈ
                monsterComponent.ActivateMonster(); // �߰� �ʱ�ȭ�� ������ �ʿ��� ���
            }
        }
    }

    // ���� ���� ��ȯ
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
            monsterToSpawn.transform.position = spawnPointPosition;   // ���� ����Ʈ ��ġ ����
            BossMonster bossComponent = monsterToSpawn.GetComponent<BossMonster>();
            if (bossComponent != null)
            {
                monsterToSpawn.SetActive(true);
                bossComponent.ActivateBossMonster(); // �߰� �ʱ�ȭ�� ������ �ʿ��� ���
            }        
        }
    }

    // ���Ͱ� �׾��� �� ȣ��Ǵ� �޼���
    public void OnMonsterDeath()
    {
        remainingMonsters--;

        if (remainingMonsters <= 0)   // ���� ���Ͱ� ���ٸ�
        {
            GameManager.instance.isBattle = false;
            StartCoroutine(StartNextWaveWithDelay(2.0f));  // 2�� �Ŀ� ���� ���̺� ����
        }
    }

    // ���� �ð� ��� �� ���̺� ����
    IEnumerator StartNextWaveWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartWave();
    }
}

[System.Serializable]
public class MonsterSpawnData
{
    [Tooltip("���� ������")]
    public GameObject monsterPrefab; // ��ȯ�� ������ ������

    [Tooltip("��ȯ�� ������ ����")]
    public int count;   // ��ȯ�� ������ ����
}

[System.Serializable]
public class SpawnInfo
{
    [Header("���� ��ȯ ������")]
    [Tooltip("�� ���̺꿡�� ��ȯ�� ������ ������")]
    public List<MonsterSpawnData> monstersToSpawn;   // ��ȯ�� ������ ������

    [Header("���� ����Ʈ ����")]
    [Tooltip("���Ͱ� ��ȯ�� ���� ����Ʈ�� �ε���")]
    public int spawnPointIndex;   // ���� ����Ʈ index
}