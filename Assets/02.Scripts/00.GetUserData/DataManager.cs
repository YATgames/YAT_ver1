using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;


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
    //private TextAsset m_ta_path;

    public Text text_Gold;
    public Text text_Friendship;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;

        // TODO:  json 파일이 저장될 위치를 path 에 저장
        //path = Path.Combine(Application.persistentDataPath, "UserData.json"); // Android 타입
        path = Path.Combine(Application.dataPath, "Resources/Data", "UserData.json");
        Debug.Log("path : " + path);
        //ta_path = Resources.Load<TextAsset>("UserData");
        LoadJson();
    }

    public List<int> list_FriendShip = new List<int>();


    string jsonExample;
    void makeJson() // 만약 json 파일 제작부터 시작하면
    {

        jsonExample+= "[\n";

        int i = 0;
        for (i = 0; i < 10; i++)
        {
            
        }
        if (i == 10 - 1)
        {
            jsonExample += "\n";
        }
        else jsonExample += ",\n]";
    }
    public void LoadJson()
    {
        //var taPath = Resources.Load<TextAsset>("/UserData/UserData");
        //m_ta_path = taPath;
        if (!File.Exists(path)) // path에 지정된 파일이 없는경우
        {
            Debug.Log("<color=red>json파일 없음</color>");

            // TODO : 초기값
            DataManager.instance.p_gold = 0;
            DataManager.instance.p_friendShip = 0;
            //DataManager.instance.p_imtinancy = 20;
            //SaveJson(); // saveJson에서 새로운 파일 생성도 함.?
            return;
        }
        else
        {
            //SaveData saveData = new SaveData();
            Debug.Log("<color=aqua>json파일 있음</color>");
            // json 파일이 있다면 ReadAllText로 불러와 string 형식의 loadJson 에 담아두고,
            // JsonUtility 를 사용해 loadJson을 SaveData 형식의 직렬화 모델로 변환한다.
            
            
            // (new 사용하지 않는 방식으로 하고있음!) 저장에서 문제생기면 
            // new SaveData()를 할당하면서 되는 방식을 좀 더 찾아봐야함
            //saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            TextAsset loadjson = Resources.Load<TextAsset>("Data/UserData");
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadjson.ToString());
            if (saveData != null)
            {
                //   for (int i = 0; i < saveData.list_FriendShip.Count; i++)
                // DataManager.instance.list_FriendShip.Add(saveData.list_FriendShip[i]);

                p_gold = saveData.local_gold;
                p_friendShip = saveData.local_Friendship;
            }
            PrintInfo();
        }
        

    }

    public void SaveJson()
    {
        // new 할당을 외부에 하게되면 같은  Json 데이터가 계속 쌓이게 됨
        // json 파일이 없는 경우라면 새로 생성시킴
        SaveData saveData = new SaveData();
        Debug.Log("데이터 저장");
        // 저장할 데이터를 saveData로 보내 직렬화 시켜줌.
        //saveData.list_FriendShip.Add(1);

        saveData.local_Friendship += 200;
        saveData.local_gold += 200;
        
        // 직렬화된 saveData를 ToJson을 이용해서 stringJson에 저장
        // prettyPrint를 해야 줄내림이 적용됨.
        string json = JsonUtility.ToJson(saveData, true);
        // Json 파일로 저장
        File.WriteAllText(path, json);


/*        
        // 이 하단은 조건문 이후과정
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
        }*/
    }

    void PrintInfo()
    {
        text_Friendship.text = DataManager.instance.p_friendShip.ToString();
        text_Gold.text = DataManager.instance.p_gold.ToString();
    }


    // @@@@@가져와야하는 정보@@@@@
    // 스토리관련 : 스테이지 정보, 
    // 인게임관련 : 보유중인 아이템 리스트
    // 캐릭터관련 : 현재 레벨(경험치), 각 캐릭터별로의 친밀도, 우정도, 
    //  파티관련 : 개방한 파티 멤버

    // 스토리
    private int data_curStage; // 깬 스테이지 정보
    // 인게임


    // 캐릭터
    private int data_EXP; // 경험치 

    // 파티
    private List<int> data_curParty;


    // 친밀도, 우정도 기타등등 값 정의해줘야함
    private int data_Imtinancy; // 친밀도
    private int data_FriendShip; // 우정도

    // 재화
    private int data_Gold;


    public List<int> p_curParty
    {
        get {return data_curParty;}
        set // List 타입은 어떻게 프로퍼티를 적용시킬까?
        {
            data_curParty = value;
        }
    }
    /// <summary> 우정도 가져오기 </summary>
    public int p_friendShip
    {
        get { return data_FriendShip; }
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