using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.ManageObject;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
	public class GameManager : UnitySingleton<GameManager>
	{
		[DependuncyInjection(typeof(SoundManager))]
		private SoundManager _soundManager;

		public ReactiveProperty<SystemLanguage> CurrentLanguage = new ReactiveProperty<SystemLanguage>();
		public ReactiveProperty<SceneName> CurrentScene = new ReactiveProperty<SceneName>();
		
		public ReactiveProperty<float> Sound = new ReactiveProperty<float>();
		public ReactiveProperty<bool> PushAlram = new ReactiveProperty<bool>();

		private const string _saveSoundCode = "Sound";
		private const string _savePushAlramCode = "PushAlram";

		public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);
			AddEvent();
			Sound.Value = PlayerPrefs.GetFloat(_saveSoundCode, 1f);
			PushAlram.Value = PlayerPrefs.GetInt(_savePushAlramCode, 1) == 1 ? true : false;
		}

		private void AddEvent()
		{
			CurrentScene.ObserveEveryValueChanged(v => v.Value).Subscribe(v =>
			{
				switch (v)
				{
					case SceneName.MainScene:
						FlowManager.Instance.Change(PopupStyle.Lobby);
						break;
				}
			}).AddTo(gameObject);

			Sound.ObserveEveryValueChanged(v => v.Value).Subscribe(_soundManager.SetVolume).AddTo(gameObject);
		}

		public void SetLanguage(SystemLanguage language)
		{
			StringTable.SetLanguage(language);
			CurrentLanguage.Value = StringTable.CurrentLanguage;
		}

		public void LoadScene(SceneName name)
		{
			var loadScene = SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Single);
			loadScene.ObserveEveryValueChanged(v => v.isDone).Where(v => v).Take(1).Subscribe(_ =>
			{
				CurrentScene.Value = name;
			});
		}

		public void SetSound(float volum)
		{
			Sound.Value = volum;
			PlayerPrefs.SetFloat(_saveSoundCode, volum);
		}

		public void SetPushAlram(bool isOn)
		{
			PushAlram.Value = isOn;
			PlayerPrefs.SetInt(_savePushAlramCode, isOn ? 1 : 0);
			LogManager.Log("GameManager :: SetPushAlram :: IsOn == {0}", isOn);
		}
	}
}
