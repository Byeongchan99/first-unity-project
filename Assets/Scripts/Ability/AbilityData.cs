using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptble Object/AbilityData")]   // 커스텀 메뉴를 생성하는 속성
public class AbilityData : ScriptableObject
{
    [Header("# Main Info")]
    public int abilityID;   // ID
    public string abilityName;   // 이름
    [TextArea]
    public string abilityDesc;   // 설명
    public Sprite abilityImage;   // 이미지
}
