using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Mesu : BaseCharacter, Unit__Yat
{
    private BaseEnemy Target = null;

    private void Awake()
    {
        p_Anim = this.GetComponent<Animator>();
    }

    public void I_Attack()
    {
        Debug.Log(this.name + " 의 공격이다");
        base_MoveAttack();
        BattleManager.instance.BM_Attack(1);
    }

    public void I_Damaged()
    {
        Debug.Log(this.name + " 가 데미지를 받았다");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            I_Attack();
        }
    }
}