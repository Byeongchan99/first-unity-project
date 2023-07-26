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
    public float moveAngle;

    [Header("구르기 관련")]
    public Vector2 rollDirection;
    float rollCoolDownTimer = 0f;

    [Header("마우스 위치")]
    public Vector3 mousePos;   // 마우스 위치
    public Vector3 mouseDirection;   // 마우스 방향
    public float mouseAngle;   // 마우스 각도

    void Start()
    {
        playerStat = GetComponent<PlayerStat>();
    }

    /*
    // 구르기 애니메이션을 멈추는 애니메이션 이벤트
    public void StopRolling()
    {
        // 구르기 멈추기
        isRolling = false;
        animator.SetBool("Roll", false);
        shadowAnimator.SetBool("Roll", false);
    }
    */

    // Update is called once per frame
    void Update()
    {
        // 이동 벡터의 각도
        moveAngle = Mathf.Atan2(inputVec.y, inputVec.x) * Mathf.Rad2Deg;
        // 각도 범위 [0, 360]으로 설정
        if (moveAngle < 0) moveAngle += 360;
        // 각도에 맞는 애니메이션
        playerStat.animator.SetFloat("MoveAngle", moveAngle);

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

    /*
    // 사망 테스트 코드
    void OnTestDead()
    {
        animator.SetTrigger("Dead");
        GameManager.instance.GameOver();
    }
    */
}
