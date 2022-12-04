using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private Transform baseMoveActor;
    private void Awake()
    {
        baseMoveActor = GameObject.Find("BaseMoveActor").GetComponent<Transform>();
        
        
        // TODO : GameManager 에서 정보 가져오기
    }
}
