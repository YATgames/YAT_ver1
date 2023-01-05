using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NovelBase : MonoBehaviour
{
    // 이 스크립트에는 기능만 적용
    
    ///protected Image[] img_Characters;
    
    //private float _showDuration= 0.5f; // 등장 딜레이시간
    //private float _moveDuration = 0.2f;
    protected void cha_Show(Image obj, float duration)
    {
        obj.DOFade(1f, duration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                obj.DOColor(Color.white, duration);
            });
    }
    protected void cha_Interact(Image obj, float duration) // 위아래로 살짝 움직이는 그거
    {
        //obj = img_Characters[num].GetComponent<RectTransform>();
        obj.GetComponent<RectTransform>() .DOAnchorPosY(45f, duration * 0.5f).SetRelative(true)
            .SetEase(Ease.InQuad).OnComplete(() =>
            {
                 obj.GetComponent<RectTransform>().DOAnchorPosY(-45f, duration * 0.5f).SetRelative(true)
                    .SetEase(Ease.Linear);
            });
    }

    protected void cha_Movement(Image obj, float duration, float destination)
    {
        //RectTransform obj = img_Characters[num].GetComponent<RectTransform>();
        obj.GetComponent<RectTransform>().DOAnchorPosX(destination, duration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            
        });
    }

    protected virtual void OnEnable()
    {
        Initialize();
    }

    private GameObject base_CharacterBase;
    private GameObject base_TextBase;

    protected Image[] m_Character;
    protected Text[] m_Text;
    
    
    private void Initialize()
    {
        base_CharacterBase = this.transform.GetChild(1).gameObject;
        base_TextBase = this.transform.GetChild(2).gameObject;
        
        
        /*
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color= Color.clear;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = "";
        }*/
    }

    protected virtual void DialoguePlay()
    {
        Debug.Log("DialoguePlay");
    }

    protected virtual void defaultSetting() {}
}
