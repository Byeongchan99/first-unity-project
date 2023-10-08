using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptble Object/AbilityData")]   // Ŀ���� �޴��� �����ϴ� �Ӽ�
public class AbilityData : ScriptableObject
{
    [Header("# Main Info")]
    public int abilityID;   // ID
    public string abilityName;   // �̸�
    [TextArea]
    public string abilityDesc;   // ����
    public Sprite abilityImage;   // �̹���
}
