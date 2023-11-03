using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    private Animator animator; // �ִϸ����� ������Ʈ
    private float attackCooldown; // ���� ���ݱ����� �ð�
    private bool isPatternActive = false; // ���� ���� ������ ���� ������ �����ϴ� ����

    void Start()
    {
        animator = GetComponent<Animator>();
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

    // ���� ���� �ڷ�ƾ

    // ���� 1: �� ������ ����ġ��, �ٸ� ������ ���
    IEnumerator Pattern1()
    {
        isPatternActive = true; // ���� ����

        // ����ġ�� ����
        // ...
        Debug.Log("���� 1");
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ���

        // ��� ���� ����
        // ...

        isPatternActive = false; // ���� ����
    }

    // ���� 2: ���� ��� ���� �۾��� ������
    IEnumerator Pattern2()
    {
        // �� ������ ����
        // ...

        Debug.Log("���� 2");
        yield return new WaitForSeconds(1.5f); // 1.5�ʰ� ���

        // ������ �߻�
        // ...
        isPatternActive = false; // ���� ����
    }

    // ���� 3: �ָ��� ������ ������ ����� ����
    IEnumerator Pattern3()
    {
        int hits = 3; // ����ĸ� ������ Ƚ��
        for (int i = 0; i < hits; i++)
        {
            // ����ġ�� ����
            // ...

            Debug.Log("���� 3");
            yield return new WaitForSeconds(0.5f); // 0.5�ʰ� ���
        }
        isPatternActive = false; // ���� ����
    }

    // ���� 4: ���� ������ �߻�
    IEnumerator Pattern4()
    {
        // �� ������ ����
        // ...

        Debug.Log("���� 4");
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ���

        // ������ �߻�
        // ...
        isPatternActive = false; // ���� ����
    }

    // ���� 5: ���� ����
    IEnumerator Pattern5()
    {
        float duration = 3.0f; // ������ �������� �� �ð�
        float interval = 0.5f; // ���� ���� �ð� ����

        while (duration > 0)
        {
            // ���� ���� ��ġ ����
            // ...

            // ���� ����
            // ...

            Debug.Log("���� 5");
            yield return new WaitForSeconds(interval);
            duration -= interval;
        }
        isPatternActive = false; // ���� ����
    }

    // ���� ������ �������� �����Ͽ� ����
    public void ExecuteRandomPattern()
    {
        int pattern = Random.Range(1, 6); // 1���� 5 ������ ������ ����

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
