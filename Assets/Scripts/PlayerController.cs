using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterController;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    SpriteRenderer spriteRenderer;

    public PlayerStat playerStat;

    [Header("이동 관련")]
    public Vector2 inputVec;   // 입력 방향값

    [Header("구르기 관련")]
    public Vector2 rollDirection;   // 구르기 방향

    [Header("마우스 위치")]
    public Vector3 mousePos;   // 마우스 위치
    public Vector3 mouseDirection;   // 마우스 방향

    [Header("공격 관련")]
    public Vector2 attackDirection;   // 공격 방향
    private int lastAttackID = -1;  // 이전에 받은 AttackArea의 공격 ID

    [Header("ChargeWeapon 관련")]
    public static Transform ChargeWeaponPosition;
    public static Transform leftHand;
    public static Transform rightHand;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStat = GetComponent<PlayerStat>();
        ChargeWeaponPosition = transform.Find("ChargeWeaponPosition");
        leftHand = ChargeWeaponPosition.Find("LeftHand");
        rightHand = ChargeWeaponPosition.Find("RightHand");

        // SpriteRenderer 비활성화
        leftHand.GetComponent<SpriteRenderer>().enabled = false;
        rightHand.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Start()
    {    
        // rollState = playerStat.stateMachine.GetState(StateName.ROLL) as RollState;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 위치
        mousePos = Mouse.current.position.ReadValue();
        // 스크린 좌표를 월드 좌표로 변환
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // 2D 게임이므로 z좌표 0으로 변경
        mousePos.z = 0;

        // 마우스 방향 벡터 정규화
        mouseDirection = (mousePos - transform.position).normalized;
        // 각도에 맞는 애니메이션
        playerStat.animator.SetFloat("MouseDirection.X", mouseDirection.x);
        playerStat.animator.SetFloat("MouseDirection.Y", mouseDirection.y);
    }

    void OnMove(InputValue value)
    {
        // 후처리로 normalized 해줌
        inputVec = value.Get<Vector2>();
    }

    void OnRoll()
    {
        if (AttackState.IsAttack || ChargeState.IsCharge || !RollState.canRoll) 
            return;

        rollDirection = inputVec;

        // 입력 방향이 (0, 0)이면 리턴
        if (rollDirection == Vector2.zero)
        {
            Debug.Log("Player is standing still, can't roll.");
            return;
        }

       // 구르고 있지 않을 때
       if (!RollState.IsRoll)
        {
            Debug.Log("대시 처음 발동");
            playerStat.stateMachine.ChangeState(StateName.ROLL);
        } 
    }

    void OnCharge()
    {
        if (RollState.IsRoll || AttackState.IsAttack || PlayerStat.Instance.CurrentEnergy < 1) 
            return;

        playerStat.stateMachine.ChangeState(StateName.CHARGE);
    }

    private bool comboAttackTriggered = false;
    // 새로운 입력 시스템의 Callback으로 사용됩니다.
    void OnAttack()
    {
        if (RollState.IsRoll || ChargeState.IsCharge)
            return;

        attackDirection = mouseDirection;

        if (playerStat.weaponManager.Weapon.ComboCount < 3)
        {
            // 공격 상태가 아닐 때
            if (!AttackState.IsAttack)
            {
                //Debug.Log("공격 상태로 전환");
                playerStat.stateMachine.ChangeState(StateName.ATTACK);
            }
            else   // 공격 중일 때
            {
                comboAttackTriggered = true;
            }
        }
    }

    public bool OnAttackWasTriggered()
    {
        //Debug.Log("OnAttackWasTriggered 호출");
        if (comboAttackTriggered)
        {
            comboAttackTriggered = false; // 입력이 감지된 후에는 리셋
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MonsterAttackArea"))
        {
            // 몬스터의 공격 영역과 충돌한 경우
            MonsterAttackArea monsterAttackArea = collision.GetComponent<MonsterAttackArea>();
            int currentAttackID = monsterAttackArea.GetAttackID();

            // 만약 현재의 공격 ID가 몬스터가 마지막으로 받은 공격 ID와 다르면 데미지 처리
            if (currentAttackID != lastAttackID)
            {
                // 현재 무적 시간이면 피해 무시
                if (GameManager.instance.isInvincible) 
                    return;

                PlayerStat.Instance.CurrentHP -= 1;   // 나중에 몬스터의 공격력을 넣어주도록 업데이트
                Debug.Log("체력 감소! 남은 체력 " + PlayerStat.Instance.CurrentHP);

                if (PlayerStat.Instance.CurrentHP <= 0)
                {
                    PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
                    playerStat.stateMachine.ChangeState(StateName.DEAD);
                }

                // 피격 이벤트 실행
                playerStat.animator.SetTrigger("Hit");
                StartCoroutine(GetHitRoutine());
                lastAttackID = currentAttackID;  // 현재 공격 ID로 업데이트
            }

            return;
        }

        if (collision.CompareTag("ExplosionArea"))
        {
            // 현재 무적 시간이면 피해 무시
            if (GameManager.instance.isInvincible)
                return;

            PlayerStat.Instance.CurrentHP -= 1;   // 나중에 몬스터의 공격력을 넣어주도록 업데이트
            Debug.Log("체력 감소! 남은 체력 " + PlayerStat.Instance.CurrentHP);

            if (PlayerStat.Instance.CurrentHP <= 0)
            {
                PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
                playerStat.stateMachine.ChangeState(StateName.DEAD);
            }

            // 피격 이벤트 실행
            playerStat.animator.SetTrigger("Hit");
            StartCoroutine(GetHitRoutine());
            return;
        }
    }

    // 무적 시간 코루틴
    private IEnumerator GetHitRoutine()
    {
        GameManager.instance.isInvincible = true;

        // Player와 Monster 레이어 간의 충돌을 무시
        int playerStandLayer = LayerMask.NameToLayer("PlayerStandArea");
        int monsterStandLayer = LayerMask.NameToLayer("MonsterStandArea");
        Physics2D.IgnoreLayerCollision(playerStandLayer, monsterStandLayer, true);

        // 무적 시간 동안 깜빡거리게 함
        StartCoroutine(FlashSprite());
        yield return new WaitForSeconds(PlayerStat.Instance.InvincibleTime);  // 1.5초 대기 - 무적 시간 1.5초

        GameManager.instance.isInvincible = false;
        // Player와 Monster 레이어 간의 충돌을 다시 활성화
        Physics2D.IgnoreLayerCollision(playerStandLayer, monsterStandLayer, false);
    }

    // 무적 시간 동안 스프라이트 깜빡거리기
    IEnumerator FlashSprite()
    {
        float elapsedTime = 0;
        bool isRed = false;
        
        if (!GameManager.instance.isLive)
        {
            spriteRenderer.color = Color.white; // 원래 색상으로 변경하고 코루틴 종료
            yield break;
        }

        while (elapsedTime < PlayerStat.Instance.InvincibleTime)
        {
            if (isRed)
            {
                spriteRenderer.color = Color.red;  // 빨간색으로 변경
                isRed = false;
            }
            else
            {
                spriteRenderer.color = new Color32(255, 255, 255, 90);
                isRed = true;
            }
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        spriteRenderer.color = Color.white;  // 마지막으로 스프라이트 색상을 원래대로 (흰색) 변경
    }

    // 사망 테스트 코드
    void OnTestDead()
    {
        playerStat.stateMachine.ChangeState(StateName.DEAD);
    }
}
