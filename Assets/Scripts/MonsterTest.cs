using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{
    Transform target;
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;   // ���� FixedUpdate���� ��ٸ�

    bool IsLive;
    public float speed;
    public float health;
    public float maxHealth;
    public float attackRange;
    private int lastAttackID = -1;  // ������ ���� AttackArea�� ���� ID
    public Vector2 attackDirection;   // ���� ����
    public Vector2 moveDirection;

    private Astar astar;
    private MonsterAttackArea monsterAttackArea;

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
        astar = GetComponent<Astar>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterAttackArea= gameObject.GetComponentInChildren<MonsterAttackArea>();
        wait = new WaitForFixedUpdate();
    }

    void Start()
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
            Vector2Int monsterPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            Vector2Int playerPos = new Vector2Int(Mathf.RoundToInt(target.position.x), Mathf.RoundToInt(target.position.y));

            List<Node> path = astar.PathFinding(monsterPos, playerPos);
            if (path != null && path.Count > 1) // ù ��° ���� ���� ��ġ�̹Ƿ� �� ��° ���� �̵�
            {
                Vector2 nextPosition = new Vector2(path[1].x, path[1].y);
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

        // ���� ������ ���� ��, �ܼ�â���� �����ߴٰ� ���
        attackDirection = moveDirection;
        monsterAttackArea.ActivateAttackRange(attackDirection, attackRange);
        Debug.Log("�÷��̾� ���� ��");
        yield return new WaitForSeconds(1); // 1�� �Ŀ� �ٽ� CHASE ���·� ��ȯ
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        ChangeState(MonsterState.CHASE);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackArea"))
        {
            // Player�� ���� ������ �浹�� ���
            PlayerAttackArea playerAttackArea = collision.GetComponent<PlayerAttackArea>();
            int currentAttackID = playerAttackArea.GetAttackID();

            // ���� ������ ���� ID�� ���Ͱ� ���������� ���� ���� ID�� �ٸ��� ������ ó��
            if (currentAttackID != lastAttackID)
            {
                health -= PlayerStat.Instance.weaponManager.Weapon.AttackDamage;
                StartCoroutine(FlashSprite());  // �����Ÿ� ����
                StartCoroutine(KnockBack());
                Debug.Log("ü�� ����! ���� ü�� " + health);

                if (health <= 0)
                {
                    ChangeState(MonsterState.DEAD);
                }

                lastAttackID = currentAttackID;  // ���� ���� ID�� ������Ʈ
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
    
    // ���� ���� �ʱ�ȭ �� �ִϸ��̼� ó�� (��: ��� �ִϸ��̼� ���)
    // anim.SetTrigger("Die");  // ���� ��� �ִϸ��̼� Ʈ���Ű� �ִٸ� ���⿡�� ȣ��
    yield return new WaitForSeconds(1); // ��� �ִϸ��̼� ��� �ð� (��: 1��)

    gameObject.SetActive(false);  // ������Ʈ ��Ȱ��ȭ
}


    void ChangeState(MonsterState newMonsterState)
    {
        monsterState = newMonsterState;
    }
}
