using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;  // TimeSpan�� ����ϱ� ���� �߰�

public class GameResult : MonoBehaviour
{
    public TextMeshProUGUI StatTime;
    public TextMeshProUGUI StatGold;
    public TextMeshProUGUI StatKill;

    public void OnEnable()
    {
        // �� �÷��� �ð��� TimeSpan ��ü�� ��ȯ
        TimeSpan timePlayed = TimeSpan.FromSeconds(GameManager.instance.gameTime);

        // �ð�, ��, �� ������ ���ڿ��� ��ȯ
        string timeFormatted = string.Format("{0:D2} : {1:D2} : {2:D2}", timePlayed.Hours, timePlayed.Minutes, timePlayed.Seconds);

        StatTime.text = timeFormatted;
        StatGold.text = "" + PlayerStat.Instance.Gold;
        StatKill.text = "" + PlayerStat.Instance.Kill;
    }
}
