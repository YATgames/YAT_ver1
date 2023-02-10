using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DigitsNFCToolkit.Samples
{
	public class WriteScreenView: MonoBehaviour
	{
		private const string TEXT_RECORD_FORMAT = "Type: {0}\nText: {1}\nLanguage Code: {2}\nText Encoding: {3}";
		private const string URI_RECORD_FORMAT = "Type: {0}\nFull Uri: {1}\nUri: {2}\nProtocol: {3}";
		private const string MIME_MEDIA_RECORD_FORMAT = "Type: {0}\nMime Type: {1}";
		private const string EXTERNAL_TYPE_FORMAT = "Type: {0}\nDomain Name: {1}\nDomain Type: {2}\nDomain Data: Length: {3}, Value: {4}";
		private const string SMART_POSTER_RECORD_FORMAT = "Type: {0}\nUri: {1}\nAction: {2}\nSize: {3}\nMime Type: {4}\nTotal Records: {5}";
		private const int Y_SPACING = 8;

		[SerializeField]
		private RecordItem textRecordItemPrefab;

		[SerializeField]
		private RecordItem uriRecordItemPrefab;

		[SerializeField]
		private ImageRecordItem mimeMediaRecordItemPrefab;

		[SerializeField]
		private RecordItem externalTypeRecordItemPrefab;

		[SerializeField]
		private RecordItem smartPosterRecordItemPrefab;

		private ScrollRect ndefMessageScrollRect;
		private Text titleLabel;
		private Dropdown typeDropdown;

		private Transform textRecordTransform;
		private InputField textInput;
		private InputField languageCodeInput;
		private Dropdown textEncodingDropdown;
		private Button addRecordButton;

		private Transform uriRecordTransform;
		private InputField uriInput;

		private Transform mimeMediaRecordTransform;
		private Dropdown iconDropdown;
		private Text mimeTypeLabel;

		private Transform externalTypeRecordTransform;
		private InputField domainNameInput;
		private InputField domainTypeInput;
		private InputField domainDataInput;

		private Button clearButton;
		private Button writeButton;

		public Dropdown TypeDropdown { get { return typeDropdown; } }
		public InputField TextInput { get { return textInput; } }
		public InputField LanguageCodeInput { get { return languageCodeInput; } }
		public Dropdown TextEncodingDropdown { get { return textEncodingDropdown; } }
		public InputField UriInput { get { return uriInput; } }
		public Dropdown IconDropdown { get { return iconDropdown; } }
		public InputField DomainNameInput { get { return domainNameInput; } }
		public InputField DomainTypeInput { get { return domainTypeInput; } }
		public InputField DomainDataInput { get { return domainDataInput; } }

		private void Awake()
		{
			ndefMessageScrollRect = transform.Find("NDEFMessage/ScrollView").GetComponent<ScrollRect>();
			titleLabel = transform.Find("TitleLabel").GetComponent<Text>();
			typeDropdown = transform.Find("AddRecord/Type/Dropdown").GetComponent<Dropdown>();
			addRecordButton = transform.Find("AddRecord/AddButton").GetComponent<Button>();

			textRecordTransform = transform.Find("AddRecord/TextRecord");
			textInput = textRecordTransform.Find("Text/InputField").GetComponent<InputField>();
			languageCodeInput = textRecordTransform.Find("Language/InputField").GetComponent<InputField>();
			textEncodingDropdown = textRecordTransform.Find("TextEncoding/Dropdown").GetComponent<Dropdown>();

			uriRecordTransform = transform.Find("AddRecord/UriRecord");
			uriInput = uriRecordTransform.Find("Uri/InputField").GetComponent<InputField>();

			mimeMediaRecordTransform = transform.Find("AddRecord/MimeMediaRecord");
			iconDropdown = mimeMediaRecordTransform.Find("Icon/Dropdown").GetComponent<Dropdown>();
			mimeTypeLabel = mimeMediaRecordTransform.Find("MimeType/ValueLabel").GetComponent<Text>();

			externalTypeRecordTransform = transform.Find("AddRecord/ExternalTypeRecord");
			domainNameInput = externalTypeRecordTransform.Find("DomainName/InputField").GetComponent<InputField>();
			domainTypeInput = externalTypeRecordTransform.Find("DomainType/InputField").GetComponent<InputField>();
			domainDataInput = externalTypeRecordTransform.Find("DomainData/InputField").GetComponent<InputField>();

			clearButton = transform.Find("NDEFMessage/ClearButton").GetComponent<Button>();
			writeButton = transform.Find("NDEFMessage/ScrollView/WriteButton").GetComponent<Button>();
		}

		public void UpdateTypeDropdownOptions(string[] options)
		{
			typeDropdown.ClearOptions();
			List<Dropdown.OptionData> optionsList = new List<Dropdown.OptionData>();

			int length = options.Length;
			for(int i = 0; i < length; i++)
			{
				optionsList.Add(new Dropdown.OptionData(options[i]));
			}

			typeDropdown.AddOptions(optionsList);
		}

		public void UpdateTextEncodingDropdownOptions(string[] options)
		{
			textEncodingDropdown.ClearOptions();
			List<Dropdown.OptionData> optionsList = new List<Dropdown.OptionData>();

			int length = options.Length;
			for(int i = 0; i < length; i++)
			{
				optionsList.Add(new Dropdown.OptionData(options[i]));
			}

			textEncodingDropdown.AddOptions(optionsList);
		}

		public void UpdateIconDropdownOptions(string[] options)
		{
			iconDropdown.ClearOptions();
			List<Dropdown.OptionData> optionsList = new List<Dropdown.OptionData>();

			int length = options.Length;
			for(int i = 0; i < length; i++)
			{
				optionsList.Add(new Dropdown.OptionData(options[i]));
			}

			iconDropdown.AddOptions(optionsList);
		}

		public void SwitchToTextRecordInput()
		{
			textRecordTransform.gameObject.SetActive(true);
			uriRecordTransform.gameObject.SetActive(false);
			mimeMediaRecordTransform.gameObject.SetActive(false);
			externalTypeRecordTransform.gameObject.SetActive(false);
		}

		public void SwitchToUriInput()
		{
			textRecordTransform.gameObject.SetActive(false);
			uriRecordTransform.gameObject.SetActive(true);
			mimeMediaRecordTransform.gameObject.SetActive(false);
			externalTypeRecordTransform.gameObject.SetActive(false);
		}

		public void SwitchToMimeMediaInput()
		{
			textRecordTransform.gameObject.SetActive(false);
			uriRecordTransform.gameObject.SetActive(false);
			mimeMediaRecordTransform.gameObject.SetActive(true);
			externalTypeRecordTransform.gameObject.SetActive(false);
		}

		public void SwitchToExternalTypeInput()
		{
			textRecordTransform.gameObject.SetActive(false);
			uriRecordTransform.gameObject.SetActive(false);
			mimeMediaRecordTransform.gameObject.SetActive(false);
			externalTypeRecordTransform.gameObject.SetActive(true);
		}

		public void ClearTextInput()
		{
			textInput.text = string.Empty;
		}

		public void ResetLanguageCodeInput()
		{
			textInput.text = "en";
		}

		public void ClearUriInput()
		{
			uriInput.text = string.Empty;
		}

		public void UpdateNDEFMessage(NDEFMessage message)
		{
			CleanupRecordItems();

			float y = 0;
			List<NDEFRecord> records = message.Records;

			int length = records.Count;
			for(int i = 0; i < length; i++)
			{
				NDEFRecord record = records[i];
				RecordItem recordItem = null;
				switch(record.Type)
				{
					case NDEFRecordType.TEXT:
						recordItem = CreateRecordItem(textRecordItemPrefab);
						TextRecord textRecord = (TextRecord)record;
						recordItem.UpdateLabel(string.Format(TEXT_RECORD_FORMAT, NDEFRecordType.TEXT, textRecord.text, textRecord.languageCode, textRecord.textEncoding));
						break;
					case NDEFRecordType.URI:
						recordItem = CreateRecordItem(uriRecordItemPrefab);
						UriRecord uriRecord = (UriRecord)record;
						recordItem.UpdateLabel(string.Format(URI_RECORD_FORMAT, NDEFRecordType.URI, uriRecord.fullUri, uriRecord.uri, uriRecord.protocol));
						break;
					case NDEFRecordType.MIME_MEDIA:
						recordItem = CreateRecordItem(mimeMediaRecordItemPrefab);
						MimeMediaRecord mimeMediaRecord = (MimeMediaRecord)record;
						recordItem.UpdateLabel(string.Format(MIME_MEDIA_RECORD_FORMAT, NDEFRecordType.MIME_MEDIA, mimeMediaRecord.mimeType));
						((ImageRecordItem)recordItem).LoadImage(mimeMediaRecord.mimeData);
						break;
					case NDEFRecordType.EXTERNAL_TYPE:
						recordItem = CreateRecordItem(externalTypeRecordItemPrefab);
						ExternalTypeRecord externalTypeRecord = (ExternalTypeRecord)record;
						int dataLength = externalTypeRecord.domainData.Length;
						string dataValue = Encoding.UTF8.GetString(externalTypeRecord.domainData);
						recordItem.UpdateLabel(string.Format(EXTERNAL_TYPE_FORMAT, NDEFRecordType.EXTERNAL_TYPE, externalTypeRecord.domainName, externalTypeRecord.domainType, dataLength, dataValue));
						break;
					case NDEFRecordType.SMART_POSTER:
						recordItem = CreateRecordItem(smartPosterRecordItemPrefab);
						SmartPosterRecord smartPosterRecord = (SmartPosterRecord)record;
						int totalRecords = smartPosterRecord.titleRecords.Count + smartPosterRecord.iconRecords.Count + smartPosterRecord.extraRecords.Count;
						recordItem.UpdateLabel(string.Format(SMART_POSTER_RECORD_FORMAT, NDEFRecordType.SMART_POSTER, smartPosterRecord.uriRecord.fullUri, smartPosterRecord.action, smartPosterRecord.size, smartPosterRecord.mimeType, totalRecords));
						break;
				}

				recordItem.RectTransform.anchoredPosition = new Vector2(0, y);
				y -= recordItem.RectTransform.rect.height;
				y -= Y_SPACING;
			}

			ndefMessageScrollRect.content.sizeDelta = new Vector2(0, Mathf.Abs(y));
		}

		public void CleanupRecordItems()
		{
			RectTransform contentTransform = ndefMessageScrollRect.content;
			int length = contentTransform.childCount;
			for(int i = 0; i < length; i++)
			{
				Destroy(contentTransform.GetChild(i).gameObject);
			}
		}

		private RecordItem CreateRecordItem(RecordItem prefab)
		{
			RecordItem recordItem = Instantiate(prefab);
			Vector2 sizeDelta = recordItem.RectTransform.sizeDelta;
			recordItem.RectTransform.SetParent(ndefMessageScrollRect.content);
			recordItem.RectTransform.localScale = Vector2.one;
			recordItem.RectTransform.localRotation = Quaternion.identity;
			recordItem.RectTransform.localPosition = Vector3.zero;
			recordItem.RectTransform.sizeDelta = sizeDelta;

			return recordItem;
		}
	}
}
