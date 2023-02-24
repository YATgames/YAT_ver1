using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Util;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI.Item
{
    public class ItemCleanSystem : MonoBehaviour,IDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject _onObj;
        [SerializeField] private GameObject _offObj;

        [SerializeField] private RectTransform _CleanRange;

        [SerializeField] private ParticleSystem _dustParticle;
        [SerializeField] private ParticleSystem _dustCloudParticle;
        [SerializeField] private ParticleSystem _cleanStarParticle;

        [SerializeField] private Vector3 addJustLocalVector = new Vector3(282.4f, 317.4f, 0); // vector3.zero로부터 떨어진 position 값

        [SerializeField] private int _maxCleanCount = 100;

        private RectTransform _rectTransform;

        private int _cleanCount = 0;
        private bool _cleanComplete = false;
        private bool _firstTouch = false;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_offObj) _offObj.SetActive(true);
            if (_onObj) _onObj.SetActive(false);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if(!_firstTouch)
            {
                SoundManager.Instance.Play("Button_Touch");
                _firstTouch = true;
            }
            if (_cleanComplete) return;

            if (_offObj) _offObj.SetActive(false);
            if (_onObj) _onObj.SetActive(true);

            Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
            _rectTransform.position = position;

            AddjustPos();
            var boxResult = _rectTransform.localPosition + addJustLocalVector;
            var cleanRange = new Vector3(_CleanRange.sizeDelta.x, _CleanRange.sizeDelta.y, 0) + _CleanRange.localPosition;

            if ((boxResult.x < -cleanRange.x * 0.5f)
             || (boxResult.x > cleanRange.x * 0.5f)
             || (boxResult.y < -cleanRange.y * 0.5f)
             || (boxResult.y > cleanRange.y * 0.5f))
            {
                _dustParticle.Stop();
                _dustCloudParticle.Stop();
                return;
            };

            _cleanCount++;
            _dustParticle.Play();
            _dustCloudParticle.Play();
            if (!_timeCheck)
            {
                Debug.Log("먼지털이중");
                _timeCheck = true;
                SoundManager.Instance.Play("FastSwipe_SFX");
            }
            IEnumerator timeCheck = TimeCheckRoutine(_cleanCount);
            StartCoroutine(timeCheck);

            if (_cleanCount >= _maxCleanCount)
            {
                _cleanComplete = true;
                _cleanCount = 0;
                OnEndDrag(eventData);
                IEnumerator cleanComplete = CleanCompleteRoutine();
                StartCoroutine(cleanComplete);
            }
        }

        private bool _timeCheck = false;
        private IEnumerator TimeCheckRoutine(int cleanCount)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            if(cleanCount%10 == 0) _timeCheck = false;
            if (cleanCount == _cleanCount) _dustParticle.Stop();
        }

        private void AddjustPos()
        {
            var resetZ = _rectTransform.localPosition;
            resetZ.z = 0;
            _rectTransform.localPosition = resetZ;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_offObj) _offObj.SetActive(true);
            if (_onObj) _onObj.SetActive(false);

            _firstTouch = false;
            _cleanCount = 0;
            _rectTransform.localPosition = Vector3.zero;
        }

        private IEnumerator CleanCompleteRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            SoundManager.Instance.Play("ClearSwipe_SFX");
            _cleanStarParticle.Play();
            yield return new WaitForSeconds(1f);
            _cleanComplete = false;
        }
    }
}
