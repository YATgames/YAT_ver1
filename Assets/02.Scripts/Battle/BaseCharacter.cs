using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YAT
{
    public class BaseCharacter : MonoBehaviour
    {
        private Animator base_animator;
        private GameObject Target;
        
        private BoxCollider2D collider;
        //protected bool canTargeting;
        /// <summary>
        /// 타겟 설정 과정
        /// </summary>
        public GameObject proper_Target
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

        protected bool collider_Enable // 캐릭터 범위지정 활성화 여부
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
        ///  적 파악
        /// </summary>
        /// <param name="other"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Monster"))
            {
                Debug.Log(this.name + " : 나 얏츄륾 만났어!");
            }
            //throw new NotImplementedException();
        }
        
        /// <summary>
        /// 적 파악이 끝나서 적 설정
        /// </summary>
        protected void SetTarget()
        {
            
        }

        protected void Attack()
        {
            base_animator.SetTrigger("Attack_1");
            // 1. 기본
            // 2. 스킬1 
            // 3. 스킬2
            // 4. 방어? 
        }
    }
}