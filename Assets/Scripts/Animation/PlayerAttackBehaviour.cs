using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerAttackBehaviour : StateMachineBehaviour
{
    private PlayerController playerController;
    private PlayerAttackArea playerAttackArea;
    [SerializeField] private float triggerPercentage;   // 진행도 설정
    [SerializeField] private float CanReInputTime;   // 연속 공격 입력 시간

    private OneHandSwordBasic weapon;
    private Coroutine checkAttackInputCor;

    // 공격 추가 입력 확인 플래그
    bool attackReceived = false;
    // 코루틴 중복 호출 방지
    private bool hasCheckedInput = false;
    private bool isCoroutineRunning = false;
    // 콜라이더 비활성화 플래그
    private bool hasDisabledCollider = false;
    // 공격 종료 확인 플래그
    private bool hasFinishedAttack = false;


    public void SetTriggerPercentage(float value)
    {
        triggerPercentage = value;
    }

    // State 시작 시 호출되는 메서드
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerController == null)
        {
            playerController = animator.GetComponent<PlayerController>();
        }
        if (playerAttackArea == null)
        {
            playerAttackArea = animator.gameObject.transform.Find("AttackArea").GetComponent<PlayerAttackArea>();
        }
        // 다른 초기화 로직이 필요하면 여기에 추가

    }

    // State 업데이트 시 호출되는 메서드
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (Weapon == null)
        {
            weapon = PlayerStat.Instance.GetComponentInChildren<OneHandSwordBasic>();
        }
        */

        if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 3)   // 세번째 공격 모션일 때 - 1타와 2타가 있음
        {
            // 애니메이션 진행도(stateInfo.normalizedTime)를 기준으로 로직을 수행
            if (!hasDisabledCollider && stateInfo.normalizedTime >= 0.3f && stateInfo.normalizedTime <= 0.4f)
            {
                MoveForward();
                // Debug.Log("콜라이더 비활성화 " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;   // 1타 공격 범위 콜라이더 비활성화
                hasDisabledCollider = true;
            }

            if (hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage && stateInfo.normalizedTime < triggerPercentage + 0.1f)   // 3번째 공격 모션에서는 추가 입력 X
            {
                playerAttackArea.ActivateAttackRange(PlayerController.Instance.attackDirection);   // 2타 공격 범위 콜라이더 활성화
                hasDisabledCollider = false;
            }

            if (!hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage + 0.1f && stateInfo.normalizedTime < triggerPercentage + 0.2f)   
            {
                // Debug.Log("콜라이더 비활성화 " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;   // 2타 공격 범위 콜라이더 비활성화
                hasDisabledCollider = true;
            }

            if (!hasFinishedAttack && stateInfo.normalizedTime >= 0.95f)   // 세번째 공격의 애니메이션 = 2초
            {
                hasFinishedAttack = true;
                PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.1f));  // (2초의 5퍼 = 0.1)초 지연 후 OnFinishedAttack 호출
            }
        }
        else   // 첫번째와 두번째 공격 모션일 때
        {
            // 애니메이션 진행도(stateInfo.normalizedTime)를 기준으로 로직을 수행
            if (!hasCheckedInput && stateInfo.normalizedTime > triggerPercentage && stateInfo.normalizedTime < triggerPercentage + 0.1f)
            {
                MoveForward();
                CheckAttackInput();
                hasCheckedInput = true;
            }

            if (!hasDisabledCollider && stateInfo.normalizedTime > triggerPercentage + 0.1f && stateInfo.normalizedTime < triggerPercentage + 0.2f)
            {
                // Debug.Log("콜라이더 비활성화 " + PlayerStat.Instance.weaponManager.Weapon.ComboCount);
                playerAttackArea.attackRangeCollider.enabled = false;
                hasDisabledCollider = true;
            }

            if (!hasFinishedAttack && stateInfo.normalizedTime >= 0.95f)
            {
                hasFinishedAttack = true;
                if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 1)   // 첫번째
                {
                    PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.075f));  // (1.5초의 5퍼 = 0.075)초 지연 후 OnFinishedAttack 호출
                }
                else if (PlayerStat.Instance.weaponManager.Weapon.ComboCount == 2)   // 두번째
                {
                    PlayerStat.Instance.StartCoroutine(DelayedOnFinishedAttack(0.05f));  // (1초의 5퍼 = 0.05)초 지연 후 OnFinishedAttack 호출
                }
            }
        }
    }

    // State 종료 시 호출되는 메서드
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasCheckedInput = false; // Reset the flag
        hasDisabledCollider = false; // Reset the flag
        hasFinishedAttack = false;
        attackReceived = false;
    }

    // 코루틴 중 추가 입력 감지 시 다음 콤보 공격 실행, 입력이 감지되지 않으면 공격 상태 종료 및 콤보 초기화
    // 플레이어가 공격 버튼을 누를 때 호출
    public void CheckAttackInput()
    {
        // Debug.Log("CheckAttackInput 실행");

        if (isCoroutineRunning)
        {
            // Debug.Log("checkAttackInputCoroutine 이미 실행 중");
            return;
        } 
            
        if (checkAttackInputCor != null)
        {
            PlayerStat.Instance.StopCoroutine(checkAttackInputCor);
        }

        // Debug.Log("checkAttackInputCoroutine 실행");
        checkAttackInputCor = PlayerStat.Instance.StartCoroutine(checkAttackInputCoroutine());
    }

    // CanReInputTime 동안 플레이어가 추가로 공격 버튼을 누르는지를 감지
    private IEnumerator checkAttackInputCoroutine()
    {
        // Debug.Log("checkAttackInputCoroutine() called.");
        float currentTime = 0f;      
        isCoroutineRunning = true;

        while (currentTime < CanReInputTime)
        {
            currentTime += Time.deltaTime;

            // 여기에서 추가 공격 입력을 감지
            if (PlayerController.Instance.OnAttackWasTriggered())
            {
                attackReceived = true;
                break;
            }

            yield return null;
        }

        isCoroutineRunning = false;
    }

    // 지연된 OnFinishedAttack 호출을 위한 코루틴
    private IEnumerator DelayedOnFinishedAttack(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 지연 시간만큼 기다림
        OnFinishedAttack(); // 지연 후 OnFinishedAttack 호출
    }

    private void OnFinishedAttack()
    {
        Debug.Log("공격 종료");

        // 추가 입력 O
        if (attackReceived)
        {
            // 연속 공격 실행
            Debug.Log("연속 공격 실행");
            PlayerStat.Instance.stateMachine.ChangeState(StateName.ATTACK);
        }
        else   // 추가 입력 X
        {
            // 연속 공격 초기화
            Debug.Log("연속 공격 초기화");
            PlayerStat.Instance.weaponManager.Weapon.ComboCount = 0;
            PlayerStat.Instance.animator.SetInteger("AttackCombo", 0);
            PlayerStat.Instance.animator.SetBool("IsAttack", false);
            PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
        }
    }

    // 공격할 때 약간 전진하는 모션
    private void MoveForward()
    {
        // Debug.Log("약간 전진");
        float advanceDistance = PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance;
        Vector2 targetPos = (Vector2)PlayerStat.Instance.transform.position + playerController.attackDirection * advanceDistance;
        int layerMask = 1 << LayerMask.NameToLayer("Wall");

        RaycastHit2D hit = Physics2D.Raycast(PlayerStat.Instance.transform.position, playerController.attackDirection, PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance, layerMask);

        if (hit.collider == null)
        {
            PlayerStat.Instance.rigidBody.MovePosition(targetPos);
        }
        else
        {
            PlayerStat.Instance.rigidBody.MovePosition(hit.point);
        }
    }
}
