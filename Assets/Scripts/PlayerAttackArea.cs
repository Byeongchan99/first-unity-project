using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : BaseAttackArea
{
    public CameraShake cameraShake; // ī�޶� ����

    private List<Vector2> points = new List<Vector2>();

    void Awake()
    {
        if (attackRangeCollider != null)
            attackRangeCollider.enabled = false;
    }

    void Start()
    {
        // ī�޶� ���� ������Ʈ�� ã�� ���� ����
        if (cameraShake == null) // �ν����Ϳ��� �Ҵ����� �ʾҴٸ�
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }
    }

    public void Initialize()
    {
        float radius = PlayerStat.Instance.weaponManager.Weapon.AttackRange;
        CalculateColiderPoints(radius);
    }

    // ���� ���� Ȱ��ȭ
    public override void ActivateAttackRange(Vector2 attackDirection)
    {
        attackID++;
        Debug.Log("attackID " + attackID);
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Debug.Log("���� ���� �ݶ��̴� Ȱ��ȭ");
        attackRangeCollider.enabled = true;
        // ���� ����
        PlayerStat.Instance.playerAudioManager.PlayAttackSound();
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

    // ���� Ÿ�� ����Ʈ
    IEnumerator DeactivateAfterSeconds(GameObject objectToDeactivate, float seconds)
    {
        yield return new WaitForSeconds(seconds); // ������ �ð���ŭ ��ٸ�
        objectToDeactivate.SetActive(false); // ��ü ��Ȱ��ȭ
    }

    public void DeactivateHitEffect(GameObject effect)
    {
        // ���� Ÿ�� �ִϸ��̼� �ð�
        StartCoroutine(DeactivateAfterSeconds(effect, 0.417f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ������ ���ߵǾ��� ��
        if (other.CompareTag("Enemy"))
        {
            // Energy ȸ��
            PlayerStat.Instance.CurrentEnergy += 1;
            // Debug.Log("������: " + PlayerStat.Instance.CurrentEnergy);
            // �ִ� Energy�� �ʰ��ϴ��� �˻��ϰ� �ʰ� �� �ִ�ġ�� ����
            PlayerStat.Instance.CurrentEnergy = Mathf.Min(PlayerStat.Instance.CurrentEnergy, PlayerStat.Instance.MaxEnergy);

            // �浹 ��ġ�� ���
            //Vector3 hitPosition = other.transform.position;
            //Vector3 hitPosition = other.ClosestPoint(transform.position);
            // ���� ������ �������� �ݶ��̴��� �� �κ��� ���
            Vector2 attackDirectionNormalized = (other.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
            Vector2 colliderEndPoint = transform.position + (Vector3)(attackDirectionNormalized * PlayerStat.Instance.weaponManager.Weapon.AttackRange);


            // ���� ��ġ�� Ÿ�� ����Ʈ Ȱ��ȭ
            GameObject hitEffect = GameManager.instance.pool.Get(5);
            hitEffect.transform.position = colliderEndPoint;

            // Ÿ�� ����Ʈ ȸ��
            float angle = Mathf.Atan2(attackDirectionNormalized.y, attackDirectionNormalized.x) * Mathf.Rad2Deg;
            hitEffect.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            DeactivateHitEffect(hitEffect);

            // ���� ���� �� ī�޶� ���� ȿ�� ����
            if (cameraShake != null)
            {
                cameraShake.ShakeCamera(0.1f, 1.2f, 1.0f); // ���� �ð��� ���� ����
            }
        }
    }
}
