using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;
using UnityEngine.InputSystem;

public class MagicStaff : BaseChargeWeapon
{
    private SpriteRenderer childSpriteRenderer;
    private SpriteRenderer rightHandRenderer;

    private Vector2 mousePosition;
    private float angle;
    private float mouseAngle;

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

    // ���� ȸ��
    public override void RotateWeaponTowardsMouse()
    {
        // ���⼭ mousePosition�� ����
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (!PlayerController.ChargeWeaponPosition)
            return;

        Vector2 direction = mousePosition - (Vector2)PlayerController.ChargeWeaponPosition.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        PlayerController.ChargeWeaponPosition.rotation = Quaternion.Euler(0, 0, angle + 90);   // ���� ��ġ�� �߽����� ȸ��
        transform.localRotation = Quaternion.Euler(0, 0, -(angle + 90));   // �ݴ�� ȸ���� ���Ⱑ ������ ��ó�� ���̰� ��
    }

    // �ݴ�� ������ ����
    public override void UpdateWeaponAndHandPosition(float chargeTime)
    {
        Vector3 handPos;
        mouseAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        // ������ ���� Pos ����
        if (mouseAngle >= -22.5f && mouseAngle < 22.5f)   // ������
        {
            handPos = new Vector3(-0.06f, -0.004f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= 22.5f && mouseAngle < 67.5f)   // �ĸ� ������
        {
            handPos = new Vector3(-0.155f, -0.111f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= 67.5f && mouseAngle < 112.5f)   // �ĸ�
        {
            handPos = new Vector3(-0.2f, 0.015f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= 112.5f && mouseAngle < 157.5f)   // �ĸ� ����
        {
            handPos = new Vector3(-0.131f, 0.071f, 0);
            rightHandRenderer.sortingOrder = 10;
        }
        else if (mouseAngle >= 157.5f && mouseAngle <= 180)   // ����
        {
            handPos = new Vector3(-0.1f, -0.05f, 0);
            rightHandRenderer.sortingOrder = 10;
        }
        else if (mouseAngle >= -67.5f && mouseAngle < -22.5f)   // ���� ������
        {
            handPos = new Vector3(-0.15f, 0.1f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= -112.5f && mouseAngle < -67.5f)   // ����
        {
            handPos = new Vector3(-0.2f, 0f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= -157.5f && mouseAngle < -112.5f)   // ���� ����
        {
            handPos = new Vector3(-0.16f, -0.1f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else   // ����
        {
            handPos = new Vector3(-0.1f, -0.05f, 0);
            rightHandRenderer.sortingOrder = 10;
        }

        PlayerController.rightHand.localPosition = handPos;
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
