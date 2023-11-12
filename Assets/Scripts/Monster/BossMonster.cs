using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    protected Transform target;   // �÷��̾� ��ġ
    private Animator animator; // �ִϸ����� ������Ʈ
    private Animator leftHandAnimator, rightHandAnimator;   // �� �� �ִϸ�����
    private Animator leftHandShadowAnimator, rightHandShadowAnimator;   // �� �� �׸��� �ִϸ�����

    private float attackCooldown; // ���� ���ݱ����� �ð�
    private bool isPatternActive = false; // ���� ���� ������ ���� ������ �����ϴ� ����

    [Header("�� ����")]
    public GameObject leftHand, rightHand; // �� ������Ʈ
    public GameObject leftHandShadow, rightHandShadow; // �� �׸��� ������Ʈ
    public Vector2 originalPositionLeft, originalPositionRight;   // ���� ���� ��ġ
    public Vector2 originalPositionLeftShadow, originalPositionRightShadow;   // ���� �� �׸����� ��ġ
    public Vector2 laserLeftHandPosition, laserRightHandPosition;   // ������ �� �� ���� ��ġ
    public Vector2 laserLeftHandShadowPosition, laserRightHandShadowPosition;   // ������ �� �� �� �׸����� ��ġ
    public Vector2 raiseLeftHandPosition, raiseRightHandPosition;   // ���÷��� ���� �� ��ġ

    [Header("������ ����")]
    public LineRenderer lineRenderer1; // Line Renderer ������Ʈ / ȸ�� ������
    public LineRenderer lineRenderer2; // Line Renderer ������Ʈ / �߾� ������
    public BoxCollider2D laserColider1; // ȸ�� ������ �ݶ��̴�
    public BoxCollider2D laserColider2; // �߾� ������ �ݶ��̴�
    public SpriteRenderer laserAttackAreaSpriteRenderer2;   // �߾� ������ ���� ���� ��������Ʈ
    public Transform laserStart; // ������ ������

    public float defDistanceRay = 100;
    public float laserDuration; // ������ ���� �ð�
    Quaternion lineRendererRotation = Quaternion.Euler(0, 0, 10); // ���� ������ ȸ���Ǿ� �ִ� ����

    public Ellipse[] ellipseObjects;
    public int bulletID;   // ����ϴ� �Ѿ��� ������ ID
    public Vector2 spawnPosition;   // ���� ���� ��ġ

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        leftHandAnimator = leftHand.GetComponent<Animator>();
        rightHandAnimator = rightHand.GetComponent<Animator>();
        leftHandShadowAnimator = leftHandShadow.GetComponent<Animator>();
        rightHandShadowAnimator = rightHandShadow.GetComponent<Animator>();

        // ���� ���� �ݶ��̴��� ��������Ʈ ��Ȱ��ȭ
        laserColider1.enabled = false;
        laserColider2.enabled = false;
        laserAttackAreaSpriteRenderer2.enabled = false;
    }

    void Start()
    {
        target = PlayerStat.Instance.transform;
        // ���� ������ ��ġ���� (-2, -2), (2, -2) �̵��� ��ġ�� ���� ���� ��ġ�� ����
        originalPositionLeft = (Vector2)transform.position + new Vector2(2, -2);
        originalPositionRight = (Vector2)transform.position + new Vector2(-2, -2);
        originalPositionLeftShadow = (Vector2)transform.position + new Vector2(2, -2.22f);
        originalPositionRightShadow = (Vector2)transform.position + new Vector2(-2, -2.22f);
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
        StartCoroutine(MoveHandRoutine(selectedHand, startPosition, endPosition, duration));
    }

    public void MoveHand(GameObject selectedHand, GameObject selectedHandShadow, Vector3 startPosition, Vector3 endPosition, Vector3 shadowStartPosition, Vector3 shadowEndPosition, float duration)
    {
        // �� + �׸��� �̵� �ڷ�ƾ ����
        StartCoroutine(MoveHandWithShadowRoutine(selectedHand, selectedHandShadow, startPosition, endPosition, shadowStartPosition, shadowEndPosition, duration));
    }

    void SetHandAnimatorPaperToRock1()
    {
        // ��
        leftHandAnimator.SetBool("IsPaper", false);
        leftHandAnimator.SetBool("IsChange", true);
        rightHandAnimator.SetBool("IsPaper", false);
        rightHandAnimator.SetBool("IsChange", true);

        // �׸���
        leftHandShadowAnimator.SetBool("IsPaper", false);
        leftHandShadowAnimator.SetBool("IsChange", true);
        rightHandShadowAnimator.SetBool("IsPaper", false);
        rightHandShadowAnimator.SetBool("IsChange", true);
    }

    void SetHandAnimatorPaperToRock2()
    {
        leftHandAnimator.SetBool("IsRock", true);
        leftHandAnimator.SetBool("IsChange", false);
        rightHandAnimator.SetBool("IsRock", true);
        rightHandAnimator.SetBool("IsChange", false);

        leftHandShadowAnimator.SetBool("IsRock", true);
        leftHandShadowAnimator.SetBool("IsChange", false);
        rightHandShadowAnimator.SetBool("IsRock", true);
        rightHandShadowAnimator.SetBool("IsChange", false);
    }

    void SetHandAnimatorRockToPaper1()
    {
        leftHandAnimator.SetBool("IsRock", false);
        leftHandAnimator.SetBool("IsChange", true);
        rightHandAnimator.SetBool("IsRock", false);
        rightHandAnimator.SetBool("IsChange", true);

        leftHandShadowAnimator.SetBool("IsRock", false);
        leftHandShadowAnimator.SetBool("IsChange", true);
        rightHandShadowAnimator.SetBool("IsRock", false);
        rightHandShadowAnimator.SetBool("IsChange", true);
    }

    void SetHandAnimatorRockToPaper2()
    {
        leftHandAnimator.SetBool("IsPaper", true);
        leftHandAnimator.SetBool("IsChange", false);
        rightHandAnimator.SetBool("IsPaper", true);
        rightHandAnimator.SetBool("IsChange", false);

        leftHandShadowAnimator.SetBool("IsPaper", true);
        leftHandShadowAnimator.SetBool("IsChange", false);
        rightHandShadowAnimator.SetBool("IsPaper", true);
        rightHandShadowAnimator.SetBool("IsChange", false);
    }

    IEnumerator ChangeHandPaperToRock()
    {
        SetHandAnimatorPaperToRock1();
        yield return new WaitForSeconds(0.25f);
        SetHandAnimatorPaperToRock2();
    }

    IEnumerator ChangeHandRockToPaper()
    {
        SetHandAnimatorRockToPaper1();
        yield return new WaitForSeconds(0.25f);
        SetHandAnimatorRockToPaper2();
    }

    // �� �̵� �ڷ�ƾ
    IEnumerator MoveHandRoutine(GameObject hand, Vector3 start, Vector3 end, float time)
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

    // �� + �׸��� �̵� �ڷ�ƾ
    IEnumerator MoveHandWithShadowRoutine(GameObject hand, GameObject shadow, Vector3 start, Vector3 end, Vector3 shadowStart, Vector3 shadowEnd, float time)
    {
        float elapsedTime = 0;   // ��� �ð�

        while (elapsedTime < time)
        {
            // ���� ��ġ�� ���� ��ġ���� �� ��ġ�� ���� ����
            hand.transform.position = Vector3.Lerp(start, end, (elapsedTime / time));
            shadow.transform.position = Vector3.Lerp(shadowStart, shadowEnd, (elapsedTime / time));

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ���� ��ġ ����
        hand.transform.position = end;
        shadow.transform.position = shadowEnd;
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
       
        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 1f);
        yield return new WaitForSeconds(1.0f);

        // �÷��̾��� ��ġ�� �������� �� ���� - ���� ���ʿ� ������ �޼�, �����ʿ� ������ ������
        GameObject firstHand = selectHand();
        GameObject secondHand = (firstHand == leftHand) ? rightHand : leftHand;
        GameObject firstHandShadow = (firstHand == leftHand) ? leftHandShadow : rightHandShadow;
        GameObject secondHandShadow = (firstHandShadow == leftHandShadow) ? rightHandShadow : leftHandShadow;

        // �÷��̾��� ��ġ�� ������ �� �̵�
        // ���ڱ� -> �ָ� ��ȯ
        StartCoroutine(ChangeHandPaperToRock());

        //Debug.Log("ù��° �� ���ø���");
        MoveHand(firstHand, firstHand.transform.position, firstHand.transform.position + new Vector3(0, 2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("ù��° �� �̵�");
        MoveHand(firstHand, firstHandShadow, firstHand.transform.position, target.position + new Vector3(0, 2f, 0), firstHandShadow.transform.position, target.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("ù��° �� �������");
        MoveHand(firstHand, firstHand.transform.position, firstHand.transform.position + new Vector3(0, -2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);

        // �ð����� �ΰ� �ݴ�� �̵�       
        //Debug.Log("�ι�° �� ���ø���");
        MoveHand(secondHand, secondHand.transform.position, secondHand.transform.position + new Vector3(0, 2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("�ι�° �� �̵�");
        MoveHand(secondHand, secondHandShadow, secondHand.transform.position, target.position + new Vector3(0, 2f, 0), secondHandShadow.transform.position, target.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("�ι�° �� �������");
        MoveHand(secondHand, secondHand.transform.position, secondHand.transform.position + new Vector3(0, -2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);

        // �� ���� ��ġ�� ����
        //Debug.Log("�� ����");
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 0.5f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 0.5f);
        yield return new WaitForSeconds(0.5f);

        // �ָ� -> ���ڱ� ��ȯ
        StartCoroutine(ChangeHandRockToPaper());

        Debug.Log("���� 1 �Ϸ�");
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // ���� 2: �ָ��� ������ ������ ����� ����
    IEnumerator Pattern2()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 1f);
        yield return new WaitForSeconds(1.0f);

        // ���ڱ� -> �ָ� ��ȯ
        StartCoroutine(ChangeHandPaperToRock());
        // ���� ����  ��������Ʈ Ȱ��ȭ
        ShowEllipseSprite();

        int hits = 3; // ����ĸ� ������ Ƚ��
        raiseLeftHandPosition = originalPositionLeft + new Vector2(0, 2f);
        raiseRightHandPosition = originalPositionRight + new Vector2(0, 2f);

        for (int i = 0; i < hits; i++)
        {
            // �� ���� �ø��� ����ġ�� ����
            Debug.Log("�� �� ���ø���");
            MoveHand(leftHand, originalPositionLeft, raiseLeftHandPosition, 0.3f);
            MoveHand(rightHand, originalPositionRight, raiseRightHandPosition, 0.3f);
            yield return new WaitForSeconds(0.3f);

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

        // �ָ� -> ���ڱ� ��ȯ
        StartCoroutine(ChangeHandRockToPaper());

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

        laserLeftHandPosition = originalPositionLeft + new Vector2(-1f, 0);
        laserRightHandPosition = originalPositionRight + new Vector2(1f, 0);
        laserLeftHandShadowPosition = originalPositionLeftShadow + new Vector2(-1f, 0);
        laserRightHandShadowPosition = originalPositionRightShadow + new Vector2(1f, 0);

        // �� �� ������
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, laserLeftHandPosition, leftHandShadow.transform.position, laserLeftHandShadowPosition, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, laserRightHandPosition, rightHandShadow.transform.position, laserRightHandShadowPosition, 1f);
        yield return new WaitForSeconds(1.0f);

        // �� ������ ���� / ���ڱ�
        animator.SetBool("IsCharge", true);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsAttack", true);

        // ������ �ʱ�ȭ �� Ȱ��ȭ
        laserStart.transform.rotation = Quaternion.identity;
        lineRenderer1.enabled = true;
        laserColider1.enabled = true;

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
        laserColider1.enabled = false;

        animator.SetBool("IsAttack", false);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsCharge", false);

        Debug.Log("���� 3 �Ϸ�");
        yield return new WaitForSeconds(1f);
        isPatternActive = false; // ���� ����
    }

    // ���� 4: ���� ��� ���� ������ �߻�
    IEnumerator Pattern4()
    {
        isPatternActive = true; // ���� ����

        laserLeftHandPosition = originalPositionLeft + new Vector2(-1f, 0);
        laserRightHandPosition = originalPositionRight + new Vector2(1f, 0);

        // ���ڱ� -> �ָ� ��ȯ
        StartCoroutine(ChangeHandPaperToRock());

        // �� �� ������
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, laserLeftHandPosition, leftHandShadow.transform.position, laserLeftHandShadowPosition, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, laserRightHandPosition, rightHandShadow.transform.position, laserRightHandShadowPosition, 1f);
        yield return new WaitForSeconds(1.0f);

        // �� ������ ���� / �ָ�
        animator.SetBool("IsCharge", true);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsAttack", true);

        // �� ������ ���� �� �÷��̾ ���� ���� ������ �߻�
        // ������ �ʱ�ȭ �� Ȱ��ȭ
        Vector3 attackDirection = (target.position - laserStart.transform.position).normalized;

        // ������ �ʱ�ȭ �� Ȱ��ȭ
        laserStart.transform.rotation = Quaternion.FromToRotation(Vector3.right, attackDirection);
        laserAttackAreaSpriteRenderer2.enabled = true;

        // ������ ���� ������ �� 1�� �� �߻�
        yield return new WaitForSeconds(1.0f);
        lineRenderer2.enabled = true;
        laserColider2.enabled = true;

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
        laserColider2.enabled = false;
        laserAttackAreaSpriteRenderer2.enabled = false;

        // �ָ� -> ���ڱ� ��ȯ
        StartCoroutine(ChangeHandRockToPaper());

        animator.SetBool("IsAttack", false);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsCharge", false);

        Debug.Log("���� 4 �Ϸ�");
        yield return new WaitForSeconds(1.0f); // 2�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // ���� ����
    public void CreateFallingRock(Vector2 spawnPosition)
    {
        Transform bulletTransform = GameManager.instance.pool.Get(bulletID).transform;
        FallingRock RockComponent = bulletTransform.GetComponent<FallingRock>();

        RockComponent.Init(1, spawnPosition);
    }

    IEnumerator ShakeHand(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Debug.Log("�� ���� ����ġ��");
            MoveHand(leftHand, originalPositionLeft, raiseLeftHandPosition, 0.2f);
            yield return new WaitForSeconds(0.2f);
            MoveHand(rightHand, originalPositionRight, raiseRightHandPosition, 0.2f);
            MoveHand(leftHand, raiseLeftHandPosition, originalPositionLeft, 0.2f);
            yield return new WaitForSeconds(0.2f);
            MoveHand(rightHand, raiseRightHandPosition, originalPositionRight, 0.2f);

            elapsedTime += 0.4f;
        }
    }

    // ���� 5: ���� ����
    IEnumerator Pattern5()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 1f);
        yield return new WaitForSeconds(1.0f);

        // ���ڱ� -> �ָ� ��ȯ
        StartCoroutine(ChangeHandPaperToRock());

        float duration = 3.0f; // ������ �������� �� �ð�
        float interval = 0.5f; // ���� ���� �ð� ����

        // ���� ����ĥ �� �ø��� ��ġ
        raiseLeftHandPosition = originalPositionLeft + new Vector2(0, 1f);
        raiseRightHandPosition = originalPositionRight + new Vector2(0, 1f);

        // ���� ����ġ�� �ڷ�ƾ ����
        StartCoroutine(ShakeHand(duration));

        while (duration > 0)
        {
            // ���� ������ ����
            // ��: �ִϸ����͸� ����Ͽ� ���� ������ �ִϸ��̼� ���
            // animator.SetTrigger("Charge");

            // �÷��̾��� ��ġ ��ó �������� ����
            spawnPosition = new Vector2(
                Random.Range(target.position.x - 2f, target.position.x + 2f), // ���� ������ �����Ͽ� ���� ����
                Random.Range(target.position.y - 2f, target.position.y + 2f) + 4f // ���ʿ��� �����ǵ���
            );

            // ���� ����
            CreateFallingRock(spawnPosition);

            // ������ ������ ���� �ð����� ���������� �÷��̾��� ������ ���� -> ���� �ð� �� �ı�
            // ��: FallingRock ��ũ��Ʈ ������ ó��

            yield return new WaitForSeconds(interval);
            duration -= interval;
        }

        // �ָ� -> ���ڱ� ��ȯ
        StartCoroutine(ChangeHandRockToPaper());

        Debug.Log("���� 5 �Ϸ�");
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // ���� ������ ������ �����ϱ� ���� ���� �߰�
    private bool alternatePattern = true;

    // ���� ������ �������� �����Ͽ� ����
    public void ExecuteRandomPattern()
    {
        if (alternatePattern)
        {
            // �׻� ���� 1�� ����
            StartCoroutine(Pattern1());
        }
        else
        {
            // ���� 1�� ������ ������ �߿��� �����ϰ� ����
            int pattern = Random.Range(1, 6); // 2���� 5 ������ ������ ����

            switch (pattern)
            {
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

        // ���� ������ �ٸ� ������ ����ǵ��� ���
        alternatePattern = !alternatePattern;
    }
}
