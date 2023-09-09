using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public int Per;
    private float originalDamage; // 원래 설정한 Damage 값 저장용
    private int originalPer;     // 원래 설정한 Per 값 저장용

    Rigidbody2D rb;

    public float bulletLifeTime = 5.0f; // 총알이 사라지기까지의 시간 (기본 5초)

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
    public void Init(float damage, int per, Vector3 dir)
    {
        this.Damage = damage;
        this.Per = per;
        originalDamage = damage;
        originalPer = per;

        if (per > -1)
        {
            rb.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || Per == -1)
            return;

        Per--;

        if (Per == -1)
        {
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
