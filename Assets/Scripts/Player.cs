using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer renderer;
    Animator animator;
    public Animator shadowAnimator;   // 그림자의 애니메이터

    public Vector2 inputVec;   // 이동 방향값
    [SerializeField] public float moveSpeed; // 이동 속도
    public float moveAngle;

    bool isRolling = false;   // 구르기 여부
    public Vector2 rollDirection;
    [SerializeField] float rollSpeed;   // 구르기 속도
    [SerializeField] float rollCoolDown;   // 구르기 쿨타임
    float rollCoolDownTimer = 0f;

    public Vector3 mousePos;   // 마우스 위치
    public Vector3 mouseDirection;   // 마우스 방향
    public float mouseAngle;   // 마우스 각도

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        shadowAnimator = transform.Find("Shadow").GetComponent<Animator>();
    }

    void Move()
    {
        if (!GameManager.instance.isLive)
            return;
    
        // 플레이어 이동(속도 변경 방식)
        rb.velocity = inputVec * moveSpeed * Time.fixedDeltaTime;

        // 속도 애니메이션 설정
        animator.SetFloat("Speed", inputVec.magnitude);
    }

    void Roll()
    {
        if (!GameManager.instance.isLive)
            return;

        // 구르기 방향을 이동 방향으로 설정
        rollDirection = inputVec;
        // 구르기 방향으로 힘을 가해 이동
        rb.AddForce(rollDirection * rollSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    // 구르기 애니메이션을 멈추는 애니메이션 이벤트
    public void StopRolling()
    {
        // 구르기 멈추기
        isRolling = false;
        animator.SetBool("Roll", false);
        shadowAnimator.SetBool("Roll", false);
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 벡터의 각도
        moveAngle = Mathf.Atan2(inputVec.y, inputVec.x) * Mathf.Rad2Deg;
        // 각도 범위 [0, 360]으로 설정
        if (moveAngle < 0) moveAngle += 360;
        // 각도에 맞는 애니메이션
        animator.SetFloat("MoveAngle", moveAngle);

        // 마우스 위치
        mousePos = Input.mousePosition;
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
        animator.SetFloat("MouseAngle", mouseAngle);

        // 구르기
        if (Input.GetKeyDown(KeyCode.Space) && rollCoolDownTimer <= 0f && inputVec.magnitude > 0) 
        {
            // 구르기 실행
            animator.SetBool("Roll", true);
            shadowAnimator.SetBool("Roll", true);
            isRolling = true;
            // 구르기 쿨타임 설정
            rollCoolDownTimer = rollCoolDown;
        }

        // 사망 테스트 코드
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    void OnMove(InputValue value)
    {
        // 후처리로 normalized 해줌
        inputVec = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // 구르는 중일 때는 다른 조작 불가능
        if (isRolling)
        {
            Roll();
        }
        else
        {
            Move();

            // 구르기 쿨타임 타이머 감소
            if (rollCoolDownTimer > 0f)
            {
                rollCoolDownTimer -= Time.fixedDeltaTime;
            }
        }
    }
}
