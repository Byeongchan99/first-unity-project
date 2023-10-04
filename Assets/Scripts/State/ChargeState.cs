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
            chargeTime = 0f;  // ���� �ʱ�ȭ
            // �� ��������Ʈ Ȱ��ȭ
            PlayerController.leftHand.GetComponent<SpriteRenderer>().enabled = true;
            PlayerController.rightHand.GetComponent<SpriteRenderer>().enabled = true;
            PlayerStat.Instance.animator.SetBool("IsCharge", true);
            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            PlayerStat.Instance.chargeWeaponManager.Weapon.BeginAttack();
        }

        public override void OnUpdateState()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (Mouse.current.rightButton.isPressed)  // ���콺 ��Ŭ���� ������ �ִ��� Ȯ��
            {
                // ���� ȸ�� ����
                RotateWeaponTowardsMouse();

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
                Vector2 bowWorldPosition = PlayerStat.Instance.transform.position + (Vector3)PlayerStat.Instance.chargeWeaponManager.Weapon.HandleData.localPosition;
                direction = mousePosition - bowWorldPosition;
                rightHandRenderer.sortingOrder = 10;

                if (chargeLevel != 0)
                    PlayerStat.Instance.chargeWeaponManager.Weapon?.ChargingAttack(this, direction.normalized, chargeLevel);
                PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
            }
        }

        // Ȱ ȸ��
        private void RotateWeaponTowardsMouse()
        {
            if (!PlayerController.ChargeWeaponPosition)
                return;

            Vector2 direction = mousePosition - (Vector2)PlayerController.ChargeWeaponPosition.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            PlayerController.ChargeWeaponPosition.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Ȱ�� ���� �� ������ ����
        private void UpdateWeaponAndHandPosition(float chargeTime)
        {
            // �ִ� ���� �ð� ����
            float maxChargeTime = 1.5f;  // ��: 1.5��

            // ���� ���� ���� ���
            float pullRatio = Mathf.Clamp01(chargeTime / maxChargeTime);

            // ���� ��ġ ���� ����
            Vector3 startPos = new Vector3(0.1f, 0, 0);
            Vector3 endPos;

            // ������ ���� endPos ����
            if (angle >= -22.5f && angle < 22.5f)   // ������
            {
                endPos = new Vector3(-0.1f, -0.05f, 0);
                rightHandRenderer.sortingOrder = 11;
            }
            else if (angle >= 22.5f && angle < 67.5f)   // �ĸ� ������
            {
                endPos = new Vector3(-0.07f, -0.1f, 0);
                rightHandRenderer.sortingOrder = 11;
            }
            else if (angle >= 67.5f && angle < 112.5f)   // �ĸ�
            {
                endPos = new Vector3(-0.07f, -0.2f, 0);
            }
            else if (angle >= 112.5f && angle < 157.5f)   // �ĸ� ����
            {
                endPos = new Vector3(-0.07f, -0.1f, 0);
            }
            else if (angle >= 157.5f && angle <= 180)   // ����
            {
                endPos = new Vector3(-0.1f, -0.05f, 0);
            }
            else if (angle >= -67.5f && angle < -22.5f)   // ���� ������
            {
                endPos = new Vector3(-0.14f, -0.14f, 0);
                rightHandRenderer.sortingOrder = 11;
            }
            else if (angle >= -112.5f && angle < -67.5f)   // ����
            {
                endPos = new Vector3(-0.1f, -0.2f, 0);
            }
            else if (angle >= -157.5f && angle < -112.5f)   // ���� ����
            {
                endPos = new Vector3(-0.14f, -0.14f, 0);
            }
            else   // ����
            {
                endPos = new Vector3(-0.1f, -0.05f, 0);
            }

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, pullRatio);

            PlayerController.rightHand.localPosition = currentPos;
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
        }
    }
}
