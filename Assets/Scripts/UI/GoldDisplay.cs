using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void Update()
    {
        goldText.text = "현재 골드: " + PlayerStat.Instance.Gold + "<color=#FFD700>G</color>"; // #FFD700은 황금색
    }
}
