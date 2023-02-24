using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField] private float _swipeTime = 0.2f;                   

    [SerializeField] private float _swipeDistance = 50.0f;              

    [SerializeField] private Scrollbar _scrollbar;

    [SerializeField] private Transform[] _circleContents;

    private float[] _scrollPageValues;
    private float _valueDistance;
    private float _startTouchX;
    private float _entTouchX;
    private float _circleContentScale = 1.2f;
    private int _currentPage;
    private int _maxpage;
    private bool _isSwipeMode = false;

    private void Awake()
    {
        _scrollPageValues = new float[transform.childCount];

        _valueDistance = 1f / (_scrollPageValues.Length - 1);

        for (int i = 0; i < _scrollPageValues.Length; i++)
        {
            _scrollPageValues[i] = _valueDistance * i;
        }

        _maxpage = transform.childCount;
    }

    private void Start()
    {
        SetScrollBarValue(0);
    }

    public void SetScrollBarValue(int index)
    {
        _currentPage = index;
        _scrollbar.value = _scrollPageValues[index];
    }
    private void Update()
    {
        UpdateInput();


        if (_circleContents.Length == 0) return;
        UpdateCircleContent();
    }

    private void UpdateInput()
    {
        if (_isSwipeMode == true) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            _startTouchX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _entTouchX = Input.mousePosition.x;

            UpdateSwipe();
        }
#else
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                _startTouchX = touch.position.x;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                _entTouchX = Input.mousePosition.x;

                UpdateSwipe();
            }
        }
#endif
    }

    private void UpdateSwipe()
    {
        if (Mathf.Abs(_startTouchX - _entTouchX) < _swipeDistance)
        {
            StartCoroutine(OnSwipreOneStep(_currentPage));
            return;
        }

        bool isLeft = _startTouchX < _entTouchX ? true : false;

        if (isLeft == true)
        {
            if (_currentPage == 0)
            {
                SoundManager.Instance.Play("ButtonFail_SFX");
                return;
            }
            SoundManager.Instance.Play("IconList_Open");
            _currentPage--;
        }
        else
        {
            if (_currentPage == _maxpage - 1)
            {
                SoundManager.Instance.Play("ButtonFail_SFX");
                return;
            }
            SoundManager.Instance.Play("IconList_Open");
            _currentPage++;
        }

        StartCoroutine(OnSwipreOneStep(_currentPage));
    }

    private IEnumerator OnSwipreOneStep(int index)
    {
        float start = _scrollbar.value;
        float current = 0;
        float percent = 0;

        _isSwipeMode = true;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / _swipeTime;

            _scrollbar.value = Mathf.Lerp(start, _scrollPageValues[index], percent);

            yield return null;
        }

        _isSwipeMode = false;
    }

    private void UpdateCircleContent()
    {
        for (int i = 0; i < _scrollPageValues.Length; i++)
        {
            _circleContents[i].localScale = Vector3.one;
            _circleContents[i].GetComponent<Image>().color = Color.white;

            if(_scrollbar.value < _scrollPageValues[i] + (_valueDistance/2) &&
                _scrollbar.value > _scrollPageValues[i] - (_valueDistance / 2))
            {
                _circleContents[i].localScale = Vector3.one * _circleContentScale;
                _circleContents[i].GetComponent<Image>().color = Color.black;
            }
        }
    }
}
