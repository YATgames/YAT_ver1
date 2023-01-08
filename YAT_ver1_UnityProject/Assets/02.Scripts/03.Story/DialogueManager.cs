using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // TODO : Novel_{num} 타입의 프리팹을 불러와서 대사 상황을 진행시킴
    public Novel_01 novel_01;
    public Novel_02 novel_02;
    
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreatePrefab(int num)
    {
        novel_01.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}