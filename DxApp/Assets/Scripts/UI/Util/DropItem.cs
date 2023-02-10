using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Util
{
	public class DropItem : MonoBehaviour, IDropHandler
	{
		public OnEventTrigger OnDrop = new OnEventTrigger();

		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			OnDrop?.Invoke();
		}
	}
}
