using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using Assets.Scripts.UI.Item;
using DXApp_AppData.Item;

namespace Assets.Scripts.UI.Popup.PopupView
{
	public class SettingsView : MonoBehaviour
	{
		public GameManager GameManager { get; set; }

		[SerializeField] private string _qnaMailAddress;

		[SerializeField] private Slider _soundSlider;
		[SerializeField] private Toggle _alarmOnToggle;
		[SerializeField] private Toggle _alarmOffToggle;
		[SerializeField] private Button _mailButton;
		[SerializeField] private Button _licenseButton;

		[SerializeField] private Text _nickNameText;
		[SerializeField] private Text _rankingText;
		[SerializeField] private Text _figureCountText;

		[SerializeField] private ItemDisplayInven_Block _favoriteFigure;

		private string _keepNickName;

		private void Start()
		{
			LogManager.Log(GameManager.PushAlram.Value.ToString());
			if(GameManager.PushAlram.Value)
				_alarmOnToggle.isOn = true;
			else
				_alarmOffToggle.isOn = true;

			_soundSlider.value = GameManager.Sound.Value;

			AddEvent();
		}

		private void AddEvent()
		{
			_alarmOnToggle.OnValueChangedAsObservable().Skip(1).Where(v => v).Subscribe(_ =>
			{
				GameManager.SetPushAlram(true);
			}).AddTo(gameObject);

			_alarmOffToggle.OnValueChangedAsObservable().Skip(1).Where(v => v).Subscribe(_ =>
			{
				GameManager.SetPushAlram(false);
			}).AddTo(gameObject);

			_soundSlider.OnValueChangedAsObservable().Skip(1).Subscribe(value =>
			{
				GameManager.SetSound(value);
			}).AddTo(gameObject);

			_mailButton.OnClickAsObservable().Subscribe(_ =>
			{
				MailToQnA();
			}).AddTo(gameObject);
		}

		public void SetEmptyFavoriteFigure()
		{
			_favoriteFigure.SetEmpty();
		}

		public void SetFavoriteFigure(CustomFigureInstance item)
		{
			_favoriteFigure.SetData(item,false);
		}

		public void SetFavoriteFigure(OriginFigureInstance item)
		{
			_favoriteFigure.SetData(item,false);
		}

		public void SetNickName(string nickName)
		{
			_nickNameText.text = nickName;
		}

		/// <summary>
		/// 나중에 추가..
		/// </summary>
		public void SetRanking()
		{
			_rankingText.text = "-";
		}

		public void SetFigureCount(int count)
		{
			_figureCountText.text = string.Format("{0} 개", count);
		}
		
		private void MailToQnA()
		{
			string subject = EscapeURL("버그 리포트 / 기타 문의사항");
			string body = EscapeURL(
				"<FigureX 문의사항>" + "\n\n\n\n" +
				"___________" + "\n\n" +
				//"Account ID : " + PlayerModel.Account.PlayFabId + "\n" +
				//"Account Nick : " + PlayerModel.Account.NickName + "\n" +
				"Device Model : " + SystemInfo.deviceModel + "\n\n" +
				"Device OS : " + SystemInfo.operatingSystem + "\n\n" +
				"___________"
				);
			Application.OpenURL("mailto:" + _qnaMailAddress + "?subject=" + subject + "&body=" + body);
		}
		private string EscapeURL(string url)
		{
			return WWW.EscapeURL(url).Replace("+", "%20");
		}
	}
}
