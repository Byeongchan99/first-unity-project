using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageData ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "Stage", menuName = "Scriptble Object/StageData")]   // Ŀ���� �޴��� �����ϴ� �Ӽ�
public class StageData : ScriptableObject
{
    [Header("Stage Settings")]
    public string stageName; // �������� �̸�
    [Tooltip("�� ���������� ID")]
    public int stageID;
    [Tooltip("�� ���������� ���̺� ����")]
    public List<WaveData> waves; // �� ���������� ���̺� ����
    public GameObject stagePrefab; // �������� ������Ʈ�� ������
    public Vector3 startPosition; // �������� ���� ��ġ
}
