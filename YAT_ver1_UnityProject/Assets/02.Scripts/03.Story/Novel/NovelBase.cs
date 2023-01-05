using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NovelBase : MonoBehaviour
{
    // 대사 관련기능을 담아둔 스크립트
    private GameObject base_CharacterBase; // [BASE]Character
    private GameObject base_DialogueBase; // [BASE]Dialogue
    
    protected Image[] m_Character;
    protected Text[] m_Dialogue;


    private Text m_dialogueChaName;
    private Text m_diaContents;

    protected virtual void OnEnable()
    {
        Initialize();
    }
    private void Initialize()
    {
        base_CharacterBase = this.transform.GetChild(1).gameObject;
        base_DialogueBase = this.transform.GetChild(2).gameObject;

        m_Character = new Image[base_CharacterBase.transform.childCount];
        m_Dialogue = new Text[base_DialogueBase.transform.childCount];
        
        for (int i = 0; i < m_Character.Length; i++)
        {
            m_Character[i].color = Color.clear;
        }

        for (int i = 0; i < m_Dialogue.Length; i++)
        {
            m_Dialogue[i].text = "";
        }
    }
    protected void cha_Show(Image obj, float duration)
    {
        obj.DOFade(1f, duration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                obj.DOColor(Color.white, duration);
            });
    }
    protected void cha_Interact(Image obj, float duration) // 위아래로 살짝 움직이는 그거
    {
        //obj = img_Characters[num].GetComponent<RectTransform>();
        obj.GetComponent<RectTransform>() .DOAnchorPosY(45f, duration * 0.5f).SetRelative(true)
            .SetEase(Ease.InQuad).OnComplete(() =>
            {
                 obj.GetComponent<RectTransform>().DOAnchorPosY(-45f, duration * 0.5f).SetRelative(true)
                    .SetEase(Ease.Linear);
            });
    }
    protected void cha_Movement(Image obj, float duration, float destination)
    {
        //RectTransform obj = img_Characters[num].GetComponent<RectTransform>();
        obj.GetComponent<RectTransform>().DOAnchorPosX(destination, duration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            
        });
    }
    protected List<Dictionary<string, object>> list_Dialouge;
    
    protected string curDialogue = null; // 대사 내용 저장
    protected string showDialogue; // 보여질 대사 (한글자씩 보여짐)
    protected int count =0; // curDialogue의 대사중 count 번째의 단어를 대사창에 출력
    protected int _maxCount = 10; // 현재 대사 분기의 총 개수
    
    // 대사가 진행중인지 true : 스킵  / false : 다음 대사 진행 
    private bool _isPlayingDialogue; 
    private IEnumerator PlayDialogue;
    private WaitForSeconds _perTime = new WaitForSeconds(0.02f);

    void DialogueRoutine()
    {
        if (list_Dialouge[count]["Contents"] != null)
        {
            if (_isPlayingDialogue) // 대사 진행 중 ( 스킵 )
            {
                StopCoroutine(PlayDialogue);
                
            }
            else // 다음 대사 진행
            {
                PlayDialogue = dialogueRoutine();
                StartCoroutine(PlayDialogue);
                count++;
            }
        }
        
    }
    private IEnumerator dialogueRoutine()
    {
        curDialogue = list_Dialouge[count]["Contents"].ToString();

        m_dialogueChaName.text =
            list_Dialouge[count]["Character"].ToString() == 
            "NONE" ? "" : list_Dialouge[count]["Character"].ToString();
        
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
        _isPlayingDialogue = false;
        m_diaContents.text += "☆";
    }

    protected virtual void DialoguePlay()
    {
        Debug.Log("DialoguePlay");
    }

    protected virtual void defaultSetting() {}
}
