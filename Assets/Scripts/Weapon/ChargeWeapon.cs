using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;
using UnityEngine.InputSystem;

public class ChargeWeapon : BaseChargeWeapon
{
    private SpriteRenderer childSpriteRenderer;

    private void Awake()
    {
        childSpriteRenderer = transform.Find("BowSprite").GetComponent<SpriteRenderer>();
    }

    public override void Attack(BaseState state)
    {
        
    }

    // 무기 활성화 시작
    public override void BeginAttack()
    {
        childSpriteRenderer.enabled = true;
    }

    public override void EndAttack()
    {
        childSpriteRenderer.enabled = false;
    }

    public override void ChargingAttack(BaseState state, Vector2 dir, int chargeLevel)
    {
        // 발사체 발사 로직
        Transform bulletTransform = GameManager.instance.pool.Get(bulletID).transform;
        Bullet bulletComponent = bulletTransform.GetComponent<Bullet>();

        bulletTransform.position = transform.position;
        bulletTransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // chargeLevel에 따라 데미지 계수를 결정(예를 들어)
        float chargeCoefficient = 0.5f + (chargeLevel * 0.5f);

        float finalDamage = bulletComponent.Damage * damageCoefficient * chargeCoefficient;
        bulletComponent.Init(finalDamage, bulletComponent.Per, dir, chargeLevel);
    }


    public override void Skill(BaseState state)
    {

    }
}
