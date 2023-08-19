using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackArea : BaseAttackArea
{
    public float attackColliderOffset = 0.5f;   // �ݶ��̴� �̵� �Ÿ�
    private Vector2 basePosition = new Vector2(0, 0.325f); // �ʱ� �ݶ��̴� ��ġ

    public override void ActivateAttackRange(Vector2 attackDirection, float weaponRange)
    {
        attackID++;
        CalculateColiderPoints(weaponRange);

        // ���� ���͸� 4 ���� �� �ϳ��� ����
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            attackDirection = attackDirection.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            attackDirection = attackDirection.y > 0 ? Vector2.up : Vector2.down;
        }

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // �ݶ��̴��� ���� ��ġ ����
        Vector2 forwardOffset = attackDirection.normalized * attackColliderOffset;
        transform.localPosition = basePosition + forwardOffset; // �ʱ� ��ġ�� �̵� �Ÿ��� ����

        attackRangeCollider.enabled = true;
    }

    public override void CalculateColiderPoints(float attackRange)
    {
        Vector2[] boxPoints = new Vector2[5];

        float halfWidth = attackRange / 2;
        float halfHeight = attackRange / 2;

        // �簢���� ������ ����
        boxPoints[0] = new Vector2(-halfWidth, -halfHeight);  // ���� �ϴ�
        boxPoints[1] = new Vector2(halfWidth, -halfHeight);   // ������ �ϴ�
        boxPoints[2] = new Vector2(halfWidth, halfHeight);    // ������ ���
        boxPoints[3] = new Vector2(-halfWidth, halfHeight);   // ���� ���
        boxPoints[4] = boxPoints[0]; // ������ �������� ó�� �������� ���ƾ� �������� ������.

        attackRangeCollider.SetPath(0, boxPoints);
    }
}
