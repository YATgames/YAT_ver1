using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DigitsNFCToolkit.Samples
{
	public class WriteScreenControl: MonoBehaviour
	{
		public enum IconID { MUSIC_NOTE, FACE, ARROW }

		[SerializeField]
		private WriteScreenView view;

		[SerializeField]
		private MessageScreenView messageScreenView;

		[SerializeField]
		private Sprite musicNoteIcon;

		[SerializeField]
		private Sprite faceIcon;

		[SerializeField]
		private Sprite arrowIcon;

		private NDEFMessage pendingMessage;

		private void Start()
		{
#if(!UNITY_EDITOR)
			NativeNFCManager.AddNDEFWriteFinishedListener(OnNDEFWriteFinished);
			NativeNFCManager.AddNDEFPushFinishedListener(OnNDEFPushFinished);
#endif
			string[] options = new string[]
			{
				NDEFRecordType.TEXT.ToString(),
				NDEFRecordType.URI.ToString(),
				NDEFRecordType.MIME_MEDIA.ToString(),
				NDEFRecordType.EXTERNAL_TYPE.ToString()
			};
			view.UpdateTypeDropdownOptions(options);

			TextRecord.TextEncoding[] textEncodings = Enum.GetValues(typeof(TextRecord.TextEncoding)).Cast<TextRecord.TextEncoding>().ToArray();
			int length = textEncodings.Length;
			options = new string[length];
			for(int i = 0; i < length; i++)
			{
				options[i] = textEncodings[i].ToString();
			}
			view.UpdateTextEncodingDropdownOptions(options);

			IconID[] iconIDs = Enum.GetValues(typeof(IconID)).Cast<IconID>().ToArray();
			length = iconIDs.Length;
			options = new string[length];
			for(int i = 0; i < length; i++)
			{
				options[i] = iconIDs[i].ToString();
			}
			view.UpdateIconDropdownOptions(options);

			//Uncomment if you want to write the test Smart Poster Record
			//if(pendingMessage == null)
			//{
			//    pendingMessage = new NDEFMessage();
			//}

			//SmartPosterRecord smartPosterRecord = CreateTestSmartPosterRecord();
			//pendingMessage.Records.Add(smartPosterRecord);
			//view.UpdateNDEFMessage(pendingMessage);
		}

		private void OnEnable()
		{
#if(!UNITY_EDITOR) && !UNITY_IOS
			NativeNFCManager.Enable();
#endif
			view.gameObject.SetActive(true);
		}

		private void OnDisable()
		{
#if(!UNITY_EDITOR) && !UNITY_IOS
			NativeNFCManager.Disable();
#endif
			if(view != null)
			{
				view.gameObject.SetActive(false);
			}
		}

		/// <summary>Example of how to create a Smart Poster Record</summary>
		private SmartPosterRecord CreateTestSmartPosterRecord()
		{
			List<TextRecord> titleRecords = new List<TextRecord>();
			titleRecords.Add(new TextRecord("Hallo wereld", "nl"));
			titleRecords.Add(new TextRecord("Hello world", "en"));

			List<MimeMediaRecord> iconRecords = new List<MimeMediaRecord>();
			//Commented because NDEF Message was getting too big to fit in test nfc tags
			//string mimeType = "image/png"; 
			//IconID iconID = (IconID)view.IconDropdown.value;
			//byte[] mimeData = GetIconBytes(iconID);
			//iconRecords.Add(new MimeMediaRecord(mimeType, mimeData)); 

			List<NDEFRecord> extraRecords = new List<NDEFRecord>();
			extraRecords.Add(new TextRecord("Tweede titel", "nl"));
			extraRecords.Add(new TextRecord("Second title", "en"));
			extraRecords.Add(new UriRecord("test.com"));

			return new SmartPosterRecord("http://google.com", SmartPosterRecord.RecommendedAction.DO_ACTION, 1534690, "image/jpeg", titleRecords, iconRecords, extraRecords);
		}

		public void OnRecordTypeChanged(int index)
		{
			string value = view.TypeDropdown.options[index].text;
			NDEFRecordType type = (NDEFRecordType)Enum.Parse(typeof(NDEFRecordType), value);
			switch(type)
			{
				case NDEFRecordType.TEXT: view.SwitchToTextRecordInput(); break;
				case NDEFRecordType.URI: view.SwitchToUriInput(); break;
				case NDEFRecordType.MIME_MEDIA: view.SwitchToMimeMediaInput(); break;
				case NDEFRecordType.EXTERNAL_TYPE: view.SwitchToExternalTypeInput(); break;
			}
		}

		public void OnAddRecordClick()
		{
			if(pendingMessage == null)
			{
				pendingMessage = new NDEFMessage();
			}

			NDEFRecord record = null;
			string value = view.TypeDropdown.options[view.TypeDropdown.value].text;
			NDEFRecordType type = (NDEFRecordType)Enum.Parse(typeof(NDEFRecordType), value);
			switch(type)
			{
				case NDEFRecordType.TEXT:
					string text = view.TextInput.text;
					string languageCode = view.LanguageCodeInput.text;
					if(languageCode.Length != 2) { languageCode = "en"; }
					TextRecord.TextEncoding textEncoding = (TextRecord.TextEncoding)view.TextEncodingDropdown.value;
					record = new TextRecord(text, languageCode, textEncoding);
					break;
				case NDEFRecordType.URI:
					string uri = view.UriInput.text;
					record = new UriRecord(uri);
					break;
				case NDEFRecordType.MIME_MEDIA:
					string mimeType = "image/png"; //We're only using png images atm
					IconID iconID = (IconID)view.IconDropdown.value;
					byte[] mimeData = GetIconBytes(iconID);
					record = new MimeMediaRecord(mimeType, mimeData);
					break;
				case NDEFRecordType.EXTERNAL_TYPE:
					string domainName = view.DomainNameInput.text;
					string domainType = view.DomainTypeInput.text;
					string domainDataString = view.DomainDataInput.text;
					byte[] domainData = Encoding.UTF8.GetBytes(domainDataString); //Data represents a string in this example
					record = new ExternalTypeRecord(domainName, domainType, domainData);
					break;

			}

			pendingMessage.Records.Add(record);

			view.UpdateNDEFMessage(pendingMessage);
		}

		private byte[] GetIconBytes(IconID iconID)
		{
			byte[] bytes = null;
			switch(iconID)
			{
				case IconID.MUSIC_NOTE: bytes = musicNoteIcon.texture.EncodeToPNG(); break;
				case IconID.FACE: bytes = faceIcon.texture.EncodeToPNG(); break;
				case IconID.ARROW: bytes = arrowIcon.texture.EncodeToPNG(); break;
			}

			return bytes;
		}

		public void OnClearMessageClick()
		{
#if(!UNITY_EDITOR)
			NativeNFCManager.CancelNDEFWriteRequest();
#endif
			pendingMessage = null;
			view.CleanupRecordItems();
		}

		public void OnWriteMessageClick()
		{
			if(pendingMessage != null)
			{
#if(!UNITY_EDITOR)
				NativeNFCManager.RequestNDEFWrite(pendingMessage);
#if UNITY_ANDROID
				messageScreenView.Show();
				messageScreenView.SwitchToPendingWrite();
#elif UNITY_IOS
				NativeNFCManager.Enable(); //Show native popup
#endif
#endif
			}
		}

		public void OnWriteOKClick()
		{
			messageScreenView.Hide();
			pendingMessage = null;
			view.CleanupRecordItems();
		}

		public void OnWriteCancelClick()
		{
			messageScreenView.Hide();
#if(!UNITY_EDITOR)
			NativeNFCManager.CancelNDEFWriteRequest();
#endif
		}

		public void OnPushMessageClick()
		{
			if(pendingMessage != null)
			{
#if(!UNITY_EDITOR) && UNITY_ANDROID
				NativeNFCManager.RequestNDEFPush(pendingMessage);
				messageScreenView.Show();
				messageScreenView.SwitchToPendingPush();
#endif
			}
		}

		public void OnPushOKClick()
		{
			messageScreenView.Hide();
			pendingMessage = null;
			view.CleanupRecordItems();
		}

		public void OnPushCancelClick()
		{
			messageScreenView.Hide();
#if(!UNITY_EDITOR) && UNITY_ANDROID
			NativeNFCManager.CancelNDEFPushRequest();
#endif
		}

		public void OnNDEFWriteFinished(NDEFWriteResult result)
		{
			view.UpdateNDEFMessage(result.Message);

			string writeResultString = string.Empty;
			if(result.Success)
			{
				writeResultString = string.Format("NDEF Message written successfully to tag {0}", result.TagID);
				pendingMessage = null;
				view.CleanupRecordItems();
			}
			else
			{
				writeResultString = string.Format("NDEF Message failed to write to tag {0}\nError: {1}", result.TagID, result.Error);
			}
			Debug.Log(writeResultString);
			messageScreenView.SwitchToWriteResult(writeResultString);
		}

		public void OnNDEFPushFinished(NDEFPushResult result)
		{
			view.UpdateNDEFMessage(result.Message);

			string pushResultString = string.Empty;
			if(result.Success)
			{
				pushResultString = "NDEF Message pushed successfully to other device";
			}
			else
			{
				pushResultString = "NDEF Message failed to push to other device";
			}
			Debug.Log(pushResultString);
			messageScreenView.SwitchToPushResult(pushResultString);
		}
	}
}
