using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutChargeChoice : MonoBehaviour
{
    RectTransform rect;
    LoadoutData selectedLoadout;
    public LoadoutData[] chargeLoadouts;
    [SerializeField]
    int lastClickedWeaponID = 10;   // 마지막으로 클릭한 무기 아이디
    // 무기 장착 버튼
    public Button equipButton;

    // 상단 무기 아이콘들
    Transform weaponIcons;
    // 무기 정보 배경 - 검은 벽
    Transform weaponChargeBackground;
    // 무기 정보 - 패널
    Transform weaponInformation;
    // 무기 이름
    Transform weaponName;
    // 무기 설명
    Transform weaponDescription;
    // 무기 이미지
    Transform weaponImage;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        weaponIcons = transform.Find("Weapon Icon");
        weaponChargeBackground = transform.Find("Weapon Charge Background");
        weaponInformation = weaponChargeBackground.Find("Weapon Information");
        weaponName = weaponInformation.Find("Weapon Name");
        weaponDescription = weaponInformation.Find("Weapon Description");
        weaponImage = weaponInformation.Find("Weapon Image");

        selectedLoadout = chargeLoadouts[0];   // 처음에 장착된 무기는 기본 무기
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
        //GameManager.instance.Resume();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }

    // 무기 장착 이펙트 표시를 위한 다시 실행
    public void ReDisplay()
    {
        DisplayWeaponIcon();
        DisplayWeaponInformation(selectedLoadout.weaponID);
    }

    // 장착 이펙트 활성화
    public void DisplayEquipEffect(Transform weapon, int weaponID)
    {
        if (PlayerStat.Instance.chargeWeaponManager.Weapon.WeaponID == weaponID)
        {
            weapon.Find("Equipped Image").gameObject.SetActive(true); // 장착 표시 이미지 활성화
        }
        else
        {
            weapon.Find("Equipped Image").gameObject.SetActive(false); // 장착 표시 이미지 비활성화
        }
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
                lastClickedWeaponID = localLoadout.weaponID;
                DisplayWeaponIcon();   // 무기 아이콘 다시 디스플레이
            });
        }

        // 선택 이펙트 활성화
        ColorBlock currentColor = weaponButton.colors;

        if (lastClickedWeaponID == loadout.weaponID)   // 마지막으로 클릭한 무기의 ID와 같을 시
        {
            currentColor.normalColor = new Color(0.565f, 0.0f, 0.125f, 1.0f);   // 붉은색으로 변경
            weaponButton.colors = currentColor;
        }
        else
        {
            currentColor.normalColor = Color.black;   // 검은색으로 변경
            weaponButton.colors = currentColor;
        }

        // 장착 표시 이미지 업데이트
        DisplayEquipEffect(weaponIcon, loadout.weaponID);
    }

    // 무기 정보 활성화 - 무기 아이콘 눌렀을 때 실행
    public void DisplayWeaponInformation(int weaponID)
    {
        Debug.Log("DisplayWeaponInformation called for weaponID: " + weaponID);
        LoadoutData loadout = GetLoadoutDataByWeaponID(weaponID);
        if (loadout != null)
        {
            UpdateWeaponInformation(loadout);

            if (weaponID >= 100)   // 무기 아이디 100 이상은 잠김 상태
                equipButton.interactable = false;
            else
                equipButton.interactable = true;
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
        if (weaponChargeBackground == null)
        {
            Debug.LogError("No Weapon Charge Background found!");
            return;
        }      

        if (weaponInformation == null)
        {
            Debug.LogError("No Weapon Information found!");
            return;
        }

        // 무기 정보 할당
        weaponName.GetComponent<Text>().text = loadout.weaponName;
        weaponDescription.GetComponent<Text>().text = loadout.weaponDesc;
        weaponImage.GetComponent<Image>().sprite = loadout.weaponImage;

        // 장착 표시 이미지 업데이트
        DisplayEquipEffect(weaponInformation, loadout.weaponID);
    }

    // chargeWeaponManager에 접근하여 무기를 장착
    public void EquipSelectedWeapon()
    {
        Debug.Log("EquipSelectedWeapon called");
        PlayerStat.Instance.chargeWeaponManager.EquipWeapon(selectedLoadout.weaponPrefab);
        UIManager.instance.InventoryIconUI.UpdateChargeWeaponIcon(selectedLoadout.weaponIcon);   // 장비창 아이콘 UI 업데이트
    }
}
