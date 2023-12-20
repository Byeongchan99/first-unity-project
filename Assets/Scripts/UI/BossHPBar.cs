using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public static BossHPBar Instance { get; private set; }
    public Slider healthSlider;
    private BossMonster bossMonster;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBossMonster(BossMonster monster)
    {
        bossMonster = monster;
    }

    void Update()
    {
        if (bossMonster != null)
        {
            if (!bossMonster.IsLive || !PlayerStat.Instance.isLive) // ������ �÷��̾ �׾����� üũ
            {
                gameObject.SetActive(false); // ü�¹� UI ��Ȱ��ȭ
            }
            else
            {
                float healthRatio = bossMonster.GetCurrentHealth() / bossMonster.GetMaxHealth();
                healthSlider.value = healthRatio;
            }
        }
    }
}
