using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutManager : MonoBehaviour
{
    [System.Serializable]
    public struct AbilityData
    {
        public Sprite abilityImage; // 선택지의 이미지
        public string abilityName; // 선택지의 이름
        public string abilityDescription; // 선택지의 설명
    }

    public List<AbilityData> abilities = new List<AbilityData>
    {
        new AbilityData { abilityName = "Strength", abilityDescription = "Increase your physical power." },
        // ... 나머지 선택지 및 설명 추가
    };

    public Button abilityButton1;
    public Button abilityButton2;
    public Button abilityButton3;

    // 설명을 표시할 UI 요소 (예: Text 또는 TMPro)
    public Text descriptionText;

    private Dictionary<string, Action> abilityActions;

    private void Start()
    {
        InitializeAbilityActions();
        DisplayRandomAbilities();
    }

    private void InitializeAbilityActions()
    {
        abilityActions = new Dictionary<string, Action>
        {
            { "Strength", IncreaseStrength },
            // ... 나머지 선택지와 함수도 연결
        };
    }

    public void DisplayRandomAbilities()
    {
        HashSet<int> selectedIndices = new HashSet<int>();

        while (selectedIndices.Count < 3)
        {
            int randomIndex = UnityEngine.Random.Range(0, abilities.Count);

            selectedIndices.Add(randomIndex);
        }

        List<int> randomIndices = new List<int>(selectedIndices);

        // UI에 선택지 설정
        abilityButton1.GetComponentInChildren<Image>().sprite = abilities[randomIndices[0]].abilityImage;
        abilityButton1.GetComponentInChildren<Text>().text = abilities[randomIndices[0]].abilityName;
        // 선택지의 설명은 필요에 따라 표시하거나, 예를 들어 버튼에 마우스를 올렸을 때 툴팁 형식으로 표시할 수 있습니다.

        abilityButton2.GetComponentInChildren<Image>().sprite = abilities[randomIndices[1]].abilityImage;
        abilityButton2.GetComponentInChildren<Text>().text = abilities[randomIndices[1]].abilityName;

        abilityButton3.GetComponentInChildren<Image>().sprite = abilities[randomIndices[2]].abilityImage;
        abilityButton3.GetComponentInChildren<Text>().text = abilities[randomIndices[2]].abilityName;
    }


    private void SetAbilityButton(Button button, AbilityData abilityData)
    {
        button.GetComponentInChildren<Text>().text = abilityData.abilityName;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            abilityActions[abilityData.abilityName]();
            descriptionText.text = abilityData.abilityDescription;
        });
    }

    private void IncreaseStrength()
    {
        // Strength 능력치를 강화하는 코드
    }

    // ... 나머지 능력치 강화 함수도 추가
}
