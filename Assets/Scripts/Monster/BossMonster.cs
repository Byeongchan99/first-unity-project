using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    protected Transform target;   // 플레이어 위치
    private Animator animator; // 애니메이터 컴포넌트
    private Animator leftHandAnimator, rightHandAnimator;   // 양 손 애니메이터
    private Animator leftHandShadowAnimator, rightHandShadowAnimator;   // 양 손 그림자 애니메이터

    private float attackCooldown; // 다음 공격까지의 시간
    private bool isPatternActive = false; // 현재 공격 패턴이 실행 중인지 추적하는 변수
    Vector2 bottomLeft, topRight;

    [Header("손 관련")]
    public GameObject leftHand, rightHand; // 손 오브젝트
    public GameObject leftHandShadow, rightHandShadow; // 손 그림자 오브젝트
    public BoxCollider2D leftHandAttackArea, rightHandAttackArea;   // 손 공격 범위 콜라이더
    public Vector2 originalPositionLeft, originalPositionRight;   // 기존 손의 위치
    public Vector2 originalPositionLeftShadow, originalPositionRightShadow;   // 기존 손 그림자의 위치
    public Vector2 laserLeftHandPosition, laserRightHandPosition;   // 레이저 쏠 때 손의 위치
    public Vector2 laserLeftHandShadowPosition, laserRightHandShadowPosition;   // 레이저 쏠 때 손 그림자의 위치
    public Vector2 raiseLeftHandPosition, raiseRightHandPosition;   // 들어올렸을 때의 손 위치

    [Header("레이저 관련")]
    public LineRenderer lineRenderer1; // Line Renderer 컴포넌트 / 회전 레이저
    public LineRenderer lineRenderer2; // Line Renderer 컴포넌트 / 중앙 레이저
    public BoxCollider2D laserColider1; // 회전 레이저 콜라이더
    public BoxCollider2D laserColider2; // 중앙 레이저 콜라이더
    public SpriteRenderer laserAttackAreaSpriteRenderer2;   // 중앙 레이저 공격 범위 스프라이트
    public Transform laserStart; // 레이저 시작점

    public float defDistanceRay = 100;
    public float laserDuration; // 레이저 지속 시간
    Quaternion lineRendererRotation = Quaternion.Euler(0, 0, 10); // 라인 렌더러 회전되어 있는 정도

    public Ellipse[] ellipseObjects;
    public int bulletID;   // 사용하는 총알의 프리팹 ID
    public Vector2 spawnPosition;   // 낙석 생성 위치

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        leftHandAnimator = leftHand.GetComponent<Animator>();
        rightHandAnimator = rightHand.GetComponent<Animator>();
        leftHandShadowAnimator = leftHandShadow.GetComponent<Animator>();
        rightHandShadowAnimator = rightHandShadow.GetComponent<Animator>();
        leftHandAttackArea = leftHand.GetComponent<BoxCollider2D>();
        rightHandAttackArea = rightHand.GetComponent<BoxCollider2D>();

        // 공격 관련 콜라이더와 스프라이트 비활성화
        laserColider1.enabled = false;
        laserColider2.enabled = false;
        laserAttackAreaSpriteRenderer2.enabled = false;
        leftHandAttackArea.enabled = false;
        rightHandAttackArea.enabled = false;
    }

    void Start()
    {
        target = PlayerStat.Instance.transform;
        topRight = StageManager.Instance.currentStage.topRight;
        bottomLeft = StageManager.Instance.currentStage.bottomLeft;
        // 보스 몬스터의 위치에서 (-2, -2), (2, -2) 이동한 위치를 기존 손의 위치로 설정
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
            attackCooldown = GetRandomCooldown(); // 다음 공격까지 무작위 시간 설정
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    // 공격 사이의 랜덤 쿨타임
    float GetRandomCooldown()
    {
        return Random.Range(2f, 4f); // 2초에서 5초 사이의 무작위 시간
    }


    // 손 관련 메서드
    // 손을 움직이는 메서드
    public void MoveHand(GameObject selectedHand, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        // 손 이동 코루틴 시작
        StartCoroutine(MoveHandRoutine(selectedHand, startPosition, endPosition, duration));
    }

    public void MoveHand(GameObject selectedHand, GameObject selectedHandShadow, Vector3 startPosition, Vector3 endPosition, Vector3 shadowStartPosition, Vector3 shadowEndPosition, float duration)
    {
        // 손 + 그림자 이동 코루틴 시작
        StartCoroutine(MoveHandWithShadowRoutine(selectedHand, selectedHandShadow, startPosition, endPosition, shadowStartPosition, shadowEndPosition, duration));
    }

    void SetHandAnimatorPaperToRock1()
    {
        // 손
        leftHandAnimator.SetBool("IsPaper", false);
        leftHandAnimator.SetBool("IsChange", true);
        rightHandAnimator.SetBool("IsPaper", false);
        rightHandAnimator.SetBool("IsChange", true);

        // 그림자
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

    // 손 이동 코루틴
    IEnumerator MoveHandRoutine(GameObject hand, Vector3 start, Vector3 end, float time)
    {
        float elapsedTime = 0;   // 경과 시간

        while (elapsedTime < time)
        {
            // 손의 위치를 시작 위치에서 끝 위치로 선형 보간
            hand.transform.position = Vector3.Lerp(start, end, (elapsedTime / time));

            // 경과 시간 증가
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종 위치 설정
        hand.transform.position = end;
    }

    // 손 + 그림자 이동 코루틴
    IEnumerator MoveHandWithShadowRoutine(GameObject hand, GameObject shadow, Vector3 start, Vector3 end, Vector3 shadowStart, Vector3 shadowEnd, float time)
    {
        float elapsedTime = 0;   // 경과 시간

        while (elapsedTime < time)
        {
            // 손의 위치를 시작 위치에서 끝 위치로 선형 보간
            hand.transform.position = Vector3.Lerp(start, end, (elapsedTime / time));
            shadow.transform.position = Vector3.Lerp(shadowStart, shadowEnd, (elapsedTime / time));

            // 경과 시간 증가
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종 위치 설정
        hand.transform.position = end;
        shadow.transform.position = shadowEnd;
    }

    // 손 선택 메서드
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

    // 충격파 관련 메서드
    void InitEllipseSprite()
    {
        // Debug.Log("충격파 범위 스프라이트 비활성화");
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
        // Debug.Log("충격파 범위 스프라이트 활성화");
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

    // 충격파 안전지대/위험지대 토글
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

    // 위험지역이면 데미지 입히는 메서드
    void CheckPlayerPositionAndApplyDamage()
    {
        foreach (var ellipse in ellipseObjects)
        {
            Ellipse ellipseScript = ellipse.GetComponent<Ellipse>();
            if (ellipseScript.InEllipse(target))
            {
                if (!ellipseScript.isSafeZone)
                {
                    // 플레이어가 위험 구역 내에 있습니다. 피해를 적용하세요.
                    Debug.Log("피해 입음");
                    break; // 한 타원 내에 있으면 추가 확인은 불필요합니다.
                }
                else
                {
                    Debug.Log("피해 안 입음");
                    break;
                }
            }
        }
    }

    // 공격 패턴 코루틴

    // 패턴 1: 한 손씩 내려찍기
    // 한 손으로는 플레이어의 위치를 추적해 내려찍기, 다른 손으로는 시간차를 두고 내려찍기
    IEnumerator Pattern1()
    {
        isPatternActive = true; // 패턴 시작
       
        // 양 손 모두 본체 근처로 이동
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 1f);
        yield return new WaitForSeconds(1.0f);

        // 플레이어의 위치를 기준으로 손 선택 - 맵의 왼쪽에 있으면 왼손, 오른쪽에 있으면 오른손
        GameObject firstHand = selectHand();
        GameObject secondHand = (firstHand == leftHand) ? rightHand : leftHand;
        GameObject firstHandShadow = (firstHand == leftHand) ? leftHandShadow : rightHandShadow;
        GameObject secondHandShadow = (firstHandShadow == leftHandShadow) ? rightHandShadow : leftHandShadow;
        BoxCollider2D firstHandAttackArea = (firstHand == leftHand) ? leftHandAttackArea : rightHandAttackArea;
        BoxCollider2D secondHandAttackArea = (firstHandAttackArea == leftHandAttackArea) ? rightHandAttackArea : leftHandAttackArea;

        // 플레이어의 위치로 선택한 손 이동
        // 보자기 -> 주먹 전환
        StartCoroutine(ChangeHandPaperToRock());

        //Debug.Log("첫번째 손 들어올리기");
        MoveHand(firstHand, firstHand.transform.position, firstHand.transform.position + new Vector3(0, 2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("첫번째 손 이동");
        MoveHand(firstHand, firstHandShadow, firstHand.transform.position, target.position + new Vector3(0, 2f, 0), firstHandShadow.transform.position, target.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("첫번째 손 내려찍기");
        firstHandAttackArea.enabled = true;
        MoveHand(firstHand, firstHand.transform.position, firstHand.transform.position + new Vector3(0, -2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        firstHandAttackArea.enabled = false;

        // 시간차를 두고 반대손 이동       
        //Debug.Log("두번째 손 들어올리기");
        MoveHand(secondHand, secondHand.transform.position, secondHand.transform.position + new Vector3(0, 2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("두번째 손 이동");
        MoveHand(secondHand, secondHandShadow, secondHand.transform.position, target.position + new Vector3(0, 2f, 0), secondHandShadow.transform.position, target.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("두번째 손 내려찍기");
        secondHandAttackArea.enabled = true;
        MoveHand(secondHand, secondHand.transform.position, secondHand.transform.position + new Vector3(0, -2f, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        secondHandAttackArea.enabled = false;

        // 손 원래 위치로 복귀
        //Debug.Log("손 복귀");
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 0.5f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 0.5f);
        yield return new WaitForSeconds(0.5f);

        // 주먹 -> 보자기 전환
        StartCoroutine(ChangeHandRockToPaper());

        Debug.Log("패턴 1 완료");
        yield return new WaitForSeconds(2.0f); // 2초간 대기
        isPatternActive = false; // 패턴 종료
    }

    // 패턴 2: 주먹을 여러번 내리쳐 충격파 생성
    IEnumerator Pattern2()
    {
        isPatternActive = true; // 패턴 시작

        // 양 손 모두 본체 근처로 이동
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 1f);
        yield return new WaitForSeconds(1.0f);

        // 보자기 -> 주먹 전환
        StartCoroutine(ChangeHandPaperToRock());
        // 공격 범위  스프라이트 활성화
        ShowEllipseSprite();

        int hits = 3; // 충격파를 생성할 횟수
        raiseLeftHandPosition = originalPositionLeft + new Vector2(0, 2f);
        raiseRightHandPosition = originalPositionRight + new Vector2(0, 2f);

        for (int i = 0; i < hits; i++)
        {
            // 양 손을 올리고 내려치는 동작
            Debug.Log("양 손 들어올리기");
            MoveHand(leftHand, originalPositionLeft, raiseLeftHandPosition, 0.3f);
            MoveHand(rightHand, originalPositionRight, raiseRightHandPosition, 0.3f);
            yield return new WaitForSeconds(0.3f);

            leftHandAttackArea.enabled = true;
            rightHandAttackArea.enabled = true;
            Debug.Log("양 손 내려치기");
            MoveHand(leftHand, raiseLeftHandPosition, originalPositionLeft, 0.3f);
            MoveHand(rightHand, raiseRightHandPosition, originalPositionRight, 0.3f);
            yield return new WaitForSeconds(0.3f);
            leftHandAttackArea.enabled = false;
            rightHandAttackArea.enabled = false;

            // 맵을 퍼쳐나가는 충격파 발생
            Debug.Log("충격파 발생");
            // 플레이어가 위험 구역 내에 있는지 확인하고 피해 적용
            CheckPlayerPositionAndApplyDamage();
            // 안전 구역과 위험 구역 재정의
            ToggleSafeZones();
        }

        InitEllipseSprite();

        // 주먹 -> 보자기 전환
        StartCoroutine(ChangeHandRockToPaper());

        Debug.Log("패턴 2 완료");
        yield return new WaitForSeconds(2f); // 0.5초간 대기
        isPatternActive = false; // 패턴 종료
    }

    // 레이저 관련 메서드
    // 레이저
    void ShootLaser(LineRenderer lineRenderer, Vector3 attackDirection)
    {
        // 레이저가 시작되는 위치를 설정합니다.
        Vector3 startPos = laserStart.position;

        // 레이저가 끝나는 위치를 'attackDirection' 방향으로 설정합니다.
        Vector3 endPos = startPos + attackDirection * defDistanceRay;

        // 라인 렌더러로 레이저를 그립니다.
        Draw2DRay(lineRenderer, startPos, endPos);
    }

    // 레이저 생성
    void Draw2DRay(LineRenderer lineRenderer, Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    // 패턴 3: 힘을 모아 전방 휩쓸기 레이저 발사
    IEnumerator Pattern3()
    {
        isPatternActive = true; // 패턴 시작

        laserLeftHandPosition = originalPositionLeft + new Vector2(-1f, 0);
        laserRightHandPosition = originalPositionRight + new Vector2(1f, 0);
        laserLeftHandShadowPosition = originalPositionLeftShadow + new Vector2(-1f, 0);
        laserRightHandShadowPosition = originalPositionRightShadow + new Vector2(1f, 0);

        // 양 손 모으기
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, laserLeftHandPosition, leftHandShadow.transform.position, laserLeftHandShadowPosition, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, laserRightHandPosition, rightHandShadow.transform.position, laserRightHandShadowPosition, 1f);
        yield return new WaitForSeconds(1.0f);

        // 힘 모으는 동작 / 보자기
        animator.SetBool("IsCharge", true);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsAttack", true);

        // 레이저 초기화 및 활성화
        Vector3 startDirection = (Quaternion.Euler(0, 0, -30) * Vector3.right).normalized;
        Vector3 targetDirection = (Quaternion.Euler(0, 0, -140) * Vector3.right).normalized;
        Quaternion startRotation = Quaternion.FromToRotation(Vector3.right, startDirection);
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, targetDirection);
        laserStart.transform.rotation = Quaternion.FromToRotation(Vector3.right, startDirection);
        lineRenderer1.enabled = true;
        laserColider1.enabled = true;

        float elapsedTime = 0; // 경과 시간

        while (elapsedTime < laserDuration)
        {
            // 레이저 회전
            float progress = elapsedTime / laserDuration; // 보간을 위한 진행도 계산
            Quaternion currentRotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            laserStart.transform.rotation = currentRotation;

            // 현재 회전 방향으로 레이저 발사
            Vector3 currentDirection = currentRotation * Vector3.right;
            ShootLaser(lineRenderer1, currentDirection);

            // 경과 시간 증가
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 레이저 비활성화
        lineRenderer1.enabled = false;
        laserColider1.enabled = false;

        animator.SetBool("IsAttack", false);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsCharge", false);

        Debug.Log("패턴 3 완료");
        yield return new WaitForSeconds(1f);
        isPatternActive = false; // 패턴 종료
    }

    // 패턴 4: 힘을 모아 강한 레이저 발사
    IEnumerator Pattern4()
    {
        isPatternActive = true; // 패턴 시작

        laserLeftHandPosition = originalPositionLeft + new Vector2(-1f, 0);
        laserRightHandPosition = originalPositionRight + new Vector2(1f, 0);
        laserLeftHandShadowPosition = originalPositionLeftShadow + new Vector2(-1f, 0);
        laserRightHandShadowPosition = originalPositionRightShadow + new Vector2(1f, 0);

        // 보자기 -> 주먹 전환
        StartCoroutine(ChangeHandPaperToRock());

        // 양 손 모으기
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, laserLeftHandPosition, leftHandShadow.transform.position, laserLeftHandShadowPosition, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, laserRightHandPosition, rightHandShadow.transform.position, laserRightHandShadowPosition, 1f);
        yield return new WaitForSeconds(1.0f);

        // 힘 모으는 동작 / 주먹
        animator.SetBool("IsCharge", true);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsAttack", true);

        // 힘 모으는 동작 후 플레이어를 향해 강한 레이저 발사
        // 레이저 초기화 및 활성화
        Vector3 attackDirection = (target.position - laserStart.transform.position).normalized;

        // 레이저 초기화 및 활성화
        laserStart.transform.rotation = Quaternion.FromToRotation(Vector3.right, attackDirection);
        laserAttackAreaSpriteRenderer2.enabled = true;

        // 레이저 범위 보여준 후 1초 후 발사
        yield return new WaitForSeconds(1.0f);
        lineRenderer2.enabled = true;
        laserColider2.enabled = true;

        float elapsedTime = 0;   // 경과 시간

        while (elapsedTime < laserDuration)
        {
            ShootLaser(lineRenderer2, attackDirection);

            // 경과 시간 증가
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 레이저 비활성화
        lineRenderer2.enabled = false;
        laserColider2.enabled = false;
        laserAttackAreaSpriteRenderer2.enabled = false;

        // 주먹 -> 보자기 전환
        StartCoroutine(ChangeHandRockToPaper());

        animator.SetBool("IsAttack", false);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsCharge", false);

        Debug.Log("패턴 4 완료");
        yield return new WaitForSeconds(1.0f); // 2초간 대기
        isPatternActive = false; // 패턴 종료
    }

    // 낙석 생성
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
            Debug.Log("손 마구 내려치기");
            MoveHand(leftHand, originalPositionLeft, raiseLeftHandPosition, 0.2f);
            yield return new WaitForSeconds(0.2f);
            MoveHand(rightHand, originalPositionRight, raiseRightHandPosition, 0.2f);
            leftHandAttackArea.enabled = true;
            MoveHand(leftHand, raiseLeftHandPosition, originalPositionLeft, 0.2f);
            yield return new WaitForSeconds(0.2f);
            rightHandAttackArea.enabled = true;
            leftHandAttackArea.enabled = false;
            MoveHand(rightHand, raiseRightHandPosition, originalPositionRight, 0.2f);
            yield return new WaitForSeconds(0.2f);
            rightHandAttackArea.enabled = false;

            elapsedTime += 0.6f;
        }
    }

    // 패턴 5: 낙석 생성
    IEnumerator Pattern5()
    {
        isPatternActive = true; // 패턴 시작

        // 양 손 모두 본체 근처로 이동
        MoveHand(leftHand, leftHandShadow, leftHand.transform.position, originalPositionLeft, leftHandShadow.transform.position, originalPositionLeftShadow, 1f);
        MoveHand(rightHand, rightHandShadow, rightHand.transform.position, originalPositionRight, rightHandShadow.transform.position, originalPositionRightShadow, 1f);
        yield return new WaitForSeconds(1.0f);

        // 보자기 -> 주먹 전환
        StartCoroutine(ChangeHandPaperToRock());
        yield return new WaitForSeconds(0.5f);

        float duration = 4.8f; // 낙석이 떨어지는 총 시간
        float interval = 0.6f; // 낙석 간의 시간 간격

        // 손을 내려칠 때 올리는 위치
        raiseLeftHandPosition = originalPositionLeft + new Vector2(0, 2f);
        raiseRightHandPosition = originalPositionRight + new Vector2(0, 2f);

        // 손을 내리치는 코루틴 시작
        StartCoroutine(ShakeHand(duration));

        while (duration > 0)
        {
            // 힘을 모으는 동작
            // 예: 애니메이터를 사용하여 힘을 모으는 애니메이션 재생
            // animator.SetTrigger("Charge");

            // 플레이어의 위치 근처 랜덤으로 결정, 맵 범위 내에서 생성되도록 함
            spawnPosition = new Vector2(
                Random.Range(Mathf.Max(bottomLeft.x, target.position.x - 2f), Mathf.Min(topRight.x, target.position.x + 2f)),
                Random.Range(Mathf.Max(bottomLeft.y, target.position.y - 2f), Mathf.Min(topRight.y, target.position.y + 2f)) + 6f
            );

            // 낙석 생성
            CreateFallingRock(spawnPosition);

            // 생성된 낙석은 일정 시간동안 남아있으며 플레이어의 동선을 방해 -> 일정 시간 후 파괴
            // 예: FallingRock 스크립트 내에서 처리

            yield return new WaitForSeconds(interval);
            duration -= interval;
        }

        // 주먹 -> 보자기 전환
        StartCoroutine(ChangeHandRockToPaper());

        Debug.Log("패턴 5 완료");
        yield return new WaitForSeconds(2.0f); // 2초간 대기
        isPatternActive = false; // 패턴 종료
    }

    // 공격 패턴을 번갈아 실행하기 위한 변수 추가
    private bool alternatePattern = true;

    // 공격 패턴을 랜덤으로 선택하여 실행
    public void ExecuteRandomPattern()
    {
        if (alternatePattern)
        {
            // 항상 패턴 1을 실행
            StartCoroutine(Pattern1());
        }
        else
        {
            // 패턴 1을 제외한 나머지 중에서 랜덤하게 실행
            int pattern = Random.Range(3, 4); // 2부터 5 사이의 랜덤한 숫자

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

        // 다음 번에는 다른 패턴이 실행되도록 토글
        alternatePattern = !alternatePattern;
    }
}
