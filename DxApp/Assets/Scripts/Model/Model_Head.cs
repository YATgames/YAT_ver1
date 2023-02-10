using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.PrefabModel
{
    public class Model_Head : ModelPrefabBase
    {
        public Transform DecoPosition { get { return _decoPosition; } }
        [SerializeField] private Transform _decoPosition;

        public override void ResetTransform()
        {
            base.ResetTransform();
        }
    }
}