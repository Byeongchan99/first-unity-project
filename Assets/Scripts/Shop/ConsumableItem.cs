using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItem : MonoBehaviour
{
    // Ư�� �ɷ��� �ߵ��ϴ� �޼���
    public void ActivateItem(int itemID)
    {
        switch (itemID % 3)
        {
            case 0:
                recoveryHealth(1);
                break;
            case 1:
                recoveryHealth(2);
                break;
            case 2:
                recoveryHealth(3);
                break;          
            default:
                Debug.LogError($"Item with ID {itemID} not found.");
                break;
        }
    }

    private void recoveryHealth(int value)
    {
        PlayerStat.Instance.CurrentHP += value;
        // ü�� ȸ�� ����
        AudioManager.Instance.PlaySound(3, 0.3f);
        HealthManager.Instance.AdjustHearts();
    }
}