using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Bathroom : MonoBehaviour
    {
        // [AcionContents-Bathroom-Wastpaper]
        private void Init_Wastpaper()
        {
            ChangeCaseImage(Side.Inside);
            _redTissue = _leftHand.transform.GetChild(0).GetComponent<Image>();
            _blueTissue = _rightHand.transform.GetChild(0).GetComponent<Image>();

            _dialogueImage = new Image[3];
            _dialogueText = new Text[3];
            for (int i = 0; i < 3; i++)
            {
                _dialogueImage[i] = _dialogueObject.GetChild(i).GetComponent<Image>();
                _dialogueImage[i].color = _alphaNone;
                _dialogueText[i] = _dialogueObject.GetChild(i).GetChild(0).GetComponent<Text>();
                _dialogueText[i].text = "";
            }

            _dialogueObject.gameObject.SetActive(true);
            _dialogueObject.gameObject.SetActive(true);
            _shudderEffect_3.gameObject.SetActive(false);
            _blackBG_3.gameObject.SetActive(true);
            _ghostFullscreen_3.gameObject.SetActive(true);

            _dialogueText[0].color = Color.red;
            _dialogueText[1].color = Color.red;
            _dialogueText[2].color = Color.green;

            _redTissue.color = _alphaNone;
            _blueTissue.color = _alphaNone;

            _leftHand.image.color = _alphaNone;
            _rightHand.image.color = _alphaNone;
            _blackBG_3.color = Color.clear;
            _ghostFullscreen_3.color = _alphaNone;

            _ghostFullscreen_3.rectTransform.anchoredPosition = new Vector3(0, -Screen.height, -300f);

            SetRayCastTarget(false);

            _leftHand.onClick.AsObservable().Subscribe(_ => ClickTissue(_redTissue, _blueTissue));
            _rightHand.onClick.AsObservable().Subscribe(_ => ClickTissue(_blueTissue, _redTissue));


            StartCoroutine(WastePaperRoutine());
            //PlayBGM(_s03);
            SoundManager.Instance.PlayBGM("Bathroom3_BGM");
        }

        private IEnumerator WastePaperRoutine()
        {
            yield return new WaitForSeconds(1f);
            _leftHand.image.DOFade(1f, 1.2f).SetEase(Ease.Linear);
            //leftHand.image.rectTransform.DOLocalMove(_leftMovePos, 1f).From(_leftZeroPos)
                //.SetEase(Ease.Linear);
            _redTissue.DOFade(1f, 1f);
            yield return new WaitForSeconds(0.2f);
            DialoguePlay(0);
            //PlaySE(_seContents1[1]);
            SoundManager.Instance.Play("Bathroom1E02_SFX");
            yield return new WaitForSeconds(2.5f);

            _rightHand.image.DOFade(1f, 1.2f).SetEase(Ease.Linear);
            //_rightHand.image.rectTransform.DOLocalMove(_rightMovePos, 1f).From(_rightZeroPos)
                //.SetEase(Ease.Linear);
            _blueTissue.DOFade(1f, 1f);
            yield return new WaitForSeconds(0.2f);
            DialoguePlay(1);
            //PlaySE(_seContents1[1]);
            SoundManager.Instance.Play("Bathroom1E02_SFX");
            yield return new WaitForSeconds(1.2f);
            SetRayCastTarget(true);
        }
        private void SetRayCastTarget(bool isActive)
        {
            _leftHand.image.raycastTarget = isActive;
            _rightHand.image.raycastTarget = isActive;
        }
        private void ClickTissue(Image chooseTissue, Image disChooseTissue)
        {
            SetRayCastTarget(false);
            // 휴지 이동과 동시에 손 사라지기
            /*
            if (chooseTissue == _blueTissue)
                _xValue = -_xValue;*/

            // 팀장님 피드백 - 선택한 휴지 떨리면서 엔딩 진행되게하기
            chooseTissue.transform.DOShakePosition(3.0f, 8f, 20, 90f, false, false).SetDelay(1f);
            _leftHand.image.DOFade(0f, 1f).SetEase(Ease.Linear);
            _rightHand.image.DOFade(0f, 1f).SetEase(Ease.Linear);
            disChooseTissue.DOFade(0f, 1f).SetEase(Ease.Linear)
                //_leftHand.image.rectTransform.DOAnchorPos(_leftZeroPos, 1f).SetEase(Ease.Linear);
                //_rightHand.image.rectTransform.DOAnchorPos(_rightZeroPos, 1f).SetEase(Ease.Linear);
                .OnComplete(() =>
                {
                    int ranNum = UnityEngine.Random.Range(0, 2);
                    if (ranNum == 0)
                        HeartEventRoutine(chooseTissue).ToObservable().Subscribe();
                    else
                        ScaryEventRoutine(chooseTissue).ToObservable().Subscribe();
                });    
        }

        // Event1
        private IEnumerator HeartEventRoutine(Image image)
        {
            
            yield return new WaitForSeconds(1f); // 손 사라지기까지 조금 기다리기
            image.DOFade(0f, 0.4f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
            _fxFlares.Play();
            //PlaySE(_seContents3[0]);
            SoundManager.Instance.Play("Bathroom3E01_SFX");
            DialoguePlay(2);

            _fxLight.SetActive(true);
            SoundManager.Instance.StopBGM();
            yield return new WaitForSeconds(2f);
            CustomView.ContentsExit(1f, 1.5f);
            yield return new WaitForSeconds(1.7f);
            ChangeCaseImage(Side.Outside);
            yield break;
        }
        
        // Event2
        private IEnumerator ScaryEventRoutine(Image image)
        {
            yield return new WaitForSeconds(1f); // 손 사라지기까지 조금 기다리기
            image.DOFade(0f, 0.4f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);

            _blackBG_3.DOFade(1f, 2f).SetEase(Ease.Linear);
            //PlaySE(_seContents3[1]); // 괴물 크아악
            SoundManager.Instance.Play("Bathroom3E02_SFX");
            yield return new WaitForSeconds(2f);
            //PlaySE(_seContents1[4]);
            SoundManager.Instance.Play("Bathroom1E05_SFX");
            ModelingMotion(Anim.SHUDDER);
            _fxShudder.Play();
            _shudderEffect_3.SetActive(true);
            
            yield return new WaitForSeconds(1.5f);
            _fxShudder.Stop();
            _shudderEffect_3.SetActive(false);
            _ghostFullscreen_3.DOFade(1f, 0.1f);
            _ghostFullscreen_3.rectTransform.DOAnchorPosY(0, 0.1f)
                .OnComplete(() =>
                {
                    //PlaySE(_seContents1[3]);
                    SoundManager.Instance.Play("Bathroom1E04_SFX");
                    _ghostFullscreen_3.rectTransform.DOScale(1.4f, 0.15f).SetEase(Ease.OutBack);
                    _ghostFullscreen_3.rectTransform.DOShakeAnchorPos(1.2f, 100, 30, 90, false, false);
                });

            yield return new WaitForSeconds(1f);
            SoundManager.Instance.StopBGM();
            CustomView.ContentsExit(1f, 2f);
            yield return new WaitForSeconds(2.2f);
            ChangeCaseImage(Side.Outside);
            yield break;

        }
        #region ### Dialogue
        private void DialoguePlay(int number)
        {
            if (number == 2) // 파랑 휴지파트
            {
                //_dialogueImage.sprite = _dialogueSprite[1];
            }
            _dialogueImage[number].DOFade(1f, 0.2f).SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    _dialogueImage[number].rectTransform.DOAnchorPosY(100, 0.2f).SetRelative(true).SetEase(Ease.OutBack);
                });
            DialogueRoutine(number)
                .ToObservable()
                .Subscribe();
        }

        string _curDialogue;
        int _curNum; // 현재 대사 보여지는 번호
        int _curDiaLength; // 현재 대사 길이
        private IEnumerator DialogueRoutine(int number)
        {
            int count = 0;
            _curDialogue = "";
            _curDiaLength = _dialogue[number].Length;
            
            //Debug.Log("다이아로그 시작! curdialenght : " + _curDiaLength);

            if(number == 2)  // 으 시원하당
            {
                _dialogueText[number].color = Color.black;
                while (count <= _curDiaLength)
                {
                    _curDialogue = _dialogue[number].Substring(0, count);
                    _dialogueText[number].text = _curDialogue;
                    count++;
                    yield return perSec;
                }
            }
            else // 공포대사
            {
                while (count <= _curDiaLength)
                {
                    _curDialogue = _dialogue[number].Substring(0, count);
                    _dialogueText[number].text = _curDialogue;
                    count++;
                    if (count <= _curDiaLength - 3)
                        yield return perSec;
                    else // ... 부분
                        yield return perSec_2;
                }
            }

            yield return new WaitForSeconds(0.6f);
            CloseButton(number);
        }

        private void CloseButton(int number)
        {
            _dialogueImage[number].DOFade(0f, 0.2f).SetEase(Ease.Linear);
            _dialogueText[number].DOFade(0f, 0.2f).SetEase(Ease.Linear);
        }
        #endregion
    }
}