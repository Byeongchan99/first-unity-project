using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace CharacterController
{
    public class ChargeState : BaseState
    {
        // ���� ����
        public static bool IsCharge = false;

        // ���� ���� �� �ִ� ���� ����
        private float chargeTime = 0f;
        private const float maxChargeTime = 100f;
        private const float chargeRate = 20f;
        // ���� ����(����) �� ���콺 ��ġ
        public Vector2 direction;
        public Vector2 mousePosition;

        public ChargeState(PlayerController controller) : base(controller)
        {
         
        }

        public override void OnEnterState()
        {
            chargeTime = 0f;  // ���� �ʱ�ȭ
            PlayerStat.Instance.animator.SetBool("IsCharge", true);
            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            PlayerStat.Instance.chargeWeaponManager.Weapon.BeginAttack();
        }

        public override void OnUpdateState()
        {
            if (Mouse.current.rightButton.isPressed)  // ���콺 ��Ŭ���� ������ �ִ��� Ȯ��
            {
                // Ȱ ȸ�� ����
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
                // ���콺 ��ư�� �� ��� ȭ�� �߻�
                direction = mousePosition - (Vector2)PlayerStat.Instance.transform.position;
                PlayerStat.Instance.chargeWeaponManager.Weapon?.ChargingAttack(this, direction.normalized);
                PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
            }
        }

        private void RotateBowToMousePosition()
        {
            // ���콺�� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = mousePosition - (Vector2)PlayerStat.Instance.transform.position;

            // ȭ��ǥ�� ������ ���
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Ȱ�� ������ ȭ��ǥ �������� ����
            PlayerStat.Instance.chargeWeaponManager.Weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void UpdateWeaponAndHandPosition(float chargeTime)
        {
            /*
            // Ȱ�� ���� ��ġ ���� ����
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
