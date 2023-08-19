using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseAttackArea : MonoBehaviour
{
    public PolygonCollider2D attackRangeCollider;
    protected int attackID = 0; // 공격 ID 변수

    void Awake()
    {
        attackRangeCollider = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        // 초기 상태에서는 공격 범위 콜라이더를 비활성화
        attackRangeCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected in attack area!");
        }
        
        Debug.Log("Collided with: " + collision.gameObject.name);
        */
    }

    public virtual void ActivateAttackRange(Vector2 attackDirection, float weaponRange)
    {
        attackID++;
        CalculateColiderPoints(weaponRange);

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        attackRangeCollider.enabled = true;
    }

    public int GetAttackID()
    {
        return attackID;
    }

    public abstract void CalculateColiderPoints(float radius);
}
