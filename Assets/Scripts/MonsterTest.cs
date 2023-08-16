using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{
    Transform target;
    Animator anim;
    Rigidbody2D rb;

    bool IsLive;
    public float speed;
    public float health;
    public float maxHealth;

    private Astar astar;
    public Transform playerTransform;

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
    }

    void Start()
    {
        target = playerTransform;
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
                Vector2 direction = (nextPosition - (Vector2)transform.position).normalized;
                anim.SetFloat("Direction.X", direction.x);
                anim.SetFloat("Direction.Y", direction.y);
                rb.velocity = direction * speed;

                // ���Ͱ� �÷��̾�� ����� ��������� ATTACK ���·� ��ȯ
                if (Vector2.Distance(transform.position, target.position) < 1.0f)
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }
            yield return new WaitForSeconds(0.2f); // 0.2�ʸ��� ��θ� ������Ʈ (�� ���� ����)
        }
    }

    IEnumerator ATTACK()
    {
        // ���� ������ ���� ��, �ܼ�â���� �����ߴٰ� ���
        Debug.Log("�÷��̾� ����");
        yield return new WaitForSeconds(1); // 1�� �Ŀ� �ٽ� CHASE ���·� ��ȯ
        ChangeState(MonsterState.CHASE);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player�� ���� ������ �浹�� ���
        if (collision.gameObject.CompareTag("AttackArea") && gameObject.CompareTag("Enemy"))
        {
            // Player�� ���� ���� �������� 
            // ü�� ����
            health -= PlayerStat.Instance.weaponManager.Weapon.AttackDamage;
            Debug.Log("ü�� ����! ���� ü�� " + health);
            // ü���� 0 ���ϰ� �Ǹ� ó�� (��: ���� �ִϸ��̼� ��� ��)
            if (health <= 0)
            {
                ChangeState(MonsterState.DEAD);
            }
        }
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
