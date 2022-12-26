using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.PlasticSCM.Editor.WebApi;

public class ScheduleManager : MonoBehaviour
{
    private Canvas m_canvas;
    private GraphicRaycaster m_gr;
    private PointerEventData m_ped;
    private Camera camera;
    private RectTransform rt_Back;
    private RectTransform rt_Front;


    enum maxState
    {
        NONE,LEFT,RIGHT,TOP,BOTTOM
    };

    private float val_Vertical = 150f; // 상/하 최대범위
    private float val_Horizontal = 300; // 좌/우 최대범위
    
    private void Awake()
    {
        camera = Camera.main;
        rt_Back = this.transform.GetChild(1).GetComponent<RectTransform>();
        rt_Front = this.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        Init();

        m_canvas = this.GetComponent<Canvas>();
        m_gr = this.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);
    }

    void Init()
    {
        isMove = false;
    }
    void Update()
    {
        mouseInput();
        
        moveWorld();
        //Debug.Log("리스트 개수 :" + results.Count);
    }

    private bool isMove;
    private Rect nyaong = new Rect(new Vector2(1,0), Vector3.one);
    void moveWorld()
    {
        if (isMove)
        {
            // 마우스 포인트 - 이동시킨 마우스 위치 = 가상의 좌표 x,y
            // x,y 만큼 rt_Back 에서  차감시키기
            // ex ) 3,3지점을 클릭했다면, 3,3 - 3,3 으로 0에서 시작되도록 빼는게 맞다.
            rt_Back.anchoredPosition = -(mousePoint - (Vector2)Input.mousePosition);
            
        }
        // TODO : 마우스 위치에 따른 rt_Back 이동
    }

    private Ray ray;
    private RaycastHit hit;
    private Vector2 mousePoint;
    private Vector2 lastSavePos;
    [SerializeField] List<RaycastResult> results = new List<RaycastResult>();
    void mouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (results.Count != 0) return;
            
            m_ped.position = Input.mousePosition;
            m_gr.Raycast(m_ped,results);
            
            if (results.Count > 0)
            {
                if (results[0].gameObject.name.Contains("moveObject")
                    || results[0].gameObject.name.Contains("building"))
                {
                    Debug.Log(results[0].gameObject.name + " 터치함");
                    isMove = true;
                    mousePoint = Input.mousePosition;
                }
            }
            else return;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isMove = false;
            results.Clear();
            
            lastSavePos = rt_Front.transform.localPosition;
            //StartCoroutine(moveUp());

            rt_Front.transform.localPosition += rt_Back.transform.localPosition;
            rt_Back.transform.localPosition = Vector3.zero;
            
            GetsavePos();
        }

    }

    void GetsavePos() // 현재 이동제한 범위를 벗어나서 마지막 저장 위치로 이동시킴.
    {
        if (Mathf.Abs(rt_Front.anchoredPosition.x) >= Mathf.Abs(val_Horizontal)) // 쨋든 최대값으로 컸음.
        {
            
        }
        else // 마우스 땐 위치에서 끝
        {
            
        }
        Debug.Log("이동 제한에 걸려서 이동시켰음");
        
        rt_Front.anchoredPosition =
            new Vector2
            (Mathf.Clamp(rt_Front.anchoredPosition.x, -val_Horizontal, val_Horizontal),
                Mathf.Clamp(rt_Front.anchoredPosition.y, -val_Vertical, val_Vertical));
    }
    
    private float returnValue;
    IEnumerator lerpmove(bool isMin,float curValue, float destValue)
    {
        float offset = 0.5f; // 자동보간이동속도
        if (isMin) // 최소값인 경우 curValue가 destValue 보다 작은 상황애서 시작
        {
            while (curValue <= destValue)
            {
                curValue+= offset * Time.deltaTime;
                yield return null;
            }
        }
        else // 최대값인 경우 curValue 가 destValue 보다 큰 상황에서 시작
        {
            while (curValue >= destValue)
            {
                curValue--;
            }    
        }
        
    }
    
    
    IEnumerator moveUp()
    {
        //rt_Back.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.InQuad);
        // 무조건 0으로가 아니라 최대한의 지점 설정해두고, 그 이상으로 더 가려하면 막는거로.
        yield return new WaitForSeconds(1f);
    }
}
