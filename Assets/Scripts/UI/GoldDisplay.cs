using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void Update()
    {
        goldText.text = "���� ���: " + PlayerStat.Instance.Gold + "<color=#FFD700>G</color>"; // #FFD700�� Ȳ�ݻ�
    }
}
