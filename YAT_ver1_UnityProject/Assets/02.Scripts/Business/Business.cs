using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YAT
{
    public class Business : MonoBehaviour
    {
        private Button btn_Merchant; // 상인
        private Button btn_Blakcmarket; // 암거래상인


        private Image backImage;
        
        // 비즈니스 카테고리 관련 인터렉션
        private void Awake()
        {
            // TODO : 가져오기
            btn_Merchant = GameObject.Find("RECT_Merchant").GetComponent<Button>();
            btn_Blakcmarket = GameObject.Find("RECT_BlackMarket").GetComponent<Button>();
            
            btn_Merchant.onClick.AddListener(btnFunc_Merchant);
            btn_Blakcmarket.onClick.AddListener(btnFunc_Blackmarket);

            backImage = GameObject.Find("UI_Back").GetComponent<Image>();
            
            // TODO : 세팅하기


            backImage.color = new Color(0, 0, 0, 0.5f);
            backImage.gameObject.SetActive(false);
        }



        /// <summary>
        /// 어떤 버튼을 눌렀는지
        /// </summary>
        /// <param name="_type">Merchant : 상인 || BM : 블랙마켓 </param>
        void showPopup(string _type)
        {
            
        }
        void btnFunc_Merchant()
        {
            showPopup("Merchant");
        }

        void btnFunc_Blackmarket()
        {
            showPopup("BM");
        }
    }
}