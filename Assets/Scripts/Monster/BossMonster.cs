using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    protected Transform target;   // �÷��̾� ��ġ
    private Animator animator; // �ִϸ����� ������Ʈ
    public GameObject leftHand; // ���� �� ������Ʈ
    public GameObject rightHand; // ������ �� ������Ʈ

    public Vector2 originalPositionLeft, originalPositionRight;   // ���� ���� ��ġ
    private float attackCooldown; // ���� ���ݱ����� �ð�
    private bool isPatternActive = false; // ���� ���� ������ ���� ������ �����ϴ� ����

    void Start()
    {
        animator = GetComponent<Animator>();
        target = PlayerStat.Instance.transform;

        // ���� ������ ��ġ���� (-2, -2), (2, -2) �̵��� ��ġ�� ���� ���� ��ġ�� ����
        originalPositionLeft = (Vector2)transform.position + new Vector2(2, -2);
        originalPositionRight = (Vector2)transform.position + new Vector2(-2, -2);
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
        // �� �� ��� ��ü ��ó�� �̵�
        // �� ������ ���� / ���ڱ�

        // �� ������ ���� �� ���� ���� ȸ����Ű�� ������ �߻�

        Debug.Log("���� 2");
        yield return new WaitForSeconds(1.5f); // 1.5�ʰ� ���
        isPatternActive = false; // ���� ����
    }

    // ���� 3: �ָ��� ������ ������ ����� ����
    IEnumerator Pattern3()
    {
        int hits = 3; // ����ĸ� ������ Ƚ��

        for (int i = 0; i < hits; i++)
        {
            // �� �� ��� ��ü ��ó�� �̵�

            // �� ���� �ø��� ����ġ�� ����

            // ���� ���ĳ����� ����� �߻�
            
        }
        Debug.Log("���� 3");
        yield return new WaitForSeconds(0.5f); // 0.5�ʰ� ���
        isPatternActive = false; // ���� ����
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
        int pattern = Random.Range(1, 1); // 1���� 5 ������ ������ ����

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
