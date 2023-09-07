using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterController
{
    public class ChargeState : BaseState
    {
        // ���� ����
        public static bool IsCharge = false;

        // Ȱ�� �� ������Ʈ
        private Transform bowComponent;
        private Transform handComponent;

        // ���� ���� �� �ִ� ���� ����
        private float chargeTime = 0f;
        private const float maxChargeTime = 100f;
        private const float chargeRate = 20f;
        // ���� ����(����)
        public Vector2 attackDirection;

        public ChargeState(PlayerController controller) : base(controller)
        {
         
        }

        public override void OnEnterState()
        {
            chargeTime = 0f;  // ���� �ʱ�ȭ
            PlayerStat.Instance.chargeWeaponManager.Weapon.BeginAttack();
        }

        public override void OnUpdateState()
        {
            if (Mouse.current.rightButton.isPressed)  // ���콺 ��Ŭ���� ������ �ִ��� Ȯ��
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
                // ���콺 ��ư�� �� ��� ȭ�� �߻�
                PlayerStat.Instance.chargeWeaponManager.Weapon?.ChargingAttack(this);
            }
        }

        private void UpdateWeaponAndHandPosition(float chargeTime)
        {
            // Ȱ�� ���� ��ġ ���� ����
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
