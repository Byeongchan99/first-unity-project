using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public GameObject firstPrefab;  // ó�� Ȱ��ȭ�Ǵ� ������
    public GameObject secondPrefab;  // ��ȣ�ۿ� �� Ȱ��ȭ�Ǵ� ������

    void Start()
    {
        firstPrefab.SetActive(true);
        secondPrefab.SetActive(false);
    }

    void SwapPrefabs()
    {
        firstPrefab.SetActive(!firstPrefab.activeSelf);
        secondPrefab.SetActive(!secondPrefab.activeSelf);
    }

    public void Interaction()
    {
        SwapPrefabs();
    }
}
