using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    protected Animator anim;
    protected Transform target;   // �÷��̾� ��ġ

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        target = PlayerStat.Instance.transform;
    }

    void Update()
    {
        Vector2 Direction = (target.position - transform.position).normalized;
        anim.SetFloat("Direction.X", Direction.x);
    }
}
