using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecoType
{
    OverHead,
    Wing,
    Tail,
}

namespace Assets.Scripts.PrefabModel
{
    public class Model_Deco : ModelPrefabBase
    {
        public DecoType DecoType { get { return _decoType; }}
        [SerializeField] private DecoType _decoType;

        public override void ResetTransform()
        {
            base.ResetTransform();
        }
    }
}