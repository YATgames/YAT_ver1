using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 친밀도, 우정도 기타등등 값 정의해줘야함
    private int Imtinancy; // 친밀도
    private int Friendship; // 우정도
    
    /// <summary> 우정도 가져오기 </summary>
    public int proper_Friend
    {
        get
        {
            return Friendship;
        }
        set
        {
            Friendship += value;
        }
    }
    /// <summary> 친밀도 가져오기 </summary>
    public int proper_Imtinancy
    {
        get
        {
            return Imtinancy;
        }
        set
        {
            Imtinancy += value;
        }
    }

    
    
    
    
    
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Imtinancy += 1;
        }
    }

    public void ChangeValue(int value)
    {
        Imtinancy += value;
    }
}
