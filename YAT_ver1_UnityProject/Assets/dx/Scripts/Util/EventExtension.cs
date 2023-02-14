using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Util
{
    public class EventExtension
    {
        [Serializable]
        public class OnEventTrigger : UnityEvent { }

        [Serializable]
        public class OnEventTrigger<T> : UnityEvent<T> { } 
    }
}