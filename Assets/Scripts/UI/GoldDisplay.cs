using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void Update()
    {
        goldText.text = "ÇöÀç °ñµå: <color=#FFD700>" + PlayerStat.Instance.Gold + "G</color>";
    }
}
