using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]   // �ν������� �Ӽ����� ���н����ִ� Ÿ��Ʋ
    public bool isLive;   // �ð� �帧 ����
    public bool isInvincible = false;   // ���� ����
    public bool isBattle;   // ���� ����
    public float gameTime;
    public float maxGameTime = 3 * 10f;

    [Header("# Game Object")]
    public PoolManager pool;
    public PlayerController playerController;
    // public Result uiResult;
    public WaveManager waveManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        instance = null;
    }

    /*
    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }
    */
    public void GameStart()
    {
        //waveManager.StartWave();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        UIManager.instance.gameOverUI.SetActive(true);
        // ��� �ִϸ��̼��� �����ϱ� ���� ������
        yield return new WaitForSeconds(1f);   // ��� �ִϸ��̼� ��� �ð� 0.5��

        Stop();
        /*
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        
        GameRetry();
        */
    }
       
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;

        UIManager.instance.gameVictoryUI.SetActive(true);
        yield return null; 

        /*
        enemyCleaner.SetActive(true);

        // ���͸� û���ϱ� ���� ������
        yield return new WaitForSeconds(0.1f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        */
    }

    public void GameRetry()
    {
        Debug.Log("GameRetry");
        // ���� ���� ���� ��� �ڷ�ƾ ����
        StopAllCoroutines();
        // ��� �̱��� �ʱ�ȭ
        GameManager.instance.DestroyInstance();
        InventoryManager.Instance.DestroyInstance();
        AchieveManager.instance.DestroyInstance();
        StageManager.Instance.DestroyInstance();
        UIManager.instance.DestroyInstance();
        WaveManager.Instance.DestroyInstance();
        PlayerController.Instance.DestroyInstance();

        SceneManager.LoadScene(0);   // ���� �����
        Resume();   // �� �ε� �ÿ��� �ð��� ���������Ƿ�, �ٽ� Ǯ���ֱ�
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;   // ����Ƽ�� �ð� �ӵ�(����)
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
