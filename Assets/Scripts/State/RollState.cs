using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class RollState : BaseState
    {
        public static bool IsRoll { get; set; }   // 구르기 여부
        public static bool canRoll { get; set; } = true;   // 구르기 가능 여부(쿨타임 관련)
        private Coroutine rollCoolTimeCoroutine;

        public RollState(PlayerController controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {
            IsRoll = true;
            Roll();
        }

        // 구르기 로직
        private void Roll()
        {
            PlayerStat.Instance.animator.SetFloat("Speed", 1);   // 에러 방지
            Vector2 rollDirection = PlayerController.Instance.rollDirection;

            canRoll = false;
            PlayerStat.Instance.animator.SetBool("IsRoll", true);
            PlayerStat.Instance.shadowAnimator.SetBool("IsRoll", true);
            PlayerStat.Instance.particleAnimator.SetBool("IsRoll", true);
            PlayerStat.Instance.playerAudioManager.PlayRollSound();
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

            // 구르기 쿨타임 코루틴 실행
            if (rollCoolTimeCoroutine != null)
                PlayerStat.Instance.StopCoroutine(rollCoolTimeCoroutine);
            rollCoolTimeCoroutine = PlayerStat.Instance.StartCoroutine(RollCooltimeTimer(PlayerStat.Instance.RollCooltime));
        }

        private IEnumerator RollCooltimeTimer(float coolTime)
        {
            yield return new WaitForSeconds(coolTime); // 여기서 지정된 시간동안 기다립니다.

            RollState.canRoll = true;
            Debug.Log("구르기 가능");
        }
    }
}
