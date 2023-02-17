using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class FadeRepeat : MonoBehaviour
{
    [SerializeField] private float _fadeInAlpha = 1f;
    [SerializeField] private float _fadeOutAlpha = 0f;

    [SerializeField] private float _time = 0.3f;

    private Image _repeatImage;
    private void OnEnable()
    {
        _repeatImage = GetComponent<Image>();
        StartCoroutine(FadeRepeatRoutine());
    }
    private void OnDisable()
    {
        StopCoroutine("FadeRepeatRoutine");
    }

    private IEnumerator FadeRepeatRoutine()
    {
        while(true)
        {

            ReactiveProperty<bool> complete = new ReactiveProperty<bool>();
            complete.Value = false;
            Sequence sequence = DOTween.Sequence()
                .SetAutoKill(true)
                .Append(_repeatImage.DOFade(_fadeInAlpha, _time))
                .Append(_repeatImage.DOFade(_fadeOutAlpha, _time))
                .OnComplete(() => { complete.Value = true; });

            yield return complete
                .ObserveEveryValueChanged(v => v.Value)
                .Where(v => v)
                .Take(1)
                .StartAsCoroutine();
        }
    }
}
