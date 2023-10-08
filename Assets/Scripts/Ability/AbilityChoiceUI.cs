using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityChoiceUI : MonoBehaviour
{
    [System.Serializable]
    public struct AbilityData
    {
        public Sprite abilityImage; // �������� �̹���
        public string abilityName; // �������� �̸�
        public string abilityDescription; // �������� ����
    }

    // �����Ƽ ��� 22��
    public List<AbilityData> abilities = new List<AbilityData>
    {
        new AbilityData { abilityName = "�Ƶ巹����", abilityDescription = "���ݷ� 5 ����, �̵� �ӵ� 15 ����" },
        new AbilityData { abilityName = "������ ��", abilityDescription = "�̵� �ӵ��� 50 ���������� �ִ� ü�� 2 ����" },
        new AbilityData { abilityName = "��ø", abilityDescription = "�̵� �ӵ��� 50 ���������� �ִ� ������ 2 ����" },
        new AbilityData { abilityName = "��ź", abilityDescription = "�ִ� ������ 1 ����, ���ݷ� 5 ����" },
        new AbilityData { abilityName = "������", abilityDescription = "���ݷ��� 15 ���������� �ִ� ü���� 2 ����" },
        new AbilityData { abilityName = "��� �¼�", abilityDescription = "�ִ� ü���� 3 ���������� ���ݷ� 10 ����" },
        new AbilityData { abilityName = "�ı�����", abilityDescription = "���ݷ��� 15 ���������� �̵� �ӵ� 30 ����" },
        new AbilityData { abilityName = "������", abilityDescription = "�ִ� ������ 2 ����" },
        new AbilityData { abilityName = "��üȭ", abilityDescription = "�ִ� �������� 3 ���������� �ִ� ü�� 2 ����" },
        new AbilityData { abilityName = "����", abilityDescription = "���ݷ��� 15 ���������� �ִ� ������ 2 ����" },
        new AbilityData { abilityName = "����", abilityDescription = "�̵� �ӵ��� 50 ���������� ���ݷ� 10 ����" },
        new AbilityData { abilityName = "����", abilityDescription = "�ִ� ü���� 3 ���������� �̵� �ӵ� 30 ����" },
        new AbilityData { abilityName = "ü�� �ܷ�", abilityDescription = "�ִ� ü�� 1 ����, ���ݷ� 5 ����" },
        new AbilityData { abilityName = "������ ��", abilityDescription = "�ִ� ������ 1 ����, �̵� �ӵ� 15 ����" },
        new AbilityData { abilityName = "���", abilityDescription = "�ִ� �������� 3 ���������� �̵� �ӵ� 30 ����" },
        new AbilityData { abilityName = "�ݼ�ȭ", abilityDescription = "�ִ� ü���� 3 ���������� �ִ� ������ 2 ����" },
        new AbilityData { abilityName = "������ ��", abilityDescription = "���ݷ� 10 ����" },
        new AbilityData { abilityName = "����", abilityDescription = "�ִ� �������� 3 ���������� ���ݷ� 10 ����" },
        new AbilityData { abilityName = "����� �߳", abilityDescription = "�̵� �ӵ� 30 ����" },
        new AbilityData { abilityName = "������ ü��", abilityDescription = "�ִ� ü�� 2 ����" },
        new AbilityData { abilityName = "�ʿ�", abilityDescription = "�ִ� ������ 1 ����, �ִ� ü�� 1 ����" },
        new AbilityData { abilityName = "Ȱ��", abilityDescription = "�̵� �ӵ� 15 ����, �ִ� ü�� 1 ����" },
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

    // �����Ƽ ��� �ʱ�ȭ
    private void InitializeAbilityActions()
    {
        abilityActions = new Dictionary<string, Action>
        {
            { "�Ƶ巹����", Adrenaline },
            { "������ ��", AgileBody },
            { "��ø", Agility },
            { "��ź", Awe },
            { "������", Berserker },
            { "��� �¼�", DefensiveStance },
            { "�ı�����", DestroyerTank },
            { "������", Enlightenment },
            { "��üȭ", Ethereal },
            { "����", Frenzy },
            { "����", Ghost },
            { "����", Giant },
            { "ü�� �ܷ�", HealthTraining },
            { "������ ��", MagicDance },
            { "���", Meditation },
            { "�ݼ�ȭ", Metalization },
            { "������ ��", MightyStrength },
            { "����� �߳", QuickReflexes },
            { "������ ü��", RobustHealth },
            { "����", Pure },
            { "����� �߳", QuickReflexes },
            { "������ ü��", RobustHealth },
            { "�ʿ�", Transcend },
            { "Ȱ��", Vitality }
        };
    }


    // �����Ƽ ��� �� �������� 3�� �����Ͽ� ���÷���
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
        SetAbilityButton(abilityButton1, abilities[randomIndices[0]]);
        SetAbilityButton(abilityButton2, abilities[randomIndices[1]]);
        SetAbilityButton(abilityButton3, abilities[randomIndices[2]]);
    }

    // �����Ƽ ���� ��ư�� �����Ƽ ���� ����
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

    // �����Ƽ �޼���
    // �⺻ �����Ƽ 4��
    // �ִ� ü�� ����
    // ������ ü��
    private void RobustHealth()
    {
        PlayerStat.Instance.MaxHP += 2;
        PlayerStat.Instance.CurrentHP += 2;
        HealthManager.Instance.AdjustHearts();
    }

    // ���ݷ� ����
    // ������ ��
    private void MightyStrength()
    {
        PlayerStat.Instance.AttackPower += 10;
    }

    // �̵� �ӵ� ����
    // ����� �߳
    private void QuickReflexes()
    {
        PlayerStat.Instance.MoveSpeed += 30;
    }

    // �ִ� ������ ����
    // ������
    private void Enlightenment()
    {
        PlayerStat.Instance.MaxHP += 2;
        PlayerStat.Instance.CurrentHP += 2;
        EnergyManager.Instance.AdjustEnergy();
    }

    // ���� + ���� ���� 6��
    // �ִ� ü��, ���ݷ� ����
    // ü�� �ܷ�
    private void HealthTraining()
    {
        PlayerStat.Instance.MaxHP += 1;
        PlayerStat.Instance.CurrentHP += 1;
        PlayerStat.Instance.AttackPower += 5;
        HealthManager.Instance.AdjustHearts();
    }

    // �ִ� ü��, �̵� �ӵ� ����
    // Ȱ��
    private void Vitality()
    {
        PlayerStat.Instance.MaxHP += 1;
        PlayerStat.Instance.CurrentHP += 1;
        PlayerStat.Instance.MoveSpeed += 15;
        HealthManager.Instance.AdjustHearts();
    }

    // �ִ� ü��, �ִ� ������ ����
    // �ʿ�
    private void Transcend()
    {
        PlayerStat.Instance.MaxHP += 1;
        PlayerStat.Instance.CurrentHP += 1;
        PlayerStat.Instance.MaxEnergy += 1;
        EnergyManager.Instance.AdjustEnergy();
    }

    // ���ݷ�, �̵� �ӵ� ����
    // �Ƶ巹����
    private void Adrenaline()
    {
        PlayerStat.Instance.AttackPower += 5;
        PlayerStat.Instance.MoveSpeed += 15;
    }

    // ���ݷ�, �ִ� ������ ����
    // ��ź
    private void Awe()
    {
        PlayerStat.Instance.AttackPower += 5;
        PlayerStat.Instance.MaxEnergy += 1;
        EnergyManager.Instance.AdjustEnergy();
    }

    // �̵� �ӵ�, �ִ� ������ ����
    // ������ ��
    private void MagicDance()
    {
        PlayerStat.Instance.MoveSpeed += 15;
        PlayerStat.Instance.MaxEnergy += 1;
        EnergyManager.Instance.AdjustEnergy();
    }

    // ���� + ����� ���� 12��
    // �ִ� ü�� ����, ���ݷ� ����
    // ��� �¼�
    private void DefensiveStance()
    {
        PlayerStat.Instance.MaxHP += 3;
        PlayerStat.Instance.CurrentHP += 3;
        PlayerStat.Instance.AttackPower -= 10;
        HealthManager.Instance.AdjustHearts();
    }

    // �ִ� ü�� ����, �̵� �ӵ� ����
    // ����
    private void Giant()
    {
        PlayerStat.Instance.MaxHP += 3;
        PlayerStat.Instance.CurrentHP += 3;
        PlayerStat.Instance.MoveSpeed -= 30;
        HealthManager.Instance.AdjustHearts();
    }

    // ü�� ����, �ִ� ������ ����
    // �ݼ�ȭ
    private void Metalization()
    {
        PlayerStat.Instance.MaxHP += 3;
        PlayerStat.Instance.CurrentHP += 3;
        PlayerStat.Instance.MaxEnergy -= 2;
        HealthManager.Instance.AdjustHearts();
        EnergyManager.Instance.AdjustEnergy();
    }

    // ���ݷ� ����, �ִ� ü�� ����
    // ������
    private void Berserker()
    {
        PlayerStat.Instance.AttackPower += 15;
        PlayerStat.Instance.MaxHP -= 2;
        HealthManager.Instance.AdjustHearts();
    }

    // ���ݷ� ����, �̵� �ӵ� ����
    // �ı�����
    private void DestroyerTank()
    {
        PlayerStat.Instance.AttackPower += 15;
        PlayerStat.Instance.MoveSpeed -= 30;
    }

    // ���ݷ� ����, �ִ� ������ ����
    // ����
    private void Frenzy()
    {
        PlayerStat.Instance.AttackPower += 15;
        PlayerStat.Instance.MaxEnergy -= 2;
        EnergyManager.Instance.AdjustEnergy();
    }

    // �̵� �ӵ� ����, �ִ� ü�� ����
    // ������ ��
    private void AgileBody()
    {
        PlayerStat.Instance.MoveSpeed += 50;
        PlayerStat.Instance.MaxHP -= 2;
        HealthManager.Instance.AdjustHearts();
    }

    // �̵� �ӵ� ����, ���ݷ� ����
    // ����
    private void Ghost()
    {
        PlayerStat.Instance.MoveSpeed += 50;
        PlayerStat.Instance.AttackPower -= 10;
    }

    // �̵� �ӵ� ����, �ִ� ������ ����
    // ��ø
    private void Agility()
    {
        PlayerStat.Instance.MoveSpeed += 50;
        PlayerStat.Instance.MaxEnergy -= 2;
        EnergyManager.Instance.AdjustEnergy();
    }

    // �ִ� ������ ����, �ִ� ü�� ����
    // ��üȭ
    private void Ethereal()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.MaxHP -= 2;
        EnergyManager.Instance.AdjustEnergy();
    }

    // �ִ� ������ ����, ���ݷ� ����
    // ����
    private void Pure()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.AttackPower -= 10;
        EnergyManager.Instance.AdjustEnergy();
    }

    // �ִ� ������ ����, �̵� �ӵ� ����
    // ���
    private void Meditation()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.MoveSpeed -= 30;
        EnergyManager.Instance.AdjustEnergy();
    }
}
