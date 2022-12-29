using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


// Json  사용을 위한 직렬화
[System.Serializable]
public class SaveData
{
    // 네이밍 타입 : SD = SaveData
    // DataManager의 데이터 컨테이너와 같은 구조를 가져야함
    public int local_Friendship;
    public int local_gold;

    /*
    public void GetData(int _gold, int _friendship)
    {
        local_gold = _gold;
        local_Friendship = _friendship;
    }*/
}