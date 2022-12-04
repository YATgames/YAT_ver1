using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YAT
{
    public class Character_Mesu : BaseCharacter, Unit
    {
        private Animator animator;

        private void Awake()
        {
            animator = this.GetComponent<Animator>();
        }

        public void Iattack()
        {
            Debug.Log(this.name+ " 의 공격이다");
        } 
        public void Idamaged()
        {
            Debug.Log(this.name+ " 가 데미지를 받았다");
        }
    }
}