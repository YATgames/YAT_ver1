using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Circus : MonoBehaviour
    {
        [Header("BoxHide")]
        [SerializeField] private GameObject[] _giftBox;
        [SerializeField] private ParticleSystem _smoke;

        private IEnumerator InitBoxHide()
        {
            _giftBox[0].transform.localScale = Vector3.zero;
            _giftBox[1].transform.localScale = Vector3.zero;

            float boxScale = 150f;
            _giftBox[0].SetActive(true);
            _giftBox[0].transform.DOScale(boxScale, 0.5f).SetEase(Ease.OutBack);
            SoundManager.Instance.Play("Pop_SFX");
            yield return new WaitForSeconds(0.35f);

            _giftBox[1].SetActive(true);
            _giftBox[1].transform.DOScale(boxScale, 0.5f).SetEase(Ease.OutBack);
            SoundManager.Instance.Play("Pop_SFX");
            yield return new WaitForSeconds(1f);

            StartCoroutine(TextBoxAppear("나랑 숨바꼭질 할래?"));
            yield return new WaitForSeconds(4f);

            _smoke.Play();
            SoundManager.Instance.Play("SmokeGrenade_SFX");
            yield return new WaitForSeconds(1.5f);

            _figureModel.transform.localScale = Vector3.zero;
            SoundManager.Instance.Play("FigureLaugh_SFX");
            yield return new WaitForSeconds(4.5f);

            StartCoroutine(TextBoxAppear("어디 숨었는지 한번 찾아봐!"));
            yield return new WaitForSeconds(2.5f);



            for (int i = 0; i < _giftBox.Length; i++)
            {
                _giftBox[i].GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
                {
                    StartCoroutine(TouchBox());
                }).AddTo(gameObject);
            }
        }

        private IEnumerator TouchBox()
        {
            Transform boxTF = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                boxTF = hit.collider.transform;
            }

            _giftBox[0].GetComponent<Button>().enabled = false;
            _giftBox[1].GetComponent<Button>().enabled = false;

            bool isCorrectBox = Random.value > 0.5f;
            // isCorrectBox = false; // debug
            if(!isCorrectBox)
            {
                bool isSurpriseBox = Random.value > 0.5f;
                // isSurpriseBox = true; // debug
                if (isSurpriseBox)
                {
                    // 공포오브젝트 연출
                    GameObject scareObject = boxTF.GetChild(4).gameObject;
                    SoundManager.Instance.Play("Stinger_SFX");
                    scareObject.SetActive(true);
                    scareObject.GetComponent<Animator>().speed = 0f;
                    scareObject.transform.localScale = Vector3.zero;
                    scareObject.transform.DOScaleZ(0.1f, 0f);
                    scareObject.transform.DOScaleX(1f, 0.15f).SetEase(Ease.OutQuart);
                    scareObject.transform.DOScaleY(1f, 0.15f).SetEase(Ease.OutQuart);
                    GetElbowTF(scareObject.transform).DOShakeRotation(0.2f, 40f, 40, 180f, false);
                    yield return new WaitForSeconds(0.1f);
                    scareObject.transform.DOShakePosition(0.1f, 0.1f, 60, 90, false, false);
                    yield return new WaitForSeconds(0.1f);

                    Transform handTF = GetElbowTF(scareObject.transform).GetChild(0);
                    GetElbowTF(scareObject.transform).DOLocalRotate(new Vector3(0f, 0f, 45f), 0.15f, RotateMode.LocalAxisAdd);
                    handTF.DOLocalRotate(new Vector3(0f, 0f, -90f), 0.15f, RotateMode.LocalAxisAdd);

                    if (scareObject.transform.parent.gameObject == _giftBox[0])
                        handTF.DOLocalMove(new Vector3(-9.4f, -4.83f, -0.85f), 0.05f);
                    else
                        handTF.DOLocalMove(new Vector3(-33.3f, -3.11f, 15.6f), 0.05f);

                    handTF.DOScale(12f, 0.2f);
                    yield return new WaitForSeconds(0.1f);

                    SoundManager.Instance.Play("GlassShatter_SFX");
                    _brokenScreen.gameObject.SetActive(true);
                    _brokenScreen.DOShakeAnchorPos(0.2f, 300f, 300);

                    StartCoroutine(PullExitImagePos(2f));
                    _customView.ContentsExit(1f, 2f);
                    yield return new WaitForSeconds(1.95f);
                    _figureModel.transform.localScale = Vector3.one;
                }

                else
                {
                    // 놀리는 오브젝트 연출
                    GameObject mockObject = boxTF.GetChild(3).gameObject;
                    SoundManager.Instance.Play("SpringBoing_SFX");
                    mockObject.SetActive(true);
                    mockObject.transform.localScale = Vector3.one * 10f;
                    mockObject.transform.DOScaleY(25f, 1.5f).SetEase(Ease.OutElastic);
                    yield return new WaitForSeconds(0.25f);
                    SoundManager.Instance.Play("FigureLaugh_SFX");
                    _customView.ContentsExit(2f, 1.5f);
                    yield return new WaitForSeconds(2.95f);
                    _figureModel.transform.localScale = Vector3.one;
                }
            }
            else
            {
                // 정답연출
                SoundManager.Instance.Play("DrumRoll_SFX");
                boxTF.GetChild(0).gameObject.SetActive(true);
                boxTF.DOShakePosition(2.8f, new Vector3(20f, 0f, 20f)).SetInverted().SetEase(Ease.OutExpo);
                yield return new WaitForSeconds(2.8f);
                boxTF.DOShakePosition(1f, new Vector3(15f, 0f, 15f));
                boxTF.GetChild(1).GetComponent<ParticleSystem>().Play();
                _figureModel.transform.position = boxTF.position;
                _figureModel.transform.localEulerAngles = new Vector2(-360f, 180f);

                _anim.SetTrigger("LowJump");
                _figureModel.transform.DOJump(_figureInitPos, 3.5f, 1, 0.8f);
                _figureModel.transform.DOLocalRotate(new Vector3(360f, 0f, 0f), 0.65f, RotateMode.LocalAxisAdd);
                _figureModel.transform.DOScale(1f, 0.65f).SetEase(Ease.OutCubic);
                yield return new WaitForSeconds(1f);

                StartCoroutine(TextBoxAppear("아니? 어떻게 알았지?"));

                yield return new WaitForSeconds(1.5f);
                _anim.SetTrigger("Dance");

                _customView.ContentsExit(5f, 1.5f);
                yield return new WaitForSeconds(6f);
                _figureModel.transform.localPosition = Vector3.zero;
            }
        }

        private Transform GetElbowTF(Transform tf)
        {
            return tf.GetChild(0).GetChild(0).GetChild(1).GetChild(0);
        }
    }
}