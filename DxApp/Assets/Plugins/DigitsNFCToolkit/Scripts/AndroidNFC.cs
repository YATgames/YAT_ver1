#if(!UNITY_EDITOR) && UNITY_ANDROID
using UnityEngine;

namespace DigitsNFCToolkit
{
	/// <summary>Class for the native nfc functionality on Android</summary>
	public class AndroidNFC: NativeNFC
	{
		private AndroidJavaObject mainClass;

		public override void Initialize()
		{
			mainClass = new AndroidJavaClass("com.apollojourney.nativenfc.NativeNFC");
			mainClass.CallStatic("_initialize", gameObject.name);
		}

		public override bool IsNFCTagInfoReadSupported()
		{
			return mainClass.CallStatic<bool>("_isNFCTagInfoReadSupported");
		}

		public override bool IsNDEFReadSupported()
		{
			return mainClass.CallStatic<bool>("_isNDEFReadSupported");
		}

		public override bool IsNDEFWriteSupported()
		{
			return mainClass.CallStatic<bool>("_isNDEFWriteSupported");
		}

		public override bool IsNDEFMakeReadonlySupported()
		{
			return mainClass.CallStatic<bool>("_isNDEFMakeReadonlySupported");
		}

		public override bool IsNFCEnabled()
		{
			return mainClass.CallStatic<bool>("_isNFCEnabled");
		}

		public override bool IsNDEFPushEnabled()
		{
			return mainClass.CallStatic<bool>("_isNDEFPushEnabled");
		}

		public override void Enable()
		{
			mainClass.CallStatic("_enable");
		}

		public override void Disable()
		{
			mainClass.CallStatic("_disable");
		}

		public override void RequestNDEFWrite(string messageJSON)
		{
			mainClass.CallStatic("_requestNDEFWrite", messageJSON);
		}

		public override void CancelNDEFWriteRequest()
		{
			mainClass.CallStatic("_cancelNDEFWriteRequest");
		}

		public override void RequestNDEFPush(string messageJSON)
		{
			mainClass.CallStatic("_requestNDEFPush", messageJSON);
		}

		public override void CancelNDEFPushRequest()
		{
			mainClass.CallStatic("_cancelNDEFPushRequest");
		}

		public override void RequestNDEFMakeReadonly()
		{
			mainClass.CallStatic("_requestNDEFMakeReadonly");
		}

		public override void CancelNDEFMakeReadonlyRequest()
		{
			mainClass.CallStatic("_cancelNDEFMakeReadonlyRequest");
		}

		public override void ShowNFCSettings()
		{
			mainClass.CallStatic("_showNFCSettings");
		}

		public override void EnableSounds()
		{
			mainClass.CallStatic("_enableSounds");
		}

		public override void DisableSounds()
		{
			mainClass.CallStatic("_disableSounds");
		}
	}
}
#endif