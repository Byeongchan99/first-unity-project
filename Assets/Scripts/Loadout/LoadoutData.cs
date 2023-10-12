using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loadout", menuName = "Scriptble Object/LoadoutData")]   // Ŀ���� �޴��� �����ϴ� �Ӽ�
public class LoadoutData : ScriptableObject
{
    [Header("# Main Info")]
    public int weaponID;   // ID
    public string weaponName;   // �̸�
    [TextArea]
    public string weaponDesc;   // ����
    public Sprite weaponImage;   // �̹���
    public Sprite weaponIcon;   // ������
}
