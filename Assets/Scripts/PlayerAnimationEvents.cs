using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;
using Unity.VisualScripting;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerAttackArea playerAttackArea;
    private Coroutine checkAttackInputCor;
    // 연속 공격 입력 시간
    public const float CanReInputTime = 1f;

    // 코루틴 중 추가 입력 감지 시 다음 콤보 공격 실행, 입력이 감지되지 않으면 공격 상태 종료 및 콤보 초기화
    // 플레이어가 공격 버튼을 누를 때 호출
    public void CheckAttackInput()
    {
        Debug.Log("CheckAttackInput() called.");
        // 공격 애니메이션 중간에 코루틴 시작
        if (checkAttackInputCor != null)
        {
            StopCoroutine(checkAttackInputCor);
        }
        checkAttackInputCor = StartCoroutine(checkAttackInputCoroutine());
    }

    // CanReInputTime 동안 플레이어가 추가로 공격 버튼을 누르는지를 감지
    private IEnumerator checkAttackInputCoroutine()
    {
        Debug.Log("checkAttackInputCoroutine() called.");
        float currentTime = 0f;
        bool attackReceived = false;

        while (currentTime < CanReInputTime)
        {
            currentTime += Time.deltaTime;

            // 여기에서 추가 공격 입력을 감지
            if (playerController.OnAttackWasTriggered())
            {
                attackReceived = true;
                break;
            }

            yield return null;
        }

        if (attackReceived)
        {
            // 연속 공격 실행
            // 여기에서 ComboCount를 증가시키고 다음 연속 공격 애니메이션을 실행합니다.
            PlayerStat.Instance.weaponManager.Weapon.ComboCount++;
            PlayerStat.Instance.animator.SetInteger("AttackCombo", PlayerStat.Instance.weaponManager.Weapon.ComboCount);
        }
        else
        {
            // 연속 공격 초기화
            PlayerStat.Instance.weaponManager.Weapon.ComboCount = 0;
            PlayerStat.Instance.animator.SetInteger("AttackCombo", 0);
            FinishedAttack();  // 공격 종료 처리
        }
    }

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

        if (hit.collider == null)
        {
            // Debug.Log("No object detected, moving to target position.");
            PlayerStat.Instance.rigidBody.MovePosition(targetPos);
        }
        else
        {
            // Debug.Log("Object hit by Raycast: " + hit.collider.gameObject.name);
            PlayerStat.Instance.rigidBody.MovePosition(hit.point);
        }
    }

    private Coroutine rollCoolTimeCoroutine;

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
