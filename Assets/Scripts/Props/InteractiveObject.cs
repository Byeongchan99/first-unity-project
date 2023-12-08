using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public GameObject firstPrefab;  // 처음 활성화되는 프리팹
    public GameObject secondPrefab;  // 상호작용 후 활성화되는 프리팹

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
