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

    // 무기 활성화 시작
    public override void BeginAttack()
    {
        childSpriteRenderer.enabled = true;
    }

    public override void EndAttack()
    {
        childSpriteRenderer.enabled = false;
    }

    // 무기 회전
    public override void RotateWeaponTowardsMouse()
    {
        // 여기서 mousePosition을 갱신
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (!PlayerController.ChargeWeaponPosition)
            return;

        Vector2 direction = mousePosition - (Vector2)PlayerController.ChargeWeaponPosition.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        PlayerController.ChargeWeaponPosition.rotation = Quaternion.Euler(0, 0, angle + 90);   // 손의 위치를 중심으로 회전
        transform.localRotation = Quaternion.Euler(0, 0, -(angle + 90));   // 반대로 회전해 무기가 고정된 것처럼 보이게 함
    }

    // 반대손 움직임 구현
    public override void UpdateWeaponAndHandPosition(float chargeTime)
    {
        Vector3 handPos;
        mouseAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        // 각도에 따라 Pos 설정
        if (mouseAngle >= -22.5f && mouseAngle < 22.5f)   // 오른쪽
        {
            handPos = new Vector3(-0.06f, -0.004f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= 22.5f && mouseAngle < 67.5f)   // 후면 오른쪽
        {
            handPos = new Vector3(-0.155f, -0.111f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= 67.5f && mouseAngle < 112.5f)   // 후면
        {
            handPos = new Vector3(-0.2f, 0.015f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= 112.5f && mouseAngle < 157.5f)   // 후면 왼쪽
        {
            handPos = new Vector3(-0.131f, 0.071f, 0);
            rightHandRenderer.sortingOrder = 10;
        }
        else if (mouseAngle >= 157.5f && mouseAngle <= 180)   // 왼쪽
        {
            handPos = new Vector3(-0.1f, -0.05f, 0);
            rightHandRenderer.sortingOrder = 10;
        }
        else if (mouseAngle >= -67.5f && mouseAngle < -22.5f)   // 정면 오른쪽
        {
            handPos = new Vector3(-0.15f, 0.1f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= -112.5f && mouseAngle < -67.5f)   // 정면
        {
            handPos = new Vector3(-0.2f, 0f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (mouseAngle >= -157.5f && mouseAngle < -112.5f)   // 정면 왼쪽
        {
            handPos = new Vector3(-0.16f, -0.1f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else   // 왼쪽
        {
            handPos = new Vector3(-0.1f, -0.05f, 0);
            rightHandRenderer.sortingOrder = 10;
        }

        PlayerController.rightHand.localPosition = handPos;
    }

    public override void ChargingAttack(BaseState state, Vector2 dir, int chargeLevel)
    {
        // Energy 소모
        PlayerStat.Instance.CurrentEnergy -= 1;
        Debug.Log("에너지: " + PlayerStat.Instance.CurrentEnergy);
        if (PlayerStat.Instance.CurrentEnergy < 0)
            PlayerStat.Instance.CurrentEnergy = 0;

        // 발사체 발사 로직
        Transform bulletTransform = GameManager.instance.pool.Get(bulletID).transform;
        Bullet bulletComponent = bulletTransform.GetComponent<Bullet>();

        bulletTransform.position = PlayerStat.Instance.transform.position + (Vector3)PlayerStat.Instance.chargeWeaponManager.Weapon.HandleData.localPosition;
        bulletTransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // chargeLevel에 따라 데미지 계수를 결정(예를 들어)
        float chargeCoefficient = 0.5f + (chargeLevel * 0.5f);

        float finalDamage = bulletComponent.originalDamage * damageCoefficient * chargeCoefficient; // OriginalDamage를 사용
        bulletComponent.Init(finalDamage, bulletComponent.originalPer, dir, chargeLevel);  // OriginalPer를 사용
    }

    public override void Skill(BaseState state)
    {

    }
}
