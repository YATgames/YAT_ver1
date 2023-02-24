using Assets.Scripts.Managers;
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

        public BoxCollider BoxCollider { get { return _boxCollider; } }
        [SerializeField] private BoxCollider _boxCollider;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public override void ResetTransform()
        {
            base.ResetTransform();
        }
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SoundManager.Instance.Play("ChaSound01_SFX");
                _animator.SetTrigger("Touch");
            }
        }
    }
}
