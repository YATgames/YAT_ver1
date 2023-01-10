using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;

public class NovelBase : MonoBehaviour
{
    // 대사 관련기능을 담아둔 스크립트
    private GameObject base_CharacterBase; // [BASE]Character
    private GameObject base_DialogueBase; // [BASE]Dialogue
    
   [SerializeField] protected Image[] m_Character;
   [SerializeField] protected Text m_dialogueChaName, m_diaContents;
    
   // 입력(터치) 관련 정보
   [SerializeField] protected Button m_inputField;
    protected virtual void OnEnable()
    {
        Initialize();
    }
    private void Initialize()
    {
        base_CharacterBase = this.transform.GetChild(1).gameObject;
        base_DialogueBase = this.transform.GetChild(2).gameObject;
        m_inputField = this.transform.GetChild(3).gameObject.GetComponent<Button>();
        
        m_Character = new Image[base_CharacterBase.transform.childCount];
        m_dialogueChaName = base_DialogueBase.transform.GetChild(0).GetComponent<Text>();
        m_diaContents = base_DialogueBase.transform.GetChild(1).GetComponent<Text>();
        
        for (int i = 0; i < m_Character.Length; i++)
        {
            m_Character[i] = base_CharacterBase.transform.GetChild(i).GetComponent<Image>();
            m_Character[i].color = Color.clear;
        }

        //m_inputField.image.color = Color.clear;
        m_inputField.onClick.AddListener(DialoguePlay);
    }
    protected void cha_Show(Image obj, float duration)
    {
        obj.DOFade(1f, duration * 0.5f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                obj.DOColor(Color.white, duration * 0.5f);
            });
    }

    protected void cha_Hide(Image obj, float duration)
    {
        // 곱하기 관련연산을 비트연산으로 하면 빠르겠지만? 0.5 가 보기 편함 ㅅㄱ
        obj.DOColor(Color.black, duration * 0.5f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                obj.DOFade(0f, duration* 0.5f);
            });
    }
    
    protected void cha_Interact(Image obj, float duration) // 위아래로 살짝 움직이는 그거
    {
        obj.GetComponent<RectTransform>() .DOAnchorPosY(45f, duration * 0.5f).SetRelative(true)
            .SetEase(Ease.InQuad).OnComplete(() =>
            {
                 obj.GetComponent<RectTransform>().DOAnchorPosY(-45f, duration * 0.5f).SetRelative(true)
                    .SetEase(Ease.Linear);
            });
    }
    
    protected void cha_Movement(Image obj, float duration, float destination)
    {
        obj.GetComponent<RectTransform>().DOAnchorPosX(destination, duration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            
        });
    }
     
    protected List<Dictionary<string, object>> list_Dialogue;
    
    protected string curDialogue = null; // 대사 내용 저장
    protected string showDialogue; // 보여질 대사 (한글자씩 보여짐)
    protected int count =0; // curDialogue의 대사중 count 번째의 단어를 대사창에 출력
    protected int _maxCount = 10; // 현재 대사 분기의 총 개수
    
    // 대사가 진행중인지 true : 스킵  / false : 다음 대사 진행 
    protected bool _isPlayingDialogue = false;
    protected bool canPlayingAction = true;
    
    // 루틴(각 Novel_num의 액션) 이 끝났는지 여부
    protected bool canInput = true;
    
    
    private IEnumerator PlayDialogue;
    private WaitForSeconds _perTime = new WaitForSeconds(0.02f);
    void dialogueFunc()
    {
        if (list_Dialogue[count]["Contents"] != null)
        {
            if (_isPlayingDialogue) // 대사 진행 중 ( 스킵 )
            {
                StopCoroutine(PlayDialogue);
                m_diaContents.text = curDialogue;
                DialogueEnd();
            }
            else // 다음 대사 진행
            {
                PlayDialogue = dialogueRoutine();
                StartCoroutine(PlayDialogue);
                count++;
            }
        }
        else
        {
            Debug.Log("아무것도 안하기");
        }
    }
    private IEnumerator dialogueRoutine()
    {
        curDialogue = list_Dialogue[count]["Contents"].ToString();

        CharacterNameSetting();
        
        int wordNum = 0;
        _isPlayingDialogue = true;
        // word : 한글자씩 보여지는 부분의 단어, count : 대사 번호, length : 대사의 길이
        while (wordNum <= curDialogue.Length)
        {
            showDialogue = curDialogue.Substring(0, wordNum);
            m_diaContents.text = showDialogue;
            yield return _perTime;
            wordNum++;
        }
        DialogueEnd();
    }
    void DialogueEnd()
    {
        canInput = true;
        _isPlayingDialogue = false;
        m_diaContents.text += "☆";
    }
    private void CharacterNameSetting()
    {
        // 대사 세팅 관련
        m_dialogueChaName.text =
            list_Dialogue[count]["Character"].ToString() == 
            "NONE" ? "" : list_Dialogue[count]["Character"].ToString();
    }
    protected virtual void DialoguePlay()
    {
        dialogueFunc();
    }

    private WaitForSeconds dotTime = new WaitForSeconds(0.3f);
    protected IEnumerator DefaultActionRoutine()
    {
        yield return dotTime;
        //canInput = true;
        canPlayingAction= true;
    }
}
