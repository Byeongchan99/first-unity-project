using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;   // 다음 FixedUpdate까지 기다림

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()   // 움직임 계산
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;   // 방향에 따른 스프라이트 뒤집기
    }

    void OnEnable()   // 스크립트가 활성화될 때 호출되는 이벤트 함수
    {
        target = GameManager.instance.playerController.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;   // Collider 활성화
        rigid.simulated = true;   // Rigidbody2D 활성화
        spriter.sortingOrder = 2;   // SpriteRenderer의 Sorting Order 증가
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    /*
    public void Init(SpawnData data)   // 초기화
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)   // 사망 판정 중복 방지
            return;

        // 무기에 맞았을 때 체력 계산
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false;   // Collider 비활성화
            rigid.simulated = false;   // Rigidbody2D 비활성화
            spriter.sortingOrder = 1;   // SpriteRenderer의 Sorting Order 감소 
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
    }
    */

    IEnumerator KnockBack()
    {
        int KnockBackRate = 3;
        yield return wait;   // 다음 하나의 물리 프레임 딜레이
        Vector3 playorPos = GameManager.instance.playerController.transform.position;
        Vector3 dirVec = transform.position - playorPos;
        rigid.AddForce(dirVec.normalized * KnockBackRate, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
