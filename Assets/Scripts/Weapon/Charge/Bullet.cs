using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;

    public float Damage;
    public int Per;
    public float originalDamage; // ���� ������ Damage �� �����
    public int originalPer;     // ���� ������ Per �� �����
    // �Ѿ��� ������������ �ð� (�⺻ 5��)
    public float bulletLifeTime = 5.0f; 
    // �⺻ �߻� �ӵ� (chargeLevel = 1�� ���� �ӵ�)
    public float baseSpeed = 15f;

    [Header("Sound Effects")]
    public AudioClip flyingSound;    // ���ư��� ȿ����
    protected AudioSource audioSource; // AudioSource ������Ʈ

    protected void Awake()
    {
        originalDamage = Damage;
        originalPer = Per;

        // AudioSource ������Ʈ �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
    }

    protected void OnEnable()
    {
        // ȭ�� �ʱ�ȭ ����
        // Rigidbody2D ���� Ȯ��
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        ResetBullet();
        StartCoroutine(DeactivateBulletAfterTime());

        // ȭ�� �߻� �� ���ư��� ȿ���� ���
        if (flyingSound != null)
        {
            audioSource.clip = flyingSound;
            audioSource.volume = 0.2f;
            audioSource.Play();
        }
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
        HandleCollision(collision);
    }

    protected virtual void HandleCollision(Collider2D collision)
    {
        // ������ �浹�� ����
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // ���� �浹���� ���� ������ ����
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
