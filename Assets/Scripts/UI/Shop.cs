using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    RectTransform rect;
    public ShopItemData[] allShopItems;   // 상점 아이템 배열

    // 아이템 상세정보 창
    public GameObject itemDetailPanel;  // 상세 정보 패널
    public Image itemDetailImage;
    public Text itemDetailName;
    public TextMeshProUGUI itemDetailPrice;
    public Text itemDetailDescription;
    public Button purchaseButton;   // 구매하기 버튼

    private ShopItemData currentItem;   // 현재 표시된 아이템 정보

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (itemDetailPanel != null)
            itemDetailPanel.SetActive(false);  // 상세정보 창 비활성화
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

    // 랜덤으로 아이템 표시
    public void DisplayRandomShopItems()
    {
        List<ShopItemData> chosenItems = ChooseRandomItems(3);

        for (int i = 0; i < chosenItems.Count; i++)
        {
            UpdateItemUI(chosenItems[i], i);
        }
    }

    // 아이템 목록 중 랜덤으로 3개 선택 후 반환
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

        // Debug.Log("선택된 어빌리티 개수: " + chosen.Count);
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

        // Shop Panel의 자식 오브젝트를 찾습니다.
        Transform itemUI = shopPanel.Find("Shop Item " + index);

        if (itemUI == null)
        {
            Debug.LogError("No UI display found for Item " + index);
            return;
        }

        // 어빌리티 데이터 할당
        itemUI.Find("Item Name").GetComponent<Text>().text = item.itemName;
        itemUI.Find("Item Price").GetComponent<Text>().text = item.itemPrice + "<color=#FFD700>G</color>";
        itemUI.Find("Item Image").GetComponent<Image>().sprite = item.itemImage;

        // 
        Button itemDetailButton = itemUI.GetComponent<Button>();
        if (itemDetailButton != null)
        {
            // 리스너를 먼저 제거하고 새로 추가 (이전 리스너가 남아있지 않도록)
            itemDetailButton.onClick.RemoveAllListeners();

            // 리스너 추가
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
            itemDetailDescription.text = currentItem.itemDesc;  // 상점 아이템 데이터에 설명 변수를 추가해야 합니다.

            if (itemDetailPanel != null)
                itemDetailPanel.SetActive(true);  // 패널을 표시합니다.
        }
    }

    // 아이템 ID에 맞는 메서드 실행 및 골드 감소
    private void PurchaseItem(int id)
    {
        // 구매 로직 - 골드가 충분하면 골드 차감 후 아이템 효과 실행, 부족할 시 구매 불가, 구매 후에는 품절되어 다시 구매 불가

        // ConsumableItem 스크립트를 가져오는 코드
        // 아이템 효과 실행
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
