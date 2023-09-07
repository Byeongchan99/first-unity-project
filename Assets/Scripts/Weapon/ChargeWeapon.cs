using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class ChargeWeapon : BaseChargeWeapon
{
    public override void Attack(BaseState state)
    {
   
    }

    // 무기 활성화 시작
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
        // 발사체 발사 로직
        
    }

    public override void Skill(BaseState state)
    {

    }
}
