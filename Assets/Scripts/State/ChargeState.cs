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
        private int chargeLevel = 0;  // 현재 차지 단계

        // 공격 각도(방향) 및 마우스 위치
        public Vector2 direction;
        private float angle;
        public Vector2 mousePosition;

        // 손 스프라이트 레이어
        SpriteRenderer rightHandRenderer = PlayerController.rightHand.GetComponent<SpriteRenderer>();

        public ChargeState(PlayerController controller) : base(controller)
        {
         
        }

        public override void OnEnterState()
        {
            chargeTime = 0f;  // 충전 초기화
            // 손 스프라이트 활성화
            PlayerController.leftHand.GetComponent<SpriteRenderer>().enabled = true;
            PlayerController.rightHand.GetComponent<SpriteRenderer>().enabled = true;
            PlayerStat.Instance.animator.SetBool("IsCharge", true);
            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            PlayerStat.Instance.chargeWeaponManager.Weapon.BeginAttack();
        }

        public override void OnUpdateState()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (Mouse.current.rightButton.isPressed)  // 마우스 우클릭이 눌러져 있는지 확인
            {
                // 무기 회전 로직
                PlayerStat.Instance.chargeWeaponManager.Weapon.RotateWeaponTowardsMouse();

                chargeTime += Time.deltaTime;  // Time.deltaTime을 통해 시간을 누적
                PlayerStat.Instance.animator.SetInteger("ChargeCombo", chargeLevel);

                // 차지 단계 업데이트
                if (chargeTime < 0.5f)
                {
                    chargeLevel = 0;
                }
                else if (chargeTime < 1.0f)
                {
                    if (chargeLevel < 1)
                    {
                        chargeLevel = 1;
                        Debug.Log("1차지");
                    }
                }
                else if (chargeTime < 1.5f)
                {
                    if (chargeLevel < 2)
                    {
                        chargeLevel = 2;
                        Debug.Log("2차지");
                    }
                }
                else
                {
                    if (chargeLevel < 3)
                    {
                        chargeLevel = 3;
                        Debug.Log("3차지");
                    }
                }

                PlayerStat.Instance.chargeWeaponManager.Weapon.UpdateWeaponAndHandPosition(chargeTime);
            }
            else
            {
                // 마우스 버튼을 뗀 경우 화살 발사           
                Vector2 bowWorldPosition = PlayerStat.Instance.transform.position + (Vector3)PlayerStat.Instance.chargeWeaponManager.Weapon.HandleData.localPosition;
                direction = mousePosition - bowWorldPosition;
                rightHandRenderer.sortingOrder = 10;

                if (chargeLevel != 0)
                    PlayerStat.Instance.chargeWeaponManager.Weapon?.ChargingAttack(this, direction.normalized, chargeLevel);
                PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
            }
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            // 손 스프라이트 비활성화
            PlayerController.leftHand.GetComponent<SpriteRenderer>().enabled = false;
            PlayerController.rightHand.GetComponent<SpriteRenderer>().enabled = false;
            PlayerStat.Instance.chargeWeaponManager.Weapon.EndAttack();
            IsCharge = false;
            PlayerStat.Instance.animator.SetBool("IsCharge", false);
            PlayerStat.Instance.animator.SetInteger("ChargeCombo", 0);
        }
    }
}
