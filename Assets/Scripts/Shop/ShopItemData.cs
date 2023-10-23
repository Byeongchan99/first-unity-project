using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptble Object/ShopItemData")]   // 커스텀 메뉴를 생성하는 속성
public class ShopItemData : ScriptableObject
{
    [Header("# Main Info")]
    public int itemID;   // ID
    public string itemName;   // 이름
    public int itemPrice;   // 가격
    [TextArea]
    public string itemDesc;   // 설명
    public Sprite itemImage;   // 이미지
}
