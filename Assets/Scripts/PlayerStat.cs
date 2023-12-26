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
    public Animator shadowAnimator { get; private set; }   // �׸���
    public Animator particleAnimator { get; private set; }   // ��ƼŬ
    public Animator magicCircleAnimator { get; private set; }   // ������
    // ü�� ������ UI �̺�Ʈ �߰�
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
            // �ʿ��� �߰� ���� (��: ü�� ���� �Ǵ� ���� �̺�Ʈ ȣ��)
        }
    }
    public int CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);  // ü�� �ִ�ġ ����
            OnHealthChange?.Invoke(); // ü�� ��ȭ �̺�Ʈ �߻�
        }
    }
    public float AttackPower
    {
        get { return attackPower; }
        set
        {
            attackPower = Mathf.Max(value, 0);  // 0 �̸��� ���̸� 0���� ����
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
    public float MoveSpeed   // �̵� �ӵ�
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = Mathf.Max(value, 0);  // 0 �̸��� ���̸� 0���� ����
        }
    }
    public float RollSpeed { get { return rollSpeed; } }   // ������ �ӵ�
    public float RollCooltime { get { return rollCooltime; } }   // ������ ��Ÿ��
    public float InvincibleTime { get { return invincibleTime; } }   // ���� �ð�
    public int MaxEnergy   // �ִ� ���Ÿ� ���� �Ҹ� �ڿ�
    {
        get { return maxEnergy; }
        set
        {
            maxEnergy = value;
            // �ʿ��� �߰� ���� (��: ü�� ���� �Ǵ� ���� �̺�Ʈ ȣ��)
        }
    }
    public int CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            currentEnergy = Mathf.Clamp(value, 0, maxEnergy); // ������ �ִ�ġ ����
            OnEnergyChange?.Invoke(); // ������ ��ȭ �̺�Ʈ �߻�
        }
    }

    [Header("ĳ���� ����")]
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
            // ��ϵ� ���Ⱑ WeaponManager���� ���ŵǴ� ��� �ش� ���⸦ ���ӿ��� ������ �ı�
            // unRegisterWeapon�� Destroy(weapon) �Ҵ� -> unRegisterWeapon ȣ�� �� Destroy(weapon) ����
            weaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };
            chargeWeaponManager = new ChargeWeaponManager(chargeWeaponPos);
            chargeWeaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };

            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            shadowAnimator = transform.Find("Shadow").GetComponent<Animator>();
            particleAnimator = transform.Find("Particle").GetComponent <Animator>();
            magicCircleAnimator = transform.Find("MagicCircle").GetComponent<Animator>();
            InitStateMachine();
            // �� ��ȯ �ÿ� �ı����� �ʵ��� �����ϴ� �Լ�
            DontDestroyOnLoad(gameObject);
            return;
        }
        // �̹� instance�� ���� �� �ı�
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


    // �� ���¿��� ������Ʈ �Ǿ�� �ϴ� �κе��� ����Ƽ ���� �Լ��鿡�� ����
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
        // ���µ��� ����� ���
        PlayerController controller = GetComponent<PlayerController>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller));
        stateMachine.AddState(StateName.ROLL, new RollState(controller));
        stateMachine.AddState(StateName.ATTACK, new AttackState(controller));
        stateMachine.AddState(StateName.CHARGE, new ChargeState(controller));
        stateMachine.AddState(StateName.DEAD, new DeadState(controller));
    }
}
