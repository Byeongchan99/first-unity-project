using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutMeleeChoice : MonoBehaviour
{
    RectTransform rect;
    LoadoutData selectedLoadout;
    public LoadoutData[] meleeLoadouts;   // ���� ������
    // ���� ���� ��ư
    public Button equipButton;

    // ��� ���� �����ܵ�
    Transform weaponIcons;
    // ���� ���� ��� - ���� ��
    Transform weaponInformationBackground;
    // ���� ���� - �г�
    Transform weaponInformation;
    // ���� �̸�
    Transform weaponName;
    // ���� ����
    Transform weaponDescription;
    // ���� �̹���
    Transform weaponImage;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        weaponIcons = transform.Find("Weapon Icon");
        weaponInformationBackground = transform.Find("Weapon Information Background");
        weaponInformation = weaponInformationBackground.Find("Weapon Information");
        weaponName = weaponInformation.Find("Weapon Name");
        weaponDescription = weaponInformation.Find("Weapon Description");
        weaponImage = weaponInformation.Find("Weapon Image");
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        DisplayWeaponInformation(PlayerStat.Instance.weaponManager.Weapon.WeaponID);
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

    // ���� ���� ����Ʈ ǥ�ø� ���� �ٽ� ����
    public void ReDisplay()
    {
        DisplayWeaponIcon();
        DisplayWeaponInformation(selectedLoadout.weaponID);
    }

    // ���� ����Ʈ Ȱ��ȭ
    public void DisplayEquipEffect(Transform weapon, int weaponID)
    {
        if (PlayerStat.Instance.weaponManager.Weapon.WeaponID == weaponID)
        {
            weapon.Find("Equipped Image").gameObject.SetActive(true); // ���� ǥ�� �̹��� Ȱ��ȭ
        }
        else
        {
            weapon.Find("Equipped Image").gameObject.SetActive(false); // ���� ǥ�� �̹��� ��Ȱ��ȭ
        }
    }

    // ���� ������ Ȱ��ȭ
    public void DisplayWeaponIcon()
    {
        Debug.Log("DisplayWeaponIcon called");

        for (int i = 0; i < meleeLoadouts.Length; i++)
        {
            UpdateWeaponIcon(meleeLoadouts[i], i);
        }
    }

    // ���� ������ ���� ������Ʈ
    private void UpdateWeaponIcon(LoadoutData loadout, int index)
    {
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
            // Ŭ���� ������ �ذ��ϱ� ���� localLoadout ������ ���
            LoadoutData localLoadout = loadout;
            weaponButton.onClick.AddListener(() =>
            {
                DisplayWeaponInformation(localLoadout.weaponID);
                selectedLoadout = localLoadout;
            });
        }

        // ���� ǥ�� �̹��� ������Ʈ
        DisplayEquipEffect(weaponIcon, loadout.weaponID);
    }

    // ���� ���� Ȱ��ȭ
    public void DisplayWeaponInformation(int weaponID)
    {
        Debug.Log("DisplayWeaponInformation called for weaponID: " + weaponID);
        LoadoutData loadout = GetLoadoutDataByWeaponID(weaponID);
        if (loadout != null)
        {
            UpdateWeaponInformation(loadout);

            if (weaponID == 100)   // ���� ���̵� 100�� ��� ����
                equipButton.interactable = false;
            else
                equipButton.interactable = true;
        }
        else
        {
            Debug.LogError("No loadout found for weaponID: " + weaponID);
        }
    }

    // WeaponID�� ���� LoadoutData�� ã�� ��ȯ
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

    // ���� ���� ������Ʈ
    private void UpdateWeaponInformation(LoadoutData loadout)
    {
        if (weaponInformationBackground == null)
        {
            Debug.LogError("No Weapon Infomation Background found!");
            return;
        }

        if (weaponInformation == null)
        {
            Debug.LogError("No Weapon Information found!");
            return;
        }

        // ���� ���� �Ҵ�
        weaponName.GetComponent<Text>().text = loadout.weaponName;
        weaponDescription.GetComponent<Text>().text = loadout.weaponDesc;
        weaponImage.GetComponent<Image>().sprite = loadout.weaponImage;

        // ���� ǥ�� �̹��� ������Ʈ
        DisplayEquipEffect(weaponInformation, loadout.weaponID);
    }

    // WeaponManager�� �����Ͽ� ���⸦ ����
    public void EquipSelectedWeapon()
    {   
        Debug.Log("EquipSelectedWeapon called");
        PlayerStat.Instance.weaponManager.EquipWeapon(selectedLoadout.weaponPrefab);
    }
}
