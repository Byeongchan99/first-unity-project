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
        public Sprite abilityImage; // 선택지의 이미지
        public string abilityName; // 선택지의 이름
        public string abilityDescription; // 선택지의 설명
    }

    // 어빌리티 목록 22개
    public List<AbilityData> abilities = new List<AbilityData>
    {
        new AbilityData { abilityName = "아드레날린", abilityDescription = "공격력 5 증가, 이동 속도 15 증가" },
        new AbilityData { abilityName = "가벼운 몸", abilityDescription = "이동 속도가 50 증가하지만 최대 체력 2 감소" },
        new AbilityData { abilityName = "민첩", abilityDescription = "이동 속도가 50 증가하지만 최대 에너지 2 감소" },
        new AbilityData { abilityName = "경탄", abilityDescription = "최대 에너지 1 증가, 공격력 5 증가" },
        new AbilityData { abilityName = "광전사", abilityDescription = "공격력이 15 증가하지만 최대 체력이 2 감소" },
        new AbilityData { abilityName = "방어 태세", abilityDescription = "최대 체력이 3 증가하지만 공격력 10 감소" },
        new AbilityData { abilityName = "파괴전차", abilityDescription = "공격력이 15 증가하지만 이동 속도 30 감소" },
        new AbilityData { abilityName = "깨달음", abilityDescription = "최대 에너지 2 증가" },
        new AbilityData { abilityName = "영체화", abilityDescription = "최대 에너지가 3 증가하지만 최대 체력 2 감소" },
        new AbilityData { abilityName = "광란", abilityDescription = "공격력이 15 증가하지만 최대 에너지 2 감소" },
        new AbilityData { abilityName = "유령", abilityDescription = "이동 속도가 50 증가하지만 공격력 10 감소" },
        new AbilityData { abilityName = "거인", abilityDescription = "최대 체력이 3 증가하지만 이동 속도 30 감소" },
        new AbilityData { abilityName = "체력 단련", abilityDescription = "최대 체력 1 증가, 공격력 5 증가" },
        new AbilityData { abilityName = "마법의 춤", abilityDescription = "최대 에너지 1 증가, 이동 속도 15 증가" },
        new AbilityData { abilityName = "명상", abilityDescription = "최대 에너지가 3 증가하지만 이동 속도 30 감소" },
        new AbilityData { abilityName = "금속화", abilityDescription = "최대 체력이 3 증가하지만 최대 에너지 2 감소" },
        new AbilityData { abilityName = "육중한 힘", abilityDescription = "공격력 10 증가" },
        new AbilityData { abilityName = "순수", abilityDescription = "최대 에너지가 3 증가하지만 공격력 10 감소" },
        new AbilityData { abilityName = "재빠른 발놀림", abilityDescription = "이동 속도 30 증가" },
        new AbilityData { abilityName = "강인한 체력", abilityDescription = "최대 체력 2 증가" },
        new AbilityData { abilityName = "초월", abilityDescription = "최대 에너지 1 증가, 최대 체력 1 증가" },
        new AbilityData { abilityName = "활력", abilityDescription = "이동 속도 15 증가, 최대 체력 1 증가" },
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

    // 어빌리티 목록 초기화
    private void InitializeAbilityActions()
    {
        abilityActions = new Dictionary<string, Action>
        {
            { "아드레날린", Adrenaline },
            { "가벼운 몸", AgileBody },
            { "민첩", Agility },
            { "경탄", Awe },
            { "광전사", Berserker },
            { "방어 태세", DefensiveStance },
            { "파괴전차", DestroyerTank },
            { "깨달음", Enlightenment },
            { "영체화", Ethereal },
            { "광란", Frenzy },
            { "유령", Ghost },
            { "거인", Giant },
            { "체력 단련", HealthTraining },
            { "마법의 춤", MagicDance },
            { "명상", Meditation },
            { "금속화", Metalization },
            { "육중한 힘", MightyStrength },
            { "재빠른 발놀림", QuickReflexes },
            { "강인한 체력", RobustHealth },
            { "순수", Pure },
            { "재빠른 발놀림", QuickReflexes },
            { "강인한 체력", RobustHealth },
            { "초월", Transcend },
            { "활력", Vitality }
        };
    }


    // 어빌리티 목록 중 랜덤으로 3개 선택하여 디스플레이
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
        SetAbilityButton(abilityButton1, abilities[randomIndices[0]]);
        SetAbilityButton(abilityButton2, abilities[randomIndices[1]]);
        SetAbilityButton(abilityButton3, abilities[randomIndices[2]]);
    }

    // 어빌리티 선택 버튼에 어빌리티 정보 설정
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

    // 어빌리티 메서드
    // 기본 어빌리티 4개
    // 최대 체력 증가
    // 강인한 체력
    private void RobustHealth()
    {
        PlayerStat.Instance.MaxHP += 2;
        PlayerStat.Instance.CurrentHP += 2;
        HealthManager.Instance.AdjustHearts();
    }

    // 공격력 증가
    // 육중한 힘
    private void MightyStrength()
    {
        PlayerStat.Instance.AttackPower += 10;
    }

    // 이동 속도 증가
    // 재빠른 발놀림
    private void QuickReflexes()
    {
        PlayerStat.Instance.MoveSpeed += 30;
    }

    // 최대 에너지 증가
    // 깨달음
    private void Enlightenment()
    {
        PlayerStat.Instance.MaxHP += 2;
        PlayerStat.Instance.CurrentHP += 2;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 버프 + 버프 조합 6개
    // 최대 체력, 공격력 증가
    // 체력 단련
    private void HealthTraining()
    {
        PlayerStat.Instance.MaxHP += 1;
        PlayerStat.Instance.CurrentHP += 1;
        PlayerStat.Instance.AttackPower += 5;
        HealthManager.Instance.AdjustHearts();
    }

    // 최대 체력, 이동 속도 증가
    // 활력
    private void Vitality()
    {
        PlayerStat.Instance.MaxHP += 1;
        PlayerStat.Instance.CurrentHP += 1;
        PlayerStat.Instance.MoveSpeed += 15;
        HealthManager.Instance.AdjustHearts();
    }

    // 최대 체력, 최대 에너지 증가
    // 초월
    private void Transcend()
    {
        PlayerStat.Instance.MaxHP += 1;
        PlayerStat.Instance.CurrentHP += 1;
        PlayerStat.Instance.MaxEnergy += 1;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 공격력, 이동 속도 증가
    // 아드레날린
    private void Adrenaline()
    {
        PlayerStat.Instance.AttackPower += 5;
        PlayerStat.Instance.MoveSpeed += 15;
    }

    // 공격력, 최대 에너지 증가
    // 경탄
    private void Awe()
    {
        PlayerStat.Instance.AttackPower += 5;
        PlayerStat.Instance.MaxEnergy += 1;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 이동 속도, 최대 에너지 증가
    // 마법의 춤
    private void MagicDance()
    {
        PlayerStat.Instance.MoveSpeed += 15;
        PlayerStat.Instance.MaxEnergy += 1;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 버프 + 디버프 조합 12개
    // 최대 체력 증가, 공격력 감소
    // 방어 태세
    private void DefensiveStance()
    {
        PlayerStat.Instance.MaxHP += 3;
        PlayerStat.Instance.CurrentHP += 3;
        PlayerStat.Instance.AttackPower -= 10;
        HealthManager.Instance.AdjustHearts();
    }

    // 최대 체력 증가, 이동 속도 감소
    // 거인
    private void Giant()
    {
        PlayerStat.Instance.MaxHP += 3;
        PlayerStat.Instance.CurrentHP += 3;
        PlayerStat.Instance.MoveSpeed -= 30;
        HealthManager.Instance.AdjustHearts();
    }

    // 체력 증가, 최대 에너지 감소
    // 금속화
    private void Metalization()
    {
        PlayerStat.Instance.MaxHP += 3;
        PlayerStat.Instance.CurrentHP += 3;
        PlayerStat.Instance.MaxEnergy -= 2;
        HealthManager.Instance.AdjustHearts();
        EnergyManager.Instance.AdjustEnergy();
    }

    // 공격력 증가, 최대 체력 감소
    // 광전사
    private void Berserker()
    {
        PlayerStat.Instance.AttackPower += 15;
        PlayerStat.Instance.MaxHP -= 2;
        HealthManager.Instance.AdjustHearts();
    }

    // 공격력 증가, 이동 속도 감소
    // 파괴전차
    private void DestroyerTank()
    {
        PlayerStat.Instance.AttackPower += 15;
        PlayerStat.Instance.MoveSpeed -= 30;
    }

    // 공격력 증가, 최대 에너지 감소
    // 광란
    private void Frenzy()
    {
        PlayerStat.Instance.AttackPower += 15;
        PlayerStat.Instance.MaxEnergy -= 2;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 이동 속도 증가, 최대 체력 감소
    // 가벼운 몸
    private void AgileBody()
    {
        PlayerStat.Instance.MoveSpeed += 50;
        PlayerStat.Instance.MaxHP -= 2;
        HealthManager.Instance.AdjustHearts();
    }

    // 이동 속도 증가, 공격력 감소
    // 유령
    private void Ghost()
    {
        PlayerStat.Instance.MoveSpeed += 50;
        PlayerStat.Instance.AttackPower -= 10;
    }

    // 이동 속도 증가, 최대 에너지 감소
    // 민첩
    private void Agility()
    {
        PlayerStat.Instance.MoveSpeed += 50;
        PlayerStat.Instance.MaxEnergy -= 2;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 최대 에너지 증가, 최대 체력 감소
    // 영체화
    private void Ethereal()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.MaxHP -= 2;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 최대 에너지 증가, 공격력 감소
    // 순수
    private void Pure()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.AttackPower -= 10;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 최대 에너지 증가, 이동 속도 감소
    // 명상
    private void Meditation()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.MoveSpeed -= 30;
        EnergyManager.Instance.AdjustEnergy();
    }
}
