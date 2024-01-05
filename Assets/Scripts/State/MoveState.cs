using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class MoveState : BaseState
    {
        public bool isVertical;
        private float footstepTimer;
        public float footstepInterval = 0.5f; // 발자국 소리 간격(초)

        // 생성자, 기반 클래스인 BaseState 생성자 호출
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

            // 플레이어 이동(속도 변경 방식)
            PlayerStat.Instance.rigidBody.velocity = Controller.inputVec * PlayerStat.Instance.MoveSpeed * Time.fixedDeltaTime;
            // 각도에 맞는 애니메이션
            PlayerStat.Instance.animator.SetFloat("MoveDirection.X", Controller.inputVec.x);
            PlayerStat.Instance.animator.SetFloat("MoveDirection.Y", Controller.inputVec.y);
            // 발자국 사운드
            // 발자국 소리 타이머 업데이트
            footstepTimer += Time.fixedDeltaTime;
            if (Controller.inputVec.magnitude > 0.1f && footstepTimer >= footstepInterval && GameManager.instance.isLive)
            {
                PlayerStat.Instance.playerAudioManager.PlayWalkSound();
                footstepTimer = 0;
            }
            // 속도 애니메이션 설정
            PlayerStat.Instance.animator.SetFloat("Speed", Controller.inputVec.magnitude);
            // 파티클 애니메이션 설정
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
