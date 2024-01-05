using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class MoveState : BaseState
    {
        public bool isVertical;
        private float footstepTimer;
        public float footstepInterval = 0.5f; // ���ڱ� �Ҹ� ����(��)

        // ������, ��� Ŭ������ BaseState ������ ȣ��
        public MoveState(PlayerController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {

        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {
            if (!GameManager.instance.isLive)
            {
                PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
                return;
            }

            // �÷��̾� �̵�(�ӵ� ���� ���)
            PlayerStat.Instance.rigidBody.velocity = Controller.inputVec * PlayerStat.Instance.MoveSpeed * Time.fixedDeltaTime;
            // ������ �´� �ִϸ��̼�
            PlayerStat.Instance.animator.SetFloat("MoveDirection.X", Controller.inputVec.x);
            PlayerStat.Instance.animator.SetFloat("MoveDirection.Y", Controller.inputVec.y);
            // ���ڱ� ����
            // ���ڱ� �Ҹ� Ÿ�̸� ������Ʈ
            footstepTimer += Time.fixedDeltaTime;
            if (Controller.inputVec.magnitude > 0.1f && footstepTimer >= footstepInterval && GameManager.instance.isLive)
            {
                PlayerStat.Instance.playerAudioManager.PlayWalkSound();
                footstepTimer = 0;
            }
            // �ӵ� �ִϸ��̼� ����
            PlayerStat.Instance.animator.SetFloat("Speed", Controller.inputVec.magnitude);
            // ��ƼŬ �ִϸ��̼� ����
            PlayerStat.Instance.particleAnimator.SetFloat("MoveDirection.X", Controller.inputVec.x);
            if (Controller.inputVec.y == -1 || Controller.inputVec.y == 1)
                PlayerStat.Instance.particleAnimator.SetBool("IsVertical", true);
            else
                PlayerStat.Instance.particleAnimator.SetBool("IsVertical", false);
            PlayerStat.Instance.particleAnimator.SetFloat("Speed", Controller.inputVec.magnitude);
        }

        public override void OnExitState()
        {
            
        }
    }
}
