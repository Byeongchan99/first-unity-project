using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterController;

public class PlayerController : MonoBehaviour
{
    public PlayerStat playerStat;
    RollState rollState;

    [Header("이동 관련")]
    public Vector2 inputVec;   // 입력 방향값
    public float moveDirection;

    [Header("구르기 관련")]
    public Vector2 rollDirection;

    [Header("마우스 위치")]
    public Vector3 mousePos;   // 마우스 위치
    public Vector3 mouseDirection;   // 마우스 방향
    public float mouseAngle;   // 마우스 각도

    [Header("공격 관련")]
    public Vector2 attackDirection;   // 공격 방향

    void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        rollState = playerStat.stateMachine.GetState(StateName.ROLL) as RollState;
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 벡터의 각도
        moveDirection = Mathf.Atan2(inputVec.y, inputVec.x) * Mathf.Rad2Deg;
        // 각도 범위 [0, 360]으로 설정
        if (moveDirection < 0) moveDirection += 360;
        // 각도에 맞는 애니메이션
        playerStat.animator.SetFloat("MoveDirection", moveDirection);

        // 마우스 위치
        mousePos = Mouse.current.position.ReadValue();
        // 스크린 좌표를 월드 좌표로 변환
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // 2D 게임이므로 z좌표 0으로 변경
        mousePos.z = 0;

        // 마우스 방향 벡터 정규화
        mouseDirection = (mousePos - transform.position).normalized;
        // 방향 벡터 각도 구하기
        mouseAngle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

        // 각도 범위 [0, 360]으로 설정
        if (mouseAngle < 0) mouseAngle += 360;
        // 각도에 맞는 애니메이션
        playerStat.animator.SetFloat("MouseAngle", mouseAngle);
    }

    void OnMove(InputValue value)
    {
        // 후처리로 normalized 해줌
        inputVec = value.Get<Vector2>();
    }

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
                rollState.IsRoll = false;
                PlayerStat.Instance.stateMachine.ChangeState(StateName.MOVE);
                break;
            }
        }

        yield return null;
    }

    void OnRoll()
    {
        rollDirection = inputVec;
       if (rollState.CanAddInputBuffer)
        {
            rollState.inputVecBuffer.Enqueue(rollDirection);
        }

       if (!rollState.IsRoll)
        {
            rollState.inputVecBuffer.Enqueue(rollDirection);
            playerStat.stateMachine.ChangeState(StateName.ROLL);
        } 
    }

    void OnAttack()
    {
        bool isAvailableAttack = !AttackState.IsAttack && (playerStat.weaponManager.Weapon.ComboCount < 3);
        
        if (isAvailableAttack)
        {
            playerStat.stateMachine.ChangeState(StateName.ATTACK);
        }
    }

    public void StartAttack()
    {
        attackDirection = mouseDirection;
    }

    public void MoveForward()
    {
        float advanceDistance = PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance;
        Vector2 targetPos = (Vector2)PlayerStat.Instance.transform.position + attackDirection * advanceDistance;
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;

        RaycastHit2D hit = Physics2D.Raycast(PlayerStat.Instance.transform.position, attackDirection, PlayerStat.Instance.weaponManager.Weapon.AdvanceDistance, layerMask);

        if (hit.collider == null)
        {
            Debug.Log("No object detected, moving to target position.");
            PlayerStat.Instance.rigidBody.MovePosition(targetPos);
        }
        else
        {
            Debug.Log("Object hit by Raycast: " + hit.collider.gameObject.name);
            PlayerStat.Instance.rigidBody.MovePosition(hit.point);
        }
    }

    // 사망 테스트 코드
    void OnTestDead()
    {
        playerStat.stateMachine.ChangeState(StateName.DEAD);
    }
}
