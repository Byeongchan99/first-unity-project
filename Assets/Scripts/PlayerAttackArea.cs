using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : BaseAttackArea
{
    private List<Vector2> points = new List<Vector2>();

    public void Initialize()
    {
        float radius = PlayerStat.Instance.weaponManager.Weapon.AttackRange;
        CalculateColiderPoints(radius);
    }

    // ���� ���� Ȱ��ȭ
    public override void ActivateAttackRange(Vector2 attackDirection)
    {
        attackID++;

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        attackRangeCollider.enabled = true;
    }


    // �ݶ��̴� ��� ���
    public override void CalculateColiderPoints(float radius)
    {
        points.Clear();
        int resolution = 30;

        points.Add(Vector2.zero);

        float startAngle = Mathf.PI / 18;
        float endAngle = 17 * Mathf.PI / 18;
        float deltaAngle = (endAngle - startAngle) / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            float t = startAngle + deltaAngle * i;
            points.Add(new Vector2(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius));
        }

        attackRangeCollider.SetPath(0, points);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ������ ���ߵǾ��� ��
        if (other.CompareTag("Enemy"))
        {
            // Energy ȸ��
            PlayerStat.Instance.CurrentEnergy += 1;
            Debug.Log("������: " + PlayerStat.Instance.CurrentEnergy);
            // �ִ� Energy�� �ʰ��ϴ��� �˻��ϰ� �ʰ� �� �ִ�ġ�� ����
            PlayerStat.Instance.CurrentEnergy = Mathf.Min(PlayerStat.Instance.CurrentEnergy, PlayerStat.Instance.MaxEnergy);
        }
    }
}
