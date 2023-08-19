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
    private int attackID = 0; // ������ ���� ID ����
    public Vector2 attackDirection;   // ���� ����
    public Vector2 moveDirection;

    private Astar astar;
    public Transform playerTransform;
    public BoxCollider2D attackRangeCollider;

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
        attackRangeCollider = GetComponent<BoxCollider2D>();
        wait = new WaitForFixedUpdate();
    }

    void Start()
    {
        target = playerTransform;
        IsLive = true;
        health = maxHealth;
        // �ʱ� ���¿����� ���� ���� �ݶ��̴��� ��Ȱ��ȭ
        attackRangeCollider.enabled = false;
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
        rb.constraints = RigidbodyConstraints2D.FreezePosition;  // ��ġ ����

        // ���� ������ ���� ��, �ܼ�â���� �����ߴٰ� ���
        attackDirection = moveDirection;
        AttackRange();
        Debug.Log("�÷��̾� ���� ��");
        yield return new WaitForSeconds(1); // 1�� �Ŀ� �ٽ� CHASE ���·� ��ȯ
        rb.constraints = RigidbodyConstraints2D.None;
        ChangeState(MonsterState.CHASE);
    }

    public void AttackRange()
    {
        attackID++; // ������ �ߵ��� ������ ���� ID ����

        attackRange = 1f;
        // BoxCollider�� ũ�⸦ attackRange�� �°� ����
        attackRangeCollider.size = new Vector2(attackRange, attackRange);
        // attackDirection�� ����Ͽ� AttackRangeObject�� ȸ����ŵ�ϴ�.
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        attackRangeCollider.enabled = true;  // ���� �ݶ��̴� Ȱ��ȭ
    }

    // �ٸ� ��ü�� ���� ID�� ������ �� �ֵ��� getter ����
    public int GetAttackID()
    {
        return attackID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AttackArea"))
        {
            // Player�� ���� ������ �浹�� ���
            AttackArea attackArea = collision.GetComponent<AttackArea>();
            int currentAttackID = attackArea.GetAttackID();

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
        for (int i = 0; i < 4; i++) // 4�� �����̰� �� (�ʿ信 ���� ����)
        {
            if (i % 2 == 0)
                spriteRenderer.color = Color.red;  // ���������� ����
            else
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            yield return new WaitForSeconds(0.1f);
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
        Destroy(gameObject);
        yield return null;
    }

    void ChangeState(MonsterState newMonsterState)
    {
        monsterState = newMonsterState;
    }
}
