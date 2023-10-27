using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    // 특정 능력을 발동하는 메서드
    public void ActivateAbility(int abilityID)
    {
        switch (abilityID)
        {
            case 0:
                Adrenaline();
                break;
            case 1:
                AgileBody();
                break;
            case 2:
                Agility();
                break;
            case 3:
                Awe();
                break;
            case 4:
                Berserker();
                break;
            case 5:
                DefensiveStance();
                break;
            case 6:
                DestroyerTank();
                break;
            case 7:
                Enlightenment();
                break;
            case 8:
                Ethereal();
                break;
            case 9:
                Frenzy();
                break;
            case 10:
                Ghost();
                break;
            case 11:
                Giant();
                break;
            case 12:
                HealthTraining();
                break;
            case 13:
                MagicDance();
                break;
            case 14:
                Meditation();
                break;
            case 15:
                Metalization();
                break;
            case 16:
                MightyStrength();
                break;
            case 17:
                QuickReflexes();
                break;
            case 18:
                RobustHealth();
                break;
            case 19:
                Pure();
                break;
            case 20:
                Transcend();
                break;
            case 21:
                Vitality();
                break;
            default:
                Debug.LogError($"Ability with ID {abilityID} not found.");
                break;
        }
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
        PlayerStat.Instance.MaxEnergy += 2;
        PlayerStat.Instance.CurrentEnergy += 2;
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
        PlayerStat.Instance.CurrentEnergy += 1;
        HealthManager.Instance.AdjustHearts();
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
        PlayerStat.Instance.CurrentEnergy += 1;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 이동 속도, 최대 에너지 증가
    // 마법의 춤
    private void MagicDance()
    {
        PlayerStat.Instance.MoveSpeed += 15;
        PlayerStat.Instance.MaxEnergy += 1;
        PlayerStat.Instance.CurrentEnergy += 1;
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
        PlayerStat.Instance.CurrentEnergy += 3;
        PlayerStat.Instance.MaxHP -= 2;
        HealthManager.Instance.AdjustHearts();
        EnergyManager.Instance.AdjustEnergy();
    }

    // 최대 에너지 증가, 공격력 감소
    // 순수
    private void Pure()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.CurrentEnergy += 3;
        PlayerStat.Instance.AttackPower -= 10;
        EnergyManager.Instance.AdjustEnergy();
    }

    // 최대 에너지 증가, 이동 속도 감소
    // 명상
    private void Meditation()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.CurrentEnergy += 3;
        PlayerStat.Instance.MoveSpeed -= 30;
        EnergyManager.Instance.AdjustEnergy();
    }
}
