using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Common;
using GooglePlayGames;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Splash
{
    public class Splash_DEV : MonoBehaviour
	{
        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;
        [SerializeField] private InputField _idField;
		[SerializeField] private Button _loginButton;

        private void Start()
		{
            DependuncyInjection.Inject(this);
			_idField.text = PlayerPrefs.GetString("DEV_ID", string.Empty);

            _loginButton.OnClickAsObservable().Subscribe(_ =>
			{
				if (string.IsNullOrEmpty(_idField.text))
					return;

				_soundManager.StopBGM();
				_soundManager.Play("Button_Click");
                PlayerPrefs.SetString("DEV_ID", _idField.text);
				ConnectionManager.Instance.CustomLogin(_idField.text, _idField.text);
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
    }
}
