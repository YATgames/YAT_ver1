using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }
    }

    private void Awake()
    {
        #if UNITY_EDITOR
        {
            Debug.Log("<color=yellow> UNITY_EDITOR </color>");
            
        }
        #else
        {
            Debug.Log("<color=yellow> UNITY_ANDROID </color>");
        }
        #endif
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        Debug.Log("<color=green> Q : gold 증가, S : 데이터 저장");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DataManager.instance.p_gold = 1;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            DataManager.instance.Set_partyMember(1,1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            DataManager.instance.SaveJson();
        }
    }
}
