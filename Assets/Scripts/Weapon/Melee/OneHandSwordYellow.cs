using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class OneHandSwordYellow : BaseWeapon
{
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");

    public override void Attack(BaseState state)
    {
        ComboCount++;
        Debug.Log("ÄÞº¸ ¼ýÀÚ " + ComboCount);
        PlayerStat.Instance.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        PlayerStat.Instance.animator.SetBool(hashIsAttackAnimation, true);
        PlayerStat.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
    }

    public override void ChargingAttack(BaseState state)
    {

    }

    public override void Skill(BaseState state)
    {

    }

    public override void EquipEffect()
    {
        PlayerStat.Instance.MoveSpeed += 30;
    }

    public override void UnEquipEffect()
    {
        PlayerStat.Instance.MoveSpeed -= 30;
    }
}
