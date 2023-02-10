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


        private void SetButton() // dx�� Splash_DEV ��ũ��Ʈ �κ��� �Լ��� �Ҵ��Ŵ
        {
            DependuncyInjection.Inject(this); // �� ������Ʈ �Ҵ��� ���� �ǹ��ϱ�?
            
            _loginButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (string.IsNullOrEmpty(_idField.text))
                    return;

                // PlayerPrefs �� ��ũ��Ʈ 
                _idField.text = "idField";
                Debug.Log("Splash ���� ���۹�ư ����");

            }).AddTo(gameObject);
            //ViewModel.Instance
                //.ObserveEveryValueChanged(v=> v)
        }
    }
}