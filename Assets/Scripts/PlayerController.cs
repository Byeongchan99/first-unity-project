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

    [Header("구르기 관련")]
    public Vector2 rollDirection;   // 구르기 방향

    [Header("마우스 위치")]
    public Vector3 mousePos;   // 마우스 위치
    public Vector3 mouseDirection;   // 마우스 방향

    [Header("공격 관련")]
    public Vector2 attackDirection;   // 공격 방향

    void Awake()
    {
        playerStat = GetComponent<PlayerStat>();
    }

    void Start()
    {    
        rollState = playerStat.stateMachine.GetState(StateName.ROLL) as RollState;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 위치
        mousePos = Mouse.current.position.ReadValue();
        // 스크린 좌표를 월드 좌표로 변환
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // 2D 게임이므로 z좌표 0으로 변경
        mousePos.z = 0;

        // 마우스 방향 벡터 정규화
        mouseDirection = (mousePos - transform.position).normalized;
        // 각도에 맞는 애니메이션
        playerStat.animator.SetFloat("MouseDirection.X", mouseDirection.x);
        playerStat.animator.SetFloat("MouseDirection.Y", mouseDirection.y);
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

        // 입력 방향이 (0, 0)이면 리턴
        if (rollDirection == Vector2.zero)
        {
            Debug.Log("Player is standing still, can't roll.");
            return;
        }

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
            attackDirection = mouseDirection;
            playerStat.stateMachine.ChangeState(StateName.ATTACK);
        }
    }

    // 사망 테스트 코드
    void OnTestDead()
    {
        playerStat.stateMachine.ChangeState(StateName.DEAD);
    }
}
