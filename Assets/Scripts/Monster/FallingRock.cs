using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonsterBullet
{
    public float fallSpeed = -9.8f; // 낙석의 하강 속도
    private Vector2 originalPosition; // 낙석이 시작하는 위치
    private float fallDistance = 6.0f; // 낙석이 떨어지는 최대 거리

    private CircleCollider2D AttackAreaColider;
    private BoxCollider2D StandAreaCollider;
    public SpriteRenderer FallingRockShadow;
    protected AudioSource audioSource;
    public AudioClip fallSound;

    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method
        AttackAreaColider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        Transform childTransform = transform.Find("StandArea");
        if (childTransform != null)
        {
            StandAreaCollider = childTransform.GetComponent<BoxCollider2D>();
        }
        FallingRockShadow.enabled = false;
    }

    protected override void ResetBullet()
    {
        base.ResetBullet();
        AttackAreaColider.enabled = false;
        StandAreaCollider.enabled = false;
        FallingRockShadow.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Init(float damage, Vector2 spawnPosition)
    {
        this.Damage = damage;
        this.transform.position = spawnPosition;
        originalPosition = spawnPosition;

        // 그림자의 월드 위치를 초기화
        FallingRockShadow.transform.position = spawnPosition + new Vector2(0, -6f); // 그림자를 낙석의 시작 위치 아래에 고정
        FallingRockShadow.enabled = true;

        // 코루틴 시작
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        // 이동할 거리 계산
        float distanceFallen = 0f;
        Vector2 shadowStartPosition = FallingRockShadow.transform.position;

        while (distanceFallen < fallDistance)
        {
            // 이 프레임에서 이동할 거리 계산
            float fallThisFrame = fallSpeed * Time.deltaTime;

            // 낙석의 위치를 업데이트 (Rigidbody를 직접 조작하지 않고 transform을 사용)
            transform.position = new Vector2(transform.position.x, transform.position.y + fallThisFrame);

            // 이동한 거리 업데이트 (음수에 음수를 더해서 양수를 만들지 않도록 수정)
            distanceFallen += Mathf.Abs(fallThisFrame);

            // 낙석이 땅에 닿기 직전에 AttackAreaCollider를 활성화
            if (fallDistance - distanceFallen < 0.1f) // 0.1은 거리 여유분입니다. 필요에 따라 조정하세요.
            {
                AttackAreaColider.enabled = true;
            }

            // 그림자의 월드 위치를 고정
            FallingRockShadow.transform.position = shadowStartPosition;

            // 다음 프레임까지 기다림
            yield return null;
        }
        // 낙석이 땅에 닿았으므로 추가 작업을 수행
        // 예를 들면, 낙석을 비활성화하거나, 플레이어의 이동 경로에 장애물로 남겨두는 로직 등
        audioSource.PlayOneShot(fallSound);
        AttackAreaColider.enabled = false;
        StandAreaCollider.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정
    }

    // 동일한 위치에 떨어지거나, 플레이어와 충돌할 시 비활성화
    protected override void HandleCollision(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        
        if (collision.CompareTag("Rock"))
        {
            gameObject.SetActive(false);
        }
    }
}