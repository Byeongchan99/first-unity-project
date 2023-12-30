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

    [Header("Sound Effects")]
    public AudioClip flyingSound;    // ���ư��� ȿ����
    public AudioClip explosionSound; // ���� ȿ����
    private AudioSource audioSource; // AudioSource ������Ʈ

    private new void Awake()
    {
        base.Awake();
        GameObject explosionArea = transform.Find("ExplosionArea").gameObject;
        explosionCollider = explosionArea.GetComponent<CircleCollider2D>();
        explosionAnimator = explosionArea.GetComponent<Animator>(); // ���� �ִϸ����� �ʱ�ȭ
        explosionCollider.enabled = false;   // ó������ ��Ȱ��ȭ ����
        // AudioSource ������Ʈ �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
    }

    private new void OnEnable()
    {
        base.OnEnable();
        ResetExplosion();
        // ���̾Ʈ �߻� �� ���ư��� ȿ���� ���
        if (flyingSound != null)
        {
            audioSource.clip = flyingSound;
            audioSource.loop = true; // ���ư��� ���� ��� ���
            audioSource.Play();
        }
    }

    private void ResetExplosion()
    {
        if (explosionAnimator)
        {
            explosionAnimator.ResetTrigger("Hit");
        }

        if (explosionCollider)
        {
            explosionCollider.enabled = false;
        }

        if (explosionSpriteRenderer)
        {
            explosionSpriteRenderer.sprite = null;  // ���� ��������Ʈ �ʱ�ȭ
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

    // ����
    private void Explode()
    {
        // ���� �� �� �ݶ��̴� Ȱ��ȭ
        explosionCollider.enabled = true;
        // ���� �ִϸ��̼� ����      
        explosionAnimator.SetTrigger("Hit");
        
        StartCoroutine(ExplostionEffectAnimation());
        // ���� ȿ���� ���
        if (explosionSound != null)
        {
            audioSource.Stop(); // ���ư��� ȿ���� ����
            audioSource.clip = explosionSound;
            audioSource.pitch = 2;
            audioSource.loop = false; // ���� ȿ������ �� ���� ���
            audioSource.Play();
        }
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
