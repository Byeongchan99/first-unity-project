using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage { get; private set; }
    public int Per { get; private set; }

    Rigidbody2D rb;

    public float bulletLifeTime = 5.0f; // 총알이 사라지기까지의 시간 (기본 5초)

    void OnEnable() // 게임 오브젝트가 활성화 될 때 호출됨
    {
        StartCoroutine(DeactivateBulletAfterTime());
    }

    IEnumerator DeactivateBulletAfterTime()
    {
        yield return new WaitForSeconds(bulletLifeTime);
        gameObject.SetActive(false);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // 데미지, 관통력, 방향
    public void Init(float damage, int per, Vector3 dir)
    {
        this.Damage = damage;
        this.Per = per;

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
