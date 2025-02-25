using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MonsterBase : MonoBehaviour
{
    protected Transform target;
    protected Animator anim;
    public Rigidbody2D rb;
    private Astar astarComponent;
    protected MonsterAttackArea monsterAttackArea;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;   // 다음 FixedUpdate까지 기다림

    [Header("스텟 관련")]
    protected bool IsLive;
    public float speed;
    public float health;
    public float maxHealth;
    public int chaseType; // A* 알고리즘 추적 타입

    [Header("공격 관련")]
    public float attackDetectionRange;   // 공격 인식 범위
    public float attackRange;
    private int lastAttackID = -1;  // 이전에 받은 AttackArea의 공격 ID
    public float attackTiming;   // 공격 타이밍
    public float totalAttackTime;   // 총 공격 시간 = chargeTime + attackDuration/rushDuration + stunTime
    public float attackDuration;  // 애니메이션 공격 모션 지속 시간
    public Vector2 attackDirection;   // 공격 방향
    public Vector2 attackBasePosition;   // 공격 범위 중심
    public float attackColliderOffset;   // 공격 범위 콜라이더 이동 거리
    private Coroutine attackPatternCoroutine;   // 공격 패턴 코루틴

    public Vector2 moveDirection;

    [Header("사운드 관련")]
    //public AudioClip spawnSound;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
    protected AudioSource audioSource;

    // 몬스터 상태
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
        audioSource = GetComponent<AudioSource>();
        wait = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        IsLive = true;
        health = maxHealth;
        chaseType = Random.Range(0, 3);  // 0 ~ 2 사이의 랜덤한 값으로 추적 타입 설정
    }

    // 몬스터를 활성화하는 코드
    public void ActivateMonster()
    {
        //audioSource.PlayOneShot(spawnSound);
        target = PlayerStat.Instance.transform;
        monsterState = MonsterState.CHASE;
        StartCoroutine(StateMachine());
    }

    // 상태 머신
    IEnumerator StateMachine()
    {     
        while (IsLive && !GameManager.instance.gameOver)
        {
            yield return StartCoroutine(monsterState.ToString());
        }
    }

    // 추적 상태
    IEnumerator CHASE()
    {
        while (monsterState == MonsterState.CHASE)
        {
            // 몬스터와 플레이어의 현재 위치 계산
            Vector2Int monsterPos = astarComponent.WorldToTilemapPosition(transform.position);
            Vector2Int playerPos = astarComponent.WorldToTilemapPosition(target.position);

            List<Node> path = astarComponent.PathFinding(monsterPos, playerPos, chaseType);   // Astar 알고리즘으로 길 찾기

            // 플레이어가 벽에 들어가 있을 때
            if (astarComponent.playerInWall)
            {
                // 플레이어를 향해 직선으로 이동
                moveDirection = (target.position - transform.position).normalized;
                anim.SetFloat("Direction.X", moveDirection.x);
                anim.SetFloat("Direction.Y", moveDirection.y);
                rb.velocity = moveDirection * speed;

                if (Vector2.Distance(transform.position, target.position) < attackDetectionRange)
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }
            else if (path != null && path.Count > 1)   // 길이 있을 때
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

            yield return new WaitForSeconds(0.1f);   // 코루틴이 일정 시간마다 대기
        }
    }

    // 공격 상태
    IEnumerator ATTACK()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;  // 위치 고정    

        yield return StartCoroutine(AttackPattern());  // 몬스터 공격 패턴 실행

        /* 공격 패턴 예시
        yield return new WaitForSeconds(attackTiming);  // 공격 타이밍에 공격 범위 콜라이더 활성화
        monsterAttackArea.ActivateAttackRange(attackDirection);   // 공격 범위 활성화

        yield return new WaitForSeconds(0.2f);
        monsterAttackArea.attackRangeCollider.enabled = false;   // 공격 범위 콜라이더 비활성화

        yield return new WaitForSeconds(attackDuration - attackTiming - 0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제
        */

        if (health <= 0)
            ChangeState(MonsterState.DEAD);  // DEAD 상태로 전환
        else
            ChangeState(MonsterState.CHASE);  // CHASE 상태로 전환
    }

    public abstract IEnumerator AttackPattern();  // 몬스터 공격 패턴


    // 공격할 때 약간 전진 - 콜라이더 무시 현상 해결
    protected void MoveForward()
    {
        // Debug.Log("약간 전진");
        float advanceDistance = 0.1f;   // 조절 가능한 전진 거리
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

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerAttackArea":  // 플레이어의 근접 공격에 맞았을 때
                ProcessDamage(collision.GetComponent<PlayerAttackArea>().GetAttackID(),
                    PlayerStat.Instance.weaponManager.Weapon.AttackDamage + PlayerStat.Instance.AttackPower);
                break;

            case "Bullet":  // 플레이어의 원거리 공격에 맞았을 때
                ProcessDamage(-1, collision.GetComponent<Bullet>().Damage); // -1은 고유 ID가 없음을 나타냄
                break;

            case "ExplosionArea":  // 폭발 범위에 맞았을 때
                ProcessDamage(-1, collision.transform.parent.GetComponent<FireBolt>().explosionDamage);
                break;         
        }
    }

    // 피해 처리
    private void ProcessDamage(int attackID, float damage)
    {
        // 공격 ID가 다르거나 ID가 없는 경우에만 피해 처리
        if (attackID != lastAttackID || attackID == -1)
        {
            health -= damage;
            audioSource.PlayOneShot(hitSound);
            if (health > 0)
            {
                StartCoroutine(FlashSprite());
                StartCoroutine(KnockBack());
                Debug.Log("체력 감소! 남은 체력 " + health);
            }
            else
            {
                ChangeState(MonsterState.DEAD);
            }

            lastAttackID = attackID;
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

    // 사망 상태
    IEnumerator DEAD()
    {
        Debug.Log("몬스터 사망");
        IsLive = false;
        audioSource.PlayOneShot(deathSound);
        WaveManager.Instance.OnMonsterDeath();
        int randomGold = Random.Range(9, 16);  // 9에서 15 사이의 값 사용
        PlayerStat.Instance.Gold += randomGold;

        // 몬스터 상태 초기화 및 애니메이션 처리
        rb.velocity = Vector2.zero;
        spriteRenderer.color = Color.white;  // 스프라이트 색상을 원래대로 (흰색) 변경
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;   // 위치 고정
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(1); // 사망 애니메이션 재생 시간
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;   // 위치 고정 해제

        gameObject.SetActive(false);  // 오브젝트 비활성화
    }

    // 상태 전환
    void ChangeState(MonsterState newMonsterState)
    {
        // Debug.Log("상태 전환 " + newMonsterState);
        monsterState = newMonsterState;
    }
}
