using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class RollState : BaseState
    {
        public bool CanAddInputBuffer { get; set; }   // 버퍼 입력 가능 여부
        public bool IsRoll { get; set; }   // 구르기 여부
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
            Debug.Log("Roll Speed"+ PlayerStat.Instance.RollSpeed);
            PlayerStat.Instance.rigidBody.AddForce(rollDirection * PlayerStat.Instance.RollSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            PlayerStat.Instance.animator.SetBool("IsRoll", false);
            PlayerStat.Instance.shadowAnimator.SetBool("IsRoll", false);
        }
    }
}
