using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static WaveManager Instance { get; private set; }

    [Header("Wave Settings")]
    public List<Wave> waves;   // ���� ���̺� ����
    public Transform[] spawnPoints;   // ���� ����Ʈ ��ġ ����
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
            // �ʱ�ȭ
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

    // ���Ͱ� �׾��� �� ȣ��Ǵ� �޼���
    public void OnMonsterDeath()
    {
        remainingMonsters--;

        if (remainingMonsters <= 0)
        {
            StartWave();  // ��� ���Ͱ� �׾����� ���� ���̺� ����
        }
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
    [Tooltip("�� ���̺꿡�� ��ȯ�� ���Ϳ� ���� ����Ʈ ����")]
    public List<SpawnInfo> spawnInfos;   // �� ���̺꿡�� ��ȯ�� ������ ����
}

