using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace CharacterController
{
    public class ChargeState : BaseState
    {
        // 충전 여부
        public static bool IsCharge = false;

        // 당기는 강도 및 최대 당기는 강도
        private float chargeTime = 0f;
        private const float maxChargeTime = 100f;
        private const float chargeRate = 20f;
        // 공격 각도(방향) 및 마우스 위치
        public Vector2 direction;
        public Vector2 mousePosition;

        public ChargeState(PlayerController controller) : base(controller)
        {
         
        }

        public override void OnEnterState()
        {
            chargeTime = 0f;  // 충전 초기화
            PlayerStat.Instance.animator.SetBool("IsCharge", true);
            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            PlayerStat.Instance.chargeWeaponManager.Weapon.BeginAttack();
        }

        public override void OnUpdateState()
        {
            if (Mouse.current.rightButton.isPressed)  // 마우스 우클릭이 눌러져 있는지 확인
            {
                // 활 회전 로직
                RotateBowToMousePosition();

                chargeTime += chargeRate * Time.deltaTime;
                if (chargeTime > maxChargeTime)
                {
                    chargeTime = maxChargeTime;
                }

                UpdateWeaponAndHandPosition(chargeTime);
            }
            else
            {
                // 마우스 버튼을 뗀 경우 화살 발사
                direction = mousePosition - (Vector2)PlayerStat.Instance.transform.position;
                PlayerStat.Instance.chargeWeaponManager.Weapon?.ChargingAttack(this, direction.normalized);
                PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
            }
        }

        private void RotateBowToMousePosition()
        {
            // 마우스의 화면 좌표를 월드 좌표로 변환
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = mousePosition - (Vector2)PlayerStat.Instance.transform.position;

            // 화살표의 방향을 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 활의 방향을 화살표 방향으로 설정
            PlayerStat.Instance.chargeWeaponManager.Weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void UpdateWeaponAndHandPosition(float chargeTime)
        {
            /*
            // 활과 손의 위치 조절 로직
            float weaponChargeAmount = chargeTime / maxChargeTime;
            bowComponent.localPosition = new Vector3(weaponChargeAmount, 0, 0);
            handComponent.localPosition = new Vector3(weaponChargeAmount, 0, 0);
            */
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            PlayerStat.Instance.chargeWeaponManager.Weapon.EndAttack();
            IsCharge = false;
            PlayerStat.Instance.animator.SetBool("IsCharge", false);   
        }
    }
}
