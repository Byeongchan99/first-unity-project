using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;
using UnityEngine.InputSystem;

public class Bow : BaseChargeWeapon
{
    private SpriteRenderer childSpriteRenderer;
    private SpriteRenderer rightHandRenderer;

    private Vector2 mousePosition;
    private float angle;

    private void Awake()
    {
        childSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rightHandRenderer = PlayerController.rightHand.GetComponent<SpriteRenderer>();
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

    // Ȱ ȸ��
    public override void RotateWeaponTowardsMouse()
    {
        // ���⼭ mousePosition�� ����
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (!PlayerController.ChargeWeaponPosition)
            return;

        Vector2 direction = mousePosition - (Vector2)PlayerController.ChargeWeaponPosition.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        PlayerController.ChargeWeaponPosition.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Ȱ�� ���� �� ������ ����
    public override void UpdateWeaponAndHandPosition(float chargeTime)
    {
        // �ִ� ���� �ð� ����
        float maxChargeTime = 1.5f;  // ��: 1.5��

        // ���� ���� ���� ���
        float pullRatio = Mathf.Clamp01(chargeTime / maxChargeTime);

        // ���� ��ġ ���� ����
        Vector3 startPos = new Vector3(0.1f, 0, 0);
        Vector3 endPos;

        // ������ ���� endPos ����
        if (angle >= -22.5f && angle < 22.5f)   // ������
        {
            endPos = new Vector3(-0.1f, -0.05f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (angle >= 22.5f && angle < 67.5f)   // �ĸ� ������
        {
            endPos = new Vector3(-0.07f, -0.1f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (angle >= 67.5f && angle < 112.5f)   // �ĸ�
        {
            endPos = new Vector3(-0.07f, -0.2f, 0);
        }
        else if (angle >= 112.5f && angle < 157.5f)   // �ĸ� ����
        {
            endPos = new Vector3(-0.07f, -0.1f, 0);
        }
        else if (angle >= 157.5f && angle <= 180)   // ����
        {
            endPos = new Vector3(-0.1f, -0.05f, 0);
        }
        else if (angle >= -67.5f && angle < -22.5f)   // ���� ������
        {
            endPos = new Vector3(-0.14f, -0.14f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (angle >= -112.5f && angle < -67.5f)   // ����
        {
            endPos = new Vector3(-0.1f, -0.2f, 0);
        }
        else if (angle >= -157.5f && angle < -112.5f)   // ���� ����
        {
            endPos = new Vector3(-0.14f, -0.14f, 0);
        }
        else   // ����
        {
            endPos = new Vector3(-0.1f, -0.05f, 0);
        }

        Vector3 currentPos = Vector3.Lerp(startPos, endPos, pullRatio);

        PlayerController.rightHand.localPosition = currentPos;
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
