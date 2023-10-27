using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    // Ư�� �ɷ��� �ߵ��ϴ� �޼���
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
        PlayerStat.Instance.MaxEnergy += 2;
        PlayerStat.Instance.CurrentEnergy += 2;
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
        PlayerStat.Instance.CurrentEnergy += 1;
        HealthManager.Instance.AdjustHearts();
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
        PlayerStat.Instance.CurrentEnergy += 1;
        EnergyManager.Instance.AdjustEnergy();
    }

    // �̵� �ӵ�, �ִ� ������ ����
    // ������ ��
    private void MagicDance()
    {
        PlayerStat.Instance.MoveSpeed += 15;
        PlayerStat.Instance.MaxEnergy += 1;
        PlayerStat.Instance.CurrentEnergy += 1;
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
        PlayerStat.Instance.CurrentEnergy += 3;
        PlayerStat.Instance.MaxHP -= 2;
        HealthManager.Instance.AdjustHearts();
        EnergyManager.Instance.AdjustEnergy();
    }

    // �ִ� ������ ����, ���ݷ� ����
    // ����
    private void Pure()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.CurrentEnergy += 3;
        PlayerStat.Instance.AttackPower -= 10;
        EnergyManager.Instance.AdjustEnergy();
    }

    // �ִ� ������ ����, �̵� �ӵ� ����
    // ���
    private void Meditation()
    {
        PlayerStat.Instance.MaxEnergy += 3;
        PlayerStat.Instance.CurrentEnergy += 3;
        PlayerStat.Instance.MoveSpeed -= 30;
        EnergyManager.Instance.AdjustEnergy();
    }
}
