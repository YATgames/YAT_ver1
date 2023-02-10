using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleManager : MonoBehaviour
{
    public Text LogText;
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        LogIn();

    }

    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success) LogText.text = Social.localUser.id + "\n" + Social.localUser.userName;
            else LogText.text = "구글 로그인 실패";
        });
    }

    public void LogOut()
    {
        //((PlayGamesPlatform)Social.Active).SignOut(); //현재 삭제됨

        LogText.text = "구글 로그아웃";
    }

    // Update is called once per frame
    void Update()
    {

    }
}