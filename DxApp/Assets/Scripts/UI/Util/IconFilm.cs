using Assets.Scripts.Common.DI;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class IconFilm : MonoBehaviour
{
    [DependuncyInjection(typeof(FlowManager))]
    private FlowManager _flowManager;

    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _CombineButton;
    [SerializeField] private Button _achievementButton;
    [SerializeField] private Button _nfcButton;
    [SerializeField] private Button _settingButton;


    void Start()
    {
        DependuncyInjection.Inject(this);

        AddEvent();
    }

    private void AddEvent()
    {
        if(_achievementButton != null) _achievementButton.OnClickAsObservable("Button_Touch").Subscribe(_ => _flowManager.Change(PopupStyle.Achievement)).AddTo(gameObject);
        if (_inventoryButton != null) _inventoryButton.OnClickAsObservable("Button_Touch").Subscribe(_ => _flowManager.Change(PopupStyle.Inventory)).AddTo(gameObject);
        if(_nfcButton != null) _nfcButton.OnClickAsObservable("Button_Touch").Subscribe(_ => _flowManager.Change(PopupStyle.NFC)).AddTo(gameObject);
        if (_CombineButton != null) _CombineButton.OnClickAsObservable("Button_Touch").Subscribe(_ => _flowManager.Change(PopupStyle.Combine)).AddTo(gameObject);
        if (_settingButton != null)_settingButton.OnClickAsObservable("Button_Touch").Subscribe(_ => _flowManager.AddSubPopup(PopupStyle.Settings)).AddTo(gameObject);
    }
}
