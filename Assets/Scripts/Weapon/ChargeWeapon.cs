using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class ChargeWeapon : BaseChargeWeapon
{
    public override void Attack(BaseState state)
    {
   
    }

    // ���� Ȱ��ȭ ����
    public override void BeginAttack()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public override void EndAttack()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void ChargingAttack(BaseState state)
    {
        // �߻�ü �߻� ����
        
    }

    public override void Skill(BaseState state)
    {

    }
}
