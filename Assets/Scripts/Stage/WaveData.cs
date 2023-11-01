using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptble Object/WaveData")]   // Ŀ���� �޴��� �����ϴ� �Ӽ�
public class WaveData : ScriptableObject
{
    [Header("���̺� ����")]
    [Tooltip("�� ���̺꿡�� ��ȯ�� ���� ����")]
    public List<SpawnInfo> spawnInfos; // �� ���̺꿡�� ��ȯ�� ������ ����

    [Tooltip("�� ���̺��� ���� ����Ʈ ��ġ ����")]
    public Vector2[] spawnPointsPositions; // �� ���̺��� ���� ����Ʈ ��ġ ����
}