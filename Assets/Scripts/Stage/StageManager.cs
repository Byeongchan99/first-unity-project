using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public List<StageData> stages; // ��� ���������� ������ ����Ʈ
    public StageData currentStage; // ���� Ȱ��ȭ�� �������� ������
    private Dictionary<int, GameObject> stageInstances = new Dictionary<int, GameObject>(); // �������� �ν��Ͻ� ����
    // �������� �Ϸ� ���¸� �����ϴ� Dictionary
    public Dictionary<int, bool> completedStages;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            completedStages = new Dictionary<int, bool>();
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

    public Vector2 TilemapToWorldPosition(Vector2Int tilemapPos, Vector2Int bottomLeft, Vector2Int topRight)   // Ÿ�ϸ� ��ǥ�� ���� ��ǥ�� ��ȯ
    {
        // ���⼭ bottomLeft�� ����Ͽ� �ùٸ� ���� ��ǥ�� ����մϴ�.
        return new Vector2(tilemapPos.x * 0.5f + bottomLeft.x, tilemapPos.y * 0.5f + bottomLeft.y);
    }

    // NodeArray�� �ʱ�ȭ�ϴ� �޼���. ���� ���� �ó� ���� �ε�� �� �� ���� ȣ��
    public void InitializeNodeArray()
    {
        int sizeX, sizeY;   // �� ũ��
        Vector2Int bottomLeft, topRight;   // ���� �ϴ� ������ ��� ������ ���� ��ǥ

        bottomLeft = currentStage.bottomLeft;
        topRight = currentStage.topRight;

        sizeX = Mathf.Abs(topRight.x - bottomLeft.x) * 2;  // Ÿ�ϸ� ���� ũ��
        sizeY = Mathf.Abs(topRight.y - bottomLeft.y) * 2;  // Ÿ�ϸ� ���� ũ��

        currentStage.NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                Vector2 worldPosition = TilemapToWorldPosition(new Vector2Int(i, j), bottomLeft, topRight);
                foreach (Collider2D col in Physics2D.OverlapCircleAll(worldPosition, 0.1f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                currentStage.NodeArray[i, j] = new Node(isWall, i, j);
            }
        }
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
            Debug.Log("�̵��� ��������: " + currentStage.stageID);
            // �� �������� Ȱ��ȭ
            stageInstances[stageIndex].SetActive(true);
            InitializeNodeArray();   // NodeArray �ʱ�ȭ

            // �÷��̾ �� ���������� ���� ��ġ�� �̵�
            PlayerStat.Instance.transform.position = currentStage.startPosition;
           
            // ���� ���������� ��� ���� ��ȯ
            if (currentStage.stageType == "battle" || currentStage.stageType == "boss") 
                WaveManager.Instance.StartWave();
        }
    }

    // �������� �Ϸ� ���� ������Ʈ �޼���
    public void SetStageCompleted(int stageID, bool completed)
    {
        if (!completedStages.ContainsKey(stageID))
        {
            completedStages.Add(stageID, completed);
        }
        else
        {
            completedStages[stageID] = completed;
        }
    }

    // �������� �Ϸ� ���� Ȯ�� �޼���
    public bool IsStageCompleted(int stageID)
    {
        return completedStages.ContainsKey(stageID) && completedStages[stageID];
    }
}
