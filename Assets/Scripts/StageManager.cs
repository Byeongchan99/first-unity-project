using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public string stageName; // �������� �̸�
        public Transform stageTransform; // �������� ������Ʈ�� Transform
        public Vector3 startPosition; // �������� ���� ��ġ
    }

    public static StageManager Instance { get; private set; }

    public List<Stage> stages; // ��� ���������� ����Ʈ
    private Stage currentStage; // ���� Ȱ��ȭ�� ��������

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

    // ���������� �̵��ϴ� �޼���
    public void TransitionToStage(int stageIndex)
    {
        if (currentStage != null)
        {
            currentStage.stageTransform.gameObject.SetActive(false); // ���� �������� ��Ȱ��ȭ
        }

        currentStage = stages[stageIndex];
        currentStage.stageTransform.gameObject.SetActive(true); // �� �������� Ȱ��ȭ                                                           
        PlayerStat.Instance.transform.position = currentStage.startPosition;   // �÷��̾ �� ���������� ���� ��ġ�� �̵�
    }
}