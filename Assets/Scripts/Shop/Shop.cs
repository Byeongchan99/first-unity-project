using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class Shop : MonoBehaviour
{
    RectTransform rect;
    public ShopItemData[] allShopItems;   // 상점 아이템 배열
    List<ShopItemData> chosenItems;   // 랜덤으로 선택된 아이템들

    // 아이템의 구매 여부를 추적하는 딕셔너리
    private Dictionary<int, bool> purchasedItems = new Dictionary<int, bool>();

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
        chosenItems = ChooseRandomItems(3);

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

        // 아이템 데이터 할당
        itemUI.Find("Item Name").GetComponent<Text>().text = item.itemName;
        itemUI.Find("Item Price").GetComponent<Text>().text = item.itemPrice + "<color=#FFD700>G</color>";
        itemUI.Find("Item Image").GetComponent<Image>().sprite = item.itemImage;

        // 버튼 누를 시 상세정보창 표시됨
        Button itemDetailButton = itemUI.GetComponent<Button>();
        if (itemDetailButton != null)
        {
            // 리스너를 먼저 제거하고 새로 추가 (이전 리스너가 남아있지 않도록)
            itemDetailButton.onClick.RemoveAllListeners();

            // 리스너 추가
            itemDetailButton.onClick.AddListener(() => DisplayItemDescription(item));
        }

        // 이미 품절일 때
        if (IsItemPurchased(item.itemID))
        {
            // 구매한 아이템의 구매 버튼 비활성화 -> 나중에 품절 스프라이트 표시
            itemUI.GetComponent<Button>().interactable = false;
        }
        else
        {
            itemUI.GetComponent<Button>().interactable = true;
        }
    }

    // 아이템 상세정보 창 디스플레이
    public void DisplayItemDescription(ShopItemData currentItem)
    {
        if (currentItem != null)
        {
            itemDetailImage.sprite = currentItem.itemImage;
            itemDetailName.text = currentItem.itemName;
            itemDetailPrice.text = currentItem.itemPrice + "<color=#FFD700>G</color>";
            itemDetailDescription.text = currentItem.itemDesc;

            if (itemDetailPanel != null)
                itemDetailPanel.SetActive(true);  // 패널을 표시

            Button purchaseButton = itemDetailPanel.GetComponentInChildren<Button>();
            if (purchaseButton != null)
            {
                // 리스너를 먼저 제거하고 새로 추가 (이전 리스너가 남아있지 않도록)
                purchaseButton.onClick.RemoveAllListeners();

                // 리스너 추가
                purchaseButton.onClick.AddListener(() => PurchaseItem(currentItem.itemID));
            }
        }
    }

    // 아이템 ID에 맞는 메서드 실행 및 골드 감소
    public void PurchaseItem(int itemID)
    {
        // 구매하려는 아이템 찾기
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

        // 품절일 때
        if (IsItemPurchased(itemToPurchase.itemID))
        {
            Debug.LogWarning("Item already purchased!");
            return;
        }

        int playerGold = PlayerStat.Instance.Gold;

        if (playerGold >= itemToPurchase.itemPrice)
        {
            // 골드 차감
            PlayerStat.Instance.Gold -= itemToPurchase.itemPrice;

            // 아이템 효과 실행
            ConsumableItem consumableItem = GetComponent<ConsumableItem>();
            if (consumableItem != null)
            {
                consumableItem.ActivateItem(itemID);
            }
            else
            {
                Debug.LogError("Consumable Item not found on this GameObject!");
            }

            // 아이템을 구매 상태로 설정
            SetItemPurchased(itemToPurchase.itemID);

            // 아이템 목록 갱신
            RedisplayShopItems();
            // 구매 완료 메시지 또는 애니메이션 추가 -> 상인의 구매 완료 대사
            // 예: "Item Purchased!" 메시지 표시
        }
        else
        {
            Debug.Log("Not enough gold to purchase the item!");
            // 골드가 부족한 경우의 메시지 또는 애니메이션 추가 (옵션)
        }
    }

    // 아이템 구매 여부를 확인하는 메서드
    public bool IsItemPurchased(int itemId)
    {
        return purchasedItems.ContainsKey(itemId) && purchasedItems[itemId];
    }

    // 아이템 구매 여부를 설정하는 메서드
    public void SetItemPurchased(int itemId)
    {
        purchasedItems[itemId] = true;
    }

    // 아이템 목록창 갱신
    public void RedisplayShopItems()
    {
        for (int i = 0; i < chosenItems.Count; i++)
        {
            UpdateItemUI(chosenItems[i], i);
        }
    }
}
