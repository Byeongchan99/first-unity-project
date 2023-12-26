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
        if (stateInfo.normalizedTime >= 0.95f)  // �ִϸ��̼� Ŭ���� ����� ����
        {
            OnFinishedRoll();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �ʿ��� ��� StateExit���� ���� �߰�
        
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
