using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private List<Image> playerHealth;
    public static HealthManager Instance { get; private set; }
    public Sprite redHeart;
    public Sprite blackHeart;

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

        // PlayerStat�� �̺�Ʈ�� ����
        if (PlayerStat.Instance != null)
        {
            PlayerStat.Instance.OnHealthChange += UpdateHealth;
        }
    }

    // ü�¹� UI �ʱ�ȭ
    void InitializeHealthUI()
    {
        foreach (Image heart in playerHealth)
        {
            Destroy(heart.gameObject);
        }
        playerHealth.Clear();

        // �ִ� ü�¸�ŭ ������ ��Ʈ ä���
        for (int i = 0; i < PlayerStat.Instance.MaxHP; i++)
        {
            GameObject heart = Instantiate(healthPrefab, this.transform);
            playerHealth.Add(heart.GetComponent<Image>());
        }

        UpdateHealth();
    }

    void UpdateHealth()
    {
        // �ִ� ü�°� ���� ü���� ��
        if (PlayerStat.Instance.CurrentHP > PlayerStat.Instance.MaxHP)
        {
            PlayerStat.Instance.CurrentHP = PlayerStat.Instance.MaxHP;   // ���� ü���� �ִ� ü������ ����
        }

        int currentHP = PlayerStat.Instance.CurrentHP;
        
        for (int i = 0; i < playerHealth.Count; i++)
        {
            // ���� ü�¸�ŭ ������ ��Ʈ
            if (i < currentHP)
            {
                playerHealth[i].gameObject.SetActive(true);
                playerHealth[i].sprite = redHeart;
            }
            // ���� ü���� ������ ��Ʈ
            else
            {
                //playerHealth[i].gameObject.SetActive(false);   
                // ������ ��Ʈ�� ����
                playerHealth[i].sprite = blackHeart;
            }
        }
    }

    // �ʿ��� ��쿡�� ��Ʈ�� �ٽ� �߰�/�����ϴ� ����
    public void AdjustHearts()
    {
        int difference = PlayerStat.Instance.MaxHP - playerHealth.Count;

        // �ִ� ü���� �þ��� ��
        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                GameObject heart = Instantiate(healthPrefab, this.transform);
                playerHealth.Add(heart.GetComponent<Image>());
            }
        }
        // �ִ� ü���� �������� ��
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
        // PlayerStat�� �̺�Ʈ���� ���� ����
        if (PlayerStat.Instance != null)
        {
            PlayerStat.Instance.OnHealthChange -= UpdateHealth;
        }
    }
}
