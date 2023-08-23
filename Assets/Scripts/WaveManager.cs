using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<Wave> waves; // ���� ���̺� ����
    public Transform[] spawnPoints; // ���� ����Ʈ ��ġ ����
    private int currentWave = 0; // ���� ���̺�

    [Header("Object Pooling")]
    public int poolSize = 10; // �� ���� ���� �غ��� ������Ʈ ��
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
    public GameObject monsterPrefab; // ��ȯ�� ������ ������
    public int count;   // ��ȯ�� ������ ����
}

[System.Serializable]
public class SpawnInfo
{
    public List<MonsterSpawnData> monstersToSpawn;   // ��ȯ�� ������ ������
    public int spawnPointIndex;   // ���� ����Ʈ index
}

[System.Serializable]
public class Wave
{
    public List<SpawnInfo> spawnInfos;   // �� ���̺꿡�� ��ȯ�� ������ ����
}
