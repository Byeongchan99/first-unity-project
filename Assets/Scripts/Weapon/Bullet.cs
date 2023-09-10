using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public int Per;
    private float originalDamage; // 원래 설정한 Damage 값 저장용
    private int originalPer;     // 원래 설정한 Per 값 저장용
    public float bulletLifeTime = 5.0f; // 총알이 사라지기까지의 시간 (기본 5초)
    // 기본 발사 속도 (chargeLevel = 1일 때의 속도)
    public float baseSpeed = 15f;

    Rigidbody2D rb;



    void OnEnable()
    {
        // 화살 초기화 로직
        ResetBullet();
        StartCoroutine(DeactivateBulletAfterTime());
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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalDamage = Damage;
        originalPer = Per;
    }

    // 데미지, 관통력, 방향
    public void Init(float damage, int per, Vector3 dir, int chargeLevel)
    {
        this.Damage = damage;
        this.Per = per;
        originalDamage = damage;
        originalPer = per;

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
