using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;
using Unity.VisualScripting;

public class PlayerAnimationEvents : MonoBehaviour
{
    /*
    public PlayerController playerController;
    public PlayerAttackArea playerAttackArea;

    // 공격 애니메이션 끝날 때
    public void FinishedAttack()
    {
        playerAttackArea.attackRangeCollider.enabled = false;
    }

    // 공격 애니메이션 중 약간 전진
    public void MoveForward()
    {
        float advanceDistance = PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance;
        Vector2 targetPos = (Vector2)PlayerStat.Instance.transform.position + playerController.attackDirection * advanceDistance;
        int layerMask = 1 << LayerMask.NameToLayer("Wall");
       
        RaycastHit2D hit = Physics2D.Raycast(PlayerStat.Instance.transform.position, playerController.attackDirection, PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance, layerMask);

        // 일정 거리 전진
        if (hit.collider == null)
        {
            // Debug.Log("No object detected, moving to target position.");
            PlayerStat.Instance.rigidBody.MovePosition(targetPos);
        }
        else   // 벽이 있을 때는 벽 앞까지 이동
        {
            // Debug.Log("Object hit by Raycast: " + hit.collider.gameObject.name);
            PlayerStat.Instance.rigidBody.MovePosition(hit.point);
        }
    }
    */

    // 구르기 쿨타임 코루틴
    private Coroutine rollCoolTimeCoroutine;

    // 구르기 애니메이션 끝날 때
    public void OnFinishedRoll()
    {
        if (RollState.inputVecBuffer.Count > 0)
        {
            PlayerStat.Instance.stateMachine.ChangeState(StateName.ROLL);
            return;
        }

        RollState.CanAddInputBuffer = false;

        if (rollCoolTimeCoroutine != null)
            StopCoroutine(rollCoolTimeCoroutine);
        rollCoolTimeCoroutine = StartCoroutine(RollCooltimeTimer(PlayerStat.Instance.RollCooltime));
    }

    private IEnumerator RollCooltimeTimer(float coolTime)
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > coolTime)
            {
                RollState.IsRoll = false;
                PlayerStat.Instance.animator.SetBool("IsRoll", false);
                PlayerStat.Instance.shadowAnimator.SetBool("IsRoll", false);
                PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
                break;
            }
        }
        yield return null;
    }
}
