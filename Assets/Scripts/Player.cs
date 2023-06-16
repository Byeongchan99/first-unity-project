using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer renderer;
    Animator animator;

    public Vector2 moveDirection;   // 이동 방향값
    [SerializeField] public float moveSpeed = 250f; // 이동 속도

    public Vector3 mousePos;   // 마우스 위치
    public Vector3 mouseDirection;   // 마우스 방향
    public float mouseAngle;   // 마우스 각도

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Move()
    {
        // 방향 벡터 정규화
        moveDirection.Normalize();
        // 플레이어 이동(속도 변경 방식)
        rb.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;

        // 속도 애니메이션 설정
        animator.SetFloat("Speed", moveDirection.magnitude);
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 입력값
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        // 마우스 위치
        mousePos = Input.mousePosition;
        // 스크린 좌표를 월드 좌표로 변환
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // 2D 게임이므로 z좌표 0으로 변경
        mousePos.z = 0;

        // 마우스 방향 벡터 정규화
        mouseDirection = (mousePos - transform.position).normalized;
        // 방향 벡터 각도 구하기
        float mouseAngle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

        // 각도 범위 [0, 360]으로 설정
        if (mouseAngle < 0) mouseAngle += 360;
        // 각도에 맞는 애니메이션
        animator.SetFloat("MouseAngle", mouseAngle);

        // 테스트용 구르기 애니메이션 실행
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Roll", true);
        }
    }

    // test
    public void StopRolling()
    {
        animator.SetBool("Roll", false);
    }

    void FixedUpdate()
    {
        Move();
    }
}
