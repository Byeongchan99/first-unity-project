using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;  // TimeSpan을 사용하기 위해 추가

public class GameResult : MonoBehaviour
{
    public TextMeshProUGUI StatTime;
    public TextMeshProUGUI StatGold;
    public TextMeshProUGUI StatKill;

    public void OnEnable()
    {
        // 총 플레이 시간을 TimeSpan 객체로 변환
        TimeSpan timePlayed = TimeSpan.FromSeconds(GameManager.instance.gameTime);

        // 시간, 분, 초 형태의 문자열로 변환
        string timeFormatted = string.Format("{0:D2} : {1:D2} : {2:D2}", timePlayed.Hours, timePlayed.Minutes, timePlayed.Seconds);

        StatTime.text = timeFormatted;
        StatGold.text = "" + PlayerStat.Instance.Gold;
        StatKill.text = "" + PlayerStat.Instance.Kill;
    }
}
