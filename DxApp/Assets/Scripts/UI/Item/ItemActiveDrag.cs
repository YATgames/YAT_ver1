using Assets.Scripts.UI.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Item
{
	public class ItemActiveDrag : DragItem<ItemActiveDrag>
	{
		[SerializeField] private GameObject _onObj;
		[SerializeField] private GameObject _offObj;

		protected override void Start()
		{
			base.Start();
			_this = this;
			if (_offObj) _offObj.SetActive(true);
			if (_onObj) _onObj.SetActive(false);
		}

		protected override void PointerDown(PointerEventData eventData)
		{
			if (_offObj) _offObj.SetActive(false);
			if (_onObj) _onObj.SetActive(true);
			base.PointerDown(eventData);
        }

        protected override void PointerUp(PointerEventData eventData)
		{
			if (_offObj) _offObj.SetActive(true);
			if (_onObj) _onObj.SetActive(false);
			base.PointerUp(eventData);
        }

        protected override void EndDrag(PointerEventData eventData)
		{
			if (_offObj) _offObj.SetActive(true);
			if (_onObj) _onObj.SetActive(false);
			base.EndDrag(eventData);
        }
    }
}
