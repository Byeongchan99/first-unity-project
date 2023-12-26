using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;
using System.ComponentModel;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance { get { return instance; } }
    public WeaponManager weaponManager { get; private set; }
    public ChargeWeaponManager chargeWeaponManager { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public Animator shadowAnimator { get; private set; }   // 그림자
    public Animator particleAnimator { get; private set; }   // 파티클
    public Animator magicCircleAnimator { get; private set; }   // 마법진
    // 체력 에너지 UI 이벤트 추가
    public delegate void StatChangeDelegate();
    public event StatChangeDelegate OnHealthChange;
    public event StatChangeDelegate OnEnergyChange;

    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform chargeWeaponPos;

    private static PlayerStat instance;

    public int PlayerID { get { return playerID; } }
    public int MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = value;
            // 필요한 추가 로직 (예: 체력 제한 또는 변경 이벤트 호출)
        }
    }
    public int CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);  // 체력 최대치 제한
            OnHealthChange?.Invoke(); // 체력 변화 이벤트 발생
        }
    }
    public float AttackPower
    {
        get { return attackPower; }
        set
        {
            attackPower = Mathf.Max(value, 0);  // 0 미만의 값이면 0으로 설정
        }
    }
    public int Level { get { return level; } }
    public int Kill 
    { 
        get { return kill; }
        set
        {
            kill = value;
        }
    }
    public int Gold { 
        get { return gold; }
        set
        {
            gold = value;
        }
    }
    public float MoveSpeed   // 이동 속도
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = Mathf.Max(value, 0);  // 0 미만의 값이면 0으로 설정
        }
    }
    public float RollSpeed { get { return rollSpeed; } }   // 구르기 속도
    public float RollCooltime { get { return rollCooltime; } }   // 구르기 쿨타임
    public float InvincibleTime { get { return invincibleTime; } }   // 무적 시간
    public int MaxEnergy   // 최대 원거리 공격 소모 자원
    {
        get { return maxEnergy; }
        set
        {
            maxEnergy = value;
            // 필요한 추가 로직 (예: 체력 제한 또는 변경 이벤트 호출)
        }
    }
    public int CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            currentEnergy = Mathf.Clamp(value, 0, maxEnergy); // 에너지 최대치 제한
            OnEnergyChange?.Invoke(); // 에너지 변화 이벤트 발생
        }
    }

    [Header("캐릭터 스탯")]
    public bool isLive;
    [SerializeField] protected int playerID;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int currentHP;
    [SerializeField] protected float attackPower;
    [SerializeField] protected int level;
    [SerializeField] protected int kill;
    [SerializeField] protected int gold;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rollSpeed;
    [SerializeField] protected float rollCooltime;
    [SerializeField] protected float invincibleTime;
    [SerializeField] protected int currentEnergy;
    [SerializeField] protected int maxEnergy;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            weaponManager = new WeaponManager(rightHand);
            // 등록된 무기가 WeaponManager에서 제거되는 경우 해당 무기를 게임에서 완전히 파괴
            // unRegisterWeapon에 Destroy(weapon) 할당 -> unRegisterWeapon 호출 시 Destroy(weapon) 수행
            weaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };
            chargeWeaponManager = new ChargeWeaponManager(chargeWeaponPos);
            chargeWeaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };

            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            shadowAnimator = transform.Find("Shadow").GetComponent<Animator>();
            particleAnimator = transform.Find("Particle").GetComponent <Animator>();
            magicCircleAnimator = transform.Find("MagicCircle").GetComponent<Animator>();
            InitStateMachine();
            // 씬 전환 시에 파괴되지 않도록 지정하는 함수
            DontDestroyOnLoad(gameObject);
            return;
        }
        // 이미 instance가 있을 시 파괴
        Destroy(gameObject);
    }

    void OnEnable()
    {
        isLive = true;
    }

        void Start()
    {
        // InitStateMachine();
    }


    // 각 상태에서 업데이트 되어야 하는 부분들은 유니티 내장 함수들에서 실행
    void Update()
    {
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    public void OnUpdateStat(int playerID, int maxHP, int currentHP, float attackPower, int level, int kill, int gold, float moveSpeed, float rollSpeed, float rollCooltime, int currentEnergy, int maxEnergy)
    {
        this.playerID = playerID;
        this.maxHP = maxHP;
        CurrentHP = currentHP;
        this.attackPower = attackPower;  
        this.level = level;
        this.kill = kill;
        this.gold = gold;
        this.moveSpeed = moveSpeed;
        this.rollSpeed = rollSpeed;
        this.rollCooltime = rollCooltime;
        CurrentEnergy = currentEnergy;
        this.maxEnergy = maxEnergy;
    }

    private void InitStateMachine()
    {
        // 상태들을 만들고 등록
        PlayerController controller = GetComponent<PlayerController>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller));
        stateMachine.AddState(StateName.ROLL, new RollState(controller));
        stateMachine.AddState(StateName.ATTACK, new AttackState(controller));
        stateMachine.AddState(StateName.CHARGE, new ChargeState(controller));
        stateMachine.AddState(StateName.DEAD, new DeadState(controller));
    }
}
