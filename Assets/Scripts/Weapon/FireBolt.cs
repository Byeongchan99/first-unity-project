using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Bullet
{
    [Header("Explosive Properties")]
    public float explosionRadius = 5.0f;     // ���� ����
    public float explosionDamage = 10.0f;    // ���� ������
    public List<string> damageTags = new List<string> { "Enemy", "Player" }; // ���߿� ������ ���� �±׵��� ���

    [Header("Explosion Collider")]
    public CircleCollider2D explosionCollider;  // ���� ������ ��Ÿ���� �� �ݶ��̴�

    private void Awake()
    {
        // ���� ������ ��Ÿ���� �� �ݶ��̴� �ʱ�ȭ
        explosionCollider.enabled = false;   // ó������ ��Ȱ��ȭ ����
    }

    // ������ ���� �� ȣ��Ǵ� �޼���
    protected override void HandleCollision(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || Per == 0)
            return;

        Per--;
        // ���� ȿ�� ����
        Explode();

        if (Per == 0)
        {
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void Explode()
    {
        // ���� �� �� �ݶ��̴� Ȱ��ȭ
        explosionCollider.enabled = true;

        // ���� �ִϸ��̼� ����
        Animator fireBoltAnimator = GetComponent<Animator>();
        fireBoltAnimator.SetTrigger("Hit");

        StartCoroutine(ExplostionEffectAnimation());     
    }

    IEnumerator ExplostionEffectAnimation()
    {
        // ���� �� �Ѿ� ��Ȱ��ȭ 
        float animationDuration = 1.333f;
        yield return new WaitForSeconds(animationDuration);
        explosionCollider.enabled = false; // ������ ������ �� �ݶ��̴��� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
