using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
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
    private bool isPointerOverUI = false;   // UI 위에 마우스가 있는지 확인

    [Header("ChargeWeapon 관련")]
    public static Transform ChargeWeaponPosition;
    public static Transform leftHand;
    public static Transform rightHand;

    [Header("상점")]
    private bool isNearShop = false;   // 상점 상호작용

    [Header("기타 상호작용")]
    private bool isNearPortal = false;   // 포탈 상호작용
    private bool isNearNPC = false;   // NPC 상호작용
    public bool isSystemNotice = false;   // 시스템 공지
    public GameObject currentNearNPC;
    public int transitionToStageIndex;

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

    // 싱글톤 파괴 메서드
    public void DestroyInstance()
    {
        Destroy(gameObject);
        Instance = null;
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

        isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    // 일시정지
    void OnPause()
    {
        if (UIManager.instance.pauseMenuUI.isPaused)   // 일시정지 창이 켜져있을 때
        {
            if (UIManager.instance.pauseMenuUI.isOpenedSoundSetting)   // 사운드 설정 창이 켜져있을 때
            {
                UIManager.instance.pauseMenuUI.soundSettingPanel.OnExitButton();
                UIManager.instance.pauseMenuUI.isOpenedSoundSetting = false;
            }
            else if (UIManager.instance.pauseMenuUI.isOpenedGameExitConfirm)   // 게임종료 확인 창이 켜져있을 때
            {
                UIManager.instance.pauseMenuUI.GameExitCancel();
                UIManager.instance.pauseMenuUI.isOpenedGameExitConfirm = false;
            }
            else   // 일시정지 창만 켜져있을 때
            {
                UIManager.instance.pauseMenuUI.Hide();
            }
        }
        else   // 일시정지 창이 꺼져있을 때
        {
            UIManager.instance.pauseMenuUI.Show();
        }
        UIManager.instance.pauseMenuUI.isPaused = !UIManager.instance.pauseMenuUI.isPaused;
    }

    // 상호작용
    void OnInteract()
    {
        if (isNearShop)
        {
            UIManager.instance.shopUI.Show();
            //UIManager.instance.shopUI.DisplayRandomShopItems();
        }

        if (isNearPortal)
        {
            StageManager.Instance.TransitionToStage(transitionToStageIndex);
        }

        if (isNearNPC)   // NPC 대화 상호작용
        {
            NPCDialogue npcDialogue = currentNearNPC.GetComponent<NPCDialogue>();

            if (npcDialogue != null)
            {
                Debug.Log("NPC와 상호작용");
                npcDialogue.ShowDialogue(); // NPCDialogue의 상호작용 메서드 호출
            }

            InteractiveRunePillar interactiveRunePillar = currentNearNPC.GetComponent<InteractiveRunePillar>();

            if (interactiveRunePillar != null)
            {
                Debug.Log("봉인석과 상호작용");
                isSystemNotice = true;
                interactiveRunePillar.Interaction(); // interactiveRunePillar의 상호작용 메서드 호출
            }
        }
    }

    // 맵 보기
    void OnMap()
    {
        if (UIManager.instance.mapBackgroundUI.isOpened)   // 맵이 켜져있을 때
        {
            UIManager.instance.mapBackgroundUI.Hide();
        }
        else   // 맵이 꺼져있을 때
        {
            UIManager.instance.mapBackgroundUI.Show();
        }
    }

    // 이동
    void OnMove(InputValue value)
    {
        // 후처리로 normalized 해줌
        inputVec = value.Get<Vector2>();
    }

    // 구르기
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
            //Debug.Log("구른다");
            playerStat.stateMachine.ChangeState(StateName.ROLL);
        } 
    }

    // 차지 공격
    void OnCharge()
    {
        if (RollState.IsRoll || AttackState.IsAttack || PlayerStat.Instance.CurrentEnergy < 1 || StageManager.Instance.currentStage.stageType == "Hub") 
            return;

        playerStat.stateMachine.ChangeState(StateName.CHARGE);
    }

    private bool comboAttackTriggered = false;
    // 새로운 입력 시스템의 Callback으로 사용됩니다.
    void OnAttack()
    {
        // UI 위에 마우스가 있으면 리턴
        if (isPointerOverUI)
        {
            return;
        }

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
        if (!GameManager.instance.isLive)
            return;

        if (collision.CompareTag("ShopInteractionRange")) // 상점 상호작용 범위 확인
        {
            HandleShopInteraction();
            return;
        }

        if (collision.CompareTag("NPCInteractionRange")) // NPC 상호작용 범위 확인
        {
            currentNearNPC = collision.gameObject.transform.parent.gameObject;
            HandleNPCInteraction();
            return;
        }

        // 무적 시간이거나 구르고 있을 경우 피해 무시
        if (GameManager.instance.isInvincible || RollState.IsRoll)
            return;

        switch (collision.gameObject.tag)
        {
            case "MonsterAttackArea":   // 일반 몬스터 공격 범위
                HandleMonsterAttack(collision);
                break;
            case "ExplosionArea":   // 폭발 범위
            case "MonsterBullet":   // 몬스터 원거리 공격
                ApplyDamage();
                break;
            case "BossAttackArea":   // 보스 몬스터 공격 범위
                HandleBossAttack(collision);
                break;
        }
    }

    public void ApplyDamage()
    {
        // 무적 시간이거나 구르고 있을 경우 피해 무시
        if (GameManager.instance.isInvincible || RollState.IsRoll)
            return;

        PlayerStat.Instance.CurrentHP -= 1;
        Debug.Log("플레이어 체력 감소! 남은 체력 " + PlayerStat.Instance.CurrentHP);

        if (PlayerStat.Instance.CurrentHP <= 0)
        {
            PlayerStat.Instance.rigidBody.velocity = Vector2.zero;
            playerStat.stateMachine.ChangeState(StateName.DEAD);
        }

        // 피격 이벤트 실행
        playerStat.animator.SetTrigger("Hit");
        StartCoroutine(GetHitRoutine());
    }

    void HandleMonsterAttack(Collider2D collision)
    {
        MonsterAttackArea monsterAttackArea = collision.GetComponent<MonsterAttackArea>();
        int currentAttackID = monsterAttackArea.GetAttackID();
        if (currentAttackID != lastAttackID)
        {
            ApplyDamage();
            lastAttackID = currentAttackID;
        }
    }

    void HandleBossAttack(Collider2D collision)
    {
        // 보스 공격 처리 로직
        ApplyDamage();
        // lastAttackID 업데이트 필요한 경우 추가
    }

    // 상점 상호작용 범위 들어올 때
    void HandleShopInteraction()
    {
        // 상점 상호작용 로직
        isNearShop = true;
    }

    void HandleNPCInteraction()
    {
        // 상점 상호작용 로직
        isNearNPC = true;
    }

    // 오브젝트 상호작용 범위 벗어날 때
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ShopInteractionRange"))
        {
            isNearShop = false;
        }

        if (collision.CompareTag("NPCInteractionRange"))
        {
            isNearNPC = false;

            if (!isSystemNotice)   // 시스템 공지가 아닌 경우
            {
                UIManager.instance.dialogueUI.Hide();   
            }
        }
    }

    // 포탈 상호작용 설정
    public void SetNearPortal(bool isNear, int stageIndex)
    {
        isNearPortal = isNear;
        transitionToStageIndex = stageIndex; // 포탈 인덱스 업데이트
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
        
        if (!PlayerStat.Instance.isLive)
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
