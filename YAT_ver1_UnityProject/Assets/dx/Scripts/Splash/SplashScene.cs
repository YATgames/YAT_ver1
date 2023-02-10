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

namespace Assets.Scripts.Splash
{
    public class SplashScene : MonoBehaviour
    {
        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;

        [SerializeField] private InputField _idField;
        [SerializeField] private Button _loginButton;
        private IEnumerator Start()
        {
            Application.targetFrameRate = 60;
            Application.runInBackground = true;

            DOTween.Init();
            SetButton();
            yield return new WaitForSeconds(1f);

        }
        private void ServiceSetting()
        {

        }


        private void SetButton() // dx의 Splash_DEV 스크립트 부분을 함수로 할당시킴
        {
            DependuncyInjection.Inject(this); // 이 오브젝트 할당이 무슨 의미일까?
            
            _loginButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (string.IsNullOrEmpty(_idField.text))
                    return;

                // PlayerPrefs 쪽 스크립트 
                _idField.text = "idField";
                Debug.Log("Splash 에서 시작버튼 누름");

            }).AddTo(gameObject);
            //ViewModel.Instance
                //.ObserveEveryValueChanged(v=> v)
        }
    }
}