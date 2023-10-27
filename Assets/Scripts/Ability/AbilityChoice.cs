using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityChoice : MonoBehaviour
{
    RectTransform rect;
    public AbilityData[] allAbilities; // Unity �����Ϳ��� ���� ������ AbilityData �迭

    // ���� Ȯ��
    public GameObject purchaseConfirmationPanel;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
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

    // �������� �����Ƽ ǥ��
    public void DisplayRandomAbilities()
    {
        // Debug.Log("DisplayRandomAbilities called");
        // Debug.Log("�����Ƽ �� ����: " + allAbilities.Length);

        List<AbilityData> chosenAbilities = ChooseRandomAbilities(3);

        for (int i = 0; i < chosenAbilities.Count; i++)
        {
            UpdateAbilityUI(chosenAbilities[i], i);
        }
    }

    // �����Ƽ ��� �� �������� 3�� ���� �� ��ȯ
    private List<AbilityData> ChooseRandomAbilities(int count)
    {
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < count && selectedIndices.Count < allAbilities.Length)
        {
            int randomIndex = Random.Range(0, allAbilities.Length);
            selectedIndices.Add(randomIndex);
        }

        List<AbilityData> chosen = new List<AbilityData>();
        foreach (int index in selectedIndices)
        {
            chosen.Add(allAbilities[index]);
        }

        // Debug.Log("���õ� �����Ƽ ����: " + chosen.Count);
        return chosen;
    }

    // �����Ƽ ������Ʈ
    private void UpdateAbilityUI(AbilityData ability, int index)
    {
        Transform abilityBackground = transform.Find("Ability BackGround");

        if (abilityBackground == null)
        {
            Debug.LogError("No Ability BackGround found!");
            return;
        }

        // Ability BackGround�� �ڽ� ������Ʈ�� ã���ϴ�.
        Transform abilityUI = abilityBackground.Find("Ability " + index); 

        if (abilityUI == null)
        {
            Debug.LogError("No UI display found for Ability " + index);
            return;
        }

        // �����Ƽ ������ �Ҵ�
        abilityUI.Find("Ability Name").GetComponent<Text>().text = ability.abilityName;
        abilityUI.Find("Ability Description").GetComponent<Text>().text = ability.abilityDesc;
        abilityUI.Find("Ability Image").GetComponent<Image>().sprite = ability.abilityImage;

        
        // ��ư�� �����Ƽ �޼��� �����ϴ� ������ �߰�
        Button abilityButton = abilityUI.GetComponent<Button>();
        if (abilityButton != null)
        {
            // �����ʸ� ���� �����ϰ� ���� �߰� (���� �����ʰ� �������� �ʵ���)
            abilityButton.onClick.RemoveAllListeners();

            // ������ �߰�
            abilityButton.onClick.AddListener(() => DisplayPurchaseConfirmation(ability.abilityID));
        }
    }

    public void DisplayPurchaseConfirmation(int abilityID)
    {
        if (purchaseConfirmationPanel != null)
            purchaseConfirmationPanel.SetActive(true);

        Button purchaseButton = purchaseConfirmationPanel.GetComponentInChildren<Button>();

        if (purchaseButton != null)
        {
            // �����ʸ� ���� �����ϰ� ���� �߰� (���� �����ʰ� �������� �ʵ���)
            purchaseButton.onClick.RemoveAllListeners();

            // ������ �߰�
            purchaseButton.onClick.AddListener(() => ActivateChosenAbility(abilityID));
        }
    }


    // ��� ���� �� �����Ƽ ID�� �´� �޼��� ����
    private void ActivateChosenAbility(int id)
    {
        // Ability ��ũ��Ʈ�� �������� �ڵ�
        Ability abilityScript = GetComponent<Ability>();

        // ���� ���
        int playerGold = PlayerStat.Instance.Gold;

        if (abilityScript != null)
        {
            if (playerGold >= 200)
            {
                PlayerStat.Instance.Gold -= 200;
                abilityScript.ActivateAbility(id);
                Hide();
            }
            else
            {
                Debug.Log("��尡 �����մϴ�.");
            }
        }
        else
        {
            Debug.LogError("Ability script not found on this GameObject!");
        }
    }
}
