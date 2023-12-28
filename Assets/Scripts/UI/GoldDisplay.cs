using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    // GoldDisplay ��ũ��Ʈ
    private void Update()
    {
        if (PlayerStat.Instance != null)
        {
            goldText.text = "���� ���: <color=#FFD700>" + PlayerStat.Instance.Gold + "G</color>";
        }
        else
        {
            goldText.text = "���� ���: ������ ����";
        }
    }

}
