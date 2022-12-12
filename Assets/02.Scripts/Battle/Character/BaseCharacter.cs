using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace YAT
{
    public class BaseCharacter : MonoBehaviour
    {
        private Animator base_animator;
        private BaseEnemy Target; // MoveAttack 시 이동하게 될 타겟의 위치
        private Vector2 savePos; // MoveAttack 후 다시 이동하게 될 위치 저장

        private float duration = 0.15f;
        private BoxCollider2D collider;

        public Animator proper_Anim
        {
            get { return base_animator; }
            set
            {
                if (base_animator == null)
                {
                    base_animator = value;
                }
            }
        }

        //protected bool canTargeting;
        /// <summary>
        /// 타겟 설정 과정
        /// </summary>
        public BaseEnemy proper_Target
        {
            get { return Target; }
            set
            {
                if (Target == null)
                {
                    Target = value;
                }
            }
        }

        public bool collider_Enable // 캐릭터 범위지정 활성화 여부
        {
            get
            {
                return collider.enabled;
            }
            set
            {
                if (collider.enabled != value)
                {
                    collider.enabled = value;
                }
            }
        }
        /// <summary>
        /// 적 파악이 끝나서 적 설정
        /// </summary>
        protected void SetTarget()
        {
            
        }

        
        public void base_MoveAttack()
        {
            if (Target == null)
            {
                Debug.Log("동작하면 안됨");
            }
            else
            {
                Debug.Log("공격성공");
                savePos = this.transform.position;
                //this.transform.position = Target.gameObject.transform.position;
                this.transform.DOMove(Target.gameObject.transform.position, duration)
                    .SetEase(Ease.InQuad);
                base_animator.SetTrigger("Attack_1");
                // 1. 기본
                // 2. 스킬1 
                // 3. 스킬2
                // 4. 방어?    
            }
             
        }

        /// <summary>
        /// 애니메이션 끝날 때 동작
        /// </summary>
        public void base_animation_AttackEnd()
        {
            //this.transform.position = savePos;
            this.transform.DOMove(savePos, duration).SetEase(Ease.InQuad);
        }

        public void base_animation_()
        {
            
        }

        /*
        /// <summary>
        ///  적 파악
        /// </summary>
        /// <param name="other"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log(this.name + " : 나 얏츄륾 만났어!");
            }
            //throw new NotImplementedException();
        }
        */

    }
}