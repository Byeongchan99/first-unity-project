using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance { get { return instance; } }
    public WeaponManager weaponManager { get; private set; }
    public ChargeWeaponManager chargeWeaponManager { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public Animator shadowAnimator { get; private set; }

    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform chargeWeaponPos;
    private static PlayerStat instance;

    public int PlayerID { get { return playerID; } }
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { 
        get { return currentHP; }
        set { currentHP = value; }
    }
    public float Armor { get { return armor; } }
    public int Level { get { return level; } }
    public int Kill { get { return kill; } }
    public float MoveSpeed { get { return moveSpeed; } }   // 이동 속도
    public float RollSpeed { get { return rollSpeed; } }   // 구르기 속도
    public float RollCooltime { get { return rollCooltime; } }   // 구르기 쿨타임
    public float InvincibleTime { get { return invincibleTime; } }   // 무적 시간
    public int MaxEnergy { get { return maxEnergy; } }   // 최대 원거리 공격 소모 자원
    public int CurrentEnergy   // 현재 원거리 공격 소모 자원
    {
        get { return currentEnergy; }
        set { currentEnergy = value; }
    }

    [Header("캐릭터 스탯")]
    [SerializeField] protected int playerID;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float armor;
    [SerializeField] protected int level;
    [SerializeField] protected int kill;
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
            InitStateMachine();
            // 씬 전환 시에 파괴되지 않도록 지정하는 함수
            DontDestroyOnLoad(gameObject);
            return;
        }
        // 이미 instance가 있을 시 파괴
        Destroy(gameObject);
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

    public void OnUpdateStat(int playerID, float maxHP, float currentHP, float armor, int level, int kill, float moveSpeed, float rollSpeed, float rollCooltime, int currentEnergy, int maxEnergy)
    {
        this.playerID = playerID;
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.armor = armor;  
        this.level = level;
        this.kill = kill;
        this.moveSpeed = moveSpeed;
        this.rollSpeed = rollSpeed;
        this.rollCooltime = rollCooltime;
        this.currentEnergy = currentEnergy;
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
