using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace YAT
{
    public class Scene_Main : MonoBehaviour
    {
        /// <summary>  </summary>
        private Button btn_Login;

        private GameObject RECT_Login;
        
        private Image img_Filter; // 검정색 덧씌워지는 이미지
        private Image img_Black; // 씬 변환시 나올 이미지
        
        private float _alphaValue = 0.7f;

        private bool isFilterOn;
        private bool canInput;
        private bool isInput;
        private Vector2 pos_Cur;
        private Vector2 pos_Point;

        private float range; // 얼만큼 이동하면 로그인 화면이 나오게 되는지

        private void Awake()
        {
            btn_Login = GameObject.Find("btn_Login").GetComponent<Button>();
            img_Filter = GameObject.Find("img_Filter").GetComponent<Image>();
            RECT_Login = GameObject.Find("RECT_Login").gameObject;
            img_Black = GameObject.Find("img_Black").GetComponent<Image>();

            range = Screen.height * 0.5f;
            isFilterOn = false;
            canInput = true;
            isInput = false;
            img_Black.color = new Color(0, 0, 0, 0);
            img_Filter.color = new Color(0, 0, 0, _alphaValue);
            img_Filter.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -Screen.height);
            RECT_Login.gameObject.transform.localScale = Vector3.zero;
            
            btn_Login.onClick.AddListener(btnFunc_Login);
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (canInput && !isFilterOn)
                {
                    isInput = true;
                    canInput = false;
                    pos_Cur = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isInput =false;
                
                if (isFilterOn)
                {
                    Filter(true);
                }
                else // 필터 활성화가 아닐 때만
                {
                    canInput = true;
                    img_Filter.GetComponent<RectTransform>().DOAnchorPosY(-Screen.height, 0.2f).SetEase(Ease.Linear);
                }
            }
            
            if (isInput)
            {
                pos_Cur = Input.mousePosition;
                float _value = pos_Point.y - pos_Cur.y;
                float _ypos = -Screen.height- _value;
                
                img_Filter.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,_ypos);

                if (Mathf.Abs(_value) >= range)
                {
                    isFilterOn = true;
                }
                else
                    isFilterOn = false;

                if (Input.mousePosition.y >= Screen.height)
                {
                    isInput = false;
                    canInput = false;
                    isFilterOn = true;
                    Filter(true);
                }
            }
        }

        void Filter(bool _isOn)
        {
            if (_isOn)
            {
                img_Filter.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.15f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        RECT_Login.transform.DOScale(1f, 0.15f).SetEase(Ease.OutBack);
                    });
                
            }
            else
            {
                
            }
        }

        void btnFunc_Login()
        {
            img_Black.DOFade(1f, 0.8f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    SceneChanger.instance.changer_Select();
                });
        }

        IEnumerator loginRoutine()
        {
            yield return new WaitForSeconds(1f);
            

        }
        
    }
}