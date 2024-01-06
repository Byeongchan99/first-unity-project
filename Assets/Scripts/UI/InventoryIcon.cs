using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour
{
    RectTransform rect;
    public Image meleeWeaponIcon;
    public Image chargeWeaponIcon;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
    }

    public void UpdateMeleeWeaponIcon(Sprite weaponIcon)
    {
        meleeWeaponIcon.sprite = weaponIcon;
    }

    public void UpdateChargeWeaponIcon(Sprite weaponIcon)
    {
        chargeWeaponIcon.sprite = weaponIcon;
    }
}
