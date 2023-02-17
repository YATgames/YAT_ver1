using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

using DG.Tweening;
using UniRx;
using Assets.Scripts.Common;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Bathroom : MonoBehaviour
    {
        // [AcionContents-Bathroom-Surprise]

        private float _showdelay = 0.15f;
        private IEnumerator SurpriseRoutine()
        {
            // 1. 귀신 보이기
            // 2. 느낌표 및 모델링 회전
            // 3. 모델링 회전및 물음표
            // 4. 머리카락 내려오기 및 느낌표 
            // 4(터치시). 머리카락 올라가기 및 캐릭터 부들부들(Shudder)
            // 5. 화면 검정색으로
            // 6. 무서운 귀신 화면 아래에서 & 진동 & 피규어 비명소리

            GhostShow();            
            PlaySE(_seContents1[0]); // SE :노려보는 공포효과음
            yield return new WaitForSeconds(1f);

            PlayExclamationMark();
            PlaySE(_seContents2[2]); //
            yield return new WaitForSeconds(1f);
            
            ModelingMotion(Anim.BACKSIDE); // 뒤돌기
            _ghostHalf.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.2f)
                .SetEase(Ease.Linear);
            PlaySE(_seContents1[1]); // SE : 휙 도는 소리
            yield return new WaitForSeconds(1f);

            PlayQuestionMark();
            //PlaySE(_seContents2[2]); //
            yield return new WaitForSeconds(1.5f);

            ModelingMotion(Anim.FRONTSIDE); // 앞보기  
            PlaySE(_seContents1[1]); // SE : 휙 도는 소리
            yield return new WaitForSeconds(2f);

            _ghostHair.gameObject.SetActive(true);
            _ghostHair.rectTransform.DOAnchorPosY(0, 0.3f);
            PlaySE(_seContents1[0]); // SE :노려보는 공포효과음
            PlayExclamationMark();
            yield return new WaitForSeconds(0.6f);
            _ghostHair.raycastTarget = true;
            _isColorChanged = true;
            StartCoroutine(ColorChangeRoutine()); // 터치 유도 상태에서 있기
        }

        private IEnumerator SurpriseRoutine2()
        {
            //_ghostHair.DOFade(0f, 1f).SetEase(Ease.Linear);
            _ghostHair.raycastTarget = false;
            _ghostHair.rectTransform.DOAnchorPosY(1024, 0.2f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
            // SE : 부들들 떨기
            ModelingMotion(Anim.SHUDDER);
            PlaySE(_seContents1[4]);
            yield return new WaitForSeconds(0.5f);
            _blackBG.DOFade(1f, 1.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1.5f);
            _ghostFullscreen.DOFade(1f, 0.2f);
            _ghostFullscreen.rectTransform.DOAnchorPosY(0, 0.1f)
                .OnComplete(() =>
                {
                    PlaySE(_seContents1[3]);
                    _ghostFullscreen.rectTransform.DOScale(1.4f, 0.15f).SetEase(Ease.OutBack);
                    _ghostFullscreen.rectTransform.DOShakeAnchorPos(1.2f, 100, 30, 90, false, false);
                });

            yield return new WaitForSeconds(1f);
            CustomView.ContentsExit(1f, 2f);
            yield break;
        }
        private void GhostShow()
        {
            _ghostHalf.DOFade(1f, 0.2f).SetEase(Ease.Linear);
            _ghostHalf.DOFillAmount(1f, 0.6f).From(0f).SetEase(Ease.Linear);
            _ghostHalf.rectTransform.DOAnchorPosY(400f, 0.6f)
                .SetEase(Ease.OutBack);
        }
        private void Init_Surprise()
        {
            _exclamationMark.gameObject.SetActive(true);
            _questionMark.gameObject.SetActive(true);
            _ghostHair.gameObject.SetActive(false);
            _ghostHalf.gameObject.SetActive(true);
            _ghostFullscreen.gameObject.SetActive(true);
            _blackBG.gameObject.SetActive(true);

            _exclamationMark.color = new Color(1, 1, 1, 0);
            _questionMark.color = new Color(1, 1, 1, 0);
            _ghostHalf.color = new Color(1, 1, 1, 0);
            _ghostFullscreen.color = new Color(1, 1, 1, 0);
            _blackBG.color = Color.clear;

            _ghostHalf.rectTransform.anchoredPosition = Vector2.zero;
            _ghostHair.raycastTarget = false;
            _ghostFullscreen.rectTransform.anchoredPosition = new Vector2(0, -1024);

            Transform tf = ObjectFinder.Find("CustomView").transform;
            _ghostHalf.transform.SetParent(tf);
            _ghostHalf.transform.SetAsFirstSibling();
            _isBackGhostShow = true;

            _ghostHair.GetComponent<Button>().onClick.
                AsObservable().Subscribe(_ =>
            {
                _isColorChanged = false;
                StartCoroutine(SurpriseRoutine2());
            });
            SurpriseRoutine()
                .ToObservable()
                .Subscribe(_ => Debug.Log("A"), () => Debug.Log("B"));
        }

        private Sequence _seqExclamationMark;
        private Sequence _seqQuestionMark;
        private void PlayExclamationMark()
        {
            if (_seqExclamationMark == null)
            {
                _seqExclamationMark = DOTween.Sequence()
                    .OnStart(() =>
                    {
                        _exclamationMark.color = new Color(1, 1, 1, 0);
                    }).SetAutoKill(false).

                    Append(_exclamationMark.DOFade(1f, 0.1f).From(0f))
                    .Join(_exclamationMark.transform.DOLocalRotate(new Vector3(0, 0, 10), 0.25f).SetEase(Ease.InQuad))

                    .Append(_exclamationMark.transform.DOLocalRotate(new Vector3(0, 0, -15), 0.15f).SetEase(Ease.InQuad))
                    .Join(_exclamationMark.rectTransform.DOAnchorPosY(+10f, 0.2f).SetRelative(true).SetEase(Ease.OutBack))

                    .Append(_exclamationMark.rectTransform.DOAnchorPosY(-10f, 0.2f).SetRelative(true).SetEase(Ease.Linear))
                    .Join(_exclamationMark.transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.Linear))

                    .Append(_exclamationMark.DOFade(0f, 0.5f).SetEase(Ease.Linear));
            }
            else
                _seqExclamationMark.Restart();
        }
        private void PlayQuestionMark()
        {
            if (_seqQuestionMark == null)
            {
                _seqQuestionMark = DOTween.Sequence()
                    .OnStart(() =>
                    {
                        _questionMark.color = new Color(1, 1, 1, 0);
                        _questionMark.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
                    }).SetAutoKill(false)

                    .Append(_questionMark.DOFade(1f, 0.1f).From(0f))
                    .Join(_questionMark.transform.DOLocalRotate(new Vector3(0, 0, 20), 0.3f).SetEase(Ease.InQuad))

                    .Append(_questionMark.transform.DOLocalRotate(new Vector3(0, 0, -20), 0.3f).SetEase(Ease.InQuad))
                    .Append(_questionMark.transform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                    .Join(_questionMark.DOFade(0f, 0.5f).SetEase(Ease.Linear));
            }
            else
                _seqQuestionMark.Restart();
        }
        private IEnumerator ColorChangeRoutine()
        {
            float time = 0.5f;
            while (_isColorChanged)
            {
                _ghostHair.DOColor(new Color(1, 0.6f, 0.6f, 1), time);
                yield return new WaitForSeconds(time);
                _ghostHair.DOColor(new Color(1, 1f, 1f, 1), time);
                yield return new WaitForSeconds(time);
            }
            yield break;
        }
    }
}