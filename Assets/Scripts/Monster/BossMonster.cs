using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    protected Transform target;   // �÷��̾� ��ġ
    private Animator animator; // �ִϸ����� ������Ʈ

    private float attackCooldown; // ���� ���ݱ����� �ð�
    private bool isPatternActive = false; // ���� ���� ������ ���� ������ �����ϴ� ����

    [Header("�� ����")]
    public GameObject leftHand; // ���� �� ������Ʈ
    public GameObject rightHand; // ������ �� ������Ʈ
    public Vector2 originalPositionLeft, originalPositionRight;   // ���� ���� ��ġ
    public Vector2 raiseLeftHandPosition, raiseRightHandPosition;   // ���÷��� ���� �� ��ġ

    [Header("������ ����")]
    public LineRenderer lineRenderer; // Line Renderer ������Ʈ
    public Transform laserStart; // ������ ������
    public float defDistanceRay = 100;
    public float laserDuration; // ������ ���� �ð�

    public Ellipse[] ellipseObjects;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = PlayerStat.Instance.transform;

        // ���� ������ ��ġ���� (-2, -2), (2, -2) �̵��� ��ġ�� ���� ���� ��ġ�� ����
        originalPositionLeft = (Vector2)transform.position + new Vector2(2, -2);
        originalPositionRight = (Vector2)transform.position + new Vector2(-2, -2);

        ToggleSafeZones();
    }

    void Update()
    {
        if (attackCooldown <= 0 && !isPatternActive)
        {
            ExecuteRandomPattern();
            attackCooldown = GetRandomCooldown(); // ���� ���ݱ��� ������ �ð� ����
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    // ���� ������ ���� ��Ÿ��
    float GetRandomCooldown()
    {
        return Random.Range(2f, 4f); // 2�ʿ��� 5�� ������ ������ �ð�
    }

    // ���� �����̴� �޼���
    public void MoveHand(GameObject selectedHand, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        // �� �̵� �ڷ�ƾ ����
        StartCoroutine(MoveArmRoutine(selectedHand, startPosition, endPosition, duration));
    }

    // �� �̵� �ڷ�ƾ
    IEnumerator MoveArmRoutine(GameObject hand, Vector3 start, Vector3 end, float time)
    {
        float elapsedTime = 0;   // ��� �ð�

        while (elapsedTime < time)
        {
            // ���� ��ġ�� ���� ��ġ���� �� ��ġ�� ���� ����
            hand.transform.position = Vector3.Lerp(start, end, (elapsedTime / time));

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ���� ��ġ ����
        hand.transform.position = end;
    }

    // ���� ���� �ڷ�ƾ

    // ���� 1: �� �վ� �������
    // �� �����δ� �÷��̾��� ��ġ�� ������ �������, �ٸ� �����δ� �ð����� �ΰ� �������
    IEnumerator Pattern1()
    {
        isPatternActive = true; // ���� ����

        // �÷��̾��� ��ġ�� �������� �� ���� - ���� ���ʿ� ������ �޼�, �����ʿ� ������ ������
        GameObject firstHand = selectHand();
        GameObject secondHand = (firstHand == leftHand) ? rightHand : leftHand;

        // �÷��̾��� ��ġ�� ������ �� �̵�
        Debug.Log("ù��° �� �̵�");
        MoveHand(firstHand, firstHand.transform.position, target.position, 1f);
        yield return new WaitForSeconds(1.0f);

        // ������� ����
        Debug.Log("ù��° �� �������");
        yield return new WaitForSeconds(0.5f);

        // �ð����� �ΰ� �ݴ�� �̵�
        yield return new WaitForSeconds(1.0f);
        Debug.Log("�ι�° �� �̵�");
        MoveHand(secondHand, secondHand.transform.position, target.position, 1f);
        yield return new WaitForSeconds(1.0f);

        // ������� ����
        Debug.Log("�ι�° �� �������");
        yield return new WaitForSeconds(0.5f);

        // �� ���� ��ġ�� ����
        Debug.Log("�� ����");
        MoveHand(leftHand, leftHand.transform.position, originalPositionLeft, 1f);
        MoveHand(rightHand, rightHand.transform.position, originalPositionRight, 1f);

        Debug.Log("���� 1 �Ϸ�");
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // �� ���� �޼���
    public GameObject selectHand()
    {
        Vector2 Direction = (target.position - transform.position).normalized;
        if (Direction.x > 0) 
        {
            return leftHand;
        }
        else
        {
            return rightHand;
        }
    }

    // ���� 2: ���� ��� ���� �۾��� ������ �߻�
    IEnumerator Pattern2()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHand.transform.position, originalPositionLeft, 1f);
        MoveHand(rightHand, rightHand.transform.position, originalPositionRight, 1f);

        // �� ������ ���� / ���ڱ�
        // ...

        // ������ �ʱ�ȭ �� Ȱ��ȭ
        laserStart.transform.rotation = Quaternion.identity;
        lineRenderer.enabled = true;

        float elapsedTime = 0;   // ��� �ð�
        float rotationSpeed = -130f / laserDuration; // �ʴ� ȸ�� �ӵ�

        while (elapsedTime < laserDuration)
        {
            ShootLaser();

            // ������ ȸ��
            laserStart.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // �� �����Ӹ��� ������ ������ŭ ȸ��

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ������ ��Ȱ��ȭ
        lineRenderer.enabled = false;

        Debug.Log("���� 2");
        yield return new WaitForSeconds(1.5f); // 1.5�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    void ShootLaser()
    {
        Draw2DRay(laserStart.position, laserStart.transform.right * defDistanceRay);
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    // ���� 3: �ָ��� ������ ������ ����� ����
    IEnumerator Pattern3()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHand.transform.position, originalPositionLeft, 1f);
        MoveHand(rightHand, rightHand.transform.position, originalPositionRight, 1f);

        int hits = 3; // ����ĸ� ������ Ƚ��
        raiseLeftHandPosition = originalPositionLeft + new Vector2(0, 2f);
        raiseRightHandPosition = originalPositionRight + new Vector2(0, 2f);

        for (int i = 0; i < hits; i++)
        {
            // �� ���� �ø��� ����ġ�� ����
            Debug.Log("�� �� ���ø���");
            MoveHand(leftHand, originalPositionLeft, raiseLeftHandPosition, 1f);
            MoveHand(rightHand, originalPositionRight, raiseRightHandPosition, 1f);
            yield return new WaitForSeconds(1f);

            Debug.Log("�� �� ����ġ��");
            MoveHand(leftHand, raiseLeftHandPosition, originalPositionLeft, 0.3f);
            MoveHand(rightHand, raiseRightHandPosition, originalPositionRight, 0.3f);
            yield return new WaitForSeconds(0.3f);

            // ���� ���ĳ����� ����� �߻�
            Debug.Log("����� �߻�");
            // �÷��̾ ���� ���� ���� �ִ��� Ȯ���ϰ� ���� ����
            CheckPlayerPositionAndApplyDamage();
            // ���� ������ ���� ���� ������
            ToggleSafeZones();
        }

        InitEllipseSprite();

        Debug.Log("���� 3");
        yield return new WaitForSeconds(2f); // 0.5�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    void InitEllipseSprite()
    {
        foreach (var ellipse in ellipseObjects)
        {
            var spriteRenderer = ellipse.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    void ToggleSafeZones()
    {
        // Ÿ������ isSafeZone ���� ����մϴ�
        foreach (var ellipse in ellipseObjects)
        {
            var ellipseComponent = ellipse.GetComponent<Ellipse>();
            var spriteRenderer = ellipse.GetComponent<SpriteRenderer>();

            if (ellipseComponent != null && spriteRenderer != null)
            {
                ellipseComponent.isSafeZone = !ellipseComponent.isSafeZone;
                if (!ellipseComponent.isSafeZone)
                    spriteRenderer.enabled = true;
                else
                    spriteRenderer.enabled = false;
            }
        }
    }

    void CheckPlayerPositionAndApplyDamage()
    {
        foreach (var ellipse in ellipseObjects)
        {
            Ellipse ellipseScript = ellipse.GetComponent<Ellipse>();
            if (ellipseScript.InEllipse(target))
            {
                if (!ellipseScript.isSafeZone)
                {
                    // �÷��̾ ���� ���� ���� �ֽ��ϴ�. ���ظ� �����ϼ���.
                    Debug.Log("���� ����");
                    break; // �� Ÿ�� ���� ������ �߰� Ȯ���� ���ʿ��մϴ�.
                }
                else
                {
                    Debug.Log("���� �� ����");
                    break;
                }
            }
        }
    }

    // ���� 4: ���� ������ �߻�
    IEnumerator Pattern4()
    {
        // �� �� ��� ��ü ��ó�� �̵�
        // �� ������ ���� / �ָ�

        // �� ������ ���� �� �� �߾��� ���� ���� ������ �߻�

        Debug.Log("���� 4");
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // ���� 5: ���� ����
    IEnumerator Pattern5()
    {
        float duration = 3.0f; // ������ �������� �� �ð�
        float interval = 0.5f; // ���� ���� �ð� ����

        while (duration > 0)
        {
            // ���� ������ ����

            // ���� ���� ��ġ �������� ����

            // ���� ����
            
            // ������ ������ ���� �ð����� ���������� �÷��̾��� ������ ���� -> ���� �ð� �� �ı�
         
            yield return new WaitForSeconds(interval);
            duration -= interval;
        }
        Debug.Log("���� 5");
        isPatternActive = false; // ���� ����
    }

    // ���� ������ �������� �����Ͽ� ����
    public void ExecuteRandomPattern()
    {
        int pattern = Random.Range(1, 4); // 1���� 5 ������ ������ ����

        switch (pattern)
        {
            case 1:
                StartCoroutine(Pattern1());
                break;
            case 2:
                StartCoroutine(Pattern2());
                break;
            case 3:
                StartCoroutine(Pattern3());
                break;
            case 4:
                StartCoroutine(Pattern4());
                break;
            case 5:
                StartCoroutine(Pattern5());
                break;
        }
    }
}
