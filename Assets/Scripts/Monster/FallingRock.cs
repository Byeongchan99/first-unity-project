using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonsterBullet
{
    public float fallSpeed = -9.8f; // 낙석의 하강 속도
    private Vector2 originalPosition; // 낙석이 시작하는 위치
    private float fallDistance = 4.0f; // 낙석이 떨어지는 최대 거리

    private BoxCollider2D AttackAreaColider, StandAreaCollider;

    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method
        AttackAreaColider = GetComponent<BoxCollider2D>();
        Transform childTransform = transform.Find("StandArea");
        if (childTransform != null)
        {
            StandAreaCollider = childTransform.GetComponent<BoxCollider2D>();
        }
    }

    protected override void ResetBullet()
    {
        base.ResetBullet();
        AttackAreaColider.enabled = false;
        StandAreaCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Init(float damage, Vector2 spawnPosition)
    {
        this.Damage = damage;
        this.transform.position = spawnPosition;
        originalPosition = spawnPosition;

        // 코루틴 시작
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        // 이동할 거리 계산
        float distanceFallen = 0f;

        while (distanceFallen < fallDistance)
        {
            // 이 프레임에서 이동할 거리 계산
            float fallThisFrame = fallSpeed * Time.deltaTime;

            // 낙석의 위치를 업데이트 (Rigidbody를 직접 조작하지 않고 transform을 사용)
            transform.position = new Vector2(transform.position.x, transform.position.y + fallThisFrame);

            // 이동한 거리 업데이트 (음수에 음수를 더해서 양수를 만들지 않도록 수정)
            distanceFallen += Mathf.Abs(fallThisFrame);

            // 낙석이 땅에 닿기 직전에 AttackAreaCollider를 활성화
            if (fallDistance - distanceFallen < 0.5f) // 0.1은 거리 여유분입니다. 필요에 따라 조정하세요.
            {
                AttackAreaColider.enabled = true;
            }

            // 다음 프레임까지 기다림
            yield return null;
        }
        // 낙석이 땅에 닿았으므로 추가 작업을 수행
        // 예를 들면, 낙석을 비활성화하거나, 플레이어의 이동 경로에 장애물로 남겨두는 로직 등
        AttackAreaColider.enabled = false;
        StandAreaCollider.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정
    }

    // 동일한 위치에 떨어지거나, 플레이어와 충돌할 시 비활성화
    protected override void HandleCollision(Collider2D collision)
    {
        // AttackAreaColider와 충돌했는지 확인
        if (collision == AttackAreaColider)
        {
            if (collision.CompareTag("Player"))
            {
                gameObject.SetActive(false);
            }
        }
        // StandAreaCollider와 충돌했는지 확인
        else if (collision == StandAreaCollider)
        {
            if (collision.CompareTag("MonsterBullet"))
            {
                gameObject.SetActive(false);
            }
        }
    }

}