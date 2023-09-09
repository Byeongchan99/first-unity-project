using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public int Per;
    private float originalDamage; // ���� ������ Damage �� �����
    private int originalPer;     // ���� ������ Per �� �����

    Rigidbody2D rb;

    public float bulletLifeTime = 5.0f; // �Ѿ��� ������������ �ð� (�⺻ 5��)

    void OnEnable()
    {
        // ȭ�� �ʱ�ȭ ����
        ResetBullet();
        StartCoroutine(DeactivateBulletAfterTime());
    }

    // ȭ�� �ʱ�ȭ
    private void ResetBullet()
    {
        rb.velocity = Vector2.zero;
        Damage = originalDamage;
        Per = originalPer;
    }

    // �ð��� ������ ��Ȱ��ȭ
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

    // ������, �����, ����
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
