using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Bathroom : MonoBehaviour
    {
        // [AcionContents-Bathroom-Wastpaper]
        private void Init_Wastpaper()
        {
            _leftHand = _bathroomImage.transform.GetChild(0).GetComponent<Button>();
            _rightHand = _bathroomImage.transform.GetChild(1).GetComponent<Button>();
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
            _bathroomImage.gameObject.SetActive(true);
            _blackBG_3.gameObject.SetActive(true);
            _inputRangeHappyEnd.gameObject.SetActive(true);
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
            _inputRangeHappyEnd.gameObject.SetActive(false);


            SetRayCastTarget(false);

            _leftHand.onClick.AsObservable().Subscribe(_ => ClickTissue(_redTissue, _blueTissue));
            _rightHand.onClick.AsObservable().Subscribe(_ => ClickTissue(_blueTissue, _redTissue));

            _inputRangeHappyEnd.onClick.AsObservable().Subscribe(_ => HeartClick());

            WastePaperRoutine().ToObservable()
                .Subscribe(_ => Debug.Log("휴지 시작"));
            PlayBGM(_s03);
        }

        private IEnumerator WastePaperRoutine()
        {
            _leftHand.image.DOFade(1f, 1.2f).SetEase(Ease.Linear);
            _leftHand.image.rectTransform.DOLocalMove(_leftMovePos, 1f).From(_leftZeroPos)
                .SetEase(Ease.Linear);
            _redTissue.DOFade(1f, 1f);
            DialoguePlay(0);
            PlaySE(_seContents1[1]);
            yield return new WaitForSeconds(2f);
            _rightHand.image.DOFade(1f, 1.2f).SetEase(Ease.Linear);
            _rightHand.image.rectTransform.DOLocalMove(_rightMovePos, 1f).From(_rightZeroPos)
                .SetEase(Ease.Linear);
            _blueTissue.DOFade(1f, 1f);
            DialoguePlay(1);
            PlaySE(_seContents1[1]);
            yield return new WaitForSeconds(1f);
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
            if (chooseTissue == _blueTissue)
                _xValue = -_xValue;

            chooseTissue.rectTransform.DOAnchorPos(new Vector2(_xValue, 100), 1f).SetEase(Ease.Linear)
             .OnComplete(() =>
             {
                 _redTissue.DOFade(0f, 0.5f).SetEase(Ease.Linear);
                 _blueTissue.DOFade(0f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                 {
                     _leftHand.image.rectTransform.DOAnchorPos(_leftZeroPos, 1f).SetEase(Ease.Linear);
                     _leftHand.image.DOFade(0f, 1f).SetEase(Ease.Linear);
                     _rightHand.image.rectTransform.DOAnchorPos(_rightZeroPos, 1f).SetEase(Ease.Linear);
                     _rightHand.image.DOFade(0f, 1f).SetEase(Ease.Linear);

                     //int ranNum = 0;
                     int ranNum = UnityEngine.Random.Range(0, 2);
                     if (ranNum == 0)
                         HeartEventRoutine().ToObservable().Subscribe();
                     else
                         ScaryEventRoutine().ToObservable().Subscribe();
                 });
             });
        }
        // Event1
        private IEnumerator HeartEventRoutine()
        {
            yield return new WaitForSeconds(1f); // 손 사라지기까지 조금 기다리기

            _fxFlares.Play();
            PlaySE(_seContents3[0]);
            _inputRangeHappyEnd.gameObject.SetActive(true);
            DialoguePlay(2);

            _fxLight.SetActive(true);
            CustomView.ContentsExit(3.5f, 1.5f);
            yield break;
        }
        
        // Event2
        private IEnumerator ScaryEventRoutine()
        {
            yield return new WaitForSeconds(1f); // 손 사라지기까지 조금 기다리기ㄴ
            _blackBG_3.DOFade(1f, 2f).SetEase(Ease.Linear);
            PlaySE(_seContents3[1]); // 괴물 크아악
            yield return new WaitForSeconds(2f);
            PlaySE(_seContents1[4]);
            ModelingMotion(Anim.SHUDDER);
            yield return new WaitForSeconds(1.5f);
            _ghostFullscreen_3.DOFade(1f, 0.1f);
            _ghostFullscreen_3.rectTransform.DOAnchorPosY(0, 0.1f)
                .OnComplete(() =>
                {
                    PlaySE(_seContents1[3]);
                    _ghostFullscreen_3.rectTransform.DOScale(1.4f, 0.15f).SetEase(Ease.OutBack);
                    _ghostFullscreen_3.rectTransform.DOShakeAnchorPos(1.2f, 100, 30, 90, false, false);
                });

            yield return new WaitForSeconds(1f);
            CustomView.ContentsExit(1f, 2f);
            yield break;

        }
        private void HeartClick()
        {
            if(_heartTouchCount >= 25)
            {
                _inputRangeHappyEnd.image.raycastTarget = false;
            }
            else
            {
                PlaySE(_seContents2[2]);
                _fxHeartTouch.Play();
            }
            
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