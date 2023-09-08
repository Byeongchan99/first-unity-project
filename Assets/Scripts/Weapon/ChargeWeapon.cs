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

    // ���� Ȱ��ȭ ����
    public override void BeginAttack()
    {
        childSpriteRenderer.enabled = true;
    }

    public override void EndAttack()
    {
        childSpriteRenderer.enabled = false;
    }

    public override void ChargingAttack(BaseState state, Vector2 dir)
    {
        // �߻�ü �߻� ����
        Transform bulletTransform = GameManager.instance.pool.Get(bulletID).transform;
        Bullet bulletComponent = bulletTransform.GetComponent<Bullet>();

        bulletTransform.position = transform.position;
        bulletTransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        float finalDamage = bulletComponent.Damage * damageCoefficient; // ������ ����� ����� ���� ������ ���
        bulletComponent.Init(finalDamage, bulletComponent.Per, dir);
    }

    public override void Skill(BaseState state)
    {

    }
}
