using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using Button = UnityEngine.UI.Button;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

namespace YAT
{
    public class Scene_Select : MonoBehaviour
    {
        private RectTransform go_RECT_Button;
        /// <summary> 사교버튼 </summary>
        private Button btn_Social;
        /// <summary> 거래버튼 </summary>
        private Button btn_Business;
        /// <summary> 의뢰버튼 </summary>
        private Button btn_Request;
    
        
        // TextObject
        private Text txt_FriendShip;
        private Text txt_Imtinancy;
        
        
        // Prefab들 코드상으로 받아오게 해야댐
        // objectPrefab
        public GameObject go_BusinessPrefab;
        // businessPrefab  
        public GameObject businessObject;

        [Header("Test용도")] 
        public GameObject test_Business;
        public GameObject test_Request;

        // 오브젝트 생성될 위치
        private Transform tf_SocialObject;
        private Transform tf_BusinessObject;
        private Transform tf_RequestObject;

        private IEnumerator btnRoutine;
        
        // Images
        private Image img_Black;
        
        
        float _duration = 0.6f; // 화면 전환할 때 사용할 전환 대기시간.

        private void Awake()
        {
            // TODO : 찾기

            go_RECT_Button = GameObject.Find("RECT_Button").GetComponent<RectTransform>();
            btn_Social = GameObject.Find("btn_Social").GetComponent<Button>();
            btn_Business = GameObject.Find("btn_Business").GetComponent<Button>();
            btn_Request = GameObject.Find("btn_Request").GetComponent<Button>();

            tf_SocialObject = GameObject.Find("scene_Social").GetComponent<Transform>();
            tf_BusinessObject = GameObject.Find("scene_Business").GetComponent<Transform>();
            tf_RequestObject = GameObject.Find("scene_Request").GetComponent<Transform>();
            txt_Imtinancy = GameObject.Find("value_Imtinancy").GetComponent<Text>();
            txt_FriendShip = GameObject.Find("value_Friendship").GetComponent<Text>();
            img_Black = GameObject.Find("img_Black_SceneConversion").GetComponent<Image>();
            
            
            // TODO : 적용

            btn_Social.onClick.AddListener(btnFunc_Social);
            btn_Business.onClick.AddListener(btnFunc_Business);
            btn_Request.onClick.AddListener(btnFunc_Request);



            txt_Imtinancy.text = GameManager.instance.proper_Imtinancy.ToString();
            txt_FriendShip.text = GameManager.instance.proper_Friend.ToString();
            businessObject = null;
            btnRoutine = null;
            init();
        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Q");
                txt_Imtinancy.text = GameManager.instance.proper_Imtinancy.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                //Friendship -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                //Imtinancy += 1;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
    //            Imtinancy -= 1;
            }
        }


        /// <summary>
        /// tf_~Obejct 들의 자식이 비어있지 않다면 비워줌.
        /// 혹시 테스트하다가 까먹었을 때 대비용
        /// </summary>
        void init()
        {
            if(tf_BusinessObject.transform.GetChild(0)!= null)
                tf_BusinessObject.transform.GetChild(0).gameObject.SetActive(false);
            if(tf_RequestObject.transform.GetChild(0)!= null)
                tf_RequestObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        
        /// <summary>
        /// 버튼 액션이 활성화, 화면 전화 연출
        /// </summary>
        IEnumerator EVENT_Click()
        {
            // TODO : 기존 화면 어두워지기
            float _length = go_RECT_Button.GetComponent<RectTransform>().sizeDelta.x;
            go_RECT_Button.transform.DOMoveX(_length, _duration).SetEase(Ease.InQuad);
            yield return new WaitForSeconds(0.2f);
            
            // TODO : 화면 닫히기 연출 
            img_Black.DOFade(1f, _duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
            
            // TODO : 화면 열리기 연출
            // 로딩 후에 열리도록
            yield break;
        }

        void btnFunc_Social()
        {
            //SceneChanger.instance.changer_Social();
        }


        void btnFunc_Business()
        {
            //btnRoutine = btnRoutine_Business();
            btnRoutine = btnRoutine_HowType("Request");
            StartCoroutine(btnRoutine);
        }
        void btnFunc_Request()
        {
            //EVENT_Click();
            btnRoutine = btnRoutine_HowType("Business");
            StartCoroutine(btnRoutine);
        }

        IEnumerator btnRoutine_HowType(string _Type)
        {
            yield return EVENT_Click(); // 화면 전환 애니메이션
            switch (_Type)
            {
                case "Business":
                    test_Business.gameObject.SetActive(true);
                    img_Black.DOFade(0f, _duration).SetEase(Ease.Linear);
                    break;
                case "Request":
                    test_Request.gameObject.SetActive(true);
                    img_Black.DOFade(0f, _duration).SetEase(Ease.Linear);
                    break;
            }
        }
        IEnumerator btnRoutine_Business()
        {
            //SceneChanger.instance.changer_Business();
            yield return EVENT_Click();
            Debug.Log("화면 전환");
            if (businessObject == null) // 생성이 안되있으므로 생성
            {
                businessObject = Instantiate(go_BusinessPrefab, tf_BusinessObject);
                // pos, rot 값을 안주면 0에 잘 생성되네? 왜인지는 모르겠음 허헣
            }
            else // on off 시켜줘야함 
            {
                businessObject.gameObject.SetActive(true);
            }
            img_Black.DOFade(0f, _duration).SetEase(Ease.Linear);
            yield break;
        }

    }
}