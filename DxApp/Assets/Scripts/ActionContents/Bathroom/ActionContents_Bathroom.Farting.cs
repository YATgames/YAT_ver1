using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using PlayFab.GroupsModels;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Bathroom : MonoBehaviour
    {
        // [AcionContents-Bathroom-Farting]

        private IEnumerator FartingRoutine()
        {
            Debug.Log("방귀 엔딩부분");
            PlaySE(_seContents2[3]); // SE : 느낌표

            _exclamationMark_2.DOFade(1f, 0.1f);
            _exclamationMark_2.transform.DOLocalRotate(new Vector3(0, 0, 10), 0.1f).SetEase(Ease.InQuad);
            yield return new WaitForSeconds(0.1f);
            _exclamationMark_2.transform.DOLocalRotate(new Vector3(0, 0, -15), 0.1f).SetEase(Ease.InQuad);
            _exclamationMark_2.rectTransform.DOAnchorPosY(+10f, 0.1f).SetRelative(true).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.1f);

            _exclamationMark_2.rectTransform.DOAnchorPosY(-10f, 0.1f).SetRelative(true).SetEase(Ease.Linear);
            _exclamationMark_2.transform.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.1f);
            _exclamationMark_2.DOFade(0f, 0.1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.6f);


            PlaySE(_seContents1[4]);
            ModelingMotion(Anim.IDLE);
            yield return new WaitForSeconds(0.1f);
            ModelingMotion(Anim.SHUDDER); // 방방 뛰는 부분
            // SE : 파티 - 케익/음료 부분 효과
            yield return new WaitForSeconds(2f);
           
            _yellowBG.gameObject.SetActive(true);
            _yellowBG.DOFade(0.6f, 1f).From(0f).SetEase(Ease.Linear);
            ModelingMotion(Anim.IDLE);
            yield return new WaitForSeconds(0.1f);
            ModelingMotion(Anim.LIEFLAT);
            yield return new WaitForSeconds(0.55f);
            PlaySE(_seContents2[4]); // 쓰러짐
            yield return new WaitForSeconds(1f);

            CustomView.ContentsExit(1f,1.5f);
            yield return new WaitForSeconds(2f);
            CustomView.temmaImage.sprite = _bathroomOutsideImage; // 
            yield break;
        }

        private void Init_Farting()
        {
            ChangeCaseImage();

            _inputRange.gameObject.SetActive(true);
            _gasParents.gameObject.SetActive(true);
            _gasEffect.gameObject.SetActive(true);
            _exclamationMark_2.gameObject.SetActive(true);
            _yellowBG.gameObject.SetActive(false);

            _gasCreateRange = (int)_gas.rectTransform.rect.width >> 1;           

            _gasEffect.color = _alphaNone;
            _exclamationMark_2.color = _alphaNone;

            _inputRange.image.color = Color.clear;
            _inputRange.onClick.
               AsObservable().Subscribe(_ =>
               {
                   InputAction();
               });

            Pooling_Init();
            PlayBGM(_s02);
        }

        private void GasEffect()
        {
            // p03 이미지 랜덤 방향으로 퍼지도록

            _gasEffect.rectTransform.localEulerAngles = new Vector3(0, 0,
                Random.Range(0 ,360f));
            _gasEffect.rectTransform.localPosition = Vector3.zero;

            _gasEffect.DOFade(0f, 0.5f).From(1f).SetEase(Ease.Linear);
            _gasEffect.transform.DOLocalMoveY(50f, 0.3f).SetEase(Ease.InQuad);
        }

        /// <summary>
        /// 터치하기 : 방귀 생성
        /// </summary>
        private void InputAction()
        {
            if(_poolingObjectQueue.Count> 0)
            {
                ModelingMotion(Anim.LOWJUMP);
                GasEffect();
                Pooling_Active();
                RandomGasSound();
            }
            else // 25회 이상 클릭 진행시 발생 이벤트
            {
                _inputRange.image.raycastTarget = false;
                StartCoroutine(FartingRoutine());
            }
        }

        private void RandomGasSound()
        {
            
            int num = UnityEngine.Random.Range(0, 2);
            if (num == 0)
                PlaySE(_seContents2[0]);
            else 
                PlaySE(_seContents2[1]);
        }
        #region ### GasPooling ###
        [SerializeField] private Queue<Image> _poolingObjectQueue = new Queue<Image>();
        
        private void Pooling_Init() // 개수만큼 복사해서 생성
        {
            _poolingObjectQueue.Enqueue(_gas);
            _gas.gameObject.SetActive(false);
            for (int i = 0; i < _farCreateCount; i++)
            {
                CreateNewGas();
                //Image gasImage = Instantiate(_gas.gameObject, _gasParents).GetComponent<Image>();
                //_poolingObjectQueue.Enqueue(gasImage);
            }
        }

        private void CreateNewGas()
        {
            Image newItem = Instantiate(_gas,_gasParents).GetComponent<Image>();
            newItem.transform.position = Vector3.zero;
            newItem.color = _alphaNone;
            _poolingObjectQueue.Enqueue(newItem);
            newItem.gameObject.SetActive(false);
            //return newItem;
        }
        private Image Pooling_Active() // Get
        {
            if (_poolingObjectQueue.Count > 0)
            {
                var obj = _poolingObjectQueue.Dequeue();
                obj.gameObject.SetActive(true);
                obj.rectTransform.localPosition = new Vector2
                    (Random.Range(-_gasCreateRange, _gasCreateRange),
                      Random.Range(-_gasCreateRange, _gasCreateRange)
                    );
                obj.rectTransform.localEulerAngles = new Vector3
                    (0, 0, Random.Range(0, 360f));
                obj.DOFade(0.5f, 0.2f).SetEase(Ease.InQuad);

                return obj;
            }
            else
                return null;
        }
        private void Pooling_DisActive(Image obj) // Return
        {
            obj.gameObject.SetActive(false);
            _poolingObjectQueue.Enqueue(obj);
        }
        #endregion
    }
}