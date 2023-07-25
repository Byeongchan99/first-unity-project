using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class MoveState : BaseState
    {

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

            // �ӵ� �ִϸ��̼� ����
            PlayerStat.Instance.animator.SetFloat("Speed", Controller.inputVec.magnitude);
        }

        public override void OnExitState()
        {
            
        }
    }
}
