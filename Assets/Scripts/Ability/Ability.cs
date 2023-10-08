using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public AbilityData data;
    Image image;
    Text textName;
    Text textDesc;

    void Awake()
    {
        image = GetComponentsInChildren<Image>()[1];   // 0은 자기 자신 1이 자식 컴포넌트 image
        image.sprite = data.abilityImage;

        Text[] texts = GetComponentsInChildren<Text>();
        textName = texts[0];
        textDesc = texts[1];
        textName.text = data.abilityName;
        textDesc.text = data.abilityDesc;
    }

    void OnEnable()
    {
        
    }

    public void OnClick()
    {
        
    }
}
