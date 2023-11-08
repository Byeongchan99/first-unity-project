using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonsterBullet
{
    public float fallSpeed = -9.8f; // ������ �ϰ� �ӵ�
    private Vector2 originalPosition; // ������ �����ϴ� ��ġ
    private float fallDistance = 4.0f; // ������ �������� �ִ� �Ÿ�

    private BoxCollider2D AttackAreaColider, StandAreaCollider;

    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method
        AttackAreaColider = GetComponent<BoxCollider2D>();
        Transform childTransform = transform.Find("StandArea");
        if (childTransform != null)
        {
            StandAreaCollider = childTransform.GetComponent<BoxCollider2D>();
        }
    }

    protected override void ResetBullet()
    {
        base.ResetBullet();
        AttackAreaColider.enabled = false;
        StandAreaCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Init(float damage, Vector2 spawnPosition)
    {
        this.Damage = damage;
        this.transform.position = spawnPosition;
        originalPosition = spawnPosition;

        // �ڷ�ƾ ����
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        // �̵��� �Ÿ� ���
        float distanceFallen = 0f;

        while (distanceFallen < fallDistance)
        {
            // �� �����ӿ��� �̵��� �Ÿ� ���
            float fallThisFrame = fallSpeed * Time.deltaTime;

            // ������ ��ġ�� ������Ʈ (Rigidbody�� ���� �������� �ʰ� transform�� ���)
            transform.position = new Vector2(transform.position.x, transform.position.y + fallThisFrame);

            // �̵��� �Ÿ� ������Ʈ (������ ������ ���ؼ� ����� ������ �ʵ��� ����)
            distanceFallen += Mathf.Abs(fallThisFrame);

            // ������ ���� ��� ������ AttackAreaCollider�� Ȱ��ȭ
            if (fallDistance - distanceFallen < 0.5f) // 0.1�� �Ÿ� �������Դϴ�. �ʿ信 ���� �����ϼ���.
            {
                AttackAreaColider.enabled = true;
            }

            // ���� �����ӱ��� ��ٸ�
            yield return null;
        }
        // ������ ���� ������Ƿ� �߰� �۾��� ����
        // ���� ���, ������ ��Ȱ��ȭ�ϰų�, �÷��̾��� �̵� ��ο� ��ֹ��� ���ܵδ� ���� ��
        AttackAreaColider.enabled = false;
        StandAreaCollider.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����
    }

    // ������ ��ġ�� �������ų�, �÷��̾�� �浹�� �� ��Ȱ��ȭ
    protected override void HandleCollision(Collider2D collision)
    {
        // AttackAreaColider�� �浹�ߴ��� Ȯ��
        if (collision == AttackAreaColider)
        {
            if (collision.CompareTag("Player"))
            {
                gameObject.SetActive(false);
            }
        }
        // StandAreaCollider�� �浹�ߴ��� Ȯ��
        else if (collision == StandAreaCollider)
        {
            if (collision.CompareTag("MonsterBullet"))
            {
                gameObject.SetActive(false);
            }
        }
    }

}