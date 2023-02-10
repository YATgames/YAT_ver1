using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Util
{
	[AddComponentMenu("UI Ex/OnOffButton", 1)]
	[RequireComponent(typeof(Image))]
	public class OnOffButtonComponenet : Button
	{
		public GameObject OnObj;
		public GameObject OffObj;
		public bool IsOn;

		protected override void Start()
		{
			base.Start();

			if(Application.isPlaying)
				this.OnClickAsObservable().Subscribe(_ => Event_Click()).AddTo(gameObject);
		}
		

		private void Event_Click()
		{
			SetData(!IsOn);
		}

		public void SetData(bool isOn)
		{
			OnObj?.SetActive(isOn);
			OffObj?.SetActive(isOn == false);
			IsOn = isOn;
		}
	}
}
