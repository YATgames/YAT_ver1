using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class StoryManager : MonoBehaviour
{
    // TODO :  CSV데이터 받아와서 할당시키기
    
    private List<Dictionary<string, object>> dia_novel_01;  // = new List<Dictionary<string, object>>();


    // 오브젝트 부모
    [SerializeField] private GameObject characterBase; // 캐릭터
    private GameObject effectBase; // 효과 
    [SerializeField] private GameObject dialogueBase; // 대사창
    
    // 객체
    private Image[] img_Characters;
    private Text text_Name;
    private Text text_Contents;
    
    
    // 상태
    private bool canIput;
    private void Awake()
    {
        // 할당
        characterBase = GameObject.Find("[BASE]Character").gameObject;
        dialogueBase = GameObject.Find("[BASE]Dialogue").gameObject;

        // 설정
        text_Name = dialogueBase.transform.GetChild(0).GetComponent<Text>();
        text_Contents = dialogueBase.transform.GetChild(1).GetComponent<Text>();

        text_Contents.text = null;
        //dia_novel_01 = CSVReader.Read("CSV/Dialogue_00");
        Initialize();
    }

    
    /// <summary>초기화</summary>
    void Initialize()
    {
        img_Characters = new Image[characterBase.transform.childCount];

        for (int i = 0; i < img_Characters.Length; i++)
        {
            img_Characters[i] = characterBase.transform.GetChild(i).GetComponent<Image>();
            img_Characters[i].color = Color.clear;
        }
        characterBase.gameObject.SetActive(true);
        dialogueBase.gameObject.SetActive(true);
    }
    /// <summary>
    /// 대사 재생
    /// </summary>
    void Dialogue()
    {
        if (dia_novel_01[count]["Contents"] != null)
        {
            if (_isPlayingDialogue) // 대사가 진행중인 상태
            {
                StopCoroutine(PlayDialogue);
                text_Contents.text = curDialogue;
                _isPlayingDialogue = false;
                DialogueEnd();
            }
            else // 대사가 진행중이 아닌 상태
            {
                PlayDialogue = dialogueRoutine();
                StartCoroutine(PlayDialogue);
                count++;
            }
        }
        else
        {
            Debug.Log("아무것도 안하기!");
        }
    }

    
    // 캐릭터
    private string curCharacter = null; //  캐릭터명


    private string curDialogue = null; // 대사 내용 저장
    private string string_sightDialogue; // 보여질 대사 (한글자씩 보여짐)
    private int count =0; // curDialogue의 대사중 count 번째의 단어를 대사창에 출력
    private int _maxCount = 10; // 현재 대사 분기의 총 개수
    private bool _isPlayingDialogue;
    
    private IEnumerator PlayDialogue;
    private WaitForSeconds _perTime = new WaitForSeconds(0.02f);
    IEnumerator dialogueRoutine()
    {
        curDialogue = dia_novel_01[count]["Contents"].ToString();
        
        text_Name.text = 
            dia_novel_01[count]["Character"].ToString() == "NONE" ?
                ""  : dia_novel_01[count]["Character"].ToString();
        
        int wordNum = 0;
        _isPlayingDialogue = true;
        // word : 한글자씩 보여지는 부분의 단어, count : 대사 번호, length : 대사의 길이
        while (wordNum <= curDialogue.Length) 
        {
            string_sightDialogue = curDialogue.Substring(0,wordNum);
            text_Contents.text = string_sightDialogue;
            yield return _perTime;
            wordNum++;
        }
        
        //Debug.Log("최종 대사 길이 : " + text_Contents.text.Lengt
        DialogueEnd();
    }

    void DialogueEnd()
    {
        _isPlayingDialogue = false;
        text_Contents.text += "☆";
    }
}