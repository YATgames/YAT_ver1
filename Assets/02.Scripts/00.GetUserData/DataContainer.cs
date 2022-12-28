using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


// Json  사용을 위한 직렬화
[System.Serializable]
public class SaveData
{
    //public List<string> data_A = new List<string>();
    public List<int> list_FriendShip = new List<int>();
    public int gold;
}
