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
        public Vector3 startPosition; // �������� ���� ��ġ(��Ż ��ġ)
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

        // �������� Transform �������� ���ǵ� ���� ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 worldStartPosition = currentStage.stageTransform.TransformPoint(currentStage.startPosition);

        // �÷��̾ �� ���������� ���� ��ġ�� �̵�
        PlayerStat.Instance.transform.position = worldStartPosition;
    }

}