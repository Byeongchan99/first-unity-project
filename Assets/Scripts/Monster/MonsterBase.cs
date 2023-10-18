using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    Transform target;
    Animator anim;
    protected Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;   // ���� FixedUpdate���� ��ٸ�

    bool IsLive;
    public float speed;
    public float health;
    public float maxHealth;
    public float attackRange;
    private int lastAttackID = -1;  // ������ ���� AttackArea�� ���� ID
    public float attackTiming;   // ���� Ÿ�̹�
    public float attackDuration;  // �ִϸ��̼� ���� ���� �ð�

    public Vector2 attackDirection;   // ���� ����
    public Vector2 moveDirection;

    private Astar astarComponent;
    protected MonsterAttackArea monsterAttackArea;

    enum MonsterState
    {
        CHASE,
        ATTACK,
        DEAD
    }

    MonsterState monsterState;

    void Awake()
    {
        anim = GetComponent<Animator>();
        astarComponent = GetComponent<Astar>();  // Astar ������Ʈ�� �����ɴϴ�.
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterAttackArea = gameObject.GetComponentInChildren<MonsterAttackArea>();
        wait = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        target = PlayerStat.Instance.transform;
        IsLive = true;
        health = maxHealth;
        monsterState = MonsterState.CHASE;   // ��ȯ�� ���ʹ� ��ٷ� ���� ����
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (IsLive)
        {
            yield return StartCoroutine(monsterState.ToString());
        }
    }

    IEnumerator CHASE()
    {
        // A* �˰����� �̿��� ���� ���� ����
        while (monsterState == MonsterState.CHASE)
        {
            Vector2Int monsterPos = astarComponent.WorldToTilemapPosition(transform.position);
            Vector2Int playerPos = astarComponent.WorldToTilemapPosition(target.position);

            List<Node> path = astarComponent.PathFinding(monsterPos, playerPos);  // Astar ������Ʈ�� �̿��� ��� Ž��

            if (path != null && path.Count > 1) // ù ��° ���� ���� ��ġ�̹Ƿ� �� ��° ���� �̵�
            {
                Vector2 nextPosition = astarComponent.TilemapToWorldPosition(new Vector2Int(path[1].x, path[1].y));
                moveDirection = (nextPosition - (Vector2)transform.position).normalized;
                anim.SetFloat("Direction.X", moveDirection.x);
                anim.SetFloat("Direction.Y", moveDirection.y);
                rb.velocity = moveDirection * speed;

                // ���Ͱ� �÷��̾�� ����� ��������� ATTACK ���·� ��ȯ
                if (Vector2.Distance(transform.position, target.position) < 1.0f)
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }
            yield return new WaitForSeconds(0.1f); // 0.1�ʸ��� ��θ� ������Ʈ (�� ���� ����)
        }
    }

    IEnumerator ATTACK()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����

        attackDirection = moveDirection;
        anim.SetBool("IsAttack", true);

        yield return StartCoroutine(AttackPattern());

        /*
        yield return new WaitForSeconds(attackTiming);  // ���� Ÿ�ֿ̹� ���� ���� �ݶ��̴� Ȱ��ȭ
        monsterAttackArea.ActivateAttackRange(attackDirection);   // ���� ���� Ȱ��ȭ

        yield return new WaitForSeconds(0.2f);
        monsterAttackArea.attackRangeCollider.enabled = false;   // ���� ���� �ݶ��̴� ��Ȱ��ȭ

        yield return new WaitForSeconds(attackDuration - attackTiming - 0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����
        */

        anim.SetBool("IsAttack", false);
        ChangeState(MonsterState.CHASE);   // CHASE ���·� ��ȯ
    }

    public abstract IEnumerator AttackPattern();  // ���� ���� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PlayerAttackArea") && !collision.CompareTag("Bullet") && !collision.CompareTag("ExplosionArea")) 
            return;

        if (collision.gameObject.CompareTag("PlayerAttackArea"))
        {
            // Player�� ���� ������ �浹�� ���
            PlayerAttackArea playerAttackArea = collision.GetComponent<PlayerAttackArea>();
            int currentAttackID = playerAttackArea.GetAttackID();

            // ���� ������ ���� ID�� ���Ͱ� ���������� ���� ���� ID�� �ٸ��� ������ ó��
            if (currentAttackID != lastAttackID)
            {
                /*
                health -= PlayerStat.Instance.weaponManager.Weapon.AttackDamage;
                StartCoroutine(FlashSprite());  // �����Ÿ� ����
                StartCoroutine(KnockBack());
                Debug.Log("ü�� ����! ���� ü�� " + health);

                if (health <= 0)
                {
                    ChangeState(MonsterState.DEAD);
                }
                */
                health -= (PlayerStat.Instance.weaponManager.Weapon.AttackDamage + PlayerStat.Instance.AttackPower);   // ������ = ���� ���ݷ� + �÷��̾� ���ݷ�

                if (health > 0)
                {                   
                    StartCoroutine(FlashSprite());  // �����Ÿ� ����
                    StartCoroutine(KnockBack());
                    Debug.Log("ü�� ����! ���� ü�� " + health);
                }
                else
                {
                    ChangeState(MonsterState.DEAD);
                }

                lastAttackID = currentAttackID;  // ���� ���� ID�� ������Ʈ
            }
        }

        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<Bullet>().Damage;

            if (health > 0)
            {
                StartCoroutine(FlashSprite());  // �����Ÿ� ����
                StartCoroutine(KnockBack());
                Debug.Log("ü�� ����! ���� ü�� " + health);
            }
            else
            {
                ChangeState(MonsterState.DEAD);
            }
        }

        if (collision.CompareTag("ExplosionArea"))
        {
            health -= collision.transform.parent.GetComponent<FireBolt>().explosionDamage;

            if (health > 0)
            {
                StartCoroutine(FlashSprite());  // �����Ÿ� ����
                StartCoroutine(KnockBack());
                Debug.Log("ü�� ����! ���� ü�� " + health);
            }
            else
            {
                ChangeState(MonsterState.DEAD);
            }
        }
    }

    // ��������Ʈ �����Ÿ���
    IEnumerator FlashSprite()
    {
        float elapsedTime = 0;
        float flashTime = 0.5f;
        bool isRed = false;

        while (elapsedTime < flashTime) 
        {
            if (isRed)
            {
                spriteRenderer.color = Color.red;  // ���������� ����
                isRed = false;
            }
            else
            {
                spriteRenderer.color = Color.white;
                isRed = true;
            }
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        spriteRenderer.color = Color.white;  // ���������� ��������Ʈ ������ ������� (���) ����
    }

    // �ǰ� �� �ణ �з���
    IEnumerator KnockBack()
    {
        Vector2 playerPos = target.position;
        Vector2 dirVec = ((Vector2)transform.position - playerPos).normalized;

        float knockbackSpeed = 3.0f;  // ���� ������ �˹� �ӵ�
        rb.velocity = Vector2.zero;  // �˹� �� ����
        rb.velocity = dirVec * knockbackSpeed;

        yield return new WaitForSeconds(0.1f);  // �˹� ���� �ð� (0.1�ʷ� ����)   
    }

    IEnumerator DEAD()
    {
        Debug.Log("���� ���");
        IsLive = false;
        WaveManager.Instance.OnMonsterDeath();
        // ���� ���� �ʱ�ȭ �� �ִϸ��̼� ó�� (��: ��� �ִϸ��̼� ���)
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(1); // ��� �ִϸ��̼� ��� �ð� (��: 1��)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����

        gameObject.SetActive(false);  // ������Ʈ ��Ȱ��ȭ
    }

    void ChangeState(MonsterState newMonsterState)
    {
        monsterState = newMonsterState;
    }
}
