using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Circus : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Transform _ContentsParent;
        [SerializeField] private RectTransform _textBox;

        private GameObject _figureModel;
        private Animator _anim;
        private float _width;
        private float _height;
        private Vector3 _figureInitPos;
        private CustomView _customView;

        // [SerializeField] [Range(0, 2)] int randomContent;

        private void Start()
        {
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

            // ¸¸¾à ¼­¹ö¿¡¼­ ÄÜÅÙÃ÷ÀÇ °¹¼ö°¡ ¸í½ÃµÇ¸é 3À» ÇØ´ç ÄÜÅÙÃ÷ °¹¼ö·Î º¯°æÇÒ ¼ö ÀÖÀ½
            int randomContent = Random.Range(0, 3);
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
        private IEnumerator PullExitImagePos(float timer)
        {
            RectTransform exitTF = transform.parent.parent.Find("ContentsExitImage").GetComponent<RectTransform>();
            exitTF.localPosition += new Vector3(0f, 0f, -2000f);

            yield return new WaitForSeconds(timer);

            exitTF.localPosition += new Vector3(0f, 0f, +2000f);
        }
    }
}
