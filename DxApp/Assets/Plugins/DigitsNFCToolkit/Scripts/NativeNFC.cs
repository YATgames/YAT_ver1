using DigitsNFCToolkit.JSON;
using UnityEngine;

namespace DigitsNFCToolkit
{
	/// <summary>Delegate for OnNFCTagDetected</summary>
	public delegate void OnNFCTagDetected(NFCTag tag);

	/// <summary>Delegate for OnNDEFReadFinished</summary>
	public delegate void OnNDEFReadFinished(NDEFReadResult result);

	/// <summary>Delegate for OnNDEFWriteFinished</summary>
	public delegate void OnNDEFWriteFinished(NDEFWriteResult result);

	/// <summary>Delegate for OnNDEFPushFinished</summary>
	public delegate void OnNDEFPushFinished(NDEFPushResult result);

	/// <summary>Delegate for OnNDEFMakeReadonlyFinished</summary>
	public delegate void OnNDEFMakeReadonlyFinished(NDEFMakeReadonlyResult result);

	/// <summary>Base class for the native nfc functionality for each platform</summary>
	public abstract class NativeNFC: MonoBehaviour
	{
		/// <summary>Event for OnNFCTagDetected</summary>
		protected event OnNFCTagDetected onNFCTagDetected;

		/// <summary>Event for OnNDEFReadFinished</summary>
		protected event OnNDEFReadFinished onNDEFReadFinished;

		/// <summary>Event for OnNDEFWriteFinished</summary>
		protected event OnNDEFWriteFinished onNDEFWriteFinished;

		/// <summary>Event for OnNDEFPushFinished</summary>
		protected event OnNDEFPushFinished onNDEFPushFinished;

		/// <summary>Event for OnNDEFMakeReadonlyFinished</summary>
		protected event OnNDEFMakeReadonlyFinished onNDEFMakeReadonlyFinished;

		/// <summary>(iOS only) Indicates whether to reset the NFC Session when it gets a timeout</summary>
		protected bool resetOnTimeout;

		/// <summary>Event for OnNFCTagDetected</summary>
		public event OnNFCTagDetected NFCTagDetected
		{
			add { onNFCTagDetected += value; }
			remove { onNFCTagDetected -= value; }
		}

		/// <summary>Event for OnNDEFReadFinished</summary>
		public event OnNDEFReadFinished NDEFReadFinished
		{
			add { onNDEFReadFinished += value; }
			remove { onNDEFReadFinished -= value; }
		}

		/// <summary>Event for OnNDEFWriteFinished</summary>
		public event OnNDEFWriteFinished NDEFWriteFinished
		{
			add { onNDEFWriteFinished += value; }
			remove { onNDEFWriteFinished -= value; }
		}

		/// <summary>Event for OnNDEFPushFinished</summary>
		public event OnNDEFPushFinished NDEFPushFinished
		{
			add { onNDEFPushFinished += value; }
			remove { onNDEFPushFinished -= value; }
		}

		/// <summary>Event for OnNDEFMakeReadonlyFinished</summary>
		public event OnNDEFMakeReadonlyFinished NDEFMakeReadonlyFinished
		{
			add { onNDEFMakeReadonlyFinished += value; }
			remove { onNDEFMakeReadonlyFinished -= value; }
		}

		public virtual bool ResetOnTimeout
		{
			get { return resetOnTimeout; }
			set { resetOnTimeout = value; }
		}

		/// <summary>Initializes this class</summary>
		public abstract void Initialize();

		/// <summary>Checks if NFC Tag Info Read is supported on this device</summary>
		public abstract bool IsNFCTagInfoReadSupported();

		/// <summary>Checks if NDEF Read is supported on this device</summary>
		public abstract bool IsNDEFReadSupported();

		/// <summary>Checks if NDEF Write is supported on this device</summary>
		public abstract bool IsNDEFWriteSupported();

		/// <summary>Checks if NDEF Make readonly is supported on this device</summary>
		public abstract bool IsNDEFMakeReadonlySupported();

		/// <summary>Checks if NFC is currently enabled on this device</summary>
		public abstract bool IsNFCEnabled();

		/// <summary>Checks if NDEF Push (Android Beam) is currently enabled on this device</summary>
		public abstract bool IsNDEFPushEnabled();

		/// <summary>Enables NFC Reading and Writing</summary>
		public abstract void Enable();

		/// <summary>Disables NFC Reading and Writing</summary>
		public abstract void Disable();

		/// <summary>Start a write request for given NDEF Message</summary>
		public abstract void RequestNDEFWrite(string messageJSON);

		/// <summary>Cancel pending NDEF Message write request (if any)</summary>
		public abstract void CancelNDEFWriteRequest();

		/// <summary>Start a push request for given NDEF Message</summary>
		public abstract void RequestNDEFPush(string messageJSON);

		/// <summary>Cancel pending NDEF Message push request (if any)</summary>
		public abstract void CancelNDEFPushRequest();

		/// <summary>Start a make readonly request</summary>
		public abstract void RequestNDEFMakeReadonly();

		/// <summary>Cancel pending make readonly request (if any)</summary>
		public abstract void CancelNDEFMakeReadonlyRequest();

		/// <summary>Shows the NFC Settings on the device (if available)</summary>
		public abstract void ShowNFCSettings();

		/// <summary>Enables the system nfc sounds when scanning a tag</summary>
		public abstract void EnableSounds();

		/// <summary>Disables the system nfc sounds when scanning a tag</summary>
		public abstract void DisableSounds();

		/// <summary>Event callback when a NFC Tag was detected</summary>
		public void OnNFCTagDetected(string tagJSON)
		{
			Debug.Log("OnNFCTagDetected: " + tagJSON);
			JSONObject jsonObject = JSONObject.Parse(tagJSON);
			NFCTag tag = new NFCTag(jsonObject);

			if(onNFCTagDetected != null)
			{
				onNFCTagDetected(tag);
			}
		}

		/// <summary>Event callback when a NDEF Message Read request was finished</summary>
		public void OnNDEFReadFinished(string resultJSON)
		{
			JSONObject jsonObject = JSONObject.Parse(resultJSON);
			NDEFReadResult result = new NDEFReadResult(jsonObject);

			if(onNDEFReadFinished != null)
			{
				onNDEFReadFinished(result);
			}
		}

		/// <summary>Event callback when a NDEF Message Write request was finished</summary>
		public void OnNDEFWriteFinished(string resultJSON)
		{
			JSONObject jsonObject = JSONObject.Parse(resultJSON);
			NDEFWriteResult result = new NDEFWriteResult(jsonObject);

			if(onNDEFWriteFinished != null)
			{
				onNDEFWriteFinished(result);
			}
		}

		/// <summary>Event callback when a NDEF Message Push request was finished</summary>
		public void OnNDEFPushFinished(string resultJSON)
		{
			JSONObject jsonObject = JSONObject.Parse(resultJSON);
			NDEFPushResult result = new NDEFPushResult(jsonObject);

			if(onNDEFPushFinished != null)
			{
				onNDEFPushFinished(result);
			}
		}

		/// <summary>Event callback when a NDEF make readonly request was finished</summary>
		public void OnNDEFMakeReadonlyFinished(string resultJSON)
		{
			JSONObject jsonObject = JSONObject.Parse(resultJSON);
			NDEFMakeReadonlyResult result = new NDEFMakeReadonlyResult(jsonObject);

			if(onNDEFMakeReadonlyFinished != null)
			{
				onNDEFMakeReadonlyFinished(result);
			}
		}
	}
}
