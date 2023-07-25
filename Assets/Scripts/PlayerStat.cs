using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance { get { return Instance; } }
    public StateMachine stateMachine { get; private set; }
    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator { get; private set; }

    private static PlayerStat instance;

    public int PlayerID { get { return playerID; } }
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float Armor { get { return armor; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public int Level { get { return level; } }
    public int Kill { get { return kill; } }

    [Header("ĳ���� ����")]
    [SerializeField] protected int playerID;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float armor;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int level;
    [SerializeField] protected int kill;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            // �� ��ȯ �ÿ� �ı����� �ʵ��� �����ϴ� �Լ�
            DontDestroyOnLoad(gameObject);
            return;
        }
        // �̹� instance�� ���� �� �ı�
        DestroyImmediate(gameObject);
    }

    void Start()
    {
        InitStateMachine();
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

    public void OnUpdateStat(int playerID, float maxHP, float currentHP, float armor, float moveSpeed, float dashCount, int level, int kill)
    {
        this.playerID = playerID;
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.armor = armor;
        this.moveSpeed = moveSpeed;
        this.level = level;
        this.kill = kill;
    }

    private void InitStateMachine()
    {
        // ���µ��� ����� ���
    }
}
