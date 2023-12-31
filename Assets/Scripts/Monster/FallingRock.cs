using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonsterBullet
{
    public float fallSpeed = -9.8f; // ������ �ϰ� �ӵ�
    private Vector2 originalPosition; // ������ �����ϴ� ��ġ
    private float fallDistance = 6.0f; // ������ �������� �ִ� �Ÿ�

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

        // �׸����� ���� ��ġ�� �ʱ�ȭ
        FallingRockShadow.transform.position = spawnPosition + new Vector2(0, -6f); // �׸��ڸ� ������ ���� ��ġ �Ʒ��� ����
        FallingRockShadow.enabled = true;

        // �ڷ�ƾ ����
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        // �̵��� �Ÿ� ���
        float distanceFallen = 0f;
        Vector2 shadowStartPosition = FallingRockShadow.transform.position;

        while (distanceFallen < fallDistance)
        {
            // �� �����ӿ��� �̵��� �Ÿ� ���
            float fallThisFrame = fallSpeed * Time.deltaTime;

            // ������ ��ġ�� ������Ʈ (Rigidbody�� ���� �������� �ʰ� transform�� ���)
            transform.position = new Vector2(transform.position.x, transform.position.y + fallThisFrame);

            // �̵��� �Ÿ� ������Ʈ (������ ������ ���ؼ� ����� ������ �ʵ��� ����)
            distanceFallen += Mathf.Abs(fallThisFrame);

            // ������ ���� ��� ������ AttackAreaCollider�� Ȱ��ȭ
            if (fallDistance - distanceFallen < 0.1f) // 0.1�� �Ÿ� �������Դϴ�. �ʿ信 ���� �����ϼ���.
            {
                AttackAreaColider.enabled = true;
            }

            // �׸����� ���� ��ġ�� ����
            FallingRockShadow.transform.position = shadowStartPosition;

            // ���� �����ӱ��� ��ٸ�
            yield return null;
        }
        // ������ ���� ������Ƿ� �߰� �۾��� ����
        // ���� ���, ������ ��Ȱ��ȭ�ϰų�, �÷��̾��� �̵� ��ο� ��ֹ��� ���ܵδ� ���� ��
        audioSource.PlayOneShot(fallSound);
        AttackAreaColider.enabled = false;
        StandAreaCollider.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����
    }

    // ������ ��ġ�� �������ų�, �÷��̾�� �浹�� �� ��Ȱ��ȭ
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