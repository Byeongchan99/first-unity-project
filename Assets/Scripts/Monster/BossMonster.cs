using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    private Animator animator; // 애니메이터 컴포넌트
    private float attackCooldown; // 다음 공격까지의 시간
    private bool isPatternActive = false; // 현재 공격 패턴이 실행 중인지 추적하는 변수

    void Start()
    {
        animator = GetComponent<Animator>();
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

    // 공격 패턴 코루틴

    // 패턴 1: 한 손으로 내리치기, 다른 손으로 방어
    IEnumerator Pattern1()
    {
        isPatternActive = true; // 패턴 시작

        // 내리치기 동작
        // ...
        Debug.Log("패턴 1");
        yield return new WaitForSeconds(2.0f); // 2초간 대기

        // 방어 상태 해제
        // ...

        isPatternActive = false; // 패턴 종료
    }

    // 패턴 2: 힘을 모아 전방 휩쓸기 레이저
    IEnumerator Pattern2()
    {
        // 힘 모으는 동작
        // ...

        Debug.Log("패턴 2");
        yield return new WaitForSeconds(1.5f); // 1.5초간 대기

        // 레이저 발사
        // ...
        isPatternActive = false; // 패턴 종료
    }

    // 패턴 3: 주먹을 여러번 내리쳐 충격파 생성
    IEnumerator Pattern3()
    {
        int hits = 3; // 충격파를 생성할 횟수
        for (int i = 0; i < hits; i++)
        {
            // 내리치기 동작
            // ...

            Debug.Log("패턴 3");
            yield return new WaitForSeconds(0.5f); // 0.5초간 대기
        }
        isPatternActive = false; // 패턴 종료
    }

    // 패턴 4: 강한 레이저 발사
    IEnumerator Pattern4()
    {
        // 힘 모으는 동작
        // ...

        Debug.Log("패턴 4");
        yield return new WaitForSeconds(2.0f); // 2초간 대기

        // 레이저 발사
        // ...
        isPatternActive = false; // 패턴 종료
    }

    // 패턴 5: 낙석 생성
    IEnumerator Pattern5()
    {
        float duration = 3.0f; // 낙석이 떨어지는 총 시간
        float interval = 0.5f; // 낙석 간의 시간 간격

        while (duration > 0)
        {
            // 낙석 생성 위치 결정
            // ...

            // 낙석 생성
            // ...

            Debug.Log("패턴 5");
            yield return new WaitForSeconds(interval);
            duration -= interval;
        }
        isPatternActive = false; // 패턴 종료
    }

    // 공격 패턴을 랜덤으로 선택하여 실행
    public void ExecuteRandomPattern()
    {
        int pattern = Random.Range(1, 6); // 1부터 5 사이의 랜덤한 숫자

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
