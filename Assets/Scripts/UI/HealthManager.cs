using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private List<Image> playerHealth;
    public static HealthManager Instance { get; private set; }

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
        InitializeHealthUI();

        // PlayerStat의 이벤트에 구독
        if (PlayerStat.Instance != null)
        {
            PlayerStat.Instance.OnHealthChange += UpdateHealth;
        }
    }

    void InitializeHealthUI()
    {
        foreach (Image heart in playerHealth)
        {
            Destroy(heart.gameObject);
        }
        playerHealth.Clear();

        for (int i = 0; i < PlayerStat.Instance.MaxHP; i++)
        {
            GameObject h = Instantiate(healthPrefab, this.transform);
            playerHealth.Add(h.GetComponent<Image>());
        }

        UpdateHealth();
    }

    void UpdateHealth()
    {
        // 최대 체력과 현재 체력을 비교
        if (PlayerStat.Instance.CurrentHP > PlayerStat.Instance.MaxHP)
        {
            PlayerStat.Instance.CurrentHP = PlayerStat.Instance.MaxHP; // 현재 체력을 최대 체력으로 설정
        }

        int currentHP = PlayerStat.Instance.CurrentHP;

        for (int i = 0; i < playerHealth.Count; i++)
        {
            if (i < currentHP)
            {
                playerHealth[i].gameObject.SetActive(true);
            }
            else
            {
                playerHealth[i].gameObject.SetActive(false);
            }
        }
    }


    // 필요한 경우에만 하트를 다시 추가/제거하는 로직
    public void AdjustHearts()
    {
        int difference = PlayerStat.Instance.MaxHP - playerHealth.Count;

        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                GameObject h = Instantiate(healthPrefab, this.transform);
                playerHealth.Add(h.GetComponent<Image>());
            }
        }
        else
        {
            for (int i = 0; i < -difference; i++)
            {
                Destroy(playerHealth[playerHealth.Count - 1 - i].gameObject);
            }

            playerHealth.RemoveRange(playerHealth.Count + difference, -difference);
        }

        UpdateHealth();
    }

    void OnDestroy()
    {
        // PlayerStat의 이벤트에서 구독 해제
        if (PlayerStat.Instance != null)
        {
            PlayerStat.Instance.OnHealthChange -= UpdateHealth;
        }
    }
}
