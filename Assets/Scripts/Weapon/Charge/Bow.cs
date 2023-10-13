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

    // 무기 활성화 시작
    public override void BeginAttack()
    {
        childSpriteRenderer.enabled = true;
    }

    public override void EndAttack()
    {
        childSpriteRenderer.enabled = false;
    }

    // 활 회전
    public override void RotateWeaponTowardsMouse()
    {
        // 여기서 mousePosition을 갱신
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (!PlayerController.ChargeWeaponPosition)
            return;

        Vector2 direction = mousePosition - (Vector2)PlayerController.ChargeWeaponPosition.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        PlayerController.ChargeWeaponPosition.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 활을 당기는 손 움직임 구현
    public override void UpdateWeaponAndHandPosition(float chargeTime)
    {
        // 최대 당기는 시간 설정
        float maxChargeTime = 1.5f;  // 예: 1.5초

        // 현재 당기는 비율 계산
        float pullRatio = Mathf.Clamp01(chargeTime / maxChargeTime);

        // 손의 위치 선형 보간
        Vector3 startPos = new Vector3(0.1f, 0, 0);
        Vector3 endPos;

        // 각도에 따라 endPos 설정
        if (angle >= -22.5f && angle < 22.5f)   // 오른쪽
        {
            endPos = new Vector3(-0.1f, -0.05f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (angle >= 22.5f && angle < 67.5f)   // 후면 오른쪽
        {
            endPos = new Vector3(-0.07f, -0.1f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (angle >= 67.5f && angle < 112.5f)   // 후면
        {
            endPos = new Vector3(-0.07f, -0.2f, 0);
        }
        else if (angle >= 112.5f && angle < 157.5f)   // 후면 왼쪽
        {
            endPos = new Vector3(-0.07f, -0.1f, 0);
        }
        else if (angle >= 157.5f && angle <= 180)   // 왼쪽
        {
            endPos = new Vector3(-0.1f, -0.05f, 0);
        }
        else if (angle >= -67.5f && angle < -22.5f)   // 정면 오른쪽
        {
            endPos = new Vector3(-0.14f, -0.14f, 0);
            rightHandRenderer.sortingOrder = 11;
        }
        else if (angle >= -112.5f && angle < -67.5f)   // 정면
        {
            endPos = new Vector3(-0.1f, -0.2f, 0);
        }
        else if (angle >= -157.5f && angle < -112.5f)   // 정면 왼쪽
        {
            endPos = new Vector3(-0.14f, -0.14f, 0);
        }
        else   // 왼쪽
        {
            endPos = new Vector3(-0.1f, -0.05f, 0);
        }

        Vector3 currentPos = Vector3.Lerp(startPos, endPos, pullRatio);

        PlayerController.rightHand.localPosition = currentPos;
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
