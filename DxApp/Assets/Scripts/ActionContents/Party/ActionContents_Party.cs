using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Party : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Transform _ContentsParent;
        [SerializeField] private RectTransform _textBox;

        private GameObject _figureModel;
        private Animator _anim;
        private float _width;
        private float _height;

        private void Start()
        {
            // 아직 기본 피규어 pos rot 애매함
            _figureModel = transform.parent.parent.GetComponentInChildren<PrefabModel.Model_Body>(false).gameObject;
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
            randomContent = 0; // DebugInt
            _ContentsParent.GetChild(randomContent).gameObject.SetActive(true);

            switch (randomContent)
            {
                case 0:
                    StartCoroutine(InitMakeCake());
                    break;
                case 1:
                    StartCoroutine(InitMakeShake());
                    break;
                case 2:
                    StartCoroutine(InitBalloonPop());
                    break;
                default:
                    Debug.LogWarning("randomContentIntOutOfRange");
                    break;
            }
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