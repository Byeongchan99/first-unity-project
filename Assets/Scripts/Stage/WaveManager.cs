using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static WaveManager Instance { get; private set; }

    [Header("Stage Settings")]
    public List<StageData> stages;  // �������� ������
    private int currentStage = 0;  // ���� ��������

    private int currentWave = 0;  // ���� ���̺�
    private int remainingMonsters; // ���� ���̺��� ���� ���� ��

    [Header("Object Pooling")]
    public int poolSize = 10; // �� ���� ���� �غ��� ������Ʈ ��
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

        // MonstersContainer ���� �� ����
        GameObject monstersContainer = new GameObject("MonstersContainer");
        monstersContainer.transform.SetParent(this.transform);

        foreach (var stage in stages)
        {
            foreach (var wave in stage.waves)
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
                                instance.transform.SetParent(monstersContainer.transform);
                                instance.SetActive(false);
                                objectPools[monsterData.monsterPrefab].Add(instance);
                            }
                        }
                    }
                }
            }
        }
    }

    // ���������� ������ �� ȣ��
    public void ChangeStage(int stageIndex)
    {
        if (stageIndex >= stages.Count)
        {
            Debug.LogWarning("Invalid stage index!");
            return;
        }

        currentStage = stageIndex;
        currentWave = 0;
        StartWave();
    }

    // ���̺� ���� �޼���
    public void StartWave()
    {
        // ���� ���������� ���̺��� �����͸� �����ɴϴ�.
        StageData currentStageData = stages[currentStage];
        if (currentWave >= currentStageData.waves.Count)
        {
            Debug.LogWarning("No more waves in the current stage!");
            return;
        }

        WaveData currentWaveData = currentStageData.waves[currentWave]; // ��ũ���ͺ� ������Ʈ ���

        Vector2[] currentSpawnPointsPositions = currentWaveData.spawnPointsPositions; // ���� ����Ʈ ��ġ ������ ���� ���̺� �����Ϳ��� ������

        // �ʱ�ȭ
        remainingMonsters = 0;

        foreach (var spawnInfo in currentWaveData.spawnInfos)
        {
            Vector3 spawnPointPosition = new Vector3(currentSpawnPointsPositions[spawnInfo.spawnPointIndex].x, currentSpawnPointsPositions[spawnInfo.spawnPointIndex].y, 0); // Vector2�� Vector3�� ��ȯ
            foreach (var monsterData in spawnInfo.monstersToSpawn)
            {
                for (int i = 0; i < monsterData.count; i++)
                {
                    SpawnMonster(monsterData.monsterPrefab, spawnPointPosition);
                    remainingMonsters++;
                }
            }
        }

        currentWave++;  // ���̺� ���� �� currentWave ���� ������ŵ�ϴ�.
    }

    // ���� ��ȯ
    void SpawnMonster(GameObject monsterPrefab, Vector3 spawnPointPosition)
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
            monsterToSpawn.SetActive(true);
        }
    }

    // ���Ͱ� �׾��� �� ȣ��Ǵ� �޼���
    public void OnMonsterDeath()
    {
        remainingMonsters--;

        if (remainingMonsters <= 0)
        {
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

[System.Serializable]
public class Wave
{
    [Header("���̺� ����")]
    [Tooltip("�� ���̺꿡�� ��ȯ�� ���� ����")]
    public List<SpawnInfo> spawnInfos; // �� ���̺꿡�� ��ȯ�� ������ ����
}