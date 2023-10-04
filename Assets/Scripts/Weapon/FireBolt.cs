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

    [Header("Explosion Sprite")]
    public SpriteRenderer explosionSpriteRenderer;  // ���� ����Ʈ�� ��������Ʈ ������ ����

    [Header("Explosion Animation")]
    private Animator explosionAnimator;  // ���̾Ʈ�� �ִϸ����� ����
    private SpriteRenderer fireBoltSpriteRenderer; // ���̾Ʈ ��������Ʈ ������ ����

    private new void Awake()
    {
        base.Awake();
        GameObject explosionArea = transform.Find("ExplosionArea").gameObject;
        explosionCollider = explosionArea.GetComponent<CircleCollider2D>();
        explosionAnimator = explosionArea.GetComponent<Animator>(); // ���� �ִϸ����� �ʱ�ȭ

        GameObject fireBoltSprite = transform.Find("FireBolt Sprite").gameObject;
        fireBoltSpriteRenderer = fireBoltSprite.GetComponent<SpriteRenderer>();

        explosionCollider.enabled = false;   // ó������ ��Ȱ��ȭ ����
    }

    private new void OnEnable()
    {
        base.OnEnable();
        ResetExplosion();
    }

    private void ResetExplosion()
    {
        if (explosionAnimator)
        {
            explosionAnimator.ResetTrigger("Hit");
            explosionAnimator.Play("Empty", 0, 0f);
        }

        if (explosionCollider)
        {
            explosionCollider.enabled = false;
        }

        if (explosionSpriteRenderer)
        {
            explosionSpriteRenderer.sprite = null;  // ���� ��������Ʈ �ʱ�ȭ
        }

        if (fireBoltSpriteRenderer)
        {
            fireBoltSpriteRenderer.enabled = true;
        }
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
            // gameObject.SetActive(false);
        }
    }

    private void Explode()
    {
        // ���� �� �� �ݶ��̴� Ȱ��ȭ
        explosionCollider.enabled = true;
        fireBoltSpriteRenderer.enabled = false; // ���̾Ʈ ��������Ʈ ��Ȱ��ȭ
        // ���� �ִϸ��̼� ����      
        explosionAnimator.SetTrigger("Hit");

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
