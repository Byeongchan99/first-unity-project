using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]   // 인스펙터의 속성들을 구분시켜주는 타이틀
    public bool isLive;   // 시간 흐름 조절
    public bool isInvincible = false;   // 무적 상태
    public bool isBattle;   // 전투 상태
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
        // 사망 애니메이션을 적용하기 위한 딜레이
        yield return new WaitForSeconds(1f);   // 사망 애니메이션 재생 시간 0.5초

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

        // 몬스터를 청소하기 위한 딜레이
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
        // 현재 실행 중인 모든 코루틴 중지
        StopAllCoroutines();
        // 모든 싱글톤 초기화
        GameManager.instance.DestroyInstance();
        InventoryManager.Instance.DestroyInstance();
        AchieveManager.instance.DestroyInstance();
        StageManager.Instance.DestroyInstance();
        UIManager.instance.DestroyInstance();
        WaveManager.Instance.DestroyInstance();
        PlayerController.Instance.DestroyInstance();

        SceneManager.LoadScene(0);   // 게임 재시작
        Resume();   // 씬 로드 시에도 시간이 멈춰있으므로, 다시 풀어주기
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
        Time.timeScale = 0;   // 유니티의 시간 속도(배율)
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
