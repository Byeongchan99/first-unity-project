using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutMeleeChoice : MonoBehaviour
{
    RectTransform rect;
    public LoadoutData[] meleeLoadouts;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }

    public void DisplayWeaponIcon()
    {
        Debug.Log("DisplayWeaponIcon called");

        for (int i = 0; i < meleeLoadouts.Length; i++)
        {
            UpdateWeaponIcon(meleeLoadouts[i], i);
        }
    }

    private void UpdateWeaponIcon(LoadoutData loadout, int index)
    {
        Transform weaponIcons = transform.Find("Weapon Icon");

        if (weaponIcons == null)
        {
            Debug.LogError("No Weapon Icon found!");
            return;
        }

        // Ability BackGround�� �ڽ� ������Ʈ�� ã���ϴ�.
        Transform weaponIcon = weaponIcons.Find("Icon " + index);

        if (weaponIcon == null)
        {
            Debug.LogError("No UI display found for Ability " + index);
            return;
        }

        // ������ �̹��� �Ҵ�
        weaponIcon.Find("Icon Image").GetComponent<Image>().sprite = loadout.weaponIcon;

        Button weaponButton = weaponIcon.GetComponent<Button>();
        if (weaponButton != null)
        {
            weaponButton.onClick.RemoveAllListeners();
            weaponButton.onClick.AddListener(() => DisplayWeaponInformation(loadout.weaponID));
        }
    }

    public void DisplayWeaponInformation(int weaponID)
    {
        Debug.Log("DisplayWeaponInformation called for weaponID: " + weaponID);
        LoadoutData loadout = GetLoadoutDataByWeaponID(weaponID);
        if (loadout != null)
        {
            UpdateWeaponInformation(loadout);
        }
        else
        {
            Debug.LogError("No loadout found for weaponID: " + weaponID);
        }
    }

    private LoadoutData GetLoadoutDataByWeaponID(int weaponID)
    {
        foreach (LoadoutData loadout in meleeLoadouts)
        {
            if (loadout.weaponID == weaponID)
            {
                return loadout;
            }
        }
        return null;
    }

    private void UpdateWeaponInformation(LoadoutData loadout)
    {
        Transform weaponInformationBackground = transform.Find("Weapon Information Background");

        if (weaponInformationBackground == null)
        {
            Debug.LogError("No Weapon Infomation Background found!");
            return;
        }

        Transform weaponInformation = weaponInformationBackground.Find("Weapon Information");

        if (weaponInformation == null)
        {
            Debug.LogError("No Weapon Information found!");
            return;
        }

        // ���� ���� �Ҵ�
        weaponInformation.Find("Weapon Name").GetComponent<Text>().text = loadout.weaponName;
        weaponInformation.Find("Weapon Description").GetComponent<Text>().text = loadout.weaponDesc;
        weaponInformation.Find("Weapon Image").GetComponent<Image>().sprite = loadout.weaponImage;
    }
}
