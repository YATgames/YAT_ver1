using Assets.Scripts.Common.Models;
using Assets.Scripts.UI.Util;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

namespace Assets.Scripts.UI.Item
{
    public class ItemCleanSystem : MonoBehaviour,IDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject _onObj;
        [SerializeField] private GameObject _offObj;

        [SerializeField] private RectTransform _case;
        [SerializeField] private BoxCollider _boxCollider;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_offObj) _offObj.SetActive(true);
            if (_onObj) _onObj.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_offObj) _offObj.SetActive(false);
            if (_onObj) _onObj.SetActive(true);

            Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
            _rectTransform.position = position;

            AddjustPos();

            //var boxResult = _boxCollider.size + _boxCollider.center;
            //var caseResult = Camera.main.WorldToScreenPoint(_case.position);

            //Debug.Log("X-" + (Camera.main.WorldToScreenPoint(_rectTransform.position).x - boxResult.x * 0.5f) + "X+" + (Camera.main.WorldToScreenPoint(_rectTransform.position).x + boxResult.x * 0.5f));
            //Debug.Log("Y-" + (Camera.main.WorldToScreenPoint(_rectTransform.position).y - boxResult.y * 0.5f) + "Y+" + (Camera.main.WorldToScreenPoint(_rectTransform.position).y + boxResult.y * 0.5f));

            //if ((Camera.main.WorldToScreenPoint(_rectTransform.position).x + boxResult.x * 0.5f) > 1500 
            //    || (Camera.main.WorldToScreenPoint(_rectTransform.position).x - boxResult.x * 0.5f) < 200) return;
            //if ((Camera.main.WorldToScreenPoint(_rectTransform.position).y + boxResult.y * 0.5f) > 1500
            //    || (Camera.main.WorldToScreenPoint(_rectTransform.position).y - boxResult.y * 0.5f) < 200) return;

            //Debug.Log("먼지털이 실행");
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

            _rectTransform.localPosition = Vector3.zero;
        }
    }
}
