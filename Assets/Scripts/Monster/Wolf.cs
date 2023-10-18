using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonsterBase
{
    public float waitTimeBeforeCharge = 2.0f; // ��� �ð�
    public float chargeSpeed = 5.0f; // ���� �ӵ�
    public float chargeDuration = 2.0f; // �����ϴ� �ð�

    public override IEnumerator AttackPattern()
    {
        yield return new WaitForSeconds(waitTimeBeforeCharge); // ���

        Vector2 chargeDirection = (target.position - transform.position).normalized; // ���� ���� ����

        float chargeEndTime = Time.time + chargeDuration; // ������ �����ϴ� �ð����� ���� ���� �ð��� ���� ���� ���� �ð� ����

        // �����ϴ� ���� �ݺ�
        while (Time.time < chargeEndTime)
        {
            transform.position += (Vector3)chargeDirection * chargeSpeed * Time.deltaTime; // ����
            yield return null;
        }
    }
}
 