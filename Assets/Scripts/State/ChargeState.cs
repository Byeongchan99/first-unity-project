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
        private float angle;
        public Vector2 mousePosition;

        // �� ��������Ʈ ���̾�
        SpriteRenderer rightHandRenderer = PlayerController.rightHand.GetComponent<SpriteRenderer>();

        public ChargeState(PlayerController controller) : base(controller)
        {
         
        }

        public override void OnEnterState()
        {
            IsCharge = true;
            chargeTime = 0f;  // ���� �ʱ�ȭ
            // �� ��������Ʈ Ȱ��ȭ
            PlayerController.leftHand.GetComponent<SpriteRenderer>().enabled = true;
            PlayerController.rightHand.GetComponent<SpriteRenderer>().enabled = true;
            // �÷��̾�� ������ �ִϸ��̼�
            PlayerStat.Instance.animator.SetBool("IsCharge", true);
            PlayerStat.Instance.magicCircleAnimator.SetBool("IsCharge", true);
            // ������ ���¿��� ���� �߻�
            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            PlayerStat.Instance.chargeWeaponManager.Weapon.BeginAttack();
            // ���� ���� ���
            PlayerStat.Instance.playerAudioManager.PlayChargeSound(PlayerStat.Instance.chargeWeaponManager.Weapon.WeaponID);
        }

        public override void OnUpdateState()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (Mouse.current.rightButton.isPressed)  // ���콺 ��Ŭ���� ������ �ִ��� Ȯ��
            {
                // ���� ȸ�� ����
                PlayerStat.Instance.chargeWeaponManager.Weapon.RotateWeaponTowardsMouse();

                chargeTime += Time.deltaTime;  // Time.deltaTime�� ���� �ð��� ����
                PlayerStat.Instance.animator.SetInteger("ChargeCombo", chargeLevel);
                PlayerStat.Instance.magicCircleAnimator.SetInteger("ChargeCombo", chargeLevel);

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

                PlayerStat.Instance.chargeWeaponManager.Weapon.UpdateWeaponAndHandPosition(chargeTime);
            }
            else
            {
                // ���콺 ��ư�� �� ��� ȭ�� �߻�           
                // Vector2 bowWorldPosition = PlayerStat.Instance.transform.position + (Vector3)PlayerStat.Instance.chargeWeaponManager.Weapon.HandleData.localPosition;
                Vector2 bowWorldPosition = PlayerStat.Instance.transform.position + PlayerStat.Instance.chargeWeaponManager.chargeWeaponPosition.localPosition;
                direction = mousePosition - bowWorldPosition;
                Debug.Log("����:" + direction.normalized);
                rightHandRenderer.sortingOrder = 10;

                // ���� ���� ���̵� �ƿ�
                PlayerStat.Instance.playerAudioManager.FadeOutSound(1f);

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
            // �� ��������Ʈ ��Ȱ��ȭ
            PlayerController.leftHand.GetComponent<SpriteRenderer>().enabled = false;
            PlayerController.rightHand.GetComponent<SpriteRenderer>().enabled = false;
            PlayerStat.Instance.chargeWeaponManager.Weapon.EndAttack();
            IsCharge = false;
            PlayerStat.Instance.animator.SetBool("IsCharge", false);
            PlayerStat.Instance.magicCircleAnimator.SetBool("IsCharge", false);
            PlayerStat.Instance.animator.SetInteger("ChargeCombo", 0);
            PlayerStat.Instance.magicCircleAnimator.SetInteger("ChargeCombo", 0);
        }
    }
}
