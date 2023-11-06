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
    public LineRenderer lineRenderer1; // Line Renderer ������Ʈ / ȸ�� ������
    public LineRenderer lineRenderer2; // Line Renderer ������Ʈ / �߾� ������
    public Transform laserStart; // ������ ������
    public float defDistanceRay = 100;
    public float laserDuration; // ������ ���� �ð�
    Quaternion lineRendererRotation = Quaternion.Euler(0, 0, 10); // ���� ������ ȸ���Ǿ� �ִ� ����

    public Ellipse[] ellipseObjects;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        target = PlayerStat.Instance.transform;
        // ���� ������ ��ġ���� (-2, -2), (2, -2) �̵��� ��ġ�� ���� ���� ��ġ�� ����
        originalPositionLeft = (Vector2)transform.position + new Vector2(2, -2);
        originalPositionRight = (Vector2)transform.position + new Vector2(-2, -2);
        InitEllipseSprite();
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


    // �� ���� �޼���
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

    // ����� ���� �޼���
    void InitEllipseSprite()
    {
        // Debug.Log("����� ���� ��������Ʈ ��Ȱ��ȭ");
        foreach (var ellipse in ellipseObjects)
        {
            var spriteRenderer = ellipse.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    void ShowEllipseSprite()
    {
        // Debug.Log("����� ���� ��������Ʈ Ȱ��ȭ");
        foreach (var ellipse in ellipseObjects)
        {
            var ellipseComponent = ellipse.GetComponent<Ellipse>();
            var spriteRenderer = ellipse.GetComponent<SpriteRenderer>();

            if (ellipseComponent != null && spriteRenderer != null)
            {
                if (!ellipseComponent.isSafeZone)
                    spriteRenderer.enabled = true;
                else
                    spriteRenderer.enabled = false;
            }
        }
    }

    // ����� ��������/�������� ���
    void ToggleSafeZones()
    {
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

    // ���������̸� ������ ������ �޼���
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
        //Debug.Log("ù��° �� �̵�");
        MoveHand(firstHand, firstHand.transform.position, target.position + new Vector3(0, 2f, 0), 1f);
        yield return new WaitForSeconds(1.0f);

        // ������� ����
        //Debug.Log("ù��° �� �������");
        MoveHand(firstHand, firstHand.transform.position, firstHand.transform.position + new Vector3(0, -2f, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);

        // �ð����� �ΰ� �ݴ�� �̵�
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("�ι�° �� �̵�");
        MoveHand(secondHand, secondHand.transform.position, target.position + new Vector3(0, 2f, 0), 1f);
        yield return new WaitForSeconds(1.0f);

        // ������� ����
        //Debug.Log("�ι�° �� �������");
        MoveHand(secondHand, secondHand.transform.position, secondHand.transform.position + new Vector3(0, -2f, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);

        // �� ���� ��ġ�� ����
        //Debug.Log("�� ����");
        MoveHand(leftHand, leftHand.transform.position, originalPositionLeft, 1f);
        MoveHand(rightHand, rightHand.transform.position, originalPositionRight, 1f);

        Debug.Log("���� 1 �Ϸ�");
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // ���� 2: �ָ��� ������ ������ ����� ����
    IEnumerator Pattern2()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHand.transform.position, originalPositionLeft, 1f);
        MoveHand(rightHand, rightHand.transform.position, originalPositionRight, 1f);
        yield return new WaitForSeconds(1.0f);

        ShowEllipseSprite();

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

        Debug.Log("���� 2 �Ϸ�");
        yield return new WaitForSeconds(2f); // 0.5�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // ������ ���� �޼���
    // �۾��� ������
    void ShootLaser(LineRenderer lineRenderer)
    {
        Draw2DRay(lineRenderer, laserStart.position, laserStart.transform.right * defDistanceRay);
    }

    // ������ ������
    void ShootLaser(LineRenderer lineRenderer, Vector3 attackDirection)
    {
        // �������� ���۵Ǵ� ��ġ�� �����մϴ�.
        Vector3 startPos = laserStart.position;

        // �������� ������ ��ġ�� 'attackDirection' �������� �����մϴ�.
        Vector3 endPos = startPos + attackDirection * defDistanceRay;

        // ���� �������� �������� �׸��ϴ�.
        Draw2DRay(lineRenderer, startPos, endPos);
    }

    // ������ ����
    void Draw2DRay(LineRenderer lineRenderer, Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    // ���� 3: ���� ��� ���� �۾��� ������ �߻�
    IEnumerator Pattern3()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHand.transform.position, originalPositionLeft, 1f);
        MoveHand(rightHand, rightHand.transform.position, originalPositionRight, 1f);
        yield return new WaitForSeconds(1.0f);

        // �� ������ ���� / ���ڱ�
        // ...

        // ������ �ʱ�ȭ �� Ȱ��ȭ
        laserStart.transform.rotation = Quaternion.identity;
        lineRenderer1.enabled = true;

        float elapsedTime = 0;   // ��� �ð�
        float rotationSpeed = -130f / laserDuration; // �ʴ� ȸ�� �ӵ�

        while (elapsedTime < laserDuration)
        {
            ShootLaser(lineRenderer1);

            // ������ ȸ��
            laserStart.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // �� �����Ӹ��� ������ ������ŭ ȸ��

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ������ ��Ȱ��ȭ
        lineRenderer1.enabled = false;

        Debug.Log("���� 3 �Ϸ�");
        yield return new WaitForSeconds(1.5f); // 1.5�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // ���� 4: ���� ��� ���� ������ �߻�
    IEnumerator Pattern4()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHand.transform.position, originalPositionLeft, 1f);
        MoveHand(rightHand, rightHand.transform.position, originalPositionRight, 1f);
        yield return new WaitForSeconds(1.0f);
        // �� ������ ���� / �ָ�

        // �� ������ ���� �� �÷��̾ ���� ���� ������ �߻�
        // ������ �ʱ�ȭ �� Ȱ��ȭ
        Vector3 attackDirection = (target.position - laserStart.transform.position).normalized;

        // ������ �ʱ�ȭ �� Ȱ��ȭ
        laserStart.transform.rotation = Quaternion.FromToRotation(Vector3.right, attackDirection);
        lineRenderer2.enabled = true;

        float elapsedTime = 0;   // ��� �ð�

        while (elapsedTime < laserDuration)
        {
            ShootLaser(lineRenderer2, attackDirection);

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ������ ��Ȱ��ȭ
        lineRenderer2.enabled = false;

        Debug.Log("���� 4 �Ϸ�");
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
        int pattern = Random.Range(1, 2); // 1���� 5 ������ ������ ����

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
