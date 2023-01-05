using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Novel_01 : NovelBase
{
    // StoryManager 에서 List 타입으로 .csv 의 내용 받아오기
    // [기본구조]
    private List<Dictionary<string, object>> dia_novel_01;  // = new List<Dictionary<string, object>>();
    private int _count =0;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        dia_novel_01 = CSVReader.Read("CSV/Dialogue_00");
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
    IEnumerator novelRoutine_0()
    {
        Debug.Log("루틴0 ");
        yield return new WaitForSeconds(1f);
    }

    IEnumerator novelRoutine_1()
    {
        Debug.Log("루틴1 ");
        yield return new WaitForSeconds(1f);
    }

    private string routine;
    protected override void DialoguePlay()
    {
        base.DialoguePlay();
        
        routine = "novelRoutine_"+ _count.ToString();
        Debug.Log("루틴명 : " + routine);
        
        StartCoroutine(routine);
        _count++;
    }

    
    protected override void defaultSetting()
    {
    }
}
