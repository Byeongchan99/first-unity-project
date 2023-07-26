using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance { get { return instance; } }
    public StateMachine stateMachine { get; private set; }
    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public Animator shadowAnimator { get; private set; }

    private static PlayerStat instance;

    public int PlayerID { get { return playerID; } }
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float Armor { get { return armor; } }
    public int Level { get { return level; } }
    public int Kill { get { return kill; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float RollSpeed { get { return rollSpeed; } }
    public float RollCooltime { get { return rollCooltime; } }

    [Header("ĳ���� ����")]
    [SerializeField] protected int playerID;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float armor;
    [SerializeField] protected int level;
    [SerializeField] protected int kill;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rollSpeed;
    [SerializeField] protected float rollCooltime;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            shadowAnimator = transform.Find("Shadow").GetComponent<Animator>();
            InitStateMachine();
            // �� ��ȯ �ÿ� �ı����� �ʵ��� �����ϴ� �Լ�
            DontDestroyOnLoad(gameObject);
            return;
        }
        // �̹� instance�� ���� �� �ı�
        DestroyImmediate(gameObject);
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

    public void OnUpdateStat(int playerID, float maxHP, float currentHP, float armor, int level, int kill, float moveSpeed, float rollSpeed, float rollCooltime)
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
    }

    private void InitStateMachine()
    {
        // ���µ��� ����� ���
        PlayerController controller = GetComponent<PlayerController>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller));
        stateMachine.AddState(StateName.ROLL, new RollState(controller));
    }
}
