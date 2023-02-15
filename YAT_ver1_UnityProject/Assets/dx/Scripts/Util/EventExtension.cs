using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Util
{
    public class EventExtension
    {

        //  PopupSub에서 불러올때 사용하는듯? 
        public  static IObservable<PointerEventData> OnPointerDownAsObservable(Component component)
        {
            if (component == null || component.gameObject == null) 
                return Observable.Empty<PointerEventData>();

            var obseraver = component.gameObject.GetComponent<ObservablePointerDownTrigger>();

            if (obseraver == null)
                obseraver = component.gameObject.AddComponent<ObservablePointerDownTrigger>();

            return obseraver.OnPointerDownAsObservable();
        }


        [Serializable]
        public class OnEventTrigger : UnityEvent { }

        [Serializable]
        public class OnEventTrigger<T> : UnityEvent<T> { }


    }
}