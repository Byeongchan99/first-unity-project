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
        public Sprite abilityImage; // �������� �̹���
        public string abilityName; // �������� �̸�
        public string abilityDescription; // �������� ����
    }

    public List<AbilityData> abilities = new List<AbilityData>
    {
        new AbilityData { abilityName = "Strength", abilityDescription = "Increase your physical power." },
        // ... ������ ������ �� ���� �߰�
    };

    public Button abilityButton1;
    public Button abilityButton2;
    public Button abilityButton3;

    // ������ ǥ���� UI ��� (��: Text �Ǵ� TMPro)
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
            // ... ������ �������� �Լ��� ����
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

        // UI�� ������ ����
        abilityButton1.GetComponentInChildren<Image>().sprite = abilities[randomIndices[0]].abilityImage;
        abilityButton1.GetComponentInChildren<Text>().text = abilities[randomIndices[0]].abilityName;
        // �������� ������ �ʿ信 ���� ǥ���ϰų�, ���� ��� ��ư�� ���콺�� �÷��� �� ���� �������� ǥ���� �� �ֽ��ϴ�.

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
        // Strength �ɷ�ġ�� ��ȭ�ϴ� �ڵ�
    }

    // ... ������ �ɷ�ġ ��ȭ �Լ��� �߰�
}
