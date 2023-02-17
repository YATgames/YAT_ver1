using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Circus : MonoBehaviour
    {
        [Header("FireShow")]
        [SerializeField] private AudioClip[] _fireClip;
        [SerializeField] private GameObject _flameThrowerVFX;
        [SerializeField] private ParticleSystem _fireExtinguisherVFX;
        [SerializeField] private RectTransform _fireExtinguisher;
        [SerializeField] private Image _questionMark;

        private IEnumerator InitFireShow()
        {
            // 피규어 위치에 따라 바뀌는 head position 따라서 불 이동
            _aSource.clip = _fireClip[0];
            _aSource.volume = 0.3f;
            _aSource.Play();
            _flameThrowerVFX.transform.position = _figureModel.GetComponent<PrefabModel.Model_Body>().HeadPosition.position;
            
            StartCoroutine(TextBoxAppear("내가 재밌는걸 보여줄게!"));
            yield return new WaitForSeconds(4f);

            _anim.SetTrigger("Fire");
            yield return new WaitForSeconds(2.3f);
            _aSource.PlayOneShot(_fireClip[4]);
            yield return new WaitForSeconds(0.5f);

            _flameThrowerVFX.SetActive(true);
            yield return new WaitForSeconds(5f);


            float extinguisherInitPos = _height * -0.5f;
            float extinguisherEndPos = _height * -0.16f;
            float extinguisherAppearDuration = 1f;
            _fireExtinguisher.anchoredPosition = new Vector2(0f, extinguisherInitPos);
            _fireExtinguisher.DOAnchorPosY(extinguisherEndPos, extinguisherAppearDuration).SetEase(Ease.OutBack);
            _fireExtinguisher.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            _aSource.PlayOneShot(_fireClip[1]);

            _questionMark.transform.position = _figureModel.GetComponent<PrefabModel.Model_Body>().HeadPosition.position + new Vector3(0f, 2f, -5f);
            _questionMark.color = Color.clear;
            _questionMark.transform.localEulerAngles = new Vector3(0f, 0f, 45f);

            _questionMark.gameObject.SetActive(true);
            _questionMark.DOColor(Color.white, 0.15f);
            _questionMark.transform.DOLocalRotate(Vector3.zero, 0.3f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(1f);

            _questionMark.DOColor(Color.clear, 0.15f);
            yield return new WaitForSeconds(0.75f);

            _fireExtinguisherVFX.gameObject.SetActive(true);

            int sprayCount = 3;
            for (int i = 0; i < sprayCount; i++)
            {
                StartCoroutine(ExtinguisherSprayCoroutine());
                yield return new WaitForSeconds(3f);
            }
            _customView.ContentsExit(1.5f, 1.5f);
        }

        private IEnumerator ExtinguisherSprayCoroutine()
        {
            _aSource.PlayOneShot(_fireClip[2]);
            _fireExtinguisherVFX.Play();
            yield return new WaitForSeconds(0.25f);

            _aSource.PlayOneShot(_fireClip[3]);
            _anim.SetTrigger("Startling");
            yield return new WaitForSeconds(1.5f);
        }
    }
}