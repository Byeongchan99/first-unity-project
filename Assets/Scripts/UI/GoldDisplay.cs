using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    // GoldDisplay 스크립트
    private void Update()
    {
        if (PlayerStat.Instance != null)
        {
            goldText.text = "현재 골드: <color=#FFD700>" + PlayerStat.Instance.Gold + "G</color>";
        }
        else
        {
            goldText.text = "현재 골드: 데이터 없음";
        }
    }

}
