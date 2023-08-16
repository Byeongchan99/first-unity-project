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
        monsterState = MonsterState.CHASE;   // 소환된 몬스터는 곧바로 추적 상태
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
        // A* 알고리즘을 이용한 추적 로직 구현
        while (monsterState == MonsterState.CHASE)
        {
            Vector2Int monsterPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            Vector2Int playerPos = new Vector2Int(Mathf.RoundToInt(target.position.x), Mathf.RoundToInt(target.position.y));

            List<Node> path = astar.PathFinding(monsterPos, playerPos);
            if (path != null && path.Count > 1) // 첫 번째 노드는 현재 위치이므로 두 번째 노드로 이동
            {
                Vector2 nextPosition = new Vector2(path[1].x, path[1].y);
                Vector2 direction = (nextPosition - (Vector2)transform.position).normalized;
                anim.SetFloat("Direction.X", direction.x);
                anim.SetFloat("Direction.Y", direction.y);
                rb.velocity = direction * speed;

                // 몬스터가 플레이어와 충분히 가까워지면 ATTACK 상태로 전환
                if (Vector2.Distance(transform.position, target.position) < 1.0f)
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }
            yield return new WaitForSeconds(0.2f); // 0.2초마다 경로를 업데이트 (빈도 조절 가능)
        }
    }

    IEnumerator ATTACK()
    {
        // 공격 범위에 들어올 시, 콘솔창으로 공격했다고 출력
        Debug.Log("플레이어 공격");
        yield return new WaitForSeconds(1); // 1초 후에 다시 CHASE 상태로 전환
        ChangeState(MonsterState.CHASE);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player의 공격 영역과 충돌한 경우
        if (collision.gameObject.CompareTag("AttackArea") && gameObject.CompareTag("Enemy"))
        {
            // Player의 무기 참조 가져오기 
            // 체력 감소
            health -= PlayerStat.Instance.weaponManager.Weapon.AttackDamage;
            Debug.Log("체력 감소! 남은 체력 " + health);
            // 체력이 0 이하가 되면 처리 (예: 죽음 애니메이션 재생 등)
            if (health <= 0)
            {
                ChangeState(MonsterState.DEAD);
            }
        }
    }

    IEnumerator DEAD()
    {
        Debug.Log("몬스터 사망");
        IsLive = false;
        Destroy(gameObject);
        yield return null;
    }

    void ChangeState(MonsterState newMonsterState)
    {
        monsterState = newMonsterState;
    }
}
