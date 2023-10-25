using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class Shop : MonoBehaviour
{
    RectTransform rect;
    public ShopItemData[] allShopItems;   // ���� ������ �迭
    List<ShopItemData> chosenItems;   // �������� ���õ� �����۵�

    // �������� ���� ���θ� �����ϴ� ��ųʸ�
    private Dictionary<int, bool> purchasedItems = new Dictionary<int, bool>();

    // ������ ������ â
    public GameObject itemDetailPanel;  // �� ���� �г�
    public Image itemDetailImage;
    public Text itemDetailName;
    public TextMeshProUGUI itemDetailPrice;
    public Text itemDetailDescription;
    public Button purchaseButton;   // �����ϱ� ��ư

    private ShopItemData currentItem;   // ���� ǥ�õ� ������ ����

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (itemDetailPanel != null)
            itemDetailPanel.SetActive(false);  // ������ â ��Ȱ��ȭ
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }

    // �������� ������ ǥ��
    public void DisplayRandomShopItems()
    {
        chosenItems = ChooseRandomItems(3);

        for (int i = 0; i < chosenItems.Count; i++)
        {
            UpdateItemUI(chosenItems[i], i);
        }
    }

    // ������ ��� �� �������� 3�� ���� �� ��ȯ
    private List<ShopItemData> ChooseRandomItems(int count)
    {
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < count && selectedIndices.Count < allShopItems.Length)
        {
            int randomIndex = Random.Range(0, allShopItems.Length);
            selectedIndices.Add(randomIndex);
        }

        List<ShopItemData> chosen = new List<ShopItemData>();
        foreach (int index in selectedIndices)
        {
            chosen.Add(allShopItems[index]);
        }

        // Debug.Log("���õ� �����Ƽ ����: " + chosen.Count);
        return chosen;
    }

    private void UpdateItemUI(ShopItemData item, int index)
    {
        Transform shopPanel = transform.Find("Shop Panel");

        if (shopPanel == null)
        {
            Debug.LogError("No Shop Panel found!");
            return;
        }

        // Shop Panel�� �ڽ� ������Ʈ�� ã���ϴ�.
        Transform itemUI = shopPanel.Find("Shop Item " + index);

        if (itemUI == null)
        {
            Debug.LogError("No UI display found for Item " + index);
            return;
        }

        // ������ ������ �Ҵ�
        itemUI.Find("Item Name").GetComponent<Text>().text = item.itemName;
        itemUI.Find("Item Price").GetComponent<Text>().text = item.itemPrice + "<color=#FFD700>G</color>";
        itemUI.Find("Item Image").GetComponent<Image>().sprite = item.itemImage;

        // ��ư ���� �� ������â ǥ�õ�
        Button itemDetailButton = itemUI.GetComponent<Button>();
        if (itemDetailButton != null)
        {
            // �����ʸ� ���� �����ϰ� ���� �߰� (���� �����ʰ� �������� �ʵ���)
            itemDetailButton.onClick.RemoveAllListeners();

            // ������ �߰�
            itemDetailButton.onClick.AddListener(() => DisplayItemDescription(item));
        }

        // �̹� ǰ���� ��
        if (IsItemPurchased(item.itemID))
        {
            // ������ �������� ���� ��ư ��Ȱ��ȭ -> ���߿� ǰ�� ��������Ʈ ǥ��
            itemUI.GetComponent<Button>().interactable = false;
        }
        else
        {
            itemUI.GetComponent<Button>().interactable = true;
        }
    }

    // ������ ������ â ���÷���
    public void DisplayItemDescription(ShopItemData currentItem)
    {
        if (currentItem != null)
        {
            itemDetailImage.sprite = currentItem.itemImage;
            itemDetailName.text = currentItem.itemName;
            itemDetailPrice.text = currentItem.itemPrice + "<color=#FFD700>G</color>";
            itemDetailDescription.text = currentItem.itemDesc;

            if (itemDetailPanel != null)
                itemDetailPanel.SetActive(true);  // �г��� ǥ��

            Button purchaseButton = itemDetailPanel.GetComponentInChildren<Button>();
            if (purchaseButton != null)
            {
                // �����ʸ� ���� �����ϰ� ���� �߰� (���� �����ʰ� �������� �ʵ���)
                purchaseButton.onClick.RemoveAllListeners();

                // ������ �߰�
                purchaseButton.onClick.AddListener(() => PurchaseItem(currentItem.itemID));
            }
        }
    }

    // ������ ID�� �´� �޼��� ���� �� ��� ����
    public void PurchaseItem(int itemID)
    {
        // �����Ϸ��� ������ ã��
        ShopItemData itemToPurchase = null;
        foreach (ShopItemData item in allShopItems)
        {
            if (item.itemID == itemID)
            {
                itemToPurchase = item;
                break;
            }
        }

        if (itemToPurchase == null)
        {
            Debug.LogError("Item with ID " + itemID + " not found!");
            return;
        }

        // ǰ���� ��
        if (IsItemPurchased(itemToPurchase.itemID))
        {
            Debug.LogWarning("Item already purchased!");
            return;
        }

        int playerGold = PlayerStat.Instance.Gold;

        if (playerGold >= itemToPurchase.itemPrice)
        {
            // ��� ����
            PlayerStat.Instance.Gold -= itemToPurchase.itemPrice;

            // ������ ȿ�� ����
            ConsumableItem consumableItem = GetComponent<ConsumableItem>();
            if (consumableItem != null)
            {
                consumableItem.ActivateItem(itemID);
            }
            else
            {
                Debug.LogError("Consumable Item not found on this GameObject!");
            }

            // �������� ���� ���·� ����
            SetItemPurchased(itemToPurchase.itemID);

            // ������ ��� ����
            RedisplayShopItems();
            // ���� �Ϸ� �޽��� �Ǵ� �ִϸ��̼� �߰� -> ������ ���� �Ϸ� ���
            // ��: "Item Purchased!" �޽��� ǥ��
        }
        else
        {
            Debug.Log("Not enough gold to purchase the item!");
            // ��尡 ������ ����� �޽��� �Ǵ� �ִϸ��̼� �߰� (�ɼ�)
        }
    }

    // ������ ���� ���θ� Ȯ���ϴ� �޼���
    public bool IsItemPurchased(int itemId)
    {
        return purchasedItems.ContainsKey(itemId) && purchasedItems[itemId];
    }

    // ������ ���� ���θ� �����ϴ� �޼���
    public void SetItemPurchased(int itemId)
    {
        purchasedItems[itemId] = true;
    }

    // ������ ���â ����
    public void RedisplayShopItems()
    {
        for (int i = 0; i < chosenItems.Count; i++)
        {
            UpdateItemUI(chosenItems[i], i);
        }
    }
}
