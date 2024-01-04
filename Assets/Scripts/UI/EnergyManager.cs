using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private List<Image> playerEnergy;

    public static EnergyManager Instance { get; private set; }
    public Sprite blueGem;
    public Sprite blackGem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeEnergyUI();

        // PlayerStat�� �̺�Ʈ�� ����
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
            GameObject e = Instantiate(energyPrefab, this.transform);
            playerEnergy.Add(e.GetComponent<Image>());
        }

        UpdateEnergy();
    }

    void UpdateEnergy()
    {
        // �ִ� �������� ���� �������� ��
        if (PlayerStat.Instance.CurrentEnergy > PlayerStat.Instance.MaxEnergy)
        {
            PlayerStat.Instance.CurrentEnergy = PlayerStat.Instance.MaxEnergy; // ���� �������� �ִ� �������� ����
        }

        int currentEnergy = PlayerStat.Instance.CurrentEnergy;

        for (int i = 0; i < playerEnergy.Count; i++)
        {
            if (i < currentEnergy)
            {
                playerEnergy[i].gameObject.SetActive(true);
                playerEnergy[i].sprite = blueGem;
            }
            else
            {
                //playerEnergy[i].gameObject.SetActive(false);
                playerEnergy[i].sprite = blackGem;
            }
        }
    }

    // �ʿ��� ��쿡�� �������� �ٽ� �߰�/�����ϴ� ����
    public void AdjustEnergy()
    {
        if (PlayerStat.Instance.MaxEnergy < 1)
            PlayerStat.Instance.MaxEnergy = 1;

        int difference = PlayerStat.Instance.MaxEnergy - playerEnergy.Count;

        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                GameObject e = Instantiate(energyPrefab, this.transform);
                playerEnergy.Add(e.GetComponent<Image>());
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
        // PlayerStat�� �̺�Ʈ���� ���� ����
        if (PlayerStat.Instance != null)
        {
            PlayerStat.Instance.OnEnergyChange -= UpdateEnergy;  // ������ �̺�Ʈ ���� ����
        }
    }
}
