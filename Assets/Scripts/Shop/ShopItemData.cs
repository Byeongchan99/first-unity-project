using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptble Object/ShopItemData")]   // Ŀ���� �޴��� �����ϴ� �Ӽ�
public class ShopItemData : ScriptableObject
{
    [Header("# Main Info")]
    public int itemID;   // ID
    public string itemName;   // �̸�
    public int itemPrice;   // ����
    [TextArea]
    public string itemDesc;   // ����
    public Sprite itemImage;   // �̹���
}
