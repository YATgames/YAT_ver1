using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Novel_01 : NovelBase
{
    // StoryManager 에서 List 타입으로 .csv 의 내용 받아오기
    // [기본구조]



    private Image _ARU;
    private Image _MIKA;
    protected override void OnEnable()
    {
        base.OnEnable();
        list_Dialogue = CSVReader.Read("CSV/Dialogue_00");
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return new WaitUntil(() => m_Character[0] != null); 
        _MIKA = m_Character[0];
        _ARU = m_Character[1];
    }
    
    // novel_01_Intro 에서 각 터치 분기에서 어떤 행동을 해야하는지 정의가 되어야함.
    // NovelBase에서 각 액션을 정의해두고, 각 novel_number 에서는 플로우만 설정함.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            DialoguePlay();
        }
    }

    IEnumerator novelRoutine;

    IEnumerator novelRoutine_00()
    {
        cha_Show(_MIKA,0.6f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_01()
    {
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_02()
    {
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_03()
    {
        cha_Show(_MIKA,0.6f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_04()
    {
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_05()
    {
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_06()
    {
        cha_Hide(_MIKA, 0.6f);
        yield return new WaitForSeconds(0.6f);
        cha_Show(_ARU,1f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_08()
    {
        cha_Hide(_ARU,0.6f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_09()
    {
        yield return defaultActoinRoutine();
    }

    private string routine;

    protected override void DialoguePlay()
    {
        base.DialoguePlay(); // 대사 진행 부분
        playAction();
        //if (_isPlayingAction) // 스킵되는 상황에서는 실행되면 안됨
        {

        }
    }

    
    private int _actionCount =0;
    void playAction()
    {
        Debug.Log("액션 카운트 :" + _actionCount);
        string fotmatNum = string.Format($"{0:D2}", _actionCount);
        routine = "novelRoutine_" + fotmatNum;
        Debug.Log("루틴명 : " + routine);
        _actionCount++;
        StartCoroutine(routine);
    }
}