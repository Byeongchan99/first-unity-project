using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private List<Image> playerEnergy;

    void Start()
    {
        InitializeEnergyUI();

        // PlayerStat의 이벤트에 구독
        if (PlayerStat.Instance != null)
        {
            PlayerStat.Instance.OnEnergyChange += UpdateEnergy;
        }
    }

    void InitializeEnergyUI()
    {
        foreach (Image energy in playerEnergy)
        {
            Destroy(energy.gameObject);
        }
        playerEnergy.Clear();

        for (int i = 0; i < PlayerStat.Instance.MaxEnergy; i++)
        {
            GameObject h = Instantiate(energyPrefab, this.transform);
            playerEnergy.Add(h.GetComponent<Image>());
        }

        UpdateEnergy();
    }

    void UpdateEnergy()
    {
        int currentEnergy = PlayerStat.Instance.CurrentEnergy;

        for (int i = 0; i < playerEnergy.Count; i++)
        {
            if (i < currentEnergy)
            {
                playerEnergy[i].gameObject.SetActive(true);
            }
            else
            {
                playerEnergy[i].gameObject.SetActive(false);
            }
        }
    }

    // 필요한 경우에만 하트를 다시 추가/제거하는 로직
    public void AdjustHearts()
    {
        int difference = PlayerStat.Instance.MaxEnergy - playerEnergy.Count;

        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                GameObject h = Instantiate(energyPrefab, this.transform);
                playerEnergy.Add(h.GetComponent<Image>());
            }
        }
        else
        {
            for (int i = 0; i < -difference; i++)
            {
                Destroy(playerEnergy[playerEnergy.Count - 1 - i].gameObject);
            }

            playerEnergy.RemoveRange(playerEnergy.Count + difference, -difference);
        }

        UpdateEnergy();
    }

    void OnDestroy()
    {
        // PlayerStat의 이벤트에서 구독 해제
        if (PlayerStat.Instance != null)
        {
            PlayerStat.Instance.OnHealthChange -= UpdateEnergy;
        }
    }
}
