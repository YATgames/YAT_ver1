using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Circus : MonoBehaviour
    {
        [Header("BoxHide")]
        [SerializeField] private AudioClip[] _boxClip;
        [SerializeField] private GameObject[] _giftBox;
        [SerializeField] private ParticleSystem _smoke;

        private IEnumerator InitBoxHide()
        {
            _giftBox[0].transform.localScale = Vector3.zero;
            _giftBox[1].transform.localScale = Vector3.zero;

            float boxScale = 150f;
            _giftBox[0].SetActive(true);
            _giftBox[0].transform.DOScale(boxScale, 0.5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.35f);

            _giftBox[1].SetActive(true);
            _giftBox[1].transform.DOScale(boxScale, 0.5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
            _giftBox[1].transform.DOScale(boxScale, 0.5f).SetEase(Ease.OutBack, 3f);

            StartCoroutine(TextBoxAppear("나랑 숨바꼭질 할래?"));
            yield return new WaitForSeconds(4f);

            _smoke.Play();
            _aSource.PlayOneShot(_boxClip[0]);
            yield return new WaitForSeconds(1.5f);

            _figureModel.SetActive(false);
            _aSource.PlayOneShot(_boxClip[1]);
            yield return new WaitForSeconds(6f);

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
            // isCorrectBox = true; // debug
            if(!isCorrectBox)
            {
                bool isSurpriseBox = Random.value > 0.5f;
                // isSurpriseBox = false; // debug
                if (isSurpriseBox)
                {
                    // 공포오브젝트 연출
                    GameObject scareObject = boxTF.GetChild(4).gameObject;
                    _aSource.PlayOneShot(_boxClip[4]);
                    scareObject.SetActive(true);
                    scareObject.GetComponent<Animator>().speed = 0f;
                    scareObject.transform.localScale = Vector3.zero;
                    scareObject.transform.DOScaleZ(0.1f, 0f);
                    scareObject.transform.DOScaleX(1f, 0.15f).SetEase(Ease.OutQuart);
                    scareObject.transform.DOScaleY(1f, 0.15f).SetEase(Ease.OutQuart);
                    yield return new WaitForSeconds(0.1f);
                    scareObject.transform.DOShakePosition(0.1f, 0.1f, 60, 90, false, false);
                }

                else
                {
                    // 놀리는 오브젝트 연출
                    GameObject mockObject = boxTF.GetChild(3).gameObject;
                    _aSource.PlayOneShot(_boxClip[3]);
                    mockObject.SetActive(true);
                    mockObject.transform.localScale = Vector3.one * 10f;
                    mockObject.transform.DOScaleY(25f, 1.5f).SetEase(Ease.OutElastic);
                    yield return new WaitForSeconds(0.25f);
                    _aSource.PlayOneShot(_boxClip[1]);
                }
            }
            else
            {
                // 정답연출
                _aSource.PlayOneShot(_boxClip[2]);
                boxTF.GetChild(0).gameObject.SetActive(true);
                yield return new WaitForSeconds(2.8f);

                boxTF.GetChild(1).GetComponent<ParticleSystem>().Play();
                _figureModel.transform.position = boxTF.position;
                _figureModel.transform.localScale = Vector3.zero;
                _figureModel.SetActive(true);
                _figureModel.transform.localEulerAngles = new Vector2(-360f, 180f);

                _anim.SetTrigger("LowJump");
                _figureModel.transform.DOJump(_figureInitPos - Vector3.forward * 5, 3.5f, 1, 0.8f);
                _figureModel.transform.DOLocalRotate(new Vector3(360f, 0f, 0f), 0.65f, RotateMode.LocalAxisAdd);
                _figureModel.transform.DOScale(1f, 0.65f).SetEase(Ease.OutCubic);
                yield return new WaitForSeconds(1f);

                StartCoroutine(TextBoxAppear("아니? 어떻게 알았지?"));

                yield return new WaitForSeconds(1.5f);
                _anim.SetTrigger("Dance");
            }
        }
    }
}