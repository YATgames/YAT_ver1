using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Collections.LowLevel.Unsafe;
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
        
        characterBase.gameObject.SetActive(false);
        dialogueBase.gameObject.SetActive(false);
    }

    
    /// <summary>초기화</summary>
    void Initialize()
    {
        for (int i = 0; i < characterBase.transform.childCount; i++)
        {
            
        }
    }

    private void Update()
    {
        InputTest();
    }


    void InputTest()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            characterBase.SetActive(true);
            dialogueBase.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(dialogueRoutine());
            //dialogueRoutine_Func();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            count = 0;
            text_Contents.text =  "";
        }
    }


    void dialogueRoutine_Func()
    {
        
        curDialogue = dia_Story[count]["Contents"].ToString();
        //text_Contents.text = curDialogue;
        text_Contents.DOText(curDialogue, 0.5f, false, 
            ScrambleMode.None, null);
    }
    private string curDialogue = null; // 현재 대사로 출력될 내용 저장
    private string string_sightDialogue;
    private string curCharacter = null;
    private WaitForSeconds perTime = new WaitForSeconds(0.1f);
    private int count =0; // curDialogue의 대사중 count 번째의 단어를 대사창에 출력 
    IEnumerator dialogueRoutine()
    {
        curDialogue = dia_Story[count]["Contents"].ToString();
        int _curNum = 0; 
        int _length = curDialogue.Length;
        while (_curNum <= count)
        {
            text_Contents.text = string_sightDialogue;
            string_sightDialogue = curDialogue.Substring(count);
            yield return perTime;
            count++;
        }
    }
}