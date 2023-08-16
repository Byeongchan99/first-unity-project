using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class AttackArea : MonoBehaviour
{
    public PlayerController playerController;
    public PolygonCollider2D attackRangeCollider;

    void Awake()
    {
        attackRangeCollider = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        // �ʱ� ���¿����� ���� ���� �ݶ��̴��� ��Ȱ��ȭ
        attackRangeCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected in attack area!");
        }
    }

    public void AttackRange()
    {
        float weaponRange = PlayerStat.Instance.weaponManager.Weapon.AttackRange;

        Vector2[] semiCirclePoints = CalculateSemiCirclePoints(weaponRange);
        attackRangeCollider.SetPath(0, semiCirclePoints);

        // attackDirection�� ����Ͽ� AttackRangeObject�� ȸ����ŵ�ϴ�.
        float angle = Mathf.Atan2(playerController.attackDirection.y, playerController.attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);  // -90�� �ݿ��� ������ ���ϰ� �ϱ� �����Դϴ�.

        attackRangeCollider.enabled = true;  // ���� �ݶ��̴� Ȱ��ȭ
    }

    private Vector2[] CalculateSemiCirclePoints(float radius, int resolution = 30)
    {
        List<Vector2> points = new List<Vector2>();

        // ���� �߽ɿ� ���� �� �߰�
        points.Add(Vector2.zero);

        // ���� ������ �� ������ �������� ����
        float startAngle = Mathf.PI / 18;    // 10 degrees in radians
        float endAngle = 17 * Mathf.PI / 18; // 170 degrees in radians

        float deltaAngle = (endAngle - startAngle) / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            float t = startAngle + deltaAngle * i;
            points.Add(new Vector2(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius));
        }

        return points.ToArray();
    }

}
