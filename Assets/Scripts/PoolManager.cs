using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index=0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ��Ȱ��ȭ �� ���� ������Ʈ�� ����
        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // �߰��ϸ� select ������ ����
                select = item;
                select.SetActive(true);
                break;
            }
        } 

        // ������
        if (!select)
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
