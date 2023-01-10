using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class InventorySystem : MonoBehaviour
{
    // InventorySystem : 인벤토리 총 괄 매니저 파트     
    private Button inputField; // 인벤토리 범위 외의 공간 클릭하면 종료되도록.
    private Button btn_Close; // 인벤토리 닫기 버튼
    private Button btn_Open; // 인벤토리 활성화
    
    [SerializeField] private GameObject m_invenObject;
    
    public static InventorySystem instance;
    private void Awake()
    {
        instance = this;
        inputField.onClick.AddListener(btnFunc_Close);
        btn_Close.onClick.AddListener(btnFunc_Close);
        btn_Open.onClick.AddListener(btnFunc_Open);
    }
    
    void Update()
    {
        
    }


    
    private float _duration = 0.3f;
    private void btnFunc_Open()
    {
        m_invenObject.transform.DOScale(1f, _duration).SetEase(Ease.OutBack).From(0f);
    }
    private void btnFunc_Close()
    {
        m_invenObject.transform.DOScale(0f, _duration).SetEase(Ease.InQuad);
    }
}
 