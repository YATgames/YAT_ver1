using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Managers;
using UnityEditor.Rendering;
using Assets.Scripts.Common.Models;
using Assets.Scripts.ManageObject;
using Assets.Scripts.Common;
using Assets.Scripts.UI;

namespace Assets.Scripts.Splash
{
    public class SplashScene : MonoBehaviour
    {
        
        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;

        [SerializeField] private InputField _idField;
        [SerializeField] private Button _loginButton;
        private IEnumerator Start()
        {
            DependuncyInjection.Inject(this); // 이 오브젝트 할당이 무슨 의미일까?

            Debug.Log("Splash Start");
            Application.targetFrameRate = 60;
            Application.runInBackground = true;

            DOTween.Init();
            ManageObjectFacade.Initialize();
            SetButton();
            yield return new WaitForSeconds(1f);

        }
        private void ServiceSetting()
        {

        }


        private void SetButton() // dx의 Splash_DEV 스크립트 부분을 함수로 할당시킴
        {
            _idField.text = "DevLogin";
            _loginButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (string.IsNullOrEmpty(_idField.text))
                    return;
                //_soundManager.StopBGM();
                //_soundManager.Play("")
                
                _idField.text = "idField";
                Debug.Log("Splash 에서 시작버튼 누름");
                GameManager.Instance.LoadScene(SceneName.MainScene);
                SystemLoading.Show(SystemLoading.LoadingSize.Big, this); // 씬 객체에 생성되는 큰 로딩
                _flowManager.AddSubPopup(PopupStyle.Main, 1);
            }).AddTo(gameObject);
            //GameManager.Instance
        }
    }
}