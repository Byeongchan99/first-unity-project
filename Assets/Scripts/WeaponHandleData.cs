using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ƽ ��� Asset �޴��� Scriptable Object �޴� ����
[CreateAssetMenu(fileName = "Weapon Handle Data", menuName = "Scriptable Object/Weapon Handle Data", order = int.MaxValue)]
public class WeaponHandleData : ScriptableObject
{
    public Vector2 localPosition;
    public Vector2 localRotation;
    public Vector2 localScale;
}
