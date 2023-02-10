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

public class CombineAction : MonoBehaviour
{
    [SerializeField] private DragAndRotateCharacter _dragAndRotateCharacter;

    [SerializeField] private Button _okayButton;
    [SerializeField] private Button _goInventoryButton;
    [SerializeField] private Button _skipButton;

    [SerializeField] private Text _combineText;
    [SerializeField] private GameObject _textBoard;

    [SerializeField] private GameObject _dustParticle;
    [SerializeField] private GameObject[] _notActionObjs;
    [SerializeField] private GameObject[] _sparks;

    [SerializeField] private Toggle _headToggle;

    [SerializeField] private Transform _parent;

    [SerializeField] private Animator _pressAnimator;

    [SerializeField] private float _textDelay = 0.05f;

    private string[] _breakTextWords = new string[5]
    {
        "�̹� ��Ʈ�� ���� �����ž�!\n���Ƹ��� ���̾�.",
        "� ��Ʈ�� ���ñ�? �αٵα�!",
        "����� ������ �ϳ��� ��� ���⿡ ����!",
        "��Ʈ�� �����Ǵ� ���Դϴ�,\n�ٸ� �ڼ��� �����ϰ� ��ٷ� �ּ���.",
        "��, � ��Ʈ�� ������ ����غ���!",
    };
    private string _selectedWord = null;
    private string _title = null;

    private int _textCount = 1;
    private bool _isClickButton = false;

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
        _okayButton.OnClickAsObservable().Subscribe(v => _isClickButton = true).AddTo(gameObject);
        _goInventoryButton.OnClickAsObservable().Subscribe(v =>
        {
            PlayerViewModel.Instance.Reset();
            FlowManager.Instance.Change(PopupStyle.Inventory, true, _title);
        }).AddTo(gameObject);
        _skipButton.OnClickAsObservable().Subscribe(v => Skip()).AddTo(gameObject);
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
        // ���۽� ����
        Init();

        yield return new WaitForSeconds(1f);
        Debug.Log("���~ Ģ! ���� ������");
        SetCloseAnim();

        yield return new WaitForSeconds(0.2f);
        Debug.Log("effect ����");
        IEnumerator textRoutine = PrintTextRoutine();
        StartCoroutine(textRoutine);
        yield return StartCoroutine(ActiveSparks(true));

        Debug.Log("ĳ���� �ϼ�ü�� ��ü");
        _parent.localScale = _changeScaleTransform.localScale;
        character = CombineCharacter();
        if (character == null) Skip();

        SetBlackCharacterRender(character);

         yield return new WaitForSeconds(3f);

        yield return StartCoroutine(ActiveSparks(false));

        yield return new WaitForSeconds(0.5f);
        Debug.Log("���� ����Ʈ ����");

        yield return new WaitForSeconds(2f);
        StopCoroutine(textRoutine);
        _combineText.text = string.Empty;
        SetOpenAnim();
        Debug.Log("���� ���� ������");

        yield return new WaitForSeconds(1.5f);
        Debug.Log("���ο� �ǱԾ� �ϼ�~");
        SetWhiteCharacterRender(character);

        yield return new WaitForSeconds(0.3f);
        _dustParticle.SetActive(true);

        yield return new WaitForSeconds(0.7f);
        _combineText.text = "���ο�\n �ǱԾ� \n \"<color=yellow>" + _title + "</color>\"\nȹ��!!";
        _dragAndRotateCharacter.enabled = true;
        _okayButton.gameObject.SetActive(true);
        _goInventoryButton.gameObject.SetActive(true);
        _skipButton.gameObject.SetActive(false);

        while(!_isClickButton)
        {
            // Ȯ�ι�ư Ŭ������ ��ٸ���.
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
        _isClickButton = false;
        _textBoard.gameObject.SetActive(true);
        transform.localPosition = new Vector3(0, 0, -500);
        _combineText.text = string.Empty;
        _textCount = 1;
    }
    private void End()
    {
        ActiveOBJ(true);

        for (int i = 0; i < _partsArchive.Count; i++)
        {
            // ���ҽ� ����
            ResourcesManager.ResetModels(_partsArchive[i].ID);
        }

        _parent.localScale = _originScale;
        _dragAndRotateCharacter.DragResetAndPause();
        _goInventoryButton.gameObject.SetActive(false);
        _okayButton.gameObject.SetActive(false);
        _textBoard.gameObject.SetActive(false);
        _dustParticle.SetActive(false);
        transform.localPosition = Vector3.zero;
        PlayerViewModel.Instance.Reset();
    }
    private void Skip()
    {
        Debug.Log("Skip is called!!!");

        StopAllCoroutines();

        DOTween.KillAll();

        SetSkipAnim();


        ActiveOBJ(true);

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
            character.SetActive(false);
        }

        for (int i = 0; i < _partsArchive.Count; i++)
        {
            ResourcesManager.ResetModels(_partsArchive[i].ID);
        }

        _parent.localScale = _originScale;
        _dragAndRotateCharacter.DragResetAndPause();
        _skipButton.gameObject.SetActive(false);
        _goInventoryButton.gameObject.SetActive(false);
        _okayButton.gameObject.SetActive(false);
        _textBoard.gameObject.SetActive(false);
        _dustParticle.SetActive(false);
        transform.localPosition = Vector3.zero;
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
        while (true)
        {
            PrintFigureText();
            yield return new WaitForSeconds(_textDelay);
        }
    }
    private void PrintFigureText()
    {
        string text = _selectedWord.Substring(0, _textCount);

        _combineText.text = text;

        if (_textCount < _selectedWord.Length) _textCount++;
        else _textCount = 1;
    }
    #endregion

    #region ::::::::::Actives
    private IEnumerator ActiveSparks(bool active)
    {
        for (int i = 0; i < _sparks.Length; i++)
        {
            _sparks[i].gameObject.SetActive(active);
            yield return new WaitForSeconds(0.2f);
        }
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
