using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class DuplicateCheckInView : MonoBehaviour
{
    [SerializeField] private Button _okayButton;
    void Start()
    {
        AddEvent();
    }

    private void AddEvent()
    {
        _okayButton.OnClickAsObservable().Subscribe(v => gameObject.SetActive(false)).AddTo(gameObject);
    }
}
