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
    // ü�� ������ UI �̺�Ʈ �߰�
    public delegate void StatChangeDelegate();
    public event StatChangeDelegate OnHealthChange;
    public event StatChangeDelegate OnEnergyChange;

    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform chargeWeaponPos;

    private static PlayerStat instance;

    public int PlayerID { get { return playerID; } }
    public int MaxHP { get { return maxHP; } }
    public int CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);  // ü�� �ִ�ġ ����
            OnHealthChange?.Invoke(); // ü�� ��ȭ �̺�Ʈ �߻�
        }
    }
    public float Armor { get { return armor; } }
    public int Level { get { return level; } }
    public int Kill { get { return kill; } }
    public float MoveSpeed { get { return moveSpeed; } }   // �̵� �ӵ�
    public float RollSpeed { get { return rollSpeed; } }   // ������ �ӵ�
    public float RollCooltime { get { return rollCooltime; } }   // ������ ��Ÿ��
    public float InvincibleTime { get { return invincibleTime; } }   // ���� �ð�
    public int MaxEnergy { get { return maxEnergy; } }   // �ִ� ���Ÿ� ���� �Ҹ� �ڿ�
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
    [SerializeField] protected int playerID;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int currentHP;
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
            // ��ϵ� ���Ⱑ WeaponManager���� ���ŵǴ� ��� �ش� ���⸦ ���ӿ��� ������ �ı�
            // unRegisterWeapon�� Destroy(weapon) �Ҵ� -> unRegisterWeapon ȣ�� �� Destroy(weapon) ����
            weaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };
            chargeWeaponManager = new ChargeWeaponManager(chargeWeaponPos);
            chargeWeaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };

            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            shadowAnimator = transform.Find("Shadow").GetComponent<Animator>();
            InitStateMachine();
            // �� ��ȯ �ÿ� �ı����� �ʵ��� �����ϴ� �Լ�
            DontDestroyOnLoad(gameObject);
            return;
        }
        // �̹� instance�� ���� �� �ı�
        Destroy(gameObject);
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

    public void OnUpdateStat(int playerID, int maxHP, int currentHP, float armor, int level, int kill, float moveSpeed, float rollSpeed, float rollCooltime, int currentEnergy, int maxEnergy)
    {
        this.playerID = playerID;
        this.maxHP = maxHP;
        CurrentHP = currentHP;
        this.armor = armor;  
        this.level = level;
        this.kill = kill;
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
