using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScheduleManager : MonoBehaviour
{
    private Canvas m_canvas;
    private GraphicRaycaster m_gr;
    private PointerEventData m_ped;
    private Camera camera;
    private RectTransform rt_Back;
    private RectTransform rt_Front;


    private bool canInput;
    private float val_Vertical = 150f; // 상/하 최대범위
    private float val_Horizontal = 300; // 좌/우 최대범위

    public GameObject[] go_interactObject; // 상호작용 건물
    
    
    enum userState
    {
        
    }
    
    private float bvalue = 0.8f;
    private void Awake()
    {
        camera = Camera.main;
        rt_Back = this.transform.GetChild(1).GetComponent<RectTransform>();
        rt_Front = this.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        Init();

        m_canvas = this.GetComponent<Canvas>();
        m_gr = this.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);

        for (int i = 0; i < go_interactObject.Length; i++)
        {
            go_interactObject[i].GetComponent<Image>().color = new Color(bvalue, bvalue, bvalue, 1f);
            go_interactObject[i].GetComponent<Button>().onClick.AddListener(btnFunc_Test);
        }
    }

    void Init()
    {
        isMove = false;
        canInput = true;
    }
    void Update()
    {
        mouseInput(); // canInput = true 조건걸려있음
        moveWorld();
    }
    public enum  State
    {
        NONE,
        APPEAR,
        BATTLE
    }

    private State state;
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
    List<RaycastResult> results = new List<RaycastResult>();
    void mouseInput()
    {
        if (Input.GetMouseButtonDown(0) && canInput)
        {
            if (results.Count != 0) return;
            
            if (results.Count != 0)
            {
                return;
            }
            
            m_ped.position = Input.mousePosition;
            m_gr.Raycast(m_ped,results);
            
            if (results.Count > 0)
            {
                if (results[0].gameObject.name.Contains("moveObject")
                    || results[0].gameObject.name.Contains("building"))
                {
                    //Debug.Log(results[0].gameObject.name + " 터치함");
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
            
            GetsavePos();
        }

    }


    private float returnDuration = 0.3f;
    /// <summary>
    /// 현재 이동제한 범위를 벗어나서 마지막 저장 위치로 이동시킴.
    /// </summary>
    void GetsavePos() 
    {
        rt_Front.transform.localPosition += rt_Back.transform.localPosition;
        rt_Back.transform.localPosition = Vector3.zero;
        
        // TODO : 좌/우 이동범위 이상으로 이동
        if (Mathf.Abs(rt_Front.anchoredPosition.x) >= Mathf.Abs(val_Horizontal))
        {
            canInput = false;
            if (rt_Front.anchoredPosition.x >= val_Horizontal) // 최대값
            {
                rt_Front.DOAnchorPosX(val_Horizontal, returnDuration, false)
                    .OnComplete(() => canInput = true);
            }
            else // 최소값
            {
                rt_Front.DOAnchorPosX(-val_Horizontal, returnDuration, false)
                    .OnComplete(() => canInput = true);
            }
        }

        // TODO : 상/하 이동범위 이상으로 이동
        if (Mathf.Abs(rt_Front.anchoredPosition.y) >= Mathf.Abs(val_Horizontal))
        {
            canInput = false;
            if (rt_Front.anchoredPosition.y >= val_Vertical) // 최대값
            {
                rt_Front.DOAnchorPosY(val_Vertical, returnDuration, false)
                    .OnComplete(() => canInput = true);
            }
            else // 최소값
            {
                rt_Front.DOAnchorPosY(-val_Vertical, returnDuration, false)
                    .OnComplete(() => canInput = true);
            }
        }
        
        /*
        rt_Front.anchoredPosition =
            new Vector2
            (Mathf.Clamp(rt_Front.anchoredPosition.x, -val_Horizontal, val_Horizontal),
                Mathf.Clamp(rt_Front.anchoredPosition.y, -val_Vertical, val_Vertical));
                */
    }
    void btnFunc_Test()
    {
        Debug.Log("버튼 누름");
    }
}
