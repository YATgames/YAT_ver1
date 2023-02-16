using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.UI;
using System.Threading;
using System.Collections;
using Assets.Scripts.Common.Models;

namespace Assets.Scripts.Splash
{
    public class Splash_Dev : MonoBehaviour
    {
        // 이 _Dev 부분을 따로 뺴는 이유
        /*
         * SystemLoading.Show를 하는데 root.trasform으로 설정된단말임
         * 근데 이게 canvas단에서 되야하기때문에 추가적으로 스크립트 작업이 필요함
         */

        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;
        [DependuncyInjection(typeof(PopupManager))]
        private PopupManager _popupManager;

        [SerializeField] private InputField _idField;
        [SerializeField] private Button _loginButton;

        private void Start()
        {
            DependuncyInjection.Inject(this);
            _idField.text = "DevLogin";
            _loginButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (string.IsNullOrEmpty(_idField.text))
                    return;
                //_soundManager.StopBGM();
                //_soundManager.Play("")

                _idField.text = "idField";
                Debug.Log("Splash 에서 시작버튼 누름");
                SystemLoading.Show(SystemLoading.LoadingSize.Big, this); // 씬 객체에 생성되는 큰 로딩
            }).AddTo(gameObject);

            //  PlayerViewModelModel 에서 인스턴스 변경이 있으면
            GameManager.Instance.LoadScene(SceneName.MainScene);
            SystemLoading.Hide(this);
        }
        private void SetButton() // dx의 Splash_DEV 스크립트 부분을 함수로 할당시킴
        {

        }
    }
}