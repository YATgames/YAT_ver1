using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class canvasControl : MonoBehaviour
{
    //  목적 : part_Top, Middle, Bottom 의 비율을 동일하게 유지시키기     
    // 0.3 , 0.5 , 0.2

    [SerializeField] private GameObject RANGE;
    private GameObject part_Top, part_Middle, part_Bottom;
    private float screen_Height;

    private float range_Width = 800f;

    [SerializeField] private Transform ContentRect;

    [SerializeField] private GameObject prefab_MiddleObj;

    private bool isInput;
    private float spawnposY;
    [SerializeField] private int objNumber = 0;

    public static canvasControl instance;

    private void Awake()
    {
        instance = this;
        part_Top = RANGE.transform.GetChild(0).gameObject;
        part_Middle = RANGE.transform.GetChild(1).gameObject;
        part_Bottom = RANGE.transform.GetChild(2).gameObject;
        screen_Height = Screen.height;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isInput = true;
            GameObject obj = Instantiate(prefab_MiddleObj, ContentRect);
            objNumber++;
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, spawnposY);
            spawnposY -= 256f;
            ContentRect.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Mathf.Abs(spawnposY));
            //contentRectSetting();
        }

        // 한 칸의 크기설정해줘야댐
        if (isInput && objNumber >= 3) //Mathf.Abs(spawnposY) >= 128 * 4f)
        {
            ContentRect.GetComponent<RectTransform>().anchoredPosition =
                // lerp = a 를 b 로 c 의 빠르기로  ( a, b, c )
                Vector2.Lerp(ContentRect.GetComponent<RectTransform>().anchoredPosition,
                    new Vector2(0, Mathf.Abs(spawnposY) - (256 * (objNumber - 2))),
                    1f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isInput = false;
        }

        if (ContentRect.GetComponent<RectTransform>().anchoredPosition.y >= Mathf.Abs(spawnposY))
        {
            //isInput = false;
        }
    }


    void contentRectSetting()
    {
        // ScrollView 범위 늘리기
        if (Mathf.Abs(spawnposY) >= 384f)
        {
        }
    }

    public int Get_objNumber()
    {
        return objNumber;
    }

    private WaitForSeconds perSec = new WaitForSeconds(0.03f);

    IEnumerator moveRoutine(float directPos)
    {
        //contentRectSetting();
        yield return perSec;
    }
}