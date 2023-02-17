using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Circus : MonoBehaviour
    {
        // ActionContents 의 Child 로 콘텐츠가 Instantiate 되지만 피규어 모델은 상위 하이라키에 만들어 질것으로 추정됨.
        // ActionContents 안에 일단 같이 피규어 게임오브젝트를 넣어놓았지만 이후 변경
        [Header("Main")]
        [SerializeField] private Transform _ContentsParent;
        [SerializeField] private RectTransform _textBox;

        private GameObject _figureModel;
        private Animator _anim;
        private AudioSource _aSource;
        private float _width;
        private float _height;
        private Vector3 _figureInitPos;
        private CustomView _customView;

        private void Start()
        {
            //_customView = transform.parent.parent.GetComponent<CustomView>();
            _aSource = GetComponent<AudioSource>();
            _figureModel = transform.parent.parent.GetComponentInChildren<PrefabModel.Model_Body>(false).gameObject;
            _figureInitPos = _figureModel.transform.parent.position;
            _anim = _figureModel.GetComponent<Animator>();
            _width = GetComponent<RectTransform>().rect.width;
            _height = GetComponent<RectTransform>().rect.height;

            InitRandomContent();
        }

        private void InitRandomContent()
        {
            for (int i = 0; i < _ContentsParent.childCount; i++)
            {
                _ContentsParent.GetChild(i).gameObject.SetActive(false);
            }

            // 만약 서버에서 콘텐츠의 갯수가 명시되면 3을 해당 콘텐츠 갯수로 변경할 수 있음
            int randomContent = Random.Range(0, 3);
            // randomContent = 2; // DebugInt
            _ContentsParent.GetChild(randomContent).gameObject.SetActive(true);

            switch (randomContent)
            {
                case 0:
                    StartCoroutine(InitFireShow());
                    break;
                case 1:
                    StartCoroutine(InitBallThrow());
                    break;
                case 2:
                    StartCoroutine(InitBoxHide());
                    break;
                default:
                    Debug.LogWarning("randomContentIntOutOfRange");
                    break;
            }
        }
        public void GetCustomView(CustomView customView)
        {
            _customView = customView;
        }
        public void GetFigure(GameObject figure)
        {
            _figureModel = figure;
            _anim = _figureModel.GetComponent<Animator>();
        }
        private IEnumerator TextBoxAppear(string shownText)
        {
            Text textBoxText = _textBox.GetChild(0).GetComponent<Text>();
            float textBoxDisappearPos = _height * 0.2f;
            float textBoxAppearPos = _height * 0.275f;
            float appearDuration = 0.75f;

            textBoxText.text = shownText;

            _textBox.gameObject.SetActive(true);
            _textBox.DOAnchorPosY(textBoxDisappearPos, 0f);
            _textBox.DOLocalMoveY(textBoxAppearPos, appearDuration).SetEase(Ease.OutBack);

            Image textBoxImage = _textBox.GetComponent<Image>();
            textBoxImage.color = Color.clear;
            textBoxText.color = Color.clear;
            textBoxImage.DOColor(Color.white, appearDuration);
            textBoxText.DOColor(Color.white, appearDuration);
            yield return new WaitForSeconds(3f);

            _textBox.DOLocalMoveY(textBoxDisappearPos, 0.5f);
            textBoxImage.DOColor(Color.clear, appearDuration);
            textBoxText.DOColor(Color.clear, appearDuration);

            yield return new WaitForSeconds(0.75f);
            _textBox.gameObject.SetActive(false);
        }
    }
}
