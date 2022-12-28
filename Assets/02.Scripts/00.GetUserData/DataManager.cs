using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
//[RequireComponent((typeof(SaveData)))]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private List<Dictionary<string, object>> json_userData;
    private SaveData _dataContainer;
    private string jsonName;

    private string data; // 내용이 저장되는 string 타입


    private string path;

    private void Awake()
    {
        if (instance != null) return;

        instance = this;
        
        // TODO:  path 관련해서 PC용(Resources.Load)작업 해야함
        path = Path.Combine(Application.dataPath, "UserData.json");
        LoadJson();
    }
    public List<int> list_FriendShip = new List<int>();
    public void LoadJson()
    {
        SaveData saveData = new SaveData();

        if (!File.Exists(path)) // path에 지정된 파일이 있을경우
        {
            Debug.Log("json파일 있음");
            DataManager.instance.p_gold = 100;
            DataManager.instance.p_friendShip = 100;
            DataManager.instance.p_imtinancy = 20;
        }
        else
        {
            Debug.Log("json파일 없음");
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                for (int i = 0; i < saveData.list_FriendShip.Count; i++)
                {
                    DataManager.instance.list_FriendShip.Add(saveData.list_FriendShip[i]);
                }
            }
        }
    }

    
    
    public void SaveJson(string _data)
    {
        try
        {
            if (_data.Equals("{}"))
            {
                Debug.Log("json이 없다?");
                return;
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("<color=red> 파일 예외 없음 </color>" + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("<color=red> 디렉토리 없음 </color>" + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("<color=red> I/O 예외 </color>" + e.Message);
        }
        finally
        {
            if (_dataContainer != null)
            {
                Debug.Log("먼가 마지막에 실행? or 꼭 실행?");
            }
            else
            {
                Debug.Log("데이터 콘테이너 비어있음");
            }
        }
    }
    
    
    
    
    
    
    
    
    
    
    // @@@@@가져와야하는 정보@@@@@
    // 스토리관련 : 스테이지 정보, 
    // 인게임관련 : 보유중인 아이템 리스트
    // 캐릭터관련 : 현재 레벨(경험치), 각 캐릭터별로의 친밀도, 우정도, 
    //  파티관련 : 개방한 파티 멤버

    // 스토리
    private int data_curStage; // 깬 스테이지 정보
    // 인게임
    // private in
    
    // 캐릭터
    private int data_EXP; // 경험치 
    
    // 파티
    private int[] data_curParty;
    
    
    // 친밀도, 우정도 기타등등 값 정의해줘야함
    private int data_Imtinancy; // 친밀도
    private int data_FriendShip; // 우정도
    
    // 재화
    private int data_Gold;
    /// <summary> 우정도 가져오기 </summary>
    public int p_friendShip
    {
        get {  return data_FriendShip; }
        set { data_FriendShip += value; }
    }
    /// <summary> 친밀도 가져오기 </summary>
    public int p_imtinancy
    {
        get { return data_Imtinancy; }
        set { data_Imtinancy += value; }
    }
    /// <summary> 골드 가져오기 </summary>
    public int p_gold
    {
        get { return data_Gold; }
        set { data_Gold += value; }
    }
}
