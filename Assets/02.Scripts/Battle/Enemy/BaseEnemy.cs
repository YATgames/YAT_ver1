using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YAT
{
    public class BaseEnemy : MonoBehaviour
    {
        /// <summary>
        /// 이거 Resources.Load 로 받아오기 가능?
        /// 아 적들을 프리팹으로 할당시켜서 하면 상관없구나.
        /// </summary>
        public EnemyData enemyData;

        public int Get_enemyData()
        {
            return enemyData.data_hp;
        }
        
        
    }
}