using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DigitsNFCToolkit.Samples
{
	public class ReadScreenView: MonoBehaviour
	{
		private const string TAG_INFO_FORMAT = "ID: {0}\nTechnologies: {1}\nManufacturer: {2}\nWritable: {3}\nMaxWriteSize: {4}";
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

		private RectTransform tagInfoTransform;
		private Button makeReadonlyButton;
		private Button startReadButton;
		private Text tagInfoContentLabel;
		private ScrollRect ndefMessageScrollRect;

		private void Awake()
		{
			tagInfoTransform = transform.Find("TagInfo").GetComponent<RectTransform>();
			makeReadonlyButton = tagInfoTransform.Find("MakeReadonlyButton").GetComponent<Button>();
			startReadButton = tagInfoTransform.Find("StartReadButton").GetComponent<Button>();
			tagInfoContentLabel = tagInfoTransform.Find("Content/Label").GetComponent<Text>();
			ndefMessageScrollRect = transform.Find("NDEFMessage/ScrollView").GetComponent<ScrollRect>();
		}

		private void Start()
		{
#if UNITY_ANDROID
			makeReadonlyButton.gameObject.SetActive(true);
#else
			makeReadonlyButton.gameObject.SetActive(false);
#endif

#if UNITY_IOS
			startReadButton.gameObject.SetActive(true);

#else
			startReadButton.gameObject.SetActive(false);
#endif
		}

		public void UpdateTagInfo(NFCTag tag)
		{
			string technologiesString = string.Empty;
			NFCTechnology[] technologies = tag.Technologies;
			int length = technologies.Length;
			for(int i = 0; i < length; i++)
			{
				if(i > 0)
				{
					technologiesString += ", ";
				}

				technologiesString += technologies[i].ToString();
			}

			string maxWriteSizeString = string.Empty;
			if(tag.MaxWriteSize > 0)
			{
				maxWriteSizeString = tag.MaxWriteSize + " bytes";
			}
			else
			{
				maxWriteSizeString = "Unknown";
			}

			string tagInfo = string.Format(TAG_INFO_FORMAT, tag.ID, technologiesString, tag.Manufacturer, tag.Writable, maxWriteSizeString);
			tagInfoContentLabel.text = tagInfo;
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

		private void CleanupRecordItems()
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
