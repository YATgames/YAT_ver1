using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Assets.Scripts.Util;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;

public class IconListButton : MonoBehaviour
{
    [SerializeField] private IconFilm _iconFilm;

    [SerializeField] private Sprite[] _iconStateSprites;

    private Button _iConListButton;
    private Image _iconImage;

    private bool isAtive = false;

    private void Start()
    {
        _iConListButton = GetComponent<Button>();
        _iconImage = GetComponent<Image>();
        AddEvent();
    }

    private void AddEvent()
    {
        _iConListButton.OnClickAsObservable("IconList_Open").Subscribe(_ => InvenIconOnOff()).AddTo(gameObject);
    }

    private void InvenIconOnOff()
    {
        if (isAtive)
        {
            _iconImage.sprite = _iconStateSprites[0];
            _iconFilm.transform.DOScaleX(0, 0.2f);
            isAtive = false;
        }
        else
        {
            _iconImage.sprite = _iconStateSprites[1];
            _iconFilm.transform.DOScaleX(1f, 0.2f);
            isAtive = true;
        }
    }
}
