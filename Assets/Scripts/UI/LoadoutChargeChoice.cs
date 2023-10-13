using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutChargeChoice : MonoBehaviour
{
    RectTransform rect;
    LoadoutData selectedLoadout;
    public LoadoutData[] chargeLoadouts;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        DisplayWeaponInformation(PlayerStat.Instance.chargeWeaponManager.Weapon.WeaponID);
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

    // 무기 아이콘 활성화
    public void DisplayWeaponIcon()
    {
        Debug.Log("DisplayWeaponIcon called");

        for (int i = 0; i < chargeLoadouts.Length; i++)
        {
            UpdateWeaponIcon(chargeLoadouts[i], i);
        }
    }

    // 무기 아이콘 정보 업데이트
    private void UpdateWeaponIcon(LoadoutData loadout, int index)
    {
        Transform weaponIcons = transform.Find("Weapon Icon");

        if (weaponIcons == null)
        {
            Debug.LogError("No Weapon Icon found!");
            return;
        }

        // Ability BackGround의 자식 오브젝트를 찾습니다.
        Transform weaponIcon = weaponIcons.Find("Icon " + index);

        if (weaponIcon == null)
        {
            Debug.LogError("No UI display found for Ability " + index);
            return;
        }

        // 아이콘 이미지 할당
        weaponIcon.Find("Icon Image").GetComponent<Image>().sprite = loadout.weaponIcon;

        Button weaponButton = weaponIcon.GetComponent<Button>();
        if (weaponButton != null)
        {
            weaponButton.onClick.RemoveAllListeners();
            // 클로저 문제를 해결하기 위해 localLoadout 변수를 사용
            LoadoutData localLoadout = loadout;
            weaponButton.onClick.AddListener(() =>
            {
                DisplayWeaponInformation(localLoadout.weaponID);
                selectedLoadout = localLoadout;
            });
        }
    }

    // 무기 정보 활성화
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

    // WeaponID를 통해 LoadoutData를 찾아 반환
    private LoadoutData GetLoadoutDataByWeaponID(int weaponID)
    {
        foreach (LoadoutData loadout in chargeLoadouts)
        {
            if (loadout.weaponID == weaponID)
            {
                return loadout;
            }
        }
        return null;
    }

    // 무기 정보 업데이트
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

        // 무기 정보 할당
        weaponInformation.Find("Weapon Name").GetComponent<Text>().text = loadout.weaponName;
        weaponInformation.Find("Weapon Description").GetComponent<Text>().text = loadout.weaponDesc;
        weaponInformation.Find("Weapon Image").GetComponent<Image>().sprite = loadout.weaponImage;
    }

    // chargeWeaponManager에 접근하여 무기를 장착
    public void EquipSelectedWeapon()
    {
        Debug.Log("EquipSelectedWeapon called");
        PlayerStat.Instance.chargeWeaponManager.EquipWeapon(selectedLoadout.weaponPrefab);
    }
}
