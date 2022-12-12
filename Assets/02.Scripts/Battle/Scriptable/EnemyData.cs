using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData",menuName = "YAT/Enemy/Data")]
public class EnemyData : EnemyBase
{
    public string data_name;
    public string data_description;
    public GameObject data_model;
    public int data_hp;
    public int data_attackDamage;
    
    
}
