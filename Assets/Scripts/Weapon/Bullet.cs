using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public int Per;
    public float originalDamage; // ���� ������ Damage �� �����
    public int originalPer;     // ���� ������ Per �� �����
    // �Ѿ��� ������������ �ð� (�⺻ 5��)
    public float bulletLifeTime = 5.0f; 
    // �⺻ �߻� �ӵ� (chargeLevel = 1�� ���� �ӵ�)
    public float baseSpeed = 15f;

    Rigidbody2D rb;



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
    public void Init(float damage, int per, Vector3 dir, int chargeLevel)
    {
        this.Damage = damage;
        this.Per = per;

        // chargeLevel�� ���� �߻� �ӵ��� ����
        float speedMultiplier = 1.0f + (chargeLevel - 1) * 0.5f; // chargeLevel 1�� �⺻ �ӵ�, �� ���ķδ� 50%�� ����
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
