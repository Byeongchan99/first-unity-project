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
        private int chargeLevel = 0;  // ���� ���� �ܰ�

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

                chargeTime += Time.deltaTime;  // Time.deltaTime�� ���� �ð��� ����

                // ���� �ܰ� ������Ʈ
                if (chargeTime < 0.5f)
                {
                    chargeLevel = 0;
                }
                else if (chargeTime < 1.0f)
                {
                    if (chargeLevel < 1)
                    {
                        chargeLevel = 1;
                        Debug.Log("1����");
                    }
                }
                else if (chargeTime < 1.5f)
                {
                    if (chargeLevel < 2)
                    {
                        chargeLevel = 2;
                        Debug.Log("2����");
                    }
                }
                else
                {
                    if (chargeLevel < 3)
                    {
                        chargeLevel = 3;
                        Debug.Log("3����");
                    }
                }

                UpdateWeaponAndHandPosition(chargeTime);
            }
            else
            {
                // ���콺 ��ư�� �� ��� ȭ�� �߻�           
                direction = mousePosition - (Vector2)PlayerStat.Instance.transform.position;
                if (chargeLevel != 0)
                    PlayerStat.Instance.chargeWeaponManager.Weapon?.ChargingAttack(this, direction.normalized, chargeLevel);
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
