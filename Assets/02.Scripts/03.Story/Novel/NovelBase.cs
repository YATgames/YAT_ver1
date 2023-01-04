using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Collections.LowLevel.Unsafe;

public class NovelBase : MonoBehaviour
{
    // 이 스크립트에는 모션 기능만 제작해놓기
    // State
    
    [HideInInspector] public int curDiaCount;
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
    
    protected void Initialize(Image[] images, Text[] texts)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color= Color.clear;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = "";
        }
    }
    
}
