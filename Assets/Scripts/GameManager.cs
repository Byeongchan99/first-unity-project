using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]   // 인스펙터의 속성들을 구분시켜주는 타이틀
    public bool isLive;   // 시간 흐름 조절
    public bool isInvincible = false;
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
        waveManager.StartWave();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        // 사망 애니메이션을 적용하기 위한 딜레이
        yield return new WaitForSeconds(3f);

        /*
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        */

        GameRetry();
    }
    

    /*
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        // 몬스터를 청소하기 위한 딜레이
        yield return new WaitForSeconds(0.1f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }
    */

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
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
