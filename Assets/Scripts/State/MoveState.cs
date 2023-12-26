using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class MoveState : BaseState
    {
        public bool isVertical;

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
            // �÷��̾� �̵�(�ӵ� ���� ���)
            PlayerStat.Instance.rigidBody.velocity = Controller.inputVec * PlayerStat.Instance.MoveSpeed * Time.fixedDeltaTime;
            // ������ �´� �ִϸ��̼�
            PlayerStat.Instance.animator.SetFloat("MoveDirection.X", Controller.inputVec.x);
            PlayerStat.Instance.animator.SetFloat("MoveDirection.Y", Controller.inputVec.y);
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
