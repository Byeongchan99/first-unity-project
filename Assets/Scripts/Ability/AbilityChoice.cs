using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityChoice : MonoBehaviour
{
    RectTransform rect;
    public AbilityData[] allAbilities; // Unity 에디터에서 직접 참조할 AbilityData 배열

    // 구매 확인
    public GameObject purchaseConfirmationPanel;
    public Button closeButton;   // 닫기 버튼

    // 리롤 버튼
    public Button rerollButton;

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

    // 랜덤으로 어빌리티 표시
    public void DisplayRandomAbilities()
    {
        // Debug.Log("DisplayRandomAbilities called");
        // Debug.Log("어빌리티 총 개수: " + allAbilities.Length);

        List<AbilityData> chosenAbilities = ChooseRandomAbilities(3);

        for (int i = 0; i < chosenAbilities.Count; i++)
        {
            UpdateAbilityUI(chosenAbilities[i], i);
        }

        // 리롤 비용보다 보유 골드가 적다면 리롤 버튼 비활성화
        if (rerollButton != null)
        {
            if (PlayerStat.Instance.Gold < 20)
            {
                rerollButton.interactable = false;
            }
            else
            {
                rerollButton.interactable = true;
            }
        }
    }

    // 어빌리티 목록 중 랜덤으로 3개 선택 후 반환
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

        // Debug.Log("선택된 어빌리티 개수: " + chosen.Count);
        return chosen;
    }

    // 어빌리티 업데이트
    private void UpdateAbilityUI(AbilityData ability, int index)
    {
        Transform abilityBackground = transform.Find("Ability BackGround");

        if (abilityBackground == null)
        {
            Debug.LogError("No Ability BackGround found!");
            return;
        }

        // Ability BackGround의 자식 오브젝트를 찾습니다.
        Transform abilityUI = abilityBackground.Find("Ability " + index); 

        if (abilityUI == null)
        {
            Debug.LogError("No UI display found for Ability " + index);
            return;
        }

        // 어빌리티 데이터 할당
        abilityUI.Find("Ability Name").GetComponent<Text>().text = ability.abilityName;
        abilityUI.Find("Ability Description").GetComponent<Text>().text = ability.abilityDesc;
        abilityUI.Find("Ability Image").GetComponent<Image>().sprite = ability.abilityImage;

        
        // 버튼에 어빌리티 메서드 실행하는 리스너 추가
        Button abilityButton = abilityUI.GetComponent<Button>();
        if (abilityButton != null)
        {
            // 리스너를 먼저 제거하고 새로 추가 (이전 리스너가 남아있지 않도록)
            abilityButton.onClick.RemoveAllListeners();

            // 리스너 추가
            abilityButton.onClick.AddListener(() => AudioManager.Instance.PlayUISound(8, 0.3f));
            abilityButton.onClick.AddListener(() => DisplayPurchaseConfirmation(ability.abilityID));
        }
    }

    // 어빌리티 구매창 띄우기
    public void DisplayPurchaseConfirmation(int abilityID)
    {
        if (purchaseConfirmationPanel != null)
            purchaseConfirmationPanel.SetActive(true);

        Button purchaseButton = purchaseConfirmationPanel.GetComponentInChildren<Button>();

        if (purchaseButton != null)
        {
            // 리스너를 먼저 제거하고 새로 추가 (이전 리스너가 남아있지 않도록)
            purchaseButton.onClick.RemoveAllListeners();

            // 리스너 추가
            purchaseButton.onClick.AddListener(() => ActivateChosenAbility(abilityID));
            purchaseButton.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() => AudioManager.Instance.PlayUISound(0));
        }
    }

    // 리롤
    public void Reroll()
    {
        // 현재 골드
        int playerGold = PlayerStat.Instance.Gold;

        if (playerGold >= 20)
        {
            PlayerStat.Instance.Gold -= 20;
            // 구매 사운드
            AudioManager.Instance.PlaySound(2, 0.3f);
            DisplayRandomAbilities();
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }


    // 어빌리티 ID에 맞는 메서드 실행
    private void ActivateChosenAbility(int id)
    {
        // Ability 스크립트를 가져오는 코드
        Ability abilityScript = GetComponent<Ability>();

        if (abilityScript != null)
        {
            abilityScript.ActivateAbility(id);
            AchieveManager.instance.CheckAchieve();   // 어빌리티를 통해 스텟이 업데이트될 때, 업적 잠금 해제 조건 확인
            Hide();
        }
        else
        {
            Debug.LogError("Ability script not found on this GameObject!");
        }
    }
}
