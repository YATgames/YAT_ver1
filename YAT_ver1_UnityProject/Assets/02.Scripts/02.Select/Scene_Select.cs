using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
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
        private Text txt_Imtimate;
        
        [Header("Test용도")]
        public GameObject test_Social;
        public GameObject test_Business;
        public GameObject test_Request;

        // 오브젝트 생성될 위치
        private Transform tf_SocialObject;
        private Transform tf_BusinessObject;
        private Transform tf_RequestObject;
        private IEnumerator btnRoutine;
        
        float _duration = 0.6f; // 화면 전환할 때 사용할 전환 대기시간.
        
        /// <summary> Social 객체 할당시키기 </summary>
        [SerializeField]
        private Button btn_StoryOn;
        
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
            
            txt_Imtimate = GameObject.Find("value_Imtinancy").GetComponent<Text>();
            txt_FriendShip = GameObject.Find("value_Friendship").GetComponent<Text>();

            //SocialObject
            btn_StoryOn = test_Social.transform.GetChild(2).GetComponent<Button>();
            btn_StoryOn.onClick.AddListener(btnFunc_StoryOn);
            
            
            // TODO : 적용
            btn_Social.onClick.AddListener(btnFunc_Social);
            btn_Business.onClick.AddListener(btnFunc_Business);
            btn_Request.onClick.AddListener(btnFunc_Request);

            if (CSVDataManager.instance != null)
            {
                txt_Imtimate.text = CSVDataManager.instance.p_Imtimate.ToString();
                txt_FriendShip.text = CSVDataManager.instance.p_friendShip.ToString();
                //txt_FriendShip.text = GameManager.instance.p_friendShip.ToString();    
            }
            //businessObject = null;
            btnRoutine = null;
            init();
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
            if(tf_SocialObject.transform.GetChild(0)!= null)
                tf_SocialObject.transform.GetChild(0).gameObject.SetActive(false);
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
            //img_Black.DOFade(1f, _duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.3f);
            
            // TODO : 화면 열리기 연출
            // 로딩 후에 열리도록
            yield break;
        }
        
        void btnFunc_Social()
        {
            btnRoutine = btnRoutine_HowType("Social");
            StartCoroutine(btnRoutine);
        }
        void btnFunc_Business()
        {
            btnRoutine = btnRoutine_HowType("Request");
            StartCoroutine(btnRoutine);
        }
        void btnFunc_Request()
        {
            btnRoutine = btnRoutine_HowType("Business");
            StartCoroutine(btnRoutine);
        }


        IEnumerator btnRoutine_HowType(string _Type)
        {
            yield return EVENT_Click(); // 화면 전환 애니메이션
            yield return new WaitForSeconds(0.3f);
            switch (_Type)
            {
                case "Business":
                    test_Business.gameObject.SetActive(true);
                    break;
                case "Request":
                    test_Request.gameObject.SetActive(true);
                    break;
                case "Social":
                    test_Social.gameObject.SetActive(true);
                    // 이 과정을 
                    break;
            }
        }


        void btnFunc_StoryOn()
        {
            SceneChanger.instance.changer_Story();
        }
    }