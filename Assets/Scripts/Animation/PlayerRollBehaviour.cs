using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerRollBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.95f)  // 애니메이션 클립이 종료될 때쯤
        {
            OnFinishedRoll();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 필요한 경우 StateExit에서 로직 추가
        
    }

    private void OnFinishedRoll()
    {
        RollState.IsRoll = false;
        PlayerStat.Instance.animator.SetBool("IsRoll", false);
        PlayerStat.Instance.shadowAnimator.SetBool("IsRoll", false);
        PlayerStat.Instance.particleAnimator.SetBool("IsRoll", false);
        PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
    }
}
