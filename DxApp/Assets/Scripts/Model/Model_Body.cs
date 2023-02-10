using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PrefabModel
{
    public class Model_Body : ModelPrefabBase
    {
        public Transform HeadPosition { get { return _headPosition; }}
        [SerializeField] private Transform _headPosition;

        public Transform WingPos { get { return _wingPos; } }
        [SerializeField] private Transform _wingPos;

        public Transform TailPos { get { return _tailPos; } }
        [SerializeField] private Transform _tailPos;
        public override void ResetTransform()
        {
            base.ResetTransform();
        }
    }
}
