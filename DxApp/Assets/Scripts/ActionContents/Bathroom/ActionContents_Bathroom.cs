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
        // ���������� ���ؼ� Model ���� �޾ƿ� �͵�
        public FlowManager FlowManager { get; set; }
        public OnEventTrigger Hide { get; set; }


        private GameObject[] _contentObjects;

        private Color _alphaNone = new Color(1, 1, 1, 0);
        // [������ ���� ������Ʈ
        
        private GameObject _modelObject;
        private Animator _animator;
        private float _zValue = -200f; // �̹����� �ǱԾ� ���� ���̱� ���� z�� ����

        private bool _isBackGhostShow = false;
        private bool _isColorChanged;

        private int _farCreateCount = 25;
        private float _gasCreateRange;

        private Image _redTissue;
        private Image _blueTissue;
        
        private string[] _dialogue = { "���� �����ٱ�...", "�Ķ� �����ٱ�...", "��~ �ÿ��ϴ�" };

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
        // �׸� �̹��� ������ ���ؼ� ������ ( ȭ��� ������������ ������� ���� )
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
        [SerializeField] private Image _gas; // ������ ������Ʈ
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

        [SerializeField] private GameObject _fxLight; // ����Ʈ�� ��ġ ���� ����
        

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
        [SerializeField] private ParticleSystem _fxHeartTouch; // 3. ���� ��ġ��
        [SerializeField] private ParticleSystem _fxFlares; // 3. ���轻��
        [SerializeField] private ParticleSystem _fxTouchIntend; // 3. ���轻��

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
            Debug.Log("�� �ǱԾ�! : " + figure.name);
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
        // ���̽� �̹��� �ٲٱ�
        private void ChangeCaseImage()
        {
            Image theme = ObjectFinder.Find(_caseName).GetComponent<Image>();
            if (theme == null)
            {
                Debug.LogError("<color=red>�׸����̽� ������ �������� ���߽��ϴ�. _caseName ���� Ȯ�����ּ���</color>");
                return;
            }

            theme.sprite = _bathroomInsideImage;
        }

        enum Anim
        {
            IDLE,
            FRONTSIDE, // �� ���� ( ȭ�� �ٶ󺸱� )
            BACKSIDE, // �� ���� 
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
       /// �𵨸� �ִϸ��̼�
       /// </summary>
       /// <param name="anim"> ����� �ִϸ��̼� �� ��� </param>
       /// <param name="turnSpeed"> FRONTSIDE or BACKSIDE ��ǿ��� ���� �ӵ� </param>
        private void ModelingMotion(Anim anim, float turnSpeed = 0f)
        {
            // 0 0 0 ���°� �ͽ� �ٶ󺸱�(BACKSIDE�� �Ǿ����)
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
                    Debug.LogError("<color=red> ���� �ִϸ��̼� �Դϴ�.���ϸ����͸� Ȯ�����ּ���</color>"); break;
            }
        }
        public void SetData()
        {
            // ������ ����?
        }

        // ������ �����ϱ�
        public void CloseContents()
        {
            if (_isBackGhostShow)
                Destroy(_ghostHalf.gameObject);
            // �� ������Ʈ ����
            // CustomView�� ButtonAni.gameobject Ȱ��ȭ �������
        }

    }
}