using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //private Scene currentScene
    private Image img_Black;
    private int prevScene;
    private int curScene;

    /// <summary>
    /// Scene Changer를 잘 활용하기 위해서 NT 타입으로 만들었음.
    /// </summary>
    enum SceneNumber
    {
        MAIN, // 메인 화면 0 
        SELECT, // 선택지 화면 1
        STORY, // 스토리 화면 2
        BATTLE // 전투 화면 3
    }

    SceneNumber sceneNumber = SceneNumber.MAIN;

    private static SceneChanger _instance;

    public static SceneChanger instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }
    }

    private Button btn_Back;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            uiObject();
            DontDestroyOnLoad(this);
            // TODO : Resources 폴더에서 각 활동 오브젝트 할당 해줘야함
        }
        else
        {
            Debug.Log("<color=yellow> SceneManager가 이미 할당되어있음 </color>");
        }
    }


    // UI 오브젝트들 할당과 초기값 설정시킴
    void uiObject()
    {
        // 일단 GetChild형식으로 받는데, 이름으로 받는게 더 좋으려나?
        img_Black = this.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        btn_Back = this.transform.GetChild(0).GetChild(1).GetComponent<Button>();


        btn_Back.onClick.AddListener(btnFunc_Back);
        btn_Back.gameObject.SetActive(false);
        img_Black.raycastTarget = true;
    }

    void btnFunc_Back() // 뒤로가기 이벤트 함수
    {
        if (curScene != null)
        {
            SceneManager.LoadScene(prevScene, LoadSceneMode.Single);
            Debug.Log("뒤로가기 간다잉");
            //curScene = prevScene;

            switch (sceneNumber)
            {
                case SceneNumber.SELECT: // 1
                    btn_Back.gameObject.SetActive(false);
                    sceneNumber = SceneNumber.MAIN;
                    break;

                case SceneNumber.STORY: // 2
                    sceneNumber = SceneNumber.SELECT;
                    break;
            }
        }
        else
        {
            Debug.Log("curScene 이 없다");
        }
    }

    public void SetCurScene()
    {
    }

    private float duration = 0.5f;

    IEnumerator SceneChangeRoutine() // 화면 전환시 암전효과
    {
        img_Black.gameObject.SetActive(true);
        img_Black.DOFade(1f, duration).From(0f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        img_Black.DOFade(0f, duration).From(0f).SetEase(Ease.Linear)
            .OnComplete(() => { img_Black.gameObject.SetActive(false); });
    }

    /// <summary>
    /// 셀렉트 씬으로 가는 함수
    /// </summary>
    public void changer_Select() // 게임시작 버튼이라고 봐도 무방함
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        btn_Back.gameObject.SetActive(true);
        sceneNumber = SceneNumber.SELECT;
    }

    /// <summary>
    /// SocialButton Event Function
    /// </summary>
    public void changer_Social()
    {
        //SceneManager.LoadScene("TEST_Social", LoadSceneMode.Additive);
        // Additive로 해야 기존 씬 다시 로드 안하고 다시 할 수 있음
    }

    /// <summary>
    /// Business Event Function
    /// </summary>
    public void changer_Business()
    {
        //SceneManager.LoadScene("TEST_Business", LoadSceneMode.Additive);
    }

    public void changer_Story()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}