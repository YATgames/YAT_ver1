using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.PrefabModel;
using DG.Tweening;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.TextCore.Text;
using System.Collections.Generic;
using DXApp_AppData.Item;
using Random = UnityEngine.Random;
using Assets.Scripts.UI;
using Assets.Scripts.Util;

public class CombineAction : MonoBehaviour
{
    [SerializeField] private DragAndRotateCharacter _dragAndRotateCharacter;
    [SerializeField] private GameObject _askInView;

    [SerializeField] private Button _okayButton;
    [SerializeField] private Button _goInventoryButton;
    [SerializeField] private Button _skipButton;

    [SerializeField] private Text _combineText;
    [SerializeField] private GameObject _textBoard;

    [SerializeField] private GameObject _dustParticle;
    [SerializeField] private GameObject[] _notActionObjs;
    [SerializeField] private GameObject[] _sparks;
    [SerializeField] private GameObject[] _lightning;
    [SerializeField] private ParticleSystem[] _bgLightningParticle;

    [SerializeField] private Toggle _headToggle;

    [SerializeField] private Transform _parent;

    [SerializeField] private Animator _pressAnimator;

    [SerializeField] private float _textDelay = 0.05f;
    [SerializeField] private float _intervalTime = 5f;

    private string[] _breakTextWords = new string[5]
    {
        "이번 고스트는 정말 멋질거야!\n…아마도 말이야.",
        "어떤 고스트가 나올까? 두근두근!",
        "모두의 파츠를 하나로 모아 여기에 등장!",
        "고스트가 조립되는 중입니다,\n바른 자세로 얌전하게 기다려 주세요.",
        "자, 어떤 고스트가 나올지 상상해보자!",
    };
    private string _selectedWord = null;
    private string _title = null;

    private int _textCount = 1;
    private bool _isClickButton = false;
    private bool _isActionning = false;

    [SerializeField] private Transform _changeScaleTransform;
    private Vector3 _originScale;

    private List<PartsInstance> _partsArchive = null;
    private GameObject character;
    private void Start()
    {
        AddEvent();
    }

    private void AddEvent()
    {
        Observable.Interval(TimeSpan.FromSeconds(_intervalTime)).Subscribe(_ =>
        {
            if (!_isActionning && !_askInView.activeSelf)
            {
                SoundManager.Instance.Play("LightningSmall_SFX");
                var lightning = _bgLightningParticle[Random.Range(0, _bgLightningParticle.Length)];
                if (lightning.gameObject.activeSelf == false)
                {
                    lightning.gameObject.SetActive(true);
                }
                else
                {
                    lightning.Play();
                }
            }
        }).AddTo(gameObject);

        _okayButton.OnClickAsObservable("Button_Touch").Subscribe(v => _isClickButton = true).AddTo(gameObject);
        _goInventoryButton.OnClickAsObservable("Button_Touch").Subscribe(v =>
        {
            PlayerViewModel.Instance.Reset();
            FlowManager.Instance.Change(PopupStyle.Inventory, true, _title);
        }).AddTo(gameObject);
        _skipButton.OnClickAsObservable("Button_Touch").Subscribe(v => Skip()).AddTo(gameObject);
    }

    public void ClickCombineButton(string title)
    {
        _partsArchive = PlayerViewModel.Instance.PartsArchive;
        _selectedWord = _breakTextWords[Random.Range(0, _breakTextWords.Length)];
        _title = title;
        StartCoroutine(CombineActionRoutine());
    }

    private IEnumerator CombineActionRoutine()
    {
        // 시작시 세팅
        Init();
        SoundManager.Instance.Play("Lightning_SFX");
        for (int i = 0; i < _bgLightningParticle.Length; i++)
        {
            var lightning = _bgLightningParticle[i];
            if (lightning.gameObject.activeSelf == false)
            {
                lightning.gameObject.SetActive(true);
            }
            else
            {
                lightning.Play();
            }
        }
        yield return new WaitForSeconds(1f);
        Debug.Log("우웅~ 칙! 문이 닫힌다");
        SetCloseAnim();
        SoundManager.Instance.Stop("CombineSFX");
        SoundManager.Instance.Play("Close_Press");

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < _lightning.Length; i++)
        {
            _lightning[i].SetActive(false);
        }

        yield return new WaitForSeconds(0.1f);
        Debug.Log("effect 실행");
        IEnumerator textRoutine = PrintTextRoutine();
        StartCoroutine(textRoutine);

        yield return StartCoroutine(ActiveSparks(true));

        Debug.Log("캐릭터 완성체로 교체");
        _parent.localScale = _changeScaleTransform.localScale;
        character = CombineCharacter();
        if (character == null) Skip();

        SetBlackCharacterRender(character);

         yield return new WaitForSeconds(3f);

        yield return StartCoroutine(ActiveSparks(false));

        yield return new WaitForSeconds(0.5f);
        Debug.Log("문양 이펙트 실행");

        yield return new WaitForSeconds(2f);
       
        StopCoroutine(textRoutine);
        _combineText.text = string.Empty;
        SetOpenAnim();

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < _lightning.Length; i++)
        {
            _lightning[i].SetActive(true);
        }
        Debug.Log("위잉 문이 열린다");
        SoundManager.Instance.Play("Close_Press");

        yield return new WaitForSeconds(1.5f);
        SoundManager.Instance.Play("Figure_Appear");
        Debug.Log("새로운 피규어 완성~");
        SetWhiteCharacterRender(character);

        yield return new WaitForSeconds(0.3f);
        _dustParticle.SetActive(true);
        SoundManager.Instance.Play("ClapSound_SFX", true);

        yield return new WaitForSeconds(0.7f);
        SoundManager.Instance.Play("Tagging_Touch");
        character.GetComponent<BoxCollider>().enabled = true;

        _combineText.text = "새로운\n 피규어 \n \"<color=yellow>" + _title + "</color>\"\n획득!!";
        _dragAndRotateCharacter.enabled = true;
        _okayButton.gameObject.SetActive(true);
        _goInventoryButton.gameObject.SetActive(true);
        _skipButton.gameObject.SetActive(false);

        while(!_isClickButton)
        {
            // 확인버튼 클릭까지 기다린다.
            yield return null;
        }

        character.gameObject.SetActive(false);
        End();
    }
    #region ::::::::::Settings
    private void Init()
    {
        _originScale = _parent.localScale;

        SetResetTrigger();

        _headToggle.isOn = true;

        ActiveOBJ(false);
        _skipButton.gameObject.SetActive(true);
        _isActionning = true;
        _isClickButton = false;
        _textBoard.gameObject.SetActive(true);
        _combineText.text = string.Empty;
        _textCount = 1;
    }
    private void End()
    {
        ActiveOBJ(true);

        if(character != null) character.GetComponent<BoxCollider>().enabled = false;
        for (int i = 0; i < _partsArchive.Count; i++)
        {
            // 리소스 리셋
            ResourcesManager.ResetModels(_partsArchive[i].ID);
        }

        _isActionning = false;
        _isClickButton = false;
        _parent.localScale = _originScale;
        _dragAndRotateCharacter.DragResetAndPause();
        _goInventoryButton.gameObject.SetActive(false);
        _okayButton.gameObject.SetActive(false);
        _textBoard.gameObject.SetActive(false);
        _dustParticle.SetActive(false);
        SoundManager.Instance.Stop();
        SoundManager.Instance.Play("Combine_SFX", true);
        PlayerViewModel.Instance.Reset();
    }
    private void Skip()
    {
        Debug.Log("Skip is called!!!");

        SoundManager.Instance.Stop();
        SoundManager.Instance.Play("Combine_SFX", true);
        SoundManager.Instance.Play("Tagging_Touch");

        StopAllCoroutines();

        DOTween.KillAll();

        SetSkipAnim();


        ActiveOBJ(true);

        for (int i = 0; i < _lightning.Length; i++)
        {
            _lightning[i].SetActive(true);
        }
        for (int i = 0; i < _sparks.Length; i++)
        {
            _sparks[i].gameObject.SetActive(false);
        }

        if (character != null)
        {
            var renders = character.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                if (renders[i] != null)
                    renders[i].material.color = Color.white;
            }

            character.GetComponent<BoxCollider>().enabled = false;

            character.SetActive(false);
        }

        for (int i = 0; i < _partsArchive.Count; i++)
        {
            ResourcesManager.ResetModels(_partsArchive[i].ID);
        }

        _isActionning = false;
        _parent.localScale = _originScale;
        _dragAndRotateCharacter.DragResetAndPause();
        _skipButton.gameObject.SetActive(false);
        _goInventoryButton.gameObject.SetActive(false);
        _okayButton.gameObject.SetActive(false);
        _textBoard.gameObject.SetActive(false);
        _dustParticle.SetActive(false);
        //transform.localPosition = Vector3.zero;
        PlayerViewModel.Instance.Reset();
    }
    #endregion

    #region :::::::::: Anims
    private void SetCloseAnim()
    {
        _pressAnimator.SetTrigger("Close");
    }
    private void SetOpenAnim()
    {
        _pressAnimator.SetTrigger("Open");
    }
    private void SetSkipAnim()
    {
        _pressAnimator.SetTrigger("Skip");
    }
    private void SetResetTrigger()
    {
        _pressAnimator.ResetTrigger("Close");
        _pressAnimator.ResetTrigger("Open");
        _pressAnimator.ResetTrigger("Skip");
    }

    #endregion

    #region::::::::::Texts
    private IEnumerator PrintTextRoutine()
    {
        while (_textCount <= _selectedWord.Length)
        {
            PrintFigureText();
            yield return new WaitForSeconds(_textDelay);
        }
    }
    private void PrintFigureText()
    {
        string text = _selectedWord.Substring(0, _textCount);

        _combineText.text = text;

        if (_textCount <= _selectedWord.Length) _textCount++;
        else _textCount = 1;
    }
    #endregion

    #region ::::::::::Actives
    private IEnumerator ActiveSparks(bool active)
    {
        if(active) SoundManager.Instance.Play("Spark_SFX", true);

        for (int i = 0; i < _sparks.Length; i++)
        {
            _sparks[i].gameObject.SetActive(active);
            yield return new WaitForSeconds(0.2f);
        }

        if (!active) SoundManager.Instance.Stop("SparkSFX");
    }

    private void ActiveOBJ(bool active)
    {
        for (int i = 0; i < _notActionObjs.Length; i++)
        {
            _notActionObjs[i].gameObject.SetActive(active);
        }
    }
    #endregion

    #region ::::::::::CharactersActions
    private void SetBlackCharacterRender(GameObject character)
    {
        if (character == null) return;

        var renders = character.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            if (renders[i] != null)
                renders[i].material.color = Color.black;
        }
    }
    private void SetWhiteCharacterRender(GameObject character)
    {
        if (character == null) return;


        var color = new Color((float)156 / 255, (float)156 / 255, (float)156 / 255, 1);
        var renders = character.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            if (renders[i] != null)
                renders[i].material.DOColor(color, 2f);
        }
    }
    #endregion
    private GameObject CombineCharacter()
    {
        GameObject body = null;
        GameObject head = null;
        GameObject deco = null;

        for (int i = 0; i < _partsArchive.Count; i++)
        {
            var item = _partsArchive[i].ID;
            switch (item.Substring(0, 1))
            {
                case "1": head = ResourcesManager.GetModel(item); break;
                case "2": body = ResourcesManager.GetModel(item); break;

                case "3":
                case "4":
                    deco = ResourcesManager.GetModel(item);
                    break;
            }
        }

        Model_Body _body = body?.GetComponent<Model_Body>();
        Model_Head _head = head?.GetComponent<Model_Head>();
        Model_Deco _deco = deco?.GetComponent<Model_Deco>();

        
        try
        {
            _body?.transform.SetParent(_parent);
            _body.ResetTransform();
            _head?.gameObject.transform.SetParent(_body.HeadPosition);
            _head?.ResetTransform();
            switch (_deco.DecoType)
            {
                case DecoType.OverHead:
                    _deco.gameObject.transform.SetParent(_head.DecoPosition);
                    break;

                case DecoType.Wing:
                    _deco.gameObject.transform.SetParent(_body.WingPos);
                    break;

                case DecoType.Tail:
                    _deco.gameObject.transform.SetParent(_body.TailPos);
                    break;
            }
            
            _deco.ResetTransform();
        }
        catch
        {

        }

        return _body?.gameObject;
    }
}
