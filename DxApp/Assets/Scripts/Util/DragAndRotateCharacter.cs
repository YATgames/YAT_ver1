using Assets.Scripts.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndRotateCharacter : MonoBehaviour, IDragHandler
{

    [SerializeField] private GameObject _character;
    [SerializeField] private float _speed = 5f;
    
    private Vector3 _rot = Vector3.zero;
    Vector3 originAngle;

    void Start()
    {
        _rot = _character.transform.localEulerAngles;
        originAngle = _character.transform.localEulerAngles;

        AddEvent();
    }

    private void AddEvent()
    {
        PlayerViewModel.Instance.UpdateItem.AsObservable().Subscribe(_ =>
        {
            _character.transform.localEulerAngles = originAngle;
            _rot = _character.transform.localEulerAngles;
        }).AddTo(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rot.y -= Input.GetAxis("Mouse X") * _speed;
        _character.transform.localEulerAngles = _rot;
    }

    public void DragResetAndPause()
    {
        _character.transform.localEulerAngles = originAngle;
        _rot = _character.transform.localEulerAngles;
        this.enabled = false;
    }
}
