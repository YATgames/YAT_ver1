using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace YAT
{
    public class SceneChanger : MonoBehaviour
    {
        private static SceneChanger _instance;
        public static SceneChanger instance
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


        private UnityEngine.UI.Button btn_Back;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
                
                // TODO : Resources 폴더에서 각 활동 오브젝트 할당 해줘야함
            }
        }

        void ScreenChange()
        {
            
            
            btn_Back = GameObject.Find("btn_Back").GetComponent<Button>();
        }


        /// <summary>
        /// 셀렉트 씬으로 가는 함수
        /// </summary>
        public void changer_Select()
        {
            SceneManager.LoadScene("Test_Select", LoadSceneMode.Single);

        }
        /// <summary>
        /// SocialButton Event Function
        /// </summary>
        public void changer_Social()
        {
            //SceneManager.LoadScene("TEST_Social", LoadSceneMode.Additive);
            // Additive로 해야 기존 씬 다시 로드 안하고 다시 할 수 있음
        }

        /// <summary>
        /// Business Event Function
        /// </summary>
        public void changer_Business()
        {
            //SceneManager.LoadScene("TEST_Business", LoadSceneMode.Additive);
            // 씬 로드 방식이 아닌 프리팹 소환식으로 변경되야할듯
        }
        
        
    }
}