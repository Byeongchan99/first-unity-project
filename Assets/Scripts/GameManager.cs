using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]   // �ν������� �Ӽ����� ���н����ִ� Ÿ��Ʋ
    public bool isLive;   // �ð� �帧 ����
    public float gameTime;
    public float maxGameTime = 3 * 10f;

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;   // ����
    public int kill;   // ų ��

    [Header("# Game Object")]
    // public PoolManager pool;
    public Player player;
    // public Result uiResult;

    void Awake()
    {
        instance = this;
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

    
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        // ��� �ִϸ��̼��� �����ϱ� ���� ������
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

        // ���͸� û���ϱ� ���� ������
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
        Time.timeScale = 0;   // ����Ƽ�� �ð� �ӵ�(����)
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
