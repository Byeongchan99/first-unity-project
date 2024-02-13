using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    RectTransform rect;
    public ShopItemData[] allShopItems;   // ���� ������ �迭
    List<ShopItemData> chosenItems;   // �������� ���õ� �����۵�
    public Button exitButton;   // ���� ������ ��ư

    // �������� ���� ���θ� �����ϴ� ��ųʸ�
    private Dictionary<int, bool> purchasedItems = new Dictionary<int, bool>();

    // ������ ������ â
    public GameObject itemDetailPanel;  // �� ���� �г�
    public Image itemDetailImage;
    public Text itemDetailName;
    public TextMeshProUGUI itemDetailPrice;
    public Text itemDetailDescription;
    public Button purchaseButton;   // �����ϱ� ��ư
    public Button closeButtonItem;   // �ݱ� ��ư

    // ���� ǥ�õ� ������ ����
    private ShopItemData currentItem;

    // �����Ƽ ���� ��ư
    public Button AbilityButton;

    // �����Ƽ ���� Ȯ�� �г�
    public GameObject purchaseConfirmationPanel;
    public AbilityChoice abilityChoice;
    public Button closeButtonAbility;   // �ݱ� ��ư

    // ���� �ִϸ�����
    public Animator merchantAnimator;   

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (itemDetailPanel != null)
            itemDetailPanel.SetActive(false);  // ������ â ��Ȱ��ȭ       

        // ���� ������ ��ư
        if (exitButton != null)
        {
            // ��ư�� ������ ���, Onclick �̺�Ʈ�� Hide �޼��带 �����մϴ�.
            exitButton.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));
        }

        // �����Ƽ ���� ��ư
        if (AbilityButton != null)
        {
            AbilityButton.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));
        }
    }

    void OnEnable()
    {
        if (UIManager.instance != null)   // UI �Ŵ����� ���� ����
        {
            UIManager.instance.SetShopUI(this);
            DisplayRandomShopItems();   // ������ ����� �� ���� ��ǰ �ʱ�ȭ
        }
        else
        {
            Debug.LogError("UIManager instance not found");
        }
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        GameManager.instance.isLive = false;
        // GameManager.instance.Stop();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.isLive = true;
        // GameManager.instance.Resume();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }

    public void EnterShop()
    {
        // ���� ��ȭ �ִϸ����� ����
        merchantAnimator.SetBool("IsTalk", true);
        // 3�� ��� �� IsTalk�� false�� �����ϴ� �ڷ�ƾ ����
        StartCoroutine(StopTalkingAfterDelay(2f));
        DisplayRandomShopItems();
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

    // ǰ���� �ƴ� ������ ��� �� �������� 3�� ���� �� ��ȯ
    private List<ShopItemData> ChooseRandomItems(int count)
    {
        HashSet<int> selectedIndices = new HashSet<int>();

        while (selectedIndices.Count < count && selectedIndices.Count < allShopItems.Length)
        {
            int randomIndex = Random.Range(0, allShopItems.Length);

            if (!IsItemPurchased(randomIndex))
            {
                selectedIndices.Add(randomIndex);
            }
        }

        List<ShopItemData> chosen = new List<ShopItemData>();
        foreach (int index in selectedIndices)
        {
            chosen.Add(allShopItems[index]);
        }

        // Debug.Log("���õ� ������ ����: " + chosen.Count);
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
        itemUI.Find("Item Price").GetComponent<Text>().text = "<color=#FFD700>" + item.itemPrice + "G</color>";
        itemUI.Find("Item Image").GetComponent<Image>().sprite = item.itemImage;

        // ��ư ���� �� ������â ǥ�õ�
        Button itemDetailButton = itemUI.GetComponent<Button>();
        if (itemDetailButton != null)
        {
            // �����ʸ� ���� �����ϰ� ���� �߰� (���� �����ʰ� �������� �ʵ���)
            itemDetailButton.onClick.RemoveAllListeners();

            // ������ �߰�
            itemDetailButton.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));
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

    // ������ ������ ������ â ���÷���
    public void DisplayItemDescription(ShopItemData currentItem)
    {
        if (currentItem != null)
        {
            // ������ ������ ������Ʈ
            itemDetailImage.sprite = currentItem.itemImage;
            itemDetailName.text = currentItem.itemName;
            itemDetailPrice.text = "<color=#FFD700>" + currentItem.itemPrice + "G</color>";
            itemDetailDescription.text = currentItem.itemDesc;

            if (itemDetailPanel != null)
                itemDetailPanel.SetActive(true);  // �г��� ǥ��
           
            // ���� ��ư
            if (purchaseButton != null)
            {
                // ������ �������� ������ ���� ��庸�� ��ΰų�, �÷��̾��� ü���� ���� �� ���� �� ���� ��ư ��Ȱ��ȭ
                if (currentItem.itemPrice > PlayerStat.Instance.Gold || PlayerStat.Instance.CurrentHP == PlayerStat.Instance.MaxHP)
                {
                    purchaseButton.interactable = false;
                }
                else
                {
                    purchaseButton.interactable = true;

                    // �����ʸ� ���� �����ϰ� ���� �߰� (���� �����ʰ� �������� �ʵ���)
                    purchaseButton.onClick.RemoveAllListeners();
                    // ������ �߰�
                    purchaseButton.onClick.AddListener(() => PurchaseItem(currentItem.itemID));
                    purchaseButton.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));
                }
            }

            // �ݱ� ��ư
            if (closeButtonItem != null)
            {
                closeButtonItem.onClick.RemoveAllListeners();
                closeButtonItem.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));
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
            AudioManager.Instance.PlayUISound(7, 0.3f);
            Debug.LogWarning("Item already purchased!");
            return;
        }

        int playerGold = PlayerStat.Instance.Gold;

        if (playerGold >= itemToPurchase.itemPrice)
        {
            // ��� ����
            PlayerStat.Instance.Gold -= itemToPurchase.itemPrice;

            // ���� ����
            AudioManager.Instance.PlaySound(2, 0.3f);

            // ������ ȿ�� ����
            ConsumableItem consumableItem = GetComponent<ConsumableItem>();
            if (consumableItem != null)
            {
                consumableItem.ActivateItem(itemID);

                // ���� ��ȭ �ִϸ����� ����
                merchantAnimator.SetBool("IsTalk", true);
                // 3�� ��� �� IsTalk�� false�� �����ϴ� �ڷ�ƾ ����
                StartCoroutine(StopTalkingAfterDelay(2f));
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
            AudioManager.Instance.PlayUISound(7, 0.3f);
            Debug.Log("Not enough gold to purchase the item!");
            // ��尡 ������ ����� �޽��� �Ǵ� �ִϸ��̼� �߰� (�ɼ�)
        }
    }

    // �����Ƽ ����Ȯ��
    public void DisplayPurchaseConfirmation()
    {
        if (purchaseConfirmationPanel != null)
            purchaseConfirmationPanel.SetActive(true);

        Button purchaseButton = purchaseConfirmationPanel.GetComponentInChildren<Button>();

        if (purchaseButton != null)
        {
            // �����ʸ� ���� �����ϰ� ���� �߰� (���� �����ʰ� �������� �ʵ���)
            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));

            // ���� ���
            int playerGold = PlayerStat.Instance.Gold;

            if (playerGold >= 200)
            {
                // ������ �߰�
                purchaseButton.interactable = true;
                purchaseButton.onClick.AddListener(() => abilityChoice.Show());
                purchaseButton.onClick.AddListener(() => abilityChoice.DisplayRandomAbilities());
                purchaseButton.onClick.AddListener(() => UseGold(200));
                purchaseButton.onClick.AddListener(() => purchaseConfirmationPanel.SetActive(false));
            }
            else
            {
                purchaseButton.interactable = false;
                Debug.Log("��尡 �����մϴ�.");
            }
        }

        // �ݱ� ��ư
        if (closeButtonAbility != null)
        {
            closeButtonAbility.onClick.RemoveAllListeners();
            closeButtonAbility.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));
        }
    }

    public void UseGold(int amount)
    {
        PlayerStat.Instance.Gold -= amount;
        // ���� ����
        AudioManager.Instance.PlaySound(2, 0.3f);
    }

    // ���� ��ȭ �ִϸ����� ����
    private IEnumerator StopTalkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // ���⼭ delay �ð���ŭ ��ٸ��ϴ�.
        merchantAnimator.SetBool("IsTalk", false);
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
