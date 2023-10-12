using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loadout", menuName = "Scriptble Object/LoadoutData")]   // 커스텀 메뉴를 생성하는 속성
public class LoadoutData : ScriptableObject
{
    [Header("# Main Info")]
    public int weaponID;   // ID
    public string weaponName;   // 이름
    [TextArea]
    public string weaponDesc;   // 설명
    public Sprite weaponImage;   // 이미지
    public Sprite weaponIcon;   // 아이콘
}
