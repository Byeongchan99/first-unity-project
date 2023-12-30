using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Bullet
{
    [Header("Explosive Properties")]
    public float explosionRadius = 5.0f;     // 폭발 범위
    public float explosionDamage = 10.0f;    // 폭발 데미지
    public List<string> damageTags = new List<string> { "Enemy", "Player" }; // 폭발에 영향을 받을 태그들의 목록

    [Header("Explosion Collider")]
    public CircleCollider2D explosionCollider;  // 폭발 범위를 나타내는 원 콜라이더

    [Header("Explosion Sprite")]
    public SpriteRenderer explosionSpriteRenderer;  // 폭발 이펙트의 스프라이트 렌더러 참조

    [Header("Explosion Animation")]
    private Animator explosionAnimator;  // 파이어볼트의 애니메이터 참조

    [Header("Sound Effects")]
    public AudioClip flyingSound;    // 날아가는 효과음
    public AudioClip explosionSound; // 폭발 효과음
    private AudioSource audioSource; // AudioSource 컴포넌트

    private new void Awake()
    {
        base.Awake();
        GameObject explosionArea = transform.Find("ExplosionArea").gameObject;
        explosionCollider = explosionArea.GetComponent<CircleCollider2D>();
        explosionAnimator = explosionArea.GetComponent<Animator>(); // 폭발 애니메이터 초기화
        explosionCollider.enabled = false;   // 처음에는 비활성화 상태
        // AudioSource 컴포넌트 초기화
        audioSource = GetComponent<AudioSource>();
    }

    private new void OnEnable()
    {
        base.OnEnable();
        ResetExplosion();
        // 파이어볼트 발사 시 날아가는 효과음 재생
        if (flyingSound != null)
        {
            audioSource.clip = flyingSound;
            audioSource.loop = true; // 날아가는 동안 계속 재생
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
            explosionSpriteRenderer.sprite = null;  // 폭발 스프라이트 초기화
        }
    }

    // 적에게 닿을 시 호출되는 메서드
    protected override void HandleCollision(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || Per == 0)
            return;

        Per--;
        // 폭발 효과 시작
        Explode();

        if (Per == 0)
        {
            rb.velocity = Vector2.zero;
            // gameObject.SetActive(false);
        }
    }

    // 폭발
    private void Explode()
    {
        // 폭발 시 원 콜라이더 활성화
        explosionCollider.enabled = true;
        // 폭발 애니메이션 실행      
        explosionAnimator.SetTrigger("Hit");
        
        StartCoroutine(ExplostionEffectAnimation());
        // 폭발 효과음 재생
        if (explosionSound != null)
        {
            audioSource.Stop(); // 날아가는 효과음 중지
            audioSource.clip = explosionSound;
            audioSource.pitch = 2;
            audioSource.loop = false; // 폭발 효과음은 한 번만 재생
            audioSource.Play();
        }
    }

    IEnumerator ExplostionEffectAnimation()
    {
        // 폭발 후 총알 비활성화 
        float animationDuration = 1.333f;
        yield return new WaitForSeconds(animationDuration);
        explosionCollider.enabled = false; // 폭발이 끝나면 원 콜라이더를 비활성화
        gameObject.SetActive(false);
    }
}
