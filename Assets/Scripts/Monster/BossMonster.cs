using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BossMonster : MonoBehaviour
{
    protected Transform target;   // �÷��̾� ��ġ
    private Animator animator; // �ִϸ����� ������Ʈ
    private Animator leftHandAnimator, rightHandAnimator;   // �� �� �ִϸ�����
    private Animator leftHandShadowAnimator, rightHandShadowAnimator;   // �� �� �׸��� �ִϸ�����
    private Animator LeftHandParticleAnimator, rightHandParticleAnimator;   // �� �� ��ƼŬ �ִϸ�����
    public GameObject shoulderSprite;   // ��� ��������Ʈ
    public CameraShake cameraShake; // ī�޶� ����
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("���� ����")]
    public bool IsLive;
    private bool isDeadWhileCoroutine;   // �ڷ�ƾ ���� ��� ���� Ȯ��
    public float health;
    private float maxHealth = 1000;
    private int lastAttackID = -1;  // ������ ���� AttackArea�� ���� ID
    private float attackCooldown; // ���� ���ݱ����� �ð�
    private bool isPatternActive = false; // ���� ���� ������ ���� ������ �����ϴ� ����
    Vector2 bottomLeft, topRight;   // ���� �� �������� �� ũ��

    [Header("�� ����")]
    public GameObject leftHand, rightHand; // �� ������Ʈ
    public GameObject leftHandShadow, rightHandShadow; // �� �׸��� ������Ʈ
    public GameObject leftHandParticle, rightHandParticle;   // �� ��ƼŬ ������Ʈ
    public BoxCollider2D leftHandAttackArea, rightHandAttackArea;   // �� ���� ���� �ݶ��̴�
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

    [Header("���� ����")]
    protected AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip laser1;
    public AudioClip laser2;

    enum BossState
    {
        CHASE,
        ATTACK,
        DEAD
    }

    BossState bossState;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        leftHandAnimator = leftHand.GetComponent<Animator>();
        rightHandAnimator = rightHand.GetComponent<Animator>();
        leftHandShadowAnimator = leftHandShadow.GetComponent<Animator>();
        LeftHandParticleAnimator = leftHandParticle.GetComponent<Animator>();
        rightHandShadowAnimator = rightHandShadow.GetComponent<Animator>();
        rightHandParticleAnimator = rightHandParticle.GetComponent<Animator>();
        leftHandAttackArea = leftHand.GetComponent<BoxCollider2D>();
        rightHandAttackArea = rightHand.GetComponent<BoxCollider2D>();

        // ���� ���� �ݶ��̴��� ��������Ʈ ��Ȱ��ȭ
        laserColider1.enabled = false;
        laserColider2.enabled = false;
        laserAttackAreaSpriteRenderer2.enabled = false;
        leftHandAttackArea.enabled = false;
        rightHandAttackArea.enabled = false;
    }

    void OnEnable()
    {
        IsLive = true;
        health = maxHealth;

        target = PlayerStat.Instance.transform;
        topRight = StageManager.Instance.currentStage.topRight;
        bottomLeft = StageManager.Instance.currentStage.bottomLeft;
        // ���� ������ ��ġ���� (-2, -2), (2, -2) �̵��� ��ġ�� ���� ���� ��ġ�� ����
        originalPositionLeft = (Vector2)transform.position + new Vector2(2, -2);
        originalPositionRight = (Vector2)transform.position + new Vector2(-2, -2);
        originalPositionLeftShadow = (Vector2)transform.position + new Vector2(2, -2.22f);
        originalPositionRightShadow = (Vector2)transform.position + new Vector2(-2, -2.22f);
        InitEllipseSprite();
    }

    void Start()
    {
        // �ó׸ӽ� ���� ī�޶� ã��
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        // ī�޶� ���� ������Ʈ�� ã�� ���� ����
        if (cinemachineVirtualCamera != null)
        {
            cameraShake = cinemachineVirtualCamera.GetComponent<CameraShake>();

        }
    }

        public void ActivateBossMonster()
    {
        bossState = BossState.CHASE;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (IsLive)
        {
            yield return StartCoroutine(bossState.ToString());
        }
    }

    /*
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
    */

    // WaitForSeconds �ڷ�ƾ�� ��ü�Ͽ� ���� ���� ����ߴ��� Ȯ�� 
    IEnumerator WaitForConditionOrTime(float waitTime)
    {
        float startTime = Time.time;

        while (Time.time - startTime < waitTime)
        {
            if (health <= 0) 
            {
                isDeadWhileCoroutine = true;
                yield return true; // ������ �����Ǹ� �ڷ�ƾ ����
            }

            yield return null;
        }
        yield return false; // �ð� �ʰ� �� false ��ȯ
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

    // ��ƼŬ �̵�
    public void MoveParticle(GameObject selectedParticle, Vector3 endPosition)
    {
        selectedParticle.transform.position = endPosition;
    }

    // �������� �� ��ƼŬ Ȱ��ȭ
    IEnumerator ActiveParticle(GameObject selectedParticle)
    {
        Debug.Log("ActiveParticle ����");
        Animator particleAnimator = selectedParticle.GetComponent<Animator>();
        particleAnimator.SetBool("IsAttack", true);
        yield return new WaitForSeconds(0.417f);
        particleAnimator.SetBool("IsAttack", false);

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
                    PlayerController.Instance.ApplyDamage();
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

    // ���� 1: �� �����δ� �÷��̾��� ��ġ�� ������ �������, �ٸ� �����δ� �ð����� �ΰ� �������
    IEnumerator Pattern1()
    {
        isPatternActive = true; // ���� ����

        //Debug.Log("�� �� ��� ��ü ��ó�� �̵�");
        // Debug.Log(originalPositionLeft + " " + originalPositionRight);
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 0.5f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 0.5f);
        yield return new WaitForSeconds(0.5f);

        // �÷��̾��� ��ġ�� �������� �� ���� - ���� ���ʿ� ������ �޼�, �����ʿ� ������ ������
        GameObject firstHand = selectHand();
        GameObject secondHand = (firstHand == leftHand) ? rightHand : leftHand;
        GameObject firstHandShadow = (firstHand == leftHand) ? leftHandShadow : rightHandShadow;
        GameObject secondHandShadow = (firstHandShadow == leftHandShadow) ? rightHandShadow : leftHandShadow;
        GameObject firstHandParticle = (firstHand == leftHand) ? leftHandParticle : rightHandParticle;
        GameObject secondHandParticle = (firstHandParticle == leftHandParticle) ? rightHandParticle : leftHandParticle;
        BoxCollider2D firstHandAttackArea = (firstHand == leftHand) ? leftHandAttackArea : rightHandAttackArea;
        BoxCollider2D secondHandAttackArea = (firstHandAttackArea == leftHandAttackArea) ? rightHandAttackArea : leftHandAttackArea;

        // �÷��̾��� ��ġ�� ������ �� �̵�
        // ���ڱ� -> �ָ� ��ȯ
        StartCoroutine(ChangeHandPaperToRock());

        yield return StartCoroutine(WaitForConditionOrTime(2.0f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }

        //Debug.Log("ù��° �� ���ø���");
        MoveHand(firstHand, firstHand.transform.position, firstHand.transform.position + new Vector3(0, 2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("ù��° �� �̵�");
        MoveHand(firstHand, firstHandShadow, firstHand.transform.position, target.position + new Vector3(0, 2f, 0), firstHandShadow.transform.position, target.position, 0.5f);
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        //Debug.Log("ù��° �� �������");
        firstHandAttackArea.enabled = true;
        MoveHand(firstHand, firstHand.transform.position, firstHand.transform.position + new Vector3(0, -2f, 0), 0.2f);
        MoveParticle(firstHandParticle, firstHand.transform.position + new Vector3(0, -2f, 0));
        yield return new WaitForSeconds(0.2f);
        firstHandAttackArea.enabled = false;
        StartCoroutine(ActiveParticle(firstHandParticle));
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(attackSound);
        cameraShake.ShakeCamera(0.3f, 2f, 2.0f); // ī�޶� ����
        

        // �ð����� �ΰ� �ݴ�� �̵�       
        //Debug.Log("�ι�° �� ���ø���");
        MoveHand(secondHand, secondHand.transform.position, secondHand.transform.position + new Vector3(0, 2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("�ι�° �� �̵�");
        MoveHand(secondHand, secondHandShadow, secondHand.transform.position, target.position + new Vector3(0, 2f, 0), secondHandShadow.transform.position, target.position, 0.5f);
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        //Debug.Log("�ι�° �� �������");
        secondHandAttackArea.enabled = true;
        MoveHand(secondHand, secondHand.transform.position, secondHand.transform.position + new Vector3(0, -2f, 0), 0.2f);
        MoveParticle(secondHandParticle, secondHand.transform.position + new Vector3(0, -2f, 0));
        yield return new WaitForSeconds(0.2f);
        secondHandAttackArea.enabled = false;
        StartCoroutine(ActiveParticle(secondHandParticle));
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(attackSound);
        cameraShake.ShakeCamera(0.3f, 2f, 2.0f); // ī�޶� ����

        // �� ���� ��ġ�� ����
        //Debug.Log("�� ����");
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 0.5f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 0.5f);
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }

        // �ָ� -> ���ڱ� ��ȯ
        StartCoroutine(ChangeHandRockToPaper());

        Debug.Log("���� 1 �Ϸ�");
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));   // 1�ʰ� ���
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        } 
        isPatternActive = false; // ���� ����

        ChangeState(BossState.ATTACK);   // ATTACK ���·� ��ȯ       
    }

    // ���� ���� �ڷ�ƾ
    // ���� 2: �ָ��� ������ ������ ����� ����
    IEnumerator Pattern2()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 0.5f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 0.5f);
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }

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
            // Debug.Log("�� �� ���ø���");
            MoveHand(leftHand, originalPositionLeft, raiseLeftHandPosition, 0.5f);
            MoveHand(rightHand, originalPositionRight, raiseRightHandPosition, 0.5f);
            yield return StartCoroutine(WaitForConditionOrTime(0.5f));
            if (isDeadWhileCoroutine)
            {
                // ������ �����Ǿ� �ڷ�ƾ ����
                yield break;
            }

            leftHandAttackArea.enabled = true;
            rightHandAttackArea.enabled = true;
            // Debug.Log("�� �� ����ġ��");
            MoveHand(leftHand, raiseLeftHandPosition, originalPositionLeft, 0.3f);
            MoveHand(rightHand, raiseRightHandPosition, originalPositionRight, 0.3f);          
            yield return StartCoroutine(WaitForConditionOrTime(0.3f));
            MoveParticle(leftHandParticle, originalPositionLeft);
            MoveParticle(rightHandParticle, originalPositionRight);
            StartCoroutine(ActiveParticle(leftHandParticle));
            StartCoroutine(ActiveParticle(rightHandParticle));
            audioSource.volume = 0.8f;
            audioSource.PlayOneShot(attackSound);
            cameraShake.ShakeCamera(0.3f, 2f, 2.0f); // ī�޶� ����
            if (isDeadWhileCoroutine)
            {
                // ������ �����Ǿ� �ڷ�ƾ ����
                yield break;
            }
            leftHandAttackArea.enabled = false;
            rightHandAttackArea.enabled = false;

            // ���� ���ĳ����� ����� �߻�
            // Debug.Log("����� �߻�");
            // �÷��̾ ���� ���� ���� �ִ��� Ȯ���ϰ� ���� ����
            CheckPlayerPositionAndApplyDamage();
            // ���� ������ ���� ���� ������
            ToggleSafeZones();
        }
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }

        InitEllipseSprite();

        // �ָ� -> ���ڱ� ��ȯ
        StartCoroutine(ChangeHandRockToPaper());

        Debug.Log("���� 2 �Ϸ�");
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        isPatternActive = false; // ���� ����
    }

    // ������ ���� �޼���
    // ������
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
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, laserLeftHandPosition, leftHandShadow.transform.position, laserLeftHandShadowPosition, 0.5f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, laserRightHandPosition, rightHandShadow.transform.position, laserRightHandShadowPosition, 0.5f);
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }

        // �� ������ ���� / ���ڱ�
        animator.SetBool("IsCharge", true);
        yield return StartCoroutine(WaitForConditionOrTime(1f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        animator.SetBool("IsAttack", true);

        // ������ �ʱ�ȭ �� Ȱ��ȭ
        Vector3 startDirection = (Quaternion.Euler(0, 0, -30) * Vector3.right).normalized;
        Vector3 targetDirection = (Quaternion.Euler(0, 0, -140) * Vector3.right).normalized;
        Quaternion startRotation = Quaternion.FromToRotation(Vector3.right, startDirection);
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, targetDirection);
        laserStart.transform.rotation = Quaternion.FromToRotation(Vector3.right, startDirection);
        lineRenderer1.enabled = true;
        laserColider1.enabled = true;
        audioSource.pitch = 2;
        audioSource.volume = 0.3f;
        audioSource.PlayOneShot(laser2);

        float elapsedTime = 0; // ��� �ð�

        while (elapsedTime < laserDuration)
        {
            if (health <= 0)
            {
                isDeadWhileCoroutine = true;
                yield break;
            }
            // ������ ȸ��
            float progress = elapsedTime / laserDuration; // ������ ���� ���൵ ���
            Quaternion currentRotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            laserStart.transform.rotation = currentRotation;

            // ���� ȸ�� �������� ������ �߻�
            Vector3 currentDirection = currentRotation * Vector3.right;
            ShootLaser(lineRenderer1, currentDirection);

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ������ ��Ȱ��ȭ
        lineRenderer1.enabled = false;
        laserColider1.enabled = false;

        animator.SetBool("IsAttack", false);
        yield return StartCoroutine(WaitForConditionOrTime(1f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        animator.SetBool("IsCharge", false);

        Debug.Log("���� 3 �Ϸ�");
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        audioSource.pitch = 1f;
        isPatternActive = false; // ���� ����
    }

    // ���� 4: ���� ��� ���� ������ �߻�
    IEnumerator Pattern4()
    {
        isPatternActive = true; // ���� ����

        laserLeftHandPosition = originalPositionLeft + new Vector2(-1f, 0);
        laserRightHandPosition = originalPositionRight + new Vector2(1f, 0);
        laserLeftHandShadowPosition = originalPositionLeftShadow + new Vector2(-1f, 0);
        laserRightHandShadowPosition = originalPositionRightShadow + new Vector2(1f, 0);

        // ���ڱ� -> �ָ� ��ȯ
        StartCoroutine(ChangeHandPaperToRock());

        // �� �� ������
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, laserLeftHandPosition, leftHandShadow.transform.position, laserLeftHandShadowPosition, 0.5f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, laserRightHandPosition, rightHandShadow.transform.position, laserRightHandShadowPosition, 0.5f);
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }

        // �� ������ ���� / �ָ�
        animator.SetBool("IsCharge", true);
        yield return StartCoroutine(WaitForConditionOrTime(1f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        animator.SetBool("IsAttack", true);

        // �� ������ ���� �� �÷��̾ ���� ���� ������ �߻�
        // ������ �ʱ�ȭ �� Ȱ��ȭ
        Vector3 attackDirection = (target.position - laserStart.transform.position).normalized;

        // ������ �ʱ�ȭ �� Ȱ��ȭ
        laserStart.transform.rotation = Quaternion.FromToRotation(Vector3.right, attackDirection);
        laserAttackAreaSpriteRenderer2.enabled = true;

        // ������ ���� ������ �� 1�� �� �߻�
        yield return StartCoroutine(WaitForConditionOrTime(1f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        lineRenderer2.enabled = true;
        laserColider2.enabled = true;
        audioSource.volume = 0.4f;
        audioSource.PlayOneShot(laser1);

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
        yield return StartCoroutine(WaitForConditionOrTime(1f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        animator.SetBool("IsCharge", false);

        Debug.Log("���� 4 �Ϸ�");
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
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
            // Debug.Log("��� ������� ���� ����ġ��");
            // �� ���ø���
            MoveHand(leftHand, originalPositionLeft, raiseLeftHandPosition, 0.2f);
            yield return new WaitForSeconds(0.2f);
            MoveHand(rightHand, originalPositionRight, raiseRightHandPosition, 0.2f);
            leftHandAttackArea.enabled = true;
            // �� ����ġ��
            MoveHand(leftHand, raiseLeftHandPosition, originalPositionLeft, 0.2f);                    
            yield return new WaitForSeconds(0.2f);
            MoveParticle(leftHandParticle, originalPositionLeft);
            StartCoroutine(ActiveParticle(leftHandParticle));
            audioSource.volume = 0.8f;
            audioSource.PlayOneShot(attackSound);
            cameraShake.ShakeCamera(0.3f, 2f, 2.0f); // ī�޶� ����
            rightHandAttackArea.enabled = true;
            leftHandAttackArea.enabled = false;
            MoveHand(rightHand, raiseRightHandPosition, originalPositionRight, 0.2f);            
            yield return StartCoroutine(WaitForConditionOrTime(0.2f));
            MoveParticle(rightHandParticle, originalPositionRight);
            StartCoroutine(ActiveParticle(rightHandParticle));
            audioSource.volume = 0.8f;
            audioSource.PlayOneShot(attackSound);
            cameraShake.ShakeCamera(0.3f, 2f, 2.0f); // ī�޶� ����
            if (isDeadWhileCoroutine)
            {
                // ������ �����Ǿ� �ڷ�ƾ ����
                yield break;
            }
            rightHandAttackArea.enabled = false;

            elapsedTime += 0.6f;
        }
    }

    // ���� 5: ���� ����
    IEnumerator Pattern5()
    {
        isPatternActive = true; // ���� ����

        // �� �� ��� ��ü ��ó�� �̵�
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 0.5f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 0.5f);
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }

        // ���ڱ� -> �ָ� ��ȯ
        StartCoroutine(ChangeHandPaperToRock());
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }

        float duration = 4.8f; // ������ �������� �� �ð�
        float interval = 0.6f; // ���� ���� �ð� ����

        // ���� ����ĥ �� �ø��� ��ġ
        raiseLeftHandPosition = originalPositionLeft + new Vector2(0, 2f);
        raiseRightHandPosition = originalPositionRight + new Vector2(0, 2f);

        // ���� ����ġ�� �ڷ�ƾ ����
        StartCoroutine(ShakeHand(duration));

        while (duration > 0)
        {
            // ���� ������ ����
            // ��: �ִϸ����͸� ����Ͽ� ���� ������ �ִϸ��̼� ���
            // animator.SetTrigger("Charge");

            // �÷��̾��� ��ġ ��ó �������� ����, �� ���� ������ �����ǵ��� ��
            spawnPosition = new Vector2(
                Random.Range(Mathf.Max(bottomLeft.x, target.position.x - 2f), Mathf.Min(topRight.x, target.position.x + 2f)),
                Random.Range(Mathf.Max(bottomLeft.y, target.position.y - 2f), Mathf.Min(topRight.y - 1f, target.position.y + 2f)) + 6f
            );

            if (isDeadWhileCoroutine)
            {
                // ������ �����Ǿ� �ڷ�ƾ ����
                yield break;
            }

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
        yield return StartCoroutine(WaitForConditionOrTime(0.5f));
        if (isDeadWhileCoroutine)
        {
            // ������ �����Ǿ� �ڷ�ƾ ����
            yield break;
        }
        isPatternActive = false; // ���� ����
    }

    IEnumerator CHASE()
    {
        yield return StartCoroutine(Pattern1());

        if (health <= 0)
            ChangeState(BossState.DEAD);   // DEAD ���·� ��ȯ
        else
            ChangeState(BossState.ATTACK);   // ATTACK ���·� ��ȯ
    }

    IEnumerator ATTACK()
    {
        yield return ExecuteRandomPattern();

        if (health <= 0)
            ChangeState(BossState.DEAD);   // DEAD ���·� ��ȯ
        else
            ChangeState(BossState.CHASE);   // CHASE ���·� ��ȯ
    }

    // ���� ������ �������� �����Ͽ� ����
    public IEnumerator ExecuteRandomPattern()
    {
        // ���� 1�� ������ ������ �߿��� �����ϰ� ����
        int pattern = Random.Range(2, 6); // 2���� 5 ������ ������ ����

        switch (pattern)
        {
            case 2:
                yield return StartCoroutine(Pattern2());
                break;
            case 3:
                yield return StartCoroutine(Pattern3());
                break;
            case 4:
                yield return StartCoroutine(Pattern4());
                break;
            case 5:
                yield return StartCoroutine(Pattern5());
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerAttackArea":
                ProcessDamage(collision.GetComponent<PlayerAttackArea>().GetAttackID(),
                    PlayerStat.Instance.weaponManager.Weapon.AttackDamage + PlayerStat.Instance.AttackPower);
                break;

            case "Bullet":
                ProcessDamage(-1, collision.GetComponent<Bullet>().Damage); // -1�� ���� ID�� ������ ��Ÿ��
                break;

            case "ExplosionArea":
                ProcessDamage(-1, collision.transform.parent.GetComponent<FireBolt>().explosionDamage);
                break;
        }
    }

    private void ProcessDamage(int attackID, float damage)
    {
        // ���� ID�� �ٸ��ų� ID�� ���� ��쿡�� ���� ó��
        if (attackID != lastAttackID || attackID == -1)
        {
            health -= damage;
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(hitSound);
            if (health > 0)
            {               
                Debug.Log("ü�� ����! ���� ü�� " + health);
            }
            else
            {
                ChangeState(BossState.DEAD);
            }

            lastAttackID = attackID;
        }
    }

    public float GetCurrentHealth()
    {   
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    IEnumerator DEAD()
    {
        Debug.Log("���� ���");
        IsLive = false;
        WaveManager.Instance.OnMonsterDeath();
        int randomGold = Random.Range(500, 600);
        PlayerStat.Instance.Gold += randomGold;

        // ���� ���� �ʱ�ȭ �� �ִϸ��̼� ó�� (��: ��� �ִϸ��̼� ���)
        audioSource.volume = 0.6f;
        audioSource.PlayOneShot(deathSound);
        shoulderSprite.SetActive(false);   // ��� �ִϸ��̼� ������ �� ��� ��������Ʈ ��Ȱ��ȭ
        animator.SetTrigger("Dead");
        Debug.Log("ī�޶� ����");
        cameraShake.ShakeCamera(2.0f, 2f, 2.0f); // ī�޶� ����
        yield return new WaitForSeconds(2f); // ��� �ִϸ��̼� ��� �ð� (��: 1��)
     
        gameObject.SetActive(false);  // ������Ʈ ��Ȱ��ȭ
        GameManager.instance.GameVictory(); // ���� �¸�
    }

    void ChangeState(BossState newMonsterState)
    {
        Debug.Log("���� ��ȯ " + newMonsterState);
        bossState = newMonsterState;
    }
}
