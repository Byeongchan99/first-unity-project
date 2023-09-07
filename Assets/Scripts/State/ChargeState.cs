using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterController
{
    public class ChargeState : BaseState
    {
        // 충전 여부
        public static bool IsCharge = false;

        // 활과 손 컴포넌트
        private Transform bowComponent;
        private Transform handComponent;

        // 당기는 강도 및 최대 당기는 강도
        private float chargeTime = 0f;
        private const float maxChargeTime = 100f;
        private const float chargeRate = 20f;
        // 공격 각도(방향)
        public Vector2 attackDirection;

        public ChargeState(PlayerController controller) : base(controller)
        {
         
        }

        public override void OnEnterState()
        {
            chargeTime = 0f;  // 충전 초기화
            PlayerStat.Instance.chargeWeaponManager.Weapon.BeginAttack();
        }

        public override void OnUpdateState()
        {
            if (Mouse.current.rightButton.isPressed)  // 마우스 우클릭이 눌러져 있는지 확인
            {
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
                PlayerStat.Instance.chargeWeaponManager.Weapon?.ChargingAttack(this);
            }
        }

        private void UpdateWeaponAndHandPosition(float chargeTime)
        {
            // 활과 손의 위치 조절 로직
            float weaponChargeAmount = chargeTime / maxChargeTime;
            bowComponent.localPosition = new Vector3(weaponChargeAmount, 0, 0);
            handComponent.localPosition = new Vector3(weaponChargeAmount, 0, 0);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            PlayerStat.Instance.chargeWeaponManager.Weapon.EndAttack();
            IsCharge = false;
            PlayerStat.Instance.animator.SetBool("IsCharge", false);
            PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
        }
    }
}
