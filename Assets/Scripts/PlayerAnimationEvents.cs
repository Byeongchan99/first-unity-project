using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void FinishedAttack()
    {
        AttackState.IsAttack = false;
        PlayerStat.Instance.animator.SetBool("IsAttack", false);
        PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
    }
}
