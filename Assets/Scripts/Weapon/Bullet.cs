using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage { get; private set; }
    public int Per { get; private set; }

    Rigidbody2D rb;

    public float bulletLifeTime = 5.0f; // �Ѿ��� ������������ �ð� (�⺻ 5��)

    void OnEnable() // ���� ������Ʈ�� Ȱ��ȭ �� �� ȣ���
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


    // ������, �����, ����
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
