using DigitsNFCToolkit.JSON;
using System;
using System.Collections.Generic;

namespace DigitsNFCToolkit
{
	/// <summary>Record class that is essentially an Uri Record with more metadata</summary>
	[Serializable]
	public class SmartPosterRecord: NDEFRecord
	{
		/// <summary>Enum for the recommended action that should be taken</summary>
		public enum RecommendedAction { UNKNOWN, DO_ACTION, SAVE_FOR_LATER, OPEN_FOR_EDITING }

		/// <summary>The main uri record</summary>
		public UriRecord uriRecord;

		/// <summary>The title records (1 per language code allowed)</summary>
		public List<TextRecord> titleRecords;

		/// <summary>The icon records</summary>
		public List<MimeMediaRecord> iconRecords;

		/// <summary>The other records</summary>
		public List<NDEFRecord> extraRecords;

		/// <summary>Specifies the recommended action that should be taken</summary>
		public RecommendedAction action;

		/// <summary>The size of the file referenced by the uri</summary>
		public int size;

		/// <summary>The mime type of the file referenced by the uri</summary>
		public String mimeType;

		public SmartPosterRecord(string uri, RecommendedAction action = RecommendedAction.UNKNOWN, int size = 0, string mimeType = "")
		{
			this.type = NDEFRecordType.SMART_POSTER;
			this.uriRecord = new UriRecord(uri);
			this.action = action;
			this.size = size;
			this.mimeType = mimeType;
			this.titleRecords = new List<TextRecord>();
			this.iconRecords = new List<MimeMediaRecord>();
			this.extraRecords = new List<NDEFRecord>();
		}

		public SmartPosterRecord(string uri, RecommendedAction action, int size, string mimeType, List<TextRecord> titleRecords)
		{
			this.type = NDEFRecordType.SMART_POSTER;
			this.uriRecord = new UriRecord(uri);
			this.action = action;
			this.size = size;
			this.mimeType = mimeType;
			this.titleRecords = titleRecords;
			this.iconRecords = new List<MimeMediaRecord>();
			this.extraRecords = new List<NDEFRecord>();
		}

		public SmartPosterRecord(string uri, RecommendedAction action, int size, string mimeType, List<TextRecord> titleRecords, List<MimeMediaRecord> iconRecords)
		{
			this.type = NDEFRecordType.SMART_POSTER;
			this.uriRecord = new UriRecord(uri);
			this.action = action;
			this.size = size;
			this.mimeType = mimeType;
			this.titleRecords = titleRecords;
			this.iconRecords = iconRecords;
			this.extraRecords = new List<NDEFRecord>();
		}

		public SmartPosterRecord(string uri, RecommendedAction action, int size, string mimeType, List<TextRecord> titleRecords, List<MimeMediaRecord> iconRecords, List<NDEFRecord> extraRecords)
		{
			this.type = NDEFRecordType.SMART_POSTER;
			this.uriRecord = new UriRecord(uri);
			this.titleRecords = titleRecords;
			this.iconRecords = iconRecords;
			this.extraRecords = extraRecords;
			this.action = action;
			this.size = size;
			this.mimeType = mimeType;
		}

		public SmartPosterRecord(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public override void ParseJSON(JSONObject jsonObject)
		{
			base.ParseJSON(jsonObject);

			JSONObject uriRecordJSON;
			if(jsonObject.TryGetObject("uri_record", out uriRecordJSON))
			{
				uriRecord = new UriRecord(uriRecordJSON);
			}

			titleRecords = new List<TextRecord>();
			JSONArray titleRecordsJSON;
			if(jsonObject.TryGetArray("title_records", out titleRecordsJSON))
			{
				int length = titleRecordsJSON.Length;
				for(int i = 0; i < length; i++)
				{
					titleRecords.Add(new TextRecord(titleRecordsJSON[i].Object));
				}
			}

			iconRecords = new List<MimeMediaRecord>();
			JSONArray iconRecordsJSON;
			if(jsonObject.TryGetArray("icon_records", out iconRecordsJSON))
			{
				int length = iconRecordsJSON.Length;
				for(int i = 0; i < length; i++)
				{
					iconRecords.Add(new MimeMediaRecord(iconRecordsJSON[i].Object));
				}
			}

			extraRecords = new List<NDEFRecord>();
			JSONArray extraRecordsJSON;
			if(jsonObject.TryGetArray("extra_records", out extraRecordsJSON))
			{
				int length = extraRecordsJSON.Length;
				for(int i = 0; i < length; i++)
				{
					JSONObject extraRecordJSON = extraRecordsJSON[i].Object;
					NDEFRecord record = null;
					NDEFRecordType type = (NDEFRecordType)extraRecordJSON["type"].Integer;
					switch(type)
					{
						case NDEFRecordType.ABSOLUTE_URI: record = new AbsoluteUriRecord(extraRecordJSON); break;
						case NDEFRecordType.EMPTY: record = new EmptyRecord(extraRecordJSON); break;
						case NDEFRecordType.EXTERNAL_TYPE: record = new ExternalTypeRecord(extraRecordJSON); break;
						case NDEFRecordType.MIME_MEDIA: record = new MimeMediaRecord(extraRecordJSON); break;
						case NDEFRecordType.SMART_POSTER: record = new SmartPosterRecord(extraRecordJSON); break;
						case NDEFRecordType.TEXT: record = new TextRecord(extraRecordJSON); break;
						case NDEFRecordType.UNKNOWN: record = new UnknownRecord(extraRecordJSON); break;
						case NDEFRecordType.URI: record = new UriRecord(extraRecordJSON); break;
						default: record = new UnknownRecord(extraRecordJSON); break;
					}

					extraRecords.Add(record);
				}
			}

			int actionValue;
			jsonObject.TryGetInt("action", out actionValue);
			action = (RecommendedAction)actionValue;

			jsonObject.TryGetInt("size", out size);
			jsonObject.TryGetString("mime_type", out mimeType);
		}

		public override JSONObject ToJSON()
		{
			JSONObject jsonObject = base.ToJSON();
			jsonObject.Add("uri_record", uriRecord.ToJSON());

			JSONArray titleRecordsJSON = new JSONArray();
			int length = titleRecords.Count;
			for(int i = 0; i < length; i++)
			{
				titleRecordsJSON.Add(titleRecords[i].ToJSON());
			}
			jsonObject.Add("title_records", titleRecordsJSON);

			JSONArray iconRecordsJSON = new JSONArray();
			length = iconRecords.Count;
			for(int i = 0; i < length; i++)
			{
				iconRecordsJSON.Add(iconRecords[i].ToJSON());
			}
			jsonObject.Add("icon_records", iconRecordsJSON);

			JSONArray extraRecordsJSON = new JSONArray();
			length = extraRecords.Count;
			for(int i = 0; i < length; i++)
			{
				extraRecordsJSON.Add(extraRecords[i].ToJSON());
			}
			jsonObject.Add("extra_records", extraRecordsJSON);

			jsonObject.Add("action", (int)action);
			jsonObject.Add("size", size);
			jsonObject.Add("mime_type", mimeType);

			return jsonObject;
		}
	}
}
