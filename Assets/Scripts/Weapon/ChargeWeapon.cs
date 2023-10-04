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
        childSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
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

    public override void ChargingAttack(BaseState state, Vector2 dir, int chargeLevel)
    {
        // Energy �Ҹ�
        PlayerStat.Instance.CurrentEnergy -= 1;
        Debug.Log("������: " + PlayerStat.Instance.CurrentEnergy);
        if (PlayerStat.Instance.CurrentEnergy < 0)
            PlayerStat.Instance.CurrentEnergy = 0;

        // �߻�ü �߻� ����
        Transform bulletTransform = GameManager.instance.pool.Get(bulletID).transform;
        Bullet bulletComponent = bulletTransform.GetComponent<Bullet>();

        bulletTransform.position = PlayerStat.Instance.transform.position + (Vector3)PlayerStat.Instance.chargeWeaponManager.Weapon.HandleData.localPosition;
        bulletTransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // chargeLevel�� ���� ������ ����� ����(���� ���)
        float chargeCoefficient = 0.5f + (chargeLevel * 0.5f);

        float finalDamage = bulletComponent.originalDamage * damageCoefficient * chargeCoefficient; // OriginalDamage�� ���
        bulletComponent.Init(finalDamage, bulletComponent.originalPer, dir, chargeLevel);  // OriginalPer�� ���
    }


    public override void Skill(BaseState state)
    {

    }
}
