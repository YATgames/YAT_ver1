using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using DG.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class CreateAction : MonoBehaviour
{
    [SerializeField] private Button _caseButton;
    [SerializeField] private Button _skipButton;

    [SerializeField] private Transform _caseObj;
    [SerializeField] private Transform _sunShineEffect;

    [SerializeField] private Text _figureText;

    [SerializeField] private GameObject[] _notActionObjs;

    [SerializeField] private float _textDelay = 0.05f;

    private bool _isClickCase = false;

    private string[] _createTextWords = new string[5]
    {
        "요술의 힘으로,\n고스트야 움직여라!",
        "고스트 에너지 충전중,\n곧 충전이 완료됩니다.",
        "나를 깨워줘!\n여기서 내보내줘!",
        "케이스에서 고스트를 해방시켜 주세요!",
        "여긴 어디? 나는 누구?",
    };
    private int _textCount = 1;
    private string _selectedWord;

    private string _title = string.Empty;
    private string _id = string.Empty;

    private Vector3 _originCasePos;
    private Vector3 _originCaseRotate;
    private Vector3 _originCaseScale;
    
    private Renderer _caseRenderer;
    private Sequence _sequence;


    private void Start()
    {
        AddEvent();
        _caseRenderer = _caseObj.GetComponent<Renderer>();
        _caseRenderer.material.color = new Color((float)70 / 255, (float)70 / 255, (float)70 / 255, 1);
    }

    private void AddEvent()
    {
        _skipButton.OnClickAsObservable().Subscribe(v => StartCoroutine(Skip())).AddTo(gameObject);
        _caseButton.OnClickAsObservable().Subscribe(_ => _isClickCase = true).AddTo(gameObject);
    }

    public void IsTagging(string title, string id)
    {
        _title = title;
        _id = id;
        _selectedWord = _createTextWords[UnityEngine.Random.Range(0, _createTextWords.Length)];
        SoundManager.Instance.PlayBGM("AfterTagging_BGM");
        StartCoroutine(TaggingRoutine());
    }

    private IEnumerator TaggingRoutine()
    {
        Init();

        IEnumerator shakeRoutine = ShakeRoutine();
        StartCoroutine(shakeRoutine);
        IEnumerator textRoutine = PrintTextRoutine();
        StartCoroutine(textRoutine);
        while (!_isClickCase)
        {
            // 클릭 전까지 반복
            yield return null;
        }
        SoundManager.Instance.Play("Tagging_Touch");
        SoundManager.Instance.StopBGM();

        Debug.Log("CompleteShake");
        StopCoroutine(shakeRoutine);
        StopCoroutine(textRoutine);
        CompleteShake();

        yield return new WaitForSeconds(1f);

        Debug.Log("빛 이펙트 발생");
        ActiveSunrise();
        SoundManager.Instance.Play("CircleEffect_SFX");
        yield return new WaitForSeconds(4.5f);

        _sunShineEffect.DOComplete();

        CreateFigure(_title, _id, false);

        yield return new WaitForSeconds(1f);

        End();
    }

    private IEnumerator PrintTextRoutine()
    {
        while (_textCount <= _selectedWord.Length)
        {
            PrintFigureText();
            yield return new WaitForSeconds(_textDelay);
        }
    }

    private void ResetObj()
    {
        _caseObj.localPosition = _originCasePos;
        _caseObj.localEulerAngles = _originCaseRotate;
        _caseObj.localScale = _originCaseScale;
    }

    private void CreateFigure(string title, string id , bool isSkip)
    {
        FlowManager.Instance.AddSubPopup(PopupStyle.CreateProduction, title, id, isSkip);
    }

    #region ::::::::Settings
    private void Init()
    {
        _skipButton.gameObject.SetActive(true);
        _originCasePos = _caseObj.localPosition;
        _originCaseRotate = _caseObj.localEulerAngles;
        _originCaseScale = _caseObj.localScale;
        _caseRenderer = _caseObj.GetComponent<Renderer>();
        _caseRenderer.material.color = new Color((float)70 / 255, (float)70 / 255, (float)70 / 255, 1);
        _textCount = 1;
        _figureText.text = string.Empty;
        ActiveOBJ(false);
    }
    private void End()
    {
        ActiveOBJ(true);
        _skipButton.gameObject.SetActive(false);

        _sunShineEffect.localScale = Vector3.zero;

        _sunShineEffect.gameObject.SetActive(false);

        _caseRenderer.material.color = new Color((float)70 / 255, (float)70 / 255, (float)70 / 255, 1);

        _isClickCase = false;
    }
    private IEnumerator Skip()
    {
        SoundManager.Instance.Stop();
        SoundManager.Instance.StopBGM();
        CreateFigure(_title, _id, true);
        yield return new WaitForSeconds(0.1f);

        StopAllCoroutines();
        ResetObj();
        ActiveOBJ(true);
        
        _figureText.text = string.Empty;

        _sunShineEffect.localScale = Vector3.zero;
        
        _skipButton.gameObject.SetActive(false);
        _sunShineEffect.gameObject.SetActive(false);

        _caseRenderer.material.color = new Color((float)70 / 255, (float)70 / 255, (float)70 / 255, 1);

        _isClickCase = false;
    }
    #endregion

    #region :::::::::Shaking
    private void CompleteShake()
    {
        _figureText.text = string.Empty;
        _caseObj.DOKill();
        _sequence.Kill();
        ResetObj();
        _caseRenderer.material.color = Color.white;
    }
    private IEnumerator ShakeRoutine()
    {
        _textCount = 1;

        while (!_isClickCase)
        {
            _caseObj.DOShakePosition(1, 15, 30, 90, false, true);
            _caseObj.DOShakeRotation(1, 1, 1, 1, true);
            _caseObj.DOShakeScale(1, 1, 10, 90, true);

            _sequence = DOTween.Sequence();
            _sequence.Append(_caseRenderer.material.DOColor(Color.white, 0.5f))
                    .Append(_caseRenderer.material.DOColor(new Color((float)70 / 255, (float)70 / 255, (float)70 / 255, 1), 0.5f));

            yield return new WaitForSeconds(1f);

            ResetObj();
        }
    }
    #endregion

    #region :::::::::Actives
    private void ActiveSunrise()
    {
        _sunShineEffect.gameObject.SetActive(true);

        _sunShineEffect.DOScale(new Vector3(8f, 8f, 8f), 1.5f).SetEase(Ease.InOutCirc);
    }

    private void ActiveOBJ(bool active)
    {
        for (int i = 0; i < _notActionObjs.Length; i++)
        {
            _notActionObjs[i].gameObject.SetActive(active);
        }
    }
    #endregion

    #region :::::::::Text
    private void PrintFigureText()
    {
        string text = _selectedWord.Substring(0, _textCount);

        _figureText.text = text;

        if (_textCount <= _selectedWord.Length) _textCount++;
        else _textCount = 1;
    }
    #endregion
    
}
