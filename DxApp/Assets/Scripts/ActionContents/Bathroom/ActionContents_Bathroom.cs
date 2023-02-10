using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;

using DG.Tweening;
using UniRx;
using Assets.Scripts.Common;
using Newtonsoft.Json.Bson;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Bathroom : MonoBehaviour
    {
        // 의존성으로 인해서 Model 에서 받아올 것들
        public FlowManager FlowManager { get; set; }
        public OnEventTrigger Hide { get; set; }


        private GameObject[] _contentObjects;

        private Color _alphaNone = new Color(1, 1, 1, 0);
        // [컨텐츠 관련 오브젝트
        
        private GameObject _modelObject;
        private Animator _animator;
        private float _zValue = -200f; // 이미지가 피규어 위에 보이기 위한 z값 설정

        private bool _isBackGhostShow = false;
        private bool _isColorChanged;

        private int _farCreateCount = 25;
        private float _gasCreateRange;

        private Image _redTissue;
        private Image _blueTissue;
        
        private string[] _dialogue = { "빨간 휴지줄까...", "파란 휴지줄까...", "오~ 시원하다" };

        private float _xValue = 140f;
        private Vector2 _leftZeroPos = new Vector2(-300, -300);
        private Vector2 _rightZeroPos = new Vector2(300, -300);

        private Vector2 _leftMovePos = new Vector2(-250, -180);
        private Vector2 _rightMovePos = new Vector2(250, -180);


        private GameObject _tissueObject= null;
       
        [SerializeField] private Sprite _bathroomInsideImage;
        private Button _leftHand;
        private Button _rightHand;
        private Image[] _dialogueImage;
        private Text[] _dialogueText;
        private int _heartTouchCount =0;
        // 테마 이미지 변경을 위해서 가져옴 ( 화장실 컨텐츠에서만 변경사항 있음 )
        [SerializeField] private string _caseName = "Button_Case"; 
           
        [Header("Surprise")]
        [SerializeField] private Image _exclamationMark;
        [SerializeField] private Image _questionMark;
        [SerializeField] private Image _ghostHalf;
        [SerializeField] private Image _ghostHair;
        [SerializeField] private Image _ghostFullscreen;
        [SerializeField] private Image _blackBG;

        [Space(5)]
        [Header("Farting")]
        [SerializeField] private Transform _gasParents;
        [SerializeField] private Image _gas; // 복사할 오브젝트
        [SerializeField] private Image _gasEffect;
        [SerializeField] private Image _exclamationMark_2;
        [SerializeField] Sprite[] _gasImage;
        [SerializeField] private Button _inputRange;
        [SerializeField] private Image _yellowBG;

        [Space(5)]
        [Header("Wastepaper")]
        [SerializeField] private Transform _bathroomImage;

        [SerializeField] private Transform _dialogueObject;
        [SerializeField] private Button _inputRangeHappyEnd;
        [SerializeField] private Image _blackBG_3;
        [SerializeField] private Image _ghostFullscreen_3;

        [SerializeField] private GameObject _fxLight; // 라이트랑 터치 범위 포함
        

        WaitForSeconds perSec = new WaitForSeconds(0.15f);
        WaitForSeconds perSec_2 = new WaitForSeconds(0.3f);
        WaitForSeconds colorTimer = new WaitForSeconds(0.3f);

        [Space(5)]
        [Header("AudioClip")]
        [SerializeField] private AudioSource _SE;
        [SerializeField] private AudioSource _BGM;
        [SerializeField] private AudioClip[] _seContents1;
        [SerializeField] private AudioClip[] _seContents2;
        [SerializeField] private AudioClip[] _seContents3;
        [SerializeField] private AudioClip _s01,_s02,_s03;

        [Header("Particle")]
        [SerializeField] private ParticleSystem _fxHeartTouch; // 3. 엔딩 터치시
        [SerializeField] private ParticleSystem _fxFlares; // 3. 슥삭슥삭
        [SerializeField] private ParticleSystem _fxTouchIntend; // 3. 슥삭슥삭

        private void Start()
        {
            Init();
            AddEvent();
        }
        private void AddEvent()
        {
            if (Hide == null)
                Debug.Log("hide = null");
            else
                Debug.Log("hide != null");
        }

        private void Init()
        {
            int count = 4;
            _contentObjects = new GameObject[count];
            for (int i = 0; i <count; i++)
            {
                _contentObjects[i] = this.transform.GetChild(i).gameObject;
                _contentObjects[i].SetActive(false);
            }
            RandomContents();            
        }

        public void GetFigure(GameObject figure)
        {
            Debug.Log("겟 피규어! : " + figure.name);
            _modelObject = figure;
            _animator = _modelObject.GetComponent<Animator>();
            
        }
        private void RandomContents()
        {
            int num = UnityEngine.Random.Range(0, 3);
            //int num = 0;
            _contentObjects[num].gameObject.SetActive(true);
            switch (num)
            {
                case 0:
                    Init_Surprise(); break;
                case 1:
                    Init_Farting(); break;
                case 2:
                    Init_Wastpaper(); break;
                default:
                    Init_Surprise(); break;
            }
        }
        // 케이스 이미지 바꾸기
        private void ChangeCaseImage()
        {
            Image theme = ObjectFinder.Find(_caseName).GetComponent<Image>();
            if (theme == null)
            {
                Debug.LogError("<color=red>테마케이스 정보를 가져오지 못했습니다. _caseName 값을 확인해주세요</color>");
                return;
            }

            theme.sprite = _bathroomInsideImage;
        }

        enum Anim
        {
            IDLE,
            FRONTSIDE, // 앞 보기 ( 화면 바라보기 )
            BACKSIDE, // 뒤 보기 
            COLLAPSE,
            DANCE,
            ELATED,
            FIRE,
            JUMPTURN,
            LIEFLAT,
            LOWJUMP,
            PANIC,
            SHUDDER,
            STARTLING,
            WALK
        }
       //Anim _anim;
       /// <summary>
       /// 모델링 애니메이션
       /// </summary>
       /// <param name="anim"> 재생할 애니메이션 및 모션 </param>
       /// <param name="turnSpeed"> FRONTSIDE or BACKSIDE 모션에서 도는 속도 </param>
        private void ModelingMotion(Anim anim, float turnSpeed = 0f)
        {
            // 0 0 0 상태가 귀신 바라보기(BACKSIDE가 되어야함)
            switch (anim)
            {
                case Anim.IDLE:
                    _animator.SetTrigger("Idle"); break;
                case Anim.FRONTSIDE:
                    _modelObject.transform.DOLocalRotate(new Vector3(0, 180, 0), turnSpeed).SetEase(Ease.Linear);
                    break;
                case Anim.BACKSIDE:
                    _modelObject.transform.DOLocalRotate(new Vector3(0, 0, 0), turnSpeed).SetEase(Ease.Linear);
                    break;
                case Anim.COLLAPSE:
                    _animator.SetTrigger("Collapse"); break;
                case Anim.DANCE:
                    _animator.SetTrigger("Dance"); break;
                case Anim.ELATED:
                    _animator.SetTrigger("Elated"); break;
                case Anim.FIRE:
                    _animator.SetTrigger("Fire"); break;
                case Anim.JUMPTURN:
                    _animator.SetTrigger("JumpTurn"); break;
                case Anim.LIEFLAT:
                    _animator.SetTrigger("LieFlat"); break;
                case Anim.LOWJUMP:
                    _animator.SetTrigger("LowJump"); break;
                case Anim.PANIC:
                    _animator.SetTrigger("Panic"); break;
                case Anim.SHUDDER:
                    _animator.SetTrigger("Shudder"); break;
                case Anim.STARTLING:
                    _animator.SetTrigger("Startling"); break;
                case Anim.WALK:
                    _animator.SetTrigger("Walk"); break;
                default:
                    Debug.LogError("<color=red> 없는 애니메이션 입니다.에니메이터를 확인해주세요</color>"); break;
            }
        }
        public void SetData()
        {
            // 데이터 설정?
        }

        // 컨텐츠 종료하기
        public void CloseContents()
        {
            if (_isBackGhostShow)
                Destroy(_ghostHalf.gameObject);
            // 이 오브젝트 삭제
            // CustomView의 ButtonAni.gameobject 활성화 해줘야함
        }

    }
}