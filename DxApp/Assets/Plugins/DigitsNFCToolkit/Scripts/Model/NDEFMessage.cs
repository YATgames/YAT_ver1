using DigitsNFCToolkit.JSON;
using System;
using System.Collections.Generic;

namespace DigitsNFCToolkit
{
	/// <summary>Enum for write state of the NDEF Message</summary>
	public enum NDEFMessageWriteState { IDLE, PENDING, SUCCESS, FAILED }

	/// <summary>Enum for write error of the NDEF Message</summary>
	public enum NDEFMessageWriteError { NONE, NOT_WRITABLE, INSUFFICIENT_SPACE, UNKNOWN }

	/// <summary>Container class for NDEF Records</summary>
	public class NDEFMessage: IJSONSerializable
	{
		/// <summary>The NDEF Records</summary>
		private List<NDEFRecord> records;

		/// <summary>The ID of the tag this NDEF Message has been written to</summary>
		private String tagID;

		/// <summary>Write state of the NDEF Message</summary>
		private NDEFMessageWriteState writeState;

		/// <summary>Write error of the NDEF Message</summary>
		private NDEFMessageWriteError writeError;

		/// <summary>The NDEF Records</summary>
		public List<NDEFRecord> Records { get { return records; } }

		/// <summary>The ID of the tag this NDEF Message has been written to</summary>
		public string TagID { get { return tagID; } }

		/// <summary>Write state of the NDEF Message</summary>
		public NDEFMessageWriteState WriteState { get { return writeState; } }

		/// <summary>Write error of the NDEF Message</summary>
		public NDEFMessageWriteError WriteError { get { return writeError; } }

		/// <summary>Indicates if the write operation was successful</summary>
		public bool WriteSuccess { get { return WriteState == NDEFMessageWriteState.SUCCESS; } }

		public NDEFMessage()
		{
			records = new List<NDEFRecord>();
			tagID = string.Empty;
			writeState = NDEFMessageWriteState.IDLE;
			writeError = NDEFMessageWriteError.NONE;
		}

		public NDEFMessage(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public void ParseJSON(JSONObject jsonObject)
		{
			JSONArray recordsJSON;
			if(jsonObject.TryGetArray("records", out recordsJSON))
			{
				int length = recordsJSON.Length;
				records = new List<NDEFRecord>();
				for(int i = 0; i < length; i++)
				{
					JSONObject recordJSON = recordsJSON[i].Object;
					int typeValue;
					recordJSON.TryGetInt("type", out typeValue);
					NDEFRecordType type = (NDEFRecordType)typeValue;
					NDEFRecord record = null;
					switch(type)
					{
						case NDEFRecordType.ABSOLUTE_URI: record = new AbsoluteUriRecord(recordJSON); break;
						case NDEFRecordType.EMPTY: record = new EmptyRecord(recordJSON); break;
						case NDEFRecordType.EXTERNAL_TYPE: record = new ExternalTypeRecord(recordJSON); break;
						case NDEFRecordType.MIME_MEDIA: record = new MimeMediaRecord(recordJSON); break;
						case NDEFRecordType.SMART_POSTER: record = new SmartPosterRecord(recordJSON); break;
						case NDEFRecordType.TEXT: record = new TextRecord(recordJSON); break;
						case NDEFRecordType.UNKNOWN: record = new UnknownRecord(recordJSON); break;
						case NDEFRecordType.URI: record = new UriRecord(recordJSON); break;
					}

					records.Add(record);
				}
			}
			else
			{
				records = new List<NDEFRecord>();
			}

			jsonObject.TryGetString("tag_id", out tagID);

			int writeStateValue;
			jsonObject.TryGetInt("write_state", out writeStateValue);
			writeState = (NDEFMessageWriteState)writeStateValue;

			int writeErrorValue;
			jsonObject.TryGetInt("write_error", out writeErrorValue);
			writeError = (NDEFMessageWriteError)writeErrorValue;
		}

		public JSONObject ToJSON()
		{
			JSONObject jsonObject = new JSONObject();

			JSONArray recordsJSON = new JSONArray();
			int length = records.Count;
			for(int i = 0; i < length; i++)
			{
				recordsJSON.Add(records[i].ToJSON());
			}
			jsonObject.Add("records", recordsJSON);

			jsonObject.Add("tag_id", tagID);
			jsonObject.Add("write_state", (int)writeState);
			jsonObject.Add("write_failure_reason", (int)writeError);

			return jsonObject;
		}
	}
}
