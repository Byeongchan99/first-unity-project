using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairAnimation : MonoBehaviour
{
    void playRepairSound()
    {
        AudioManager.Instance.PlaySound(5);
    }
}
