using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Novel_02 : NovelBase
{
    // [기본구조]
/*    
0	ARU	김민머리 맨들맨들 반짝반짝
1	ARU	김산대머리 맨들맨들
2	ARU	얏츄대머리 맨들맨들
3	ARU	손한별 대머리
4	ARU	탈목승훈 탈모현재진행형
5	ARU	김준기러끼룩끼룩끼
6	MIKA	얏츄베르류류르릅 대머링
7	MIKA	손한별은 역시 다 빠졌어 역시 그래 와 그래
8	NONE	ㄹㅇ 손한별 탈모련
9	NONE	김준기가 부릅니다 내 머리 어디갔어
*/
    private Image _ARU;
    private Image _MIKA;
    protected override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("novel2 활성화");
        list_Dialogue = CSVReader.Read("CSV/Dialogue_02");
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
        cha_Show(_MIKA,1f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_01()
    {
        cha_Show(_ARU,1f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_02()
    {
        cha_Interact(_MIKA,0.4f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_03()
    {
        //cha_Show(_MIKA,0.6f);
        cha_Interact(_ARU, 0.4f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_04()
    {
        cha_Hide(_MIKA,0.6f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_05()
    {
        cha_Hide(_ARU,0.6f);
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_06()
    {
        
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_08()
    {
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_09()
    {
        yield return defaultActoinRoutine();
    }

    IEnumerator novelRoutine_10()
    {
        yield return defaultActoinRoutine();
    }

    private string routine;

    protected override void DialoguePlay()
    {
        base.DialoguePlay(); // 대사 진행 부분
        
        if (_isPlayingAction) // 스킵되는 상황에서는 실행되면 안됨
        {
            playAction();
        }
    }

    
    private int _actionCount =0;
    void playAction()
    {
        string formatString = string.Format("novelRoutine_{0:D2}", (int)_actionCount);
        StartCoroutine(formatString);
        _actionCount++;
    }
}