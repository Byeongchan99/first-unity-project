using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MonsterBase : MonoBehaviour
{
    protected Transform target;
    protected Animator anim;
    protected Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;   // ���� FixedUpdate���� ��ٸ�

    protected bool IsLive;
    public float speed;
    public float health;
    public float maxHealth;

    public float attackDetectionRange;   // ���� �ν� ����
    public float attackRange;
    private int lastAttackID = -1;  // ������ ���� AttackArea�� ���� ID
    public float attackTiming;   // ���� Ÿ�̹�
    public float totalAttackTime;   // �� ���� �ð� = chargeTime + attackDuration/rushDuration + stunTime
    public float attackDuration;  // �ִϸ��̼� ���� ��� ���� �ð�
    public Vector2 attackDirection;   // ���� ����
    public Vector2 attackBasePosition;   // ���� ���� �߽�
    public float attackColliderOffset;   // ���� ���� �ݶ��̴� �̵� �Ÿ�
    private Coroutine attackPatternCoroutine;   // ���� ���� �ڷ�ƾ

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
        IsLive = true;
        health = maxHealth;
    }

    // ���͸� Ȱ��ȭ�ϴ� �ڵ�
    public void ActivateMonster()
    {
        target = PlayerStat.Instance.transform;
        monsterState = MonsterState.CHASE;
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
        while (monsterState == MonsterState.CHASE)
        {
            Vector2Int monsterPos = astarComponent.WorldToTilemapPosition(transform.position);
            Vector2Int playerPos = astarComponent.WorldToTilemapPosition(target.position);

            List<Node> path = astarComponent.PathFinding(monsterPos, playerPos);

            if (astarComponent.playerInWall)
            {
                moveDirection = (target.position - transform.position).normalized;
                rb.velocity = moveDirection * speed;

                if (Vector2.Distance(transform.position, target.position) < attackDetectionRange)
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }
            else if (path != null && path.Count > 1)
            {
                Vector2 nextPosition = astarComponent.TilemapToWorldPosition(new Vector2Int(path[1].x, path[1].y));
                moveDirection = (nextPosition - (Vector2)transform.position).normalized;
                anim.SetFloat("Direction.X", moveDirection.x);
                anim.SetFloat("Direction.Y", moveDirection.y);
                rb.velocity = moveDirection * speed;

                if (Vector2.Distance(transform.position, target.position) < attackDetectionRange)
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }

            yield return new WaitForSeconds(0.1f); // �ڷ�ƾ�� ���� �ð����� ���
        }
    }


    IEnumerator ATTACK()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����    

        yield return StartCoroutine(AttackPattern());

        /*
        yield return new WaitForSeconds(attackTiming);  // ���� Ÿ�ֿ̹� ���� ���� �ݶ��̴� Ȱ��ȭ
        monsterAttackArea.ActivateAttackRange(attackDirection);   // ���� ���� Ȱ��ȭ

        yield return new WaitForSeconds(0.2f);
        monsterAttackArea.attackRangeCollider.enabled = false;   // ���� ���� �ݶ��̴� ��Ȱ��ȭ

        yield return new WaitForSeconds(attackDuration - attackTiming - 0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����
        */

        if (health <= 0)
            ChangeState(MonsterState.DEAD);   // DEAD ���·� ��ȯ
        else
            ChangeState(MonsterState.CHASE);   // CHASE ���·� ��ȯ
    }

    public abstract IEnumerator AttackPattern();  // ���� ���� ����


    // ������ �� �ణ ���� - �ݶ��̴� ���� ���� �ذ�
    protected void MoveForward()
    {
        // Debug.Log("�ణ ����");
        float advanceDistance = 0.1f;
        Vector2 targetPos = (Vector2)transform.position + attackDirection * advanceDistance;
        int layerMask = 1 << LayerMask.NameToLayer("Wall");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, advanceDistance, layerMask);

        if (hit.collider == null)
        {
            rb.MovePosition(targetPos);
        }
        else
        {
            rb.MovePosition(hit.point);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerAttackArea":
                ProcessDamage(collision.GetComponent<PlayerAttackArea>().GetAttackID(),
                    PlayerStat.Instance.weaponManager.Weapon.AttackDamage + PlayerStat.Instance.AttackPower);
                break;

            case "Bullet":
                ProcessDamage(-1, collision.GetComponent<Bullet>().Damage); // -1�� ���� ID�� ������ ��Ÿ��
                break;

            case "ExplosionArea":
                ProcessDamage(-1, collision.transform.parent.GetComponent<FireBolt>().explosionDamage);
                break;
        }
    }

    private void ProcessDamage(int attackID, float damage)
    {
        // ���� ID�� �ٸ��ų� ID�� ���� ��쿡�� ���� ó��
        if (attackID != lastAttackID || attackID == -1)
        {
            health -= damage;
            if (health > 0)
            {
                StartCoroutine(FlashSprite());
                StartCoroutine(KnockBack());
                Debug.Log("ü�� ����! ���� ü�� " + health);
            }
            else
            {
                ChangeState(MonsterState.DEAD);
            }

            lastAttackID = attackID;
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
        int randomGold = Random.Range(10, 16);  // 10���� 15 ������ ���� ��� ����
        PlayerStat.Instance.Gold += randomGold;

        // ���� ���� �ʱ�ȭ �� �ִϸ��̼� ó�� (��: ��� �ִϸ��̼� ���)
        rb.velocity = Vector2.zero;
        spriteRenderer.color = Color.white;  // ��������Ʈ ������ ������� (���) ����
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // ��ġ ����
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(1); // ��� �ִϸ��̼� ��� �ð� (��: 1��)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // ��ġ ���� ����

        gameObject.SetActive(false);  // ������Ʈ ��Ȱ��ȭ
    }

    void ChangeState(MonsterState newMonsterState)
    {
        // Debug.Log("���� ��ȯ " + newMonsterState);
        monsterState = newMonsterState;
    }
}
