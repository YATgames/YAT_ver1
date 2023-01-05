using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCharacter : MonoBehaviour
{
    private enum E_Character
    {
        ARU, MIKA,
        EXTRA // 잡캐릭터?
    }
    private E_Character _Echaracter;
    private void Awake()
    {
        // 오브젝트 이름의 ""CHA_" 부분을 없애서 이름만 남김
        string strName = this.gameObject.name.Replace("CHA_", "");
        
        _Echaracter = (E_Character)(int)Enum.Parse(typeof(E_Character), strName);
         Debug.Log("활성화 된 캐릭터 이름 : " + _Echaracter);
        //Debug.Log("enum 카운트 : " +Enum.GetValues(typeof(E_Character)).Length);
    }
}
