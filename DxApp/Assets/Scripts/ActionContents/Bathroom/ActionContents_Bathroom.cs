using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Bathroom : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        private CustomView CustomView;
        private GameObject[] _contentObjects;
        private Color _alphaNone = new Color(1, 1, 1, 0);
        private GameObject _modelObject;
        private Animator _animator;

        private bool _isColorChanged; // Surprise에서 터치 했는지 검사

        private int _farCreateCount = 20; // 방귀 생성 개수(터치 해야하는 수) default: 25
        private float _gasCreateRange; // 방귀 생성 범위

        private Image _redTissue;
        private Image _blueTissue;
        
        private string[] _dialogue = { "빨간 휴지줄까...", "파란 휴지줄까...", "오~ 시원하다" };

        [SerializeField] private Sprite _bathroomOutsideImage;
        [SerializeField] private Sprite _bathroomInsideImage;
        private Image[] _dialogueImage;
        private Text[] _dialogueText;

        // 테마 이미지 변경을 위해서 가져옴 ( 화장실 컨텐츠에서만 변경사항 있음 )
        [SerializeField] private string _caseName = "Button_Case";

        [Header("Surprise")]
        [SerializeField] private Image _exclamationMark;
        [SerializeField] private Image[] _questionMark;
        [SerializeField] private Image _ghostHalf;
        [SerializeField] private Image _ghostHair;
        [SerializeField] private GameObject _shudderEffect;
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
        [SerializeField] private Image _redBG;

        [Space(5)]
        [Header("Wastepaper")]
        [SerializeField] private Button _leftHand;
        [SerializeField] private Button _rightHand;
        [SerializeField] private Transform _dialogueObject;
        [SerializeField] private GameObject _shudderEffect_3;
        [SerializeField] private Image _blackBG_3;
        [SerializeField] private Image _ghostFullscreen_3;

        [SerializeField] private GameObject _fxLight; // 라이트랑 터치 범위 포함
        

        WaitForSeconds perSec = new WaitForSeconds(0.15f);
        WaitForSeconds perSec_2 = new WaitForSeconds(0.3f);

        [Space(5)]
        [Header("Particle")]
        [SerializeField] private ParticleSystem _fxHeartTouch; // 3. 엔딩 터치시
        [SerializeField] private ParticleSystem _fxFlares; // 3. 슥삭슥삭
        [SerializeField] private ParticleSystem _fxShudder; // 오들오들 떨기
        private void Start()
        {
            Init();
        }
        private void Init()
        {
            int count = 3;
            _contentObjects = new GameObject[count];
            for (int i = 0; i <count; i++)
            {
                _contentObjects[i] = this.transform.GetChild(i).gameObject;
                _contentObjects[i].SetActive(false);
            }
            RandomContents();            
        }
        public void GetCustomView(CustomView customView)
        {
            CustomView = customView;
        }
        public void GetFigure(GameObject figure)
        {
            _modelObject = figure;
            _animator = _modelObject.GetComponent<Animator>();   
        }
        private void RandomContents()
        {
            int num = UnityEngine.Random.Range(0, 3);
            //int num = 2;
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
        
        private enum Side
        {
            Inside,Outside,
        }
        // 케이스 이미지 바꾸기(화장실에서만 다른 이미지가 사용됨)
        private void ChangeCaseImage(Side side)
        {
            if (side == Side.Inside)
                CustomView.temmaImage.sprite = _bathroomInsideImage;
            else if (side == Side.Outside)
                CustomView.temmaImage.sprite = _bathroomOutsideImage;
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
        private void ModelingMotion(Anim anim)
        {
            // 0 0 0 상태가 귀신 바라보기(BACKSIDE가 되어야함)
            switch (anim)
            {
                case Anim.IDLE:
                    _animator.SetTrigger("Idle"); break;
                case Anim.FRONTSIDE:
                    _modelObject.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.3f).SetEase(Ease.Linear);
                    break;
                case Anim.BACKSIDE:
                    _modelObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.Linear);
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
    }
}