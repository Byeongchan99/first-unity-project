using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    RectTransform rect;
    public ShopItemData[] allShopItems;   // ���� ������ �迭

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
        List<ShopItemData> chosenItems = ChooseRandomItems(3);

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

        // �����Ƽ ������ �Ҵ�
        itemUI.Find("Item Name").GetComponent<Text>().text = item.itemName;
        itemUI.Find("Item Price").GetComponent<Text>().text = item.itemPrice + "<color=#FFD700>G</color>";
        itemUI.Find("Item Image").GetComponent<Image>().sprite = item.itemImage;

        // 
        Button itemDetailButton = itemUI.GetComponent<Button>();
        if (itemDetailButton != null)
        {
            // �����ʸ� ���� �����ϰ� ���� �߰� (���� �����ʰ� �������� �ʵ���)
            itemDetailButton.onClick.RemoveAllListeners();

            // ������ �߰�
            itemDetailButton.onClick.AddListener(() => DisplayItemDescription(item));
        }
    }

    public void DisplayItemDescription(ShopItemData currentItem)
    {
        if (currentItem != null)
        {
            itemDetailImage.sprite = currentItem.itemImage;
            itemDetailName.text = currentItem.itemName;
            itemDetailPrice.text = currentItem.itemPrice + "<color=#FFD700>G</color>";
            itemDetailDescription.text = currentItem.itemDesc;  // ���� ������ �����Ϳ� ���� ������ �߰��ؾ� �մϴ�.

            if (itemDetailPanel != null)
                itemDetailPanel.SetActive(true);  // �г��� ǥ���մϴ�.
        }
    }

    // ������ ID�� �´� �޼��� ���� �� ��� ����
    private void PurchaseItem(int id)
    {
        // ���� ���� - ��尡 ����ϸ� ��� ���� �� ������ ȿ�� ����, ������ �� ���� �Ұ�, ���� �Ŀ��� ǰ���Ǿ� �ٽ� ���� �Ұ�

        // ConsumableItem ��ũ��Ʈ�� �������� �ڵ�
        // ������ ȿ�� ����
        ConsumableItem ConsumableItem = GetComponent<ConsumableItem>();
        if (ConsumableItem != null)
        {
            ConsumableItem.ActivateItem(id);
        }
        else
        {
            Debug.LogError("Consumable Item not found on this GameObject!");
        }
    }
}
