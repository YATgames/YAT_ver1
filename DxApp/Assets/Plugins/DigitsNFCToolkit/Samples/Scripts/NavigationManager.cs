using UnityEngine;

namespace DigitsNFCToolkit.Samples
{
	public class NavigationManager: MonoBehaviour
	{
		[SerializeField]
		private ReadScreenControl readScreenControl;

		[SerializeField]
		private WriteScreenControl writeScreenControl;

		private void Start()
		{
			SwitchToReadScreen();
		}

		public void SwitchToReadScreen()
		{
			writeScreenControl.gameObject.SetActive(false);
			readScreenControl.gameObject.SetActive(true);
		}

		public void SwitchToWriteScreen()
		{
			readScreenControl.gameObject.SetActive(false);
			writeScreenControl.gameObject.SetActive(true);
		}
	}
}
