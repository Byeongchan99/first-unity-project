using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public List<StageData> stages; // ��� ���������� ������ ����Ʈ
    private StageData currentStage; // ���� Ȱ��ȭ�� �������� ������
    private Dictionary<int, GameObject> stageInstances = new Dictionary<int, GameObject>(); // �������� �ν��Ͻ� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���� ���������� �����ϰ� ��Ȱ��ȭ
    private void Start()
    {
        // ��� �������� �ν��Ͻ� ���� �� ��Ȱ��ȭ
        for (int i = 0; i < stages.Count; i++)
        {
            if (stages[i] != null)
            {
                GameObject stageInstance = Instantiate(stages[i].stagePrefab);
                stageInstance.SetActive(i == 0); // ù ��° ���������� Ȱ��ȭ
                stageInstances.Add(i, stageInstance); // �ν��Ͻ� ����
            }
        }

        // ù ��° ���������� ���� ���������� ����
        currentStage = stages[0];
    }


    // ���������� �̵��ϴ� �޼���
    public void TransitionToStage(int stageIndex)
    {
        if (stageInstances.ContainsKey(stageIndex))
        {
            if (currentStage != null)
            {
                // ���� �������� ��Ȱ��ȭ
                stageInstances[currentStage.stageID].SetActive(false);
            }

            currentStage = stages[stageIndex];
            // �� �������� Ȱ��ȭ
            stageInstances[stageIndex].SetActive(true);

            // �÷��̾ �� ���������� ���� ��ġ�� �̵�
            PlayerStat.Instance.transform.position = currentStage.startPosition;
        }
    }
}
