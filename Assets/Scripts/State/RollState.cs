using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class RollState : BaseState
    {
        public static bool CanAddInputBuffer { get; set; }   // 버퍼 입력 가능 여부
        public static bool IsRoll { get; set; }   // 구르기 여부
        public static Queue<Vector2> inputVecBuffer { get; private set; }

        public RollState(PlayerController controller) : base(controller)
        {
            inputVecBuffer = new Queue<Vector2>();
        }

        public override void OnEnterState()
        {
            IsRoll = true;
            CanAddInputBuffer = true;
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
