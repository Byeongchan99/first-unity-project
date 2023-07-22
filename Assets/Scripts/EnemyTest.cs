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
    WaitForFixedUpdate wait;   // ���� FixedUpdate���� ��ٸ�

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()   // ������ ���
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

        spriter.flipX = target.position.x < rigid.position.x;   // ���⿡ ���� ��������Ʈ ������
    }

    void OnEnable()   // ��ũ��Ʈ�� Ȱ��ȭ�� �� ȣ��Ǵ� �̺�Ʈ �Լ�
    {
        target = GameManager.instance.playerController.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;   // Collider Ȱ��ȭ
        rigid.simulated = true;   // Rigidbody2D Ȱ��ȭ
        spriter.sortingOrder = 2;   // SpriteRenderer�� Sorting Order ����
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    /*
    public void Init(SpawnData data)   // �ʱ�ȭ
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)   // ��� ���� �ߺ� ����
            return;

        // ���⿡ �¾��� �� ü�� ���
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
            coll.enabled = false;   // Collider ��Ȱ��ȭ
            rigid.simulated = false;   // Rigidbody2D ��Ȱ��ȭ
            spriter.sortingOrder = 1;   // SpriteRenderer�� Sorting Order ���� 
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
        yield return wait;   // ���� �ϳ��� ���� ������ ������
        Vector3 playorPos = GameManager.instance.playerController.transform.position;
        Vector3 dirVec = transform.position - playorPos;
        rigid.AddForce(dirVec.normalized * KnockBackRate, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
