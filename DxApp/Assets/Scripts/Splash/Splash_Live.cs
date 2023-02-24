using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Common;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Splash
{
    public class Splash_Live : MonoBehaviour
    {
        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;
        [SerializeField] private Button _loginButton;
        [SerializeField] private Text _loginFailText;
        [SerializeField] private Text _pleaseTouchText;
         private string _idField;

        private void Start()
        {
            DependuncyInjection.Inject(this);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            LogIn();

            _loginButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (string.IsNullOrEmpty(_idField))
                {
                    return;
                }

                _soundManager.StopBGM();
                _soundManager.Play("Button_Click");
                ConnectionManager.Instance.CustomLogin(_idField, Social.localUser.userName);
                SystemLoading.Show(SystemLoading.LoadingSize.Big, GameManager.Instance);

            }).AddTo(gameObject);

            PlayerViewModel.Instance
                .ObserveEveryValueChanged(v => v.Player)
                .Where(v => v != null)
                .Subscribe(player =>
                {
                    GameManager.Instance.LoadScene(SceneName.MainScene);
                }).AddTo(gameObject);
        }

        public void LogIn()
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    _idField = "FigureX_" + Social.localUser.id;
                    _pleaseTouchText.gameObject.SetActive(true);
                }
                else
                {
                    _idField = string.Empty;
                    _loginFailText.gameObject.SetActive(true);
                }
            });
        }
    }
}
