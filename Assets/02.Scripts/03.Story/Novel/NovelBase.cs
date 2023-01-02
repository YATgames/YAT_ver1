using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NovelBase : MonoBehaviour
{
    // Instance
    public static NovelBase instance;
    
    public novel_01 novel_01; // 스토리타입 : 인트로
    
    // State
    private int curDiaCount;

    private void Awake()
    {
        instance = this;
    }
    
    protected Image[] img_Characters;
    
    private float _showDuration= 0.5f; // 등장 딜레이시간
    private float _moveDuration = 0.2f;
    
    
    protected void cha_Show(int num, float duration)
    {
        img_Characters[num].DOFade(1f, duration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                img_Characters[num].DOColor(Color.white, duration);
            });
    }
    protected void cha_Movement(int num, float duration) // 위아래로 살짝 움직이는 그거
    {
        RectTransform obj = img_Characters[num].GetComponent<RectTransform>();
        obj.DOAnchorPosY(30f, duration).SetRelative(true)
            .SetEase(Ease.InQuad).OnComplete(() =>
            {
                obj.DOAnchorPosY(-30f, duration).SetRelative(true)
                    .SetEase(Ease.Linear);
            });
    }


    public void Call() // call
    {
        switch (curDiaCount)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }
}
