using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Circus : MonoBehaviour
    {
        [Header("FireShow")]
        [SerializeField] private GameObject _flameThrowerVFX;
        [SerializeField] private ParticleSystem _fireExtinguisherVFX;
        [SerializeField] private RectTransform _fireExtinguisher;
        [SerializeField] private Image _questionMark;
        [SerializeField] private GameObject _fireWarning;

        private IEnumerator InitFireShow()
        {
            // 피규어 위치에 따라 바뀌는 head position 따라서 불 이동
            _flameThrowerVFX.transform.position = _figureModel.GetComponent<PrefabModel.Model_Body>().HeadPosition.position;
            SoundManager.Instance.PlayBGM("FireShow_BGM");

            StartCoroutine(TextBoxAppear("내가 재밌는걸 보여줄게!"));
            yield return new WaitForSeconds(4f);

            _anim.SetTrigger("Fire");
            yield return new WaitForSeconds(2.3f);
            SoundManager.Instance.Play("FlameThrower_SFX");

            _flameThrowerVFX.SetActive(true);
            yield return new WaitForSeconds(1.5f);

            StartCoroutine(WarningSpawn());
            _anim.speed = 0f;


            yield return new WaitForSeconds(4f);

            float extinguisherInitPos = _height * -0.5f;
            float extinguisherEndPos = _height * -0.16f;
            float extinguisherAppearDuration = 1f;
            _fireExtinguisher.anchoredPosition = new Vector2(0f, extinguisherInitPos);
            _fireExtinguisher.DOAnchorPosY(extinguisherEndPos, extinguisherAppearDuration).SetEase(Ease.OutBack);
            _fireExtinguisher.gameObject.SetActive(true);
            _fireExtinguisher.GetComponent<Button>().enabled = false;
            yield return new WaitForSeconds(1f);
            SoundManager.Instance.Play("QuestionMarkPop_SFX");

            _questionMark.transform.position = _figureModel.GetComponent<PrefabModel.Model_Body>().HeadPosition.position + new Vector3(0f, 2f, -5f);
            _questionMark.color = Color.clear;
            _questionMark.transform.localEulerAngles = new Vector3(0f, 0f, 45f);

            _questionMark.gameObject.SetActive(true);
            _questionMark.DOColor(Color.white, 0.15f);
            _questionMark.transform.DOLocalRotate(Vector3.zero, 0.3f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(1f);

            _questionMark.DOColor(Color.clear, 0.15f);
            yield return new WaitForSeconds(0.75f);

            _fireExtinguisher.GetComponent<Button>().enabled = true;
            _fireExtinguisher.GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
            {
                StartCoroutine(FireExtinguisherTouchCoroutine());
            }).AddTo(gameObject);
        }

        private IEnumerator FireExtinguisherTouchCoroutine()
        {
            _anim.speed = 1f;
            _fireExtinguisherVFX.gameObject.SetActive(true);
            _fireExtinguisher.GetComponent<Button>().enabled = false;
            _flameThrowerVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();

            int sprayCount = 2;
            for (int i = 0; i < sprayCount; i++)
            {
                StartCoroutine(ExtinguisherSprayCoroutine());
                yield return new WaitForSeconds(1.5f);
            }

            foreach(var part in _fireExtinguisherVFX.GetComponentsInChildren<ParticleSystem>())
            {
                var mainModule = part.main;
                mainModule.loop = true;
            }
            
            SoundManager.Instance.Play("WaterSpray_SFX");
            _fireExtinguisherVFX.Play();
            yield return new WaitForSeconds(0.2f);

            SoundManager.Instance.Play("Boing_SFX");
            _anim.SetTrigger("Startling");

            _customView.ContentsExit(1f, 2f);
            yield return new WaitForSeconds(1.9f);
            SoundManager.Instance.StopBGM();
        }

        private IEnumerator ExtinguisherSprayCoroutine()
        {
            SoundManager.Instance.Play("WaterSpray_SFX");
            _fireExtinguisherVFX.Play();
            yield return new WaitForSeconds(0.2f);

            SoundManager.Instance.Play("Boing_SFX");
            _anim.SetTrigger("LowJump");
            yield return new WaitForSeconds(0.1f);
        }

        private IEnumerator WarningSpawn()
        {
            for (int i = 0; i < 8; i++)
            {
                Transform warningObj = Instantiate(_fireWarning, transform).transform;
                warningObj.gameObject.SetActive(true);
                StartCoroutine(WarningCoroutine(warningObj));
                Destroy(warningObj.gameObject, 2f);

                if (i < 2)
                    yield return new WaitForSeconds(1f);
                else
                    yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            }

            IEnumerator WarningCoroutine(Transform tf)
            {
                Vector3 randomPos = Random.insideUnitCircle * 2.5f;
                tf.localScale = Vector3.zero;
                tf.transform.position += randomPos;
                SoundManager.Instance.Play("ErrorAlarm_SFX");
                tf.DOScale(Random.Range(1.5f, 3f), 0.25f).SetEase(Ease.OutBack);
                yield return new WaitForSeconds(1.5f);
                tf.DOScale(0f, 0.25f).SetEase(Ease.InBack);
            }
        }
    }
}