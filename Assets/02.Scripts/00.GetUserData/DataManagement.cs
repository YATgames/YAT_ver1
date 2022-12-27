using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataManagement : MonoBehaviour
{
    // @@@@@가져와야하는 정보@@@@@
    // 스토리관련 : 스테이지 정보, 
    // 인게임관련 : 보유중인 아이템 리스트
    // 캐릭터관련 : 현재 레벨(경험치), 각 캐릭터별로의 친밀도, 우정도, 
    //  파티관련 : 개방한 파티 멤버

    // 스토리
    private int data_curStage; // 깬 스테이지 정보
    
    // 인게임
    // private in
    
    // 캐릭터
    private int data_EXP; // 경험치 
    
    // 파티
    private int[] data_curParty;
}
