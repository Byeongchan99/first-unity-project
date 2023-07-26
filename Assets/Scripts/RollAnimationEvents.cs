using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class RollAnimationEvents : MonoBehaviour
{
    RollState rollState = PlayerStat.Instance.stateMachine.GetState(StateName.ROLL) as RollState;
    private Coroutine rollCoolTimeCoroutine;

    public void OnFinishedRoll()
    {
        if (rollState.inputVecBuffer.Count > 0)
        {
            PlayerStat.Instance.stateMachine.ChangeState(StateName.ROLL);
            return;
        }

        rollState.CanAddInputBuffer = false;
        rollState.OnExitState();

        if (rollCoolTimeCoroutine != null) 
            StopCoroutine(rollCoolTimeCoroutine);
        rollCoolTimeCoroutine = StartCoroutine(RollCooltimeTimer(PlayerStat.Instance.RollCooltime);
    }

    private IEnumerator RollCooltimeTimer(float coolTime)
    {
        float timer = 0f;

        while(true)
        {
            timer += Time.deltaTime;

            if (timer > coolTime)
            {
                rollState.IsRoll = false;
                PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
                break;
            }
        }

        yield return null;
    }
}
