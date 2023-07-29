using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유니티 상단 Asset 메뉴에 Scriptable Object 메뉴 생성
[CreateAssetMenu(fileName = "Weapon Handle Data", menuName = "Scriptable Object/Weapon Handle Data", order = int.MaxValue)]
public class WeaponHandleData : ScriptableObject
{
    public Vector2 localPosition;
    public Vector2 localRotation;
    public Vector2 localScale;
}
