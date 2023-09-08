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

    public override void ChargingAttack(BaseState state)
    {
        // �߻�ü �߻� ����
        Transform bullet = GameManager.instance.pool.Get(bulletID).transform;
        bullet.position = transform.position;
        //bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        //bullet.GetComponent<Bullet>().Init(damage, StepCounter, dir);
    }

    public override void Skill(BaseState state)
    {

    }
}
