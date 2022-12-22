using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class StoryManager : MonoBehaviour
{
    // Dialogue 매니저도 같은 컴포넌트가 가지고 있도록 해야함
   
    // 오브젝트 부모
    [SerializeField] private GameObject characterBase; // 캐릭터
    private GameObject effectBase; // 효과 
    [SerializeField] private GameObject dialogueBase; // 대사창
    
    // 객체
    private Image[] img_Characters;


    private void Awake()
    {
        // 할당
        characterBase = GameObject.Find("[BASE]Character").gameObject;
        dialogueBase = GameObject.Find("[BASE]Dialogue").gameObject;

        // 설정
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

    private KeyCode keyCode;
    private Input _Input;
    private void Update()
    {
        switch (keyCode)
        {
            case KeyCode.Q: //KeyCode.Q:
                Debug.Log("활성화");
                break;
            case KeyCode.Keypad2:
                Debug.Log("대사 진행");
                break;
        }
        InputTest();
    }
    void InputTest()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            characterBase.SetActive(true);
            dialogueBase.SetActive(true);
        }
    }
}
