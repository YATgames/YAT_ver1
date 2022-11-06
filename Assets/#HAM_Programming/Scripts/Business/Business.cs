using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace YAT
{
    public class Business : MonoBehaviour
    {
        private Button btn_Merchant; // 상인
        private Button btn_Blakcmarket; // 암거래상인
        
        
        // 비즈니스 카테고리 관련 인터렉션
        private void Awake()
        {
            btn_Merchant = GameObject.Find("RECT_Merchant").GetComponent<Button>();
            btn_Blakcmarket = GameObject.Find("RECT_BlackMarket").GetComponent<Button>();
            
            btn_Merchant.onClick.AddListener(btnFunc_Merchant);
            btn_Blakcmarket.onClick.AddListener(btnFunc_Blackmarket);
        }


        void btnFunc_Merchant()
        {
            
        }

        void btnFunc_Blackmarket()
        {
            
        }
    }
}