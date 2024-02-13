using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class RollState : BaseState
    {
        public static bool IsRoll { get; set; }   // ������ ����
        public static bool canRoll { get; set; } = true;   // ������ ���� ����(��Ÿ�� ����)
        private Coroutine rollCoolTimeCoroutine;

        public RollState(PlayerController controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {
            IsRoll = true;
            Roll();
        }

        // ������ ����
        private void Roll()
        {
            PlayerStat.Instance.animator.SetFloat("Speed", 1);   // ���� ����
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

            // ������ ��Ÿ�� �ڷ�ƾ ����
            if (rollCoolTimeCoroutine != null)
                PlayerStat.Instance.StopCoroutine(rollCoolTimeCoroutine);
            rollCoolTimeCoroutine = PlayerStat.Instance.StartCoroutine(RollCooltimeTimer(PlayerStat.Instance.RollCooltime));
        }

        private IEnumerator RollCooltimeTimer(float coolTime)
        {
            yield return new WaitForSeconds(coolTime); // ���⼭ ������ �ð����� ��ٸ��ϴ�.

            RollState.canRoll = true;
            Debug.Log("������ ����");
        }
    }
}
