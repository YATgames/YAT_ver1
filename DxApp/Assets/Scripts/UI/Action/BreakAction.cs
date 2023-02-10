using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.PrefabModel;
using DG.Tweening;
using DXApp_AppData.Item;
using DXApp_AppData.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BreakAction : MonoBehaviour
{
    [SerializeField] private DragAndRotateCharacter _dragAndRotateCharacter;

    [SerializeField] private Button _okayButton;
    [SerializeField] private Button _skipButton;

    [SerializeField] private Text _breakText;
    
    [SerializeField] private GameObject _textBoard;

    [SerializeField] private GameObject[] _notActionObjs;
    [SerializeField] private GameObject[] _clouds;

    [SerializeField] private Transform[] _parent;

    [SerializeField] private Toggle _originToggle;

    [SerializeField] private float _textDelay = 0.05f;

    private string[] _combineTextWords = new string[5]
    {
        "오늘의 마술은 분해 마술!\n자, 제 몸이 분해… 어라?",
        "나… 다시 합체될 수 있는거지?",
        "더 강한 고스트로 태어나기 위한 준비중…",
        "머리 연결 해제중...\n몸 연결 해제중...\n악세서리 해제중…",
        "이 파츠만 있으면 완벽할꺼야! 기대되는데?",
    };
    private int _textCount = 1;
    private string _selectedWord = null;

    private bool _isClickButton = false;

    private Vector3[] _originpos;

    private List<GameObject> character;

    private PlayfabItemInstance _figureArchive;
    private PlayerModel _player;
    private InventoryModel _inventory;
    public void BreakeAction(string title)
    {
        _figureArchive = PlayerViewModel.Instance.FigureArchive;
        _player = PlayerViewModel.Instance.Player;
        _inventory = PlayerViewModel.Instance.Inventory;
        _selectedWord = _combineTextWords[Random.Range(0, _combineTextWords.Length)];
        StartCoroutine(BreakRoutine(title));
    }

    private IEnumerator BreakRoutine(string title)
    {
        Init();

        character = BreaKCharacter();
        _originpos = new Vector3[character.Count];

        yield return new WaitForSeconds(1f);
        Debug.Log("위로 붕 뜬다");
        MoveUpCharacter(character);

        yield return new WaitForSeconds(1f);
        IEnumerator textRoutine = PrintTextRoutine();
        StartCoroutine(textRoutine);

        yield return new WaitForSeconds(1f);
        Debug.Log("우우웅~ 흔들린다");
        ShakingCharacter(character);

        yield return new WaitForSeconds(1f);
        Debug.Log("파츠들이 각자 위치로 이동한다");
        MoveCharacterParts(character);

        yield return new WaitForSeconds(3f);
        StopCoroutine(textRoutine);
        _breakText.text = string.Empty;

        yield return new WaitForSeconds(1.5f);
        _breakText.text = "\"<color=yellow>" + title + "</color>\"\n분해 성공!!";
        _okayButton.gameObject.SetActive(true);
        _skipButton.gameObject.SetActive(false);

        while (!_isClickButton)
        {
            // 확인버튼 클릭까지 기다린다.
            yield return null;
        }

        Debug.Log("구름 이펙트가 나오면서 파츠가 사라진다.");
        _okayButton.gameObject.SetActive(false);
        yield return StartCoroutine(DisappearCharacter(character));
        yield return new WaitForSeconds(2f);

        End();
    }

    #region :::::::::::Settings
    private void Start()
    {
        AddEvent();
    }

    private void AddEvent()
    {
        _skipButton.OnClickAsObservable().Subscribe(v => Skip()).AddTo(gameObject);
        _okayButton.OnClickAsObservable().Subscribe(v => _isClickButton = true).AddTo(gameObject);
    }
    private void Init()
    {
        _dragAndRotateCharacter.DragResetAndPause();
        _skipButton.gameObject.SetActive(true);
        ActiveOBJ(false);
        _textBoard.gameObject.SetActive(true);
        _isClickButton = false;
        _breakText.text = string.Empty;
        _textCount = 1;
    }
    private void End()
    {
        StopAllCoroutines();
        _dragAndRotateCharacter.enabled = true;
        PlayerViewModel.Instance.Reset();
        ActiveOBJ(true);
        FindFavoriteFigure();
        _breakText.text = string.Empty;
        for (int i = 0; i < _clouds.Length; i++)
        {
            _clouds[i].SetActive(false);
        }
        _textBoard.gameObject.SetActive(false);
        PlayerViewModel.Instance.UpdateItem.Invoke();
    }

    private void Skip()
    {
        StopAllCoroutines();
        DOTween.KillAll();

        for (int i = 0; i < character.Count; i++)
        {
            character[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _clouds.Length; i++)
        {
            _clouds[i].SetActive(false);
        }

        var customFigure = _figureArchive as CustomFigureInstance;
        var items = customFigure.CustomData.Parts;

        for (int i = 0; i < items.Count; i++)
        {
            ResourcesManager.ResetModels(items[i]);
        }
        _dragAndRotateCharacter.enabled = true;
        _okayButton.gameObject.SetActive(false);
        _skipButton.gameObject.SetActive(false);
        _textBoard.gameObject.SetActive(false);
        PlayerViewModel.Instance.Reset();
        ActiveOBJ(true);
        FindFavoriteFigure();
        _breakText.text = string.Empty;
        
        PlayerViewModel.Instance.UpdateItem.Invoke();
    }
    #endregion

    #region :::::::::::CharacterActions
    private void MoveUpCharacter(List<GameObject> character)
    {
        for (int i = 0; i < character.Count; i++)
        {
            character[i].transform.DOLocalMoveY(character[i].transform.localPosition.y + 0.2f, 1f).SetEase(Ease.InOutSine);
        }
    }

    private void SetBlackCharacterRender(List<GameObject> character)
    {
        for (int i = 0; i < character.Count; i++)
        {
            var renders = character[i].GetComponentsInChildren<Renderer>();
            for (int j = 0; j < renders.Length; j++)
            {
                renders[j].material.DOColor(Color.black, 1f);
            }
        }
    }

    private void ShakingCharacter(List<GameObject> character)
    {
        for (int i = 0; i < character.Count; i++)
        {
            _originpos[i] = character[i].transform.localPosition;
            character[i].transform.DOShakePosition(1f, 0.01f, 40, 0.01f, false, true);
        }
    }

    private void MoveCharacterParts(List<GameObject> character)
    {
        for (int i = 0; i < character.Count; i++)
        {
            character[i].transform.localPosition = _originpos[i];
            character[i].transform.DOLocalMove(Vector3.zero, 3f);
        }
    }

    private void SetWhitePartsRender(List<GameObject> character)
    {
        for (int i = 0; i < character.Count; i++)
        {
            _originpos[i] = character[i].transform.localPosition;
            var renders = character[i].GetComponentsInChildren<Renderer>();
            for (int j = 0; j < renders.Length; j++)
            {
                renders[j].material.DOColor(Color.white, 1f);
            }
        }
    }

    private IEnumerator DisappearCharacter(List<GameObject> character)
    {
        for (int i = 0; i < character.Count; i++)
        {
            _clouds[i].SetActive(true);
            yield return new WaitForSeconds(0.3f);
            character[i].transform.DOScale(Vector3.zero, 0.1f);
        }
    }
    #endregion

    #region :::::::::::Texts
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

        _breakText.text = text;

        if (_textCount < _selectedWord.Length) _textCount++;
        else _textCount = 1;
    }
    #endregion

    #region :::::::::::Actives
    private void ActiveOBJ(bool active)
    {
        for (int i = 0; i < _notActionObjs.Length; i++)
        {
            _notActionObjs[i].gameObject.SetActive(active);
        }
    }
    #endregion

    private void FindFavoriteFigure()
    {
        PlayerViewModel.Instance.OnFigureArchive.Invoke(false);
        bool findFavoriteFigure = false;
        for (int i = 0; i < _inventory.OriginFigures.Count; i++)
        {
            if (_player.FavoriteInstanceID == _inventory.OriginFigures[i].InstanceID)
            {
                PlayerViewModel.Instance.FigureArchive = _inventory.OriginFigures[i];
                _originToggle.isOn = true;
                findFavoriteFigure = true;
                break;
            }
        }

        if (!findFavoriteFigure)
        {
            for (int i = 0; i < _inventory.CustomFigures.Count; i++)
            {
                if (_player.FavoriteInstanceID == _inventory.CustomFigures[i].InstanceID)
                {
                    PlayerViewModel.Instance.FigureArchive = _inventory.CustomFigures[i];
                    break;
                }
            }
        }
    }

    private List<GameObject> BreaKCharacter()
    {
        var customFigure = _figureArchive as CustomFigureInstance;
        var items = customFigure.CustomData.Parts;

        GameObject body = null;
        GameObject head = null;
        GameObject deco = null;

        for (int i = 0; i < items.Count; i++)
        {
            switch (items[i].Substring(0, 1))
            {
                case "1": head = ResourcesManager.GetModel(items[i]); break;
                case "2": body = ResourcesManager.GetModel(items[i]); break;

                case "3":
                case "4":
                    deco = ResourcesManager.GetModel(items[i]);
                    break;
            }
        }

        Model_Body _body = body?.GetComponent<Model_Body>();
        Model_Head _head = head?.GetComponent<Model_Head>();
        Model_Deco _deco = deco?.GetComponent<Model_Deco>();

        List<GameObject> character = new List<GameObject>();

        _head?.transform.SetParent(_parent[0]);
        _body?.transform.SetParent(_parent[1]);
        _deco?.transform.SetParent(_parent[2]);

        try
        {
            character.Add(_body.gameObject);
            character.Add(_head.gameObject);
            character.Add(_deco.gameObject);
        }
        catch
        {

        }

        return character;
    }

}
