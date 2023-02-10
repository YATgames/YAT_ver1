using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PrefabModel
{
    public class ModelPrefabBase : MonoBehaviour
    {

        public Vector3 OriginPos { get { return _originPos; } }
        [SerializeField] protected Vector3 _originPos = Vector3.zero;

        public Vector3 OriginScale { get { return _originScale; } }
        [SerializeField] protected Vector3 _originScale = Vector3.one;

        public Transform Parent { get; set; }

        public virtual void ResetTransform()
        {
            transform.localPosition = _originPos;
            transform.localScale = _originScale;
        }
    }
}
