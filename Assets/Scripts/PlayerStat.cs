using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int PlayerID { get { return playerID; } }
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float Armor { get { return armor; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public int Level { get { return level; } }
    public int Kill { get { return kill; } }

    [SerializeField] protected int playerID;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float armor;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int level;
    [SerializeField] protected int kill;

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
}
