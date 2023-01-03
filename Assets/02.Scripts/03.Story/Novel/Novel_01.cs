using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Novel_01 : NovelBase
{
    
    private int dialogueCount;
    public void NovelSetting()
    {
        
    }
    // novel_01_Intro 에서 각 터치 분기에서 어떤 행동을 해야하는지 정의가 되어야함.
    // NovelBase에서 각 액션을 정의해두고, 각 novel_number 에서는 플로우만 설정함.
    
    
    // 0	나는 눈을 떳다 여기는 어딜까 나는 모루겠다
    // 0	어디선가 본 적이 있는거 같기도 하고 아닌거 같기도 하고
    // 0	그래도 뭐라도 해야겠지 가만히 있을수는 업서
    // 1	안녕 나는 미카야 너는 샬레의 선생이구나
    // 1	이곳은 트리니티 종합학원의 다과회실이야
    // 1	아루도 와있네 인사해 아루
    // 2	안녕 나는 아루야 내이름은 아루 히히
    // 2	내 소개는 여기까지야 너의 소개를 해 이제
    // 0	그래 나는 샬레의 선생이자 이 대사 시스템을 테스트하기 위해 대사를 길게 썻다
    // 0	첫번째 스토리 대사가 끝났다 캐릭터들 사라지는 효과 주고 화면 전환 효과까지


    IEnumerator novelroutine;
    public IEnumerator novelRoutine_01()
    {
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator novelRoutine_02()
    {
        yield return new WaitForSeconds(1f);
    }
}
