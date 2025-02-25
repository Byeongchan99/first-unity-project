using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;

    public float Damage;
    public int Per;
    public float originalDamage; // 원래 설정한 Damage 값 저장용
    public int originalPer;     // 원래 설정한 Per 값 저장용
    // 총알이 사라지기까지의 시간 (기본 5초)
    public float bulletLifeTime = 5.0f; 
    // 기본 발사 속도 (chargeLevel = 1일 때의 속도)
    public float baseSpeed = 15f;

    [Header("Sound Effects")]
    public AudioClip flyingSound;    // 날아가는 효과음
    protected AudioSource audioSource; // AudioSource 컴포넌트

    protected void Awake()
    {
        originalDamage = Damage;
        originalPer = Per;

        // AudioSource 컴포넌트 초기화
        audioSource = GetComponent<AudioSource>();
    }

    protected void OnEnable()
    {
        // 화살 초기화 로직
        // Rigidbody2D 참조 확인
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        ResetBullet();
        StartCoroutine(DeactivateBulletAfterTime());

        // 화살 발사 시 날아가는 효과음 재생
        if (flyingSound != null)
        {
            audioSource.clip = flyingSound;
            audioSource.volume = 0.2f;
            audioSource.Play();
        }
    }

    // 화살 초기화
    private void ResetBullet()
    {
        rb.velocity = Vector2.zero;
        Damage = originalDamage;
        Per = originalPer;
    }

    // 시간이 지나면 비활성화
    IEnumerator DeactivateBulletAfterTime()
    {
        yield return new WaitForSeconds(bulletLifeTime);
        gameObject.SetActive(false);
    }

    // 데미지, 관통력, 방향
    public void Init(float damage, int per, Vector3 dir, int chargeLevel)
    {
        this.Damage = damage;
        this.Per = per;

        // chargeLevel에 따라 발사 속도를 조절
        float speedMultiplier = 1.0f + (chargeLevel - 1) * 0.5f; // chargeLevel 1은 기본 속도, 그 이후로는 50%씩 증가
        float finalSpeed = baseSpeed * speedMultiplier;

        if (per > 0)
        {
            rb.velocity = dir * finalSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    protected virtual void HandleCollision(Collider2D collision)
    {
        // 벽과의 충돌을 감지
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // 벽과 충돌했을 때의 로직을 수행
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
            return;
        }

        if (!collision.CompareTag("Enemy") || Per == 0)
            return;

        Per--;

        if (Per == 0)
        {
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
