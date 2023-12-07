using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageData ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "Stage", menuName = "Scriptble Object/StageData")]   // Ŀ���� �޴��� �����ϴ� �Ӽ�
public class StageData : ScriptableObject
{
    [Header("Stage Settings")]
    public string stageName;   // �������� �̸�
    public string stageType;   // �������� ����
    [Tooltip("�� ���������� ����")]
    public int stageID;
    public Node[,] NodeArray;   // �� ��� Ȯ��
    [Tooltip("�� ���������� ���̺� ����")]
    public List<WaveData> waves;   // �� ���������� ���̺� ����
    public GameObject stagePrefab;   // �������� ������Ʈ�� ������
    public Vector2 startPosition;   // �������� ���� ��ġ
    public Vector2Int bottomLeft, topRight;   // ���� Astar �˰����� ���� �� ũ�� ���� ��ǥ
}
