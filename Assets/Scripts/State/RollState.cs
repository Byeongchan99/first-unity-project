using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class RollState : BaseState
    {
        public bool CanAddInputBuffer { get; set; }   // ���� �Է� ���� ����
        public bool IsRoll { get; set; }   // ������ ����
        public Queue<Vector2> inputVecBuffer { get; private set; }

        public RollState(PlayerController controller) : base(controller)
        {
            inputVecBuffer = new Queue<Vector2>();
        }

        public override void OnEnterState()
        {
            IsRoll = true;
            CanAddInputBuffer = false;
            Roll();
        }

        private void Roll()
        {           
            Vector2 rollDirection = inputVecBuffer.Dequeue();
            
            PlayerStat.Instance.animator.SetBool("IsRoll", true);
            PlayerStat.Instance.shadowAnimator.SetBool("IsRoll", true);
            PlayerStat.Instance.rigidBody.velocity = rollDirection * PlayerStat.Instance.RollSpeed * PlayerStat.Instance.MoveSpeed;
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            PlayerStat.Instance.animator.SetBool("IsRoll", false);
            PlayerStat.Instance.shadowAnimator.SetBool("IsRoll", false);
        }
    }
}
