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
    WaitForFixedUpdate wait;   // 다음 FixedUpdate까지 기다림

    bool IsLive;
    public float speed;
    public float health;
    public float maxHealth;
    public float attackRange;
    private int lastAttackID = -1;  // 이전에 받은 AttackArea의 공격 ID
    public Vector2 attackDirection;   // 공격 방향
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
                moveDirection = (nextPosition - (Vector2)transform.position).normalized;
                anim.SetFloat("Direction.X", moveDirection.x);
                anim.SetFloat("Direction.Y", moveDirection.y);
                rb.velocity = moveDirection * speed;

                // 몬스터가 플레이어와 충분히 가까워지면 ATTACK 상태로 전환
                if (Vector2.Distance(transform.position, target.position) < 1.0f)
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }
            yield return new WaitForSeconds(0.1f); // 0.1초마다 경로를 업데이트 (빈도 조절 가능)
        }
    }

    IEnumerator ATTACK()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정

        // 공격 범위에 들어올 시, 콘솔창으로 공격했다고 출력
        attackDirection = moveDirection;
        monsterAttackArea.ActivateAttackRange(attackDirection, attackRange);
        Debug.Log("플레이어 공격 중");
        yield return new WaitForSeconds(1); // 1초 후에 다시 CHASE 상태로 전환
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        ChangeState(MonsterState.CHASE);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackArea"))
        {
            // Player의 공격 영역과 충돌한 경우
            PlayerAttackArea playerAttackArea = collision.GetComponent<PlayerAttackArea>();
            int currentAttackID = playerAttackArea.GetAttackID();

            // 만약 현재의 공격 ID가 몬스터가 마지막으로 받은 공격 ID와 다르면 데미지 처리
            if (currentAttackID != lastAttackID)
            {
                health -= PlayerStat.Instance.weaponManager.Weapon.AttackDamage;
                StartCoroutine(FlashSprite());  // 깜빡거림 시작
                StartCoroutine(KnockBack());
                Debug.Log("체력 감소! 남은 체력 " + health);

                if (health <= 0)
                {
                    ChangeState(MonsterState.DEAD);
                }

                lastAttackID = currentAttackID;  // 현재 공격 ID로 업데이트
            }
        }
    }

    // 스프라이트 깜빡거리기
    IEnumerator FlashSprite()
    {
        float elapsedTime = 0;
        float flashTime = 0.5f;
        bool isRed = false;

        while (elapsedTime < flashTime) 
        {
            if (isRed)
            {
                spriteRenderer.color = Color.red;  // 빨간색으로 변경
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

        spriteRenderer.color = Color.white;  // 마지막으로 스프라이트 색상을 원래대로 (흰색) 변경
    }


    // 피격 시 약간 밀려남
    IEnumerator KnockBack()
    {
        Vector2 playerPos = target.position;
        Vector2 dirVec = ((Vector2)transform.position - playerPos).normalized;

        float knockbackSpeed = 3.0f;  // 조절 가능한 넉백 속도
        rb.velocity = Vector2.zero;  // 넉백 전 정지
        rb.velocity = dirVec * knockbackSpeed;

        yield return new WaitForSeconds(0.1f);  // 넉백 지속 시간 (0.1초로 설정)   
    }

IEnumerator DEAD()
{
    Debug.Log("몬스터 사망");
    IsLive = false;
    
    // 몬스터 상태 초기화 및 애니메이션 처리 (예: 사망 애니메이션 재생)
    // anim.SetTrigger("Die");  // 만약 사망 애니메이션 트리거가 있다면 여기에서 호출
    yield return new WaitForSeconds(1); // 사망 애니메이션 재생 시간 (예: 1초)

    gameObject.SetActive(false);  // 오브젝트 비활성화
}


    void ChangeState(MonsterState newMonsterState)
    {
        monsterState = newMonsterState;
    }
}
