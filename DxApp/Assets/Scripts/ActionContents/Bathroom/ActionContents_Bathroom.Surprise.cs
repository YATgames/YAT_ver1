using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UniRx;
using Assets.Scripts.Common;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Bathroom : MonoBehaviour
    {
        // [AcionContents-Bathroom-Surprise]
        private IEnumerator SurpriseRoutine()
        { 
            // 등장1
            GhostShow();
            //PlaySE(_seContents1[0]); // SE :노려보는 공포효과음
            SoundManager.Instance.Play("Bathroom1E01_SFX");
            yield return new WaitForSeconds(1f);

            PlayExclamationMark();
            //PlaySE(_seContents2[2]);
            SoundManager.Instance.Play("Bathroom1E03_SFX");

            yield return new WaitForSeconds(1f);

            ModelingMotion(Anim.BACKSIDE); // 뒤돌기
            _ghostHalf.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.2f)
                .SetEase(Ease.Linear);
            //PlaySE(_seContents1[1]); // SE : 휙 도는 소리
            SoundManager.Instance.Play("Bathroom1E02_SFX");
            yield return new WaitForSeconds(1f);

            PlayQuestionMark(0);
            yield return new WaitForSeconds(0.1f);
            PlayQuestionMark(1);
            yield return new WaitForSeconds(1.5f);


            ModelingMotion(Anim.FRONTSIDE); // 앞보기  
            //PlaySE(_seContents1[1]); // SE : 휙 도는 소리
            SoundManager.Instance.Play("Bathroom1E02_SFX");
            yield return new WaitForSeconds(2f);

            // 등장2
            GhostShow();
            //PlaySE(_seContents1[0]); // SE :노려보는 공포효과음
            SoundManager.Instance.Play("Bathroom1E01_SFX");
            yield return new WaitForSeconds(1f);

            PlayExclamationMark();
            //PlaySE(_seContents2[2]);
            SoundManager.Instance.Play("Bathroom2E03_SFX");
            yield return new WaitForSeconds(1f);
            ModelingMotion(Anim.BACKSIDE); // 뒤돌기
            _ghostHalf.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.2f)
                .SetEase(Ease.Linear);
            //PlaySE(_seContents1[1]); // SE : 휙 도는 소리
            SoundManager.Instance.Play("Bathroom1E02_SFX");
            yield return new WaitForSeconds(1.5f);

            ModelingMotion(Anim.FRONTSIDE); // 앞보기  
            //PlaySE(_seContents1[1]); // SE : 휙 도는 소리
            SoundManager.Instance.Play("Bathroom1E02_SFX");
            yield return new WaitForSeconds(1f);


            // 달달떨기
            ModelingMotion(Anim.SHUDDER);
            _shudderEffect.SetActive(true);
            //PlaySE(_seContents1[4]);
            SoundManager.Instance.Play("Bathroom1E05_SFX", true);
            _fxShudder.Play();
            // 이펙트 추가해야함
            yield return new WaitForSeconds(1f);


            _ghostHair.gameObject.SetActive(true);
            _ghostHair.rectTransform.DOAnchorPosY(0, 0.3f);
            //PlaySE(_seContents1[0]); // SE :노려보는 공포효과음
            SoundManager.Instance.Play("Bathroom1E01_SFX");
            PlayExclamationMark();
            yield return new WaitForSeconds(0.6f);
            _ghostHair.raycastTarget = true;
            _isColorChanged = true;
            StartCoroutine(ColorChangeRoutine()); // 터치 유도 상태에서 있기
        }

        private IEnumerator SurpriseRoutine2()
        {
            _ghostHair.raycastTarget = false;
            _ghostHair.rectTransform.DOAnchorPosY(1024, 0.2f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
            // SE : 부들들 떨기
            //ModelingMotion(Anim.SHUDDER);
            _fxShudder.Stop();
            _shudderEffect.gameObject.SetActive(false);
            //PlaySE(_seContents1[4]);
            //SoundManager.Instance.Play("Bathroom1E05_SFX");
            yield return new WaitForSeconds(0.5f);
            _blackBG.DOFade(1f, 1.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1.5f);
            SoundManager.Instance.Stop("Bathroom1E05SFX");
            _ghostFullscreen.DOFade(1f, 0.2f);
            _ghostFullscreen.rectTransform.DOAnchorPosY(0, 0.1f)
                .OnComplete(() =>
                {
                    //PlaySE(_seContents1[3]);
                    SoundManager.Instance.Play("Bathroom1E04_SFX");
                    _ghostFullscreen.rectTransform.DOScale(1.4f, 0.15f).SetEase(Ease.OutBack);
                    _ghostFullscreen.rectTransform.DOShakeAnchorPos(1.5f, 100, 30, 90, false, false);
                });
            yield return new WaitForSeconds(0.4f);
            SoundManager.Instance.StopBGM();
            CustomView.ContentsExit(1f, 2f);
            yield break;
        }
        private void GhostShow()
        {
            _ghostHalf.DOFade(1f, 0.2f).SetEase(Ease.Linear);
            //_ghostHalf.DOFillAmount(1f, 0.6f).From(0f).SetEase(Ease.Linear);
            _ghostHalf.rectTransform.DOAnchorPosY(400f, 1f)
                .SetEase(Ease.OutBack);
        }
        private void Init_Surprise()
        {
            _exclamationMark.gameObject.SetActive(true);
            _questionMark[0].gameObject.SetActive(true);
            _questionMark[1].gameObject.SetActive(true);
            _ghostHair.gameObject.SetActive(false);
            _ghostHalf.gameObject.SetActive(true);
            _shudderEffect.gameObject.SetActive(false);
            _ghostFullscreen.gameObject.SetActive(true);
            _blackBG.gameObject.SetActive(true);
            

            _exclamationMark.color = new Color(1, 1, 1, 0);
            _questionMark[0].color = new Color(1, 1, 1, 0);
            _questionMark[1].color = new Color(1, 1, 1, 0);
            _ghostHalf.color = new Color(1, 1, 1, 0);
            _ghostFullscreen.color = new Color(1, 1, 1, 0);
            _blackBG.color = Color.clear;

            _ghostHalf.rectTransform.anchoredPosition = Vector2.zero;
            _ghostHair.raycastTarget = false;
            _ghostFullscreen.rectTransform.anchoredPosition = new Vector2(0, -1024);

            Transform tf = ObjectFinder.Find("CustomView").transform;
            _ghostHalf.transform.SetParent(tf);
            _ghostHalf.transform.SetAsFirstSibling();

            _ghostHair.GetComponent<Button>().onClick.
                AsObservable().Subscribe(_ =>
            {
                _isColorChanged = false;
                StartCoroutine(SurpriseRoutine2());
            });
            
            StartCoroutine(SurpriseRoutine());
            //PlayBGM(_s01);
            SoundManager.Instance.PlayBGM("Bathroom1_BGM");
        }

        private Sequence _seqExclamationMark;
        private Sequence[] _seqQuestionMark = new Sequence[2];
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

                    .Append(_exclamationMark.DOFade(0f, 0.3f).SetEase(Ease.Linear));
            }
            else
                _seqExclamationMark.Restart();
        }
        private void PlayQuestionMark(int number)
        {
            if (_seqQuestionMark[number] == null)
            {
                _seqQuestionMark[number] = DOTween.Sequence()
                    .OnStart(() =>
                    {
                        _questionMark[number].color = new Color(1, 1, 1, 0);
                        _questionMark[number].GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
                    }).SetAutoKill(false)

                    .Append(_questionMark[number].DOFade(1f, 0.1f).From(0f))
                    .Join(_questionMark[number].transform.DOLocalRotate(new Vector3(0, 0, 20), 0.3f).SetEase(Ease.InQuad))

                    .Append(_questionMark[number].transform.DOLocalRotate(new Vector3(0, 0, -20), 0.3f).SetEase(Ease.InQuad))
                    .Append(_questionMark[number].transform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                    .Join(_questionMark[number].DOFade(0f, 0.5f).SetEase(Ease.Linear));
            }
            else
                _seqQuestionMark[number].Restart();
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