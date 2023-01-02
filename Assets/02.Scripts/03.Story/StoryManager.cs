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
    private List<Dictionary<string, object>> dia_Story = new List<Dictionary<string, object>>();
    // Dialogue 매니저도 같은 컴포넌트가 가지고 있도록 해야함

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
        dia_Story = CSVReader.Read("CSV/0.dia_Intro");

        
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


        characterBase.gameObject.SetActive(false);
        dialogueBase.gameObject.SetActive(false);
    }

    private void Update()
    {
        Input_Touch();
    }
    
    void Input_Touch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            characterBase.SetActive(true);
            dialogueBase.SetActive(true);
            Debug.Log("최대 카운트 : " + dia_Story.Count);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Dialogue();
        }
        else if (Input.GetKeyDown(KeyCode.E)) // 초기화 테스트
        {
            count = 0;
            text_Contents.text =  "";
        }
        else if(Input.GetKeyDown(KeyCode.Z)) // 등장 테스트
        {
            //cha_Show(0, 0.3f);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            //cha_Movement(0, 0.15f);
        }
    }

    /// <summary>
    /// 대사 재생
    /// </summary>
    void Dialogue()
    {
        
        if (dia_Story[count]["Contents"] == null)
            return;

        if (isPlayingDialogue) // 대사가 진행중인 상태
        {
            StopCoroutine(PlayDialogue);
            text_Contents.text = curDialogue;
            isPlayingDialogue = false;
            DialogueEnd();
        }
        else // 대사가 진행중이 아닌 상태
        {
            PlayDialogue = dialogueRoutine();
            StartCoroutine(PlayDialogue);
            count++;
        }
    }

    
    // 캐릭터
    private string curCharacter = null; //  캐릭터명


    private string curDialogue = null; // 대사 내용 저장
    private string string_sightDialogue; // 보여질 대사 (한글자씩 보여짐)
    private int count =0; // curDialogue의 대사중 count 번째의 단어를 대사창에 출력
    private int maxCount = 10; // 현재 대사 분기의 총 개수
    private bool isPlayingDialogue;
    private IEnumerator PlayDialogue;
    private WaitForSeconds perTime = new WaitForSeconds(0.03f);
    IEnumerator dialogueRoutine()
    {
        curDialogue = dia_Story[count]["Contents"].ToString();

        int _curNum = 0;
        isPlayingDialogue = true;
        // curNum : 한글자씩 보여지는 부분의 
        // count : 대사 번호
        // length : 대사의 길이
         Debug.Log("대사 : " + curDialogue  + " " + curDialogue.Length);
        // curNum을 처음에 더하지 않으니까 < 식으로 해야 끝까지 대사를 한다.
        while (_curNum <= curDialogue.Length) 
        {
            string_sightDialogue = curDialogue.Substring(0,_curNum);
            text_Contents.text = string_sightDialogue;
            yield return perTime;
            _curNum++;
        }
        Debug.Log("최종 대사 길이 : " + text_Contents.text.Length);
        // 대사 재생 끝난 부분
        DialogueEnd();
    }

    void DialogueEnd()
    {
        isPlayingDialogue = false;
        text_Contents.text += "☆";
    }
}