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
    WaitForFixedUpdate wait;   // 다음 FixedUpdate까지 기다림

    bool IsLive;
    public float speed;
    public float health;
    public float maxHealth;
    public float attackRange;
    private int lastAttackID = -1;  // 이전에 받은 AttackArea의 공격 ID
    public float attackTiming;   // 공격 타이밍
    public float attackDuration;  // 애니메이션 공격 지속 시간

    public Vector2 attackDirection;   // 공격 방향
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
        astarComponent = GetComponent<Astar>();  // Astar 컴포넌트를 가져옵니다.
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
            Vector2Int monsterPos = astarComponent.WorldToTilemapPosition(transform.position);
            Vector2Int playerPos = astarComponent.WorldToTilemapPosition(target.position);

            List<Node> path = astarComponent.PathFinding(monsterPos, playerPos);  // Astar 컴포넌트를 이용해 경로 탐색

            if (path != null && path.Count > 1) // 첫 번째 노드는 현재 위치이므로 두 번째 노드로 이동
            {
                Vector2 nextPosition = astarComponent.TilemapToWorldPosition(new Vector2Int(path[1].x, path[1].y));
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

        attackDirection = moveDirection;
        anim.SetBool("IsAttack", true);

        yield return StartCoroutine(AttackPattern());

        /*
        yield return new WaitForSeconds(attackTiming);  // 공격 타이밍에 공격 범위 콜라이더 활성화
        monsterAttackArea.ActivateAttackRange(attackDirection);   // 공격 범위 활성화

        yield return new WaitForSeconds(0.2f);
        monsterAttackArea.attackRangeCollider.enabled = false;   // 공격 범위 콜라이더 비활성화

        yield return new WaitForSeconds(attackDuration - attackTiming - 0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제
        */

        anim.SetBool("IsAttack", false);
        ChangeState(MonsterState.CHASE);   // CHASE 상태로 전환
    }

    public abstract IEnumerator AttackPattern();  // 몬스터 공격 패턴

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PlayerAttackArea") && !collision.CompareTag("Bullet") && !collision.CompareTag("ExplosionArea")) 
            return;

        if (collision.gameObject.CompareTag("PlayerAttackArea"))
        {
            // Player의 공격 영역과 충돌한 경우
            PlayerAttackArea playerAttackArea = collision.GetComponent<PlayerAttackArea>();
            int currentAttackID = playerAttackArea.GetAttackID();

            // 만약 현재의 공격 ID가 몬스터가 마지막으로 받은 공격 ID와 다르면 데미지 처리
            if (currentAttackID != lastAttackID)
            {
                /*
                health -= PlayerStat.Instance.weaponManager.Weapon.AttackDamage;
                StartCoroutine(FlashSprite());  // 깜빡거림 시작
                StartCoroutine(KnockBack());
                Debug.Log("체력 감소! 남은 체력 " + health);

                if (health <= 0)
                {
                    ChangeState(MonsterState.DEAD);
                }
                */
                health -= (PlayerStat.Instance.weaponManager.Weapon.AttackDamage + PlayerStat.Instance.AttackPower);   // 데미지 = 무기 공격력 + 플레이어 공격력

                if (health > 0)
                {                   
                    StartCoroutine(FlashSprite());  // 깜빡거림 시작
                    StartCoroutine(KnockBack());
                    Debug.Log("체력 감소! 남은 체력 " + health);
                }
                else
                {
                    ChangeState(MonsterState.DEAD);
                }

                lastAttackID = currentAttackID;  // 현재 공격 ID로 업데이트
            }
        }

        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<Bullet>().Damage;

            if (health > 0)
            {
                StartCoroutine(FlashSprite());  // 깜빡거림 시작
                StartCoroutine(KnockBack());
                Debug.Log("체력 감소! 남은 체력 " + health);
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
                StartCoroutine(FlashSprite());  // 깜빡거림 시작
                StartCoroutine(KnockBack());
                Debug.Log("체력 감소! 남은 체력 " + health);
            }
            else
            {
                ChangeState(MonsterState.DEAD);
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
        WaveManager.Instance.OnMonsterDeath();
        // 몬스터 상태 초기화 및 애니메이션 처리 (예: 사망 애니메이션 재생)
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(1); // 사망 애니메이션 재생 시간 (예: 1초)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제

        gameObject.SetActive(false);  // 오브젝트 비활성화
    }

    void ChangeState(MonsterState newMonsterState)
    {
        monsterState = newMonsterState;
    }
}
