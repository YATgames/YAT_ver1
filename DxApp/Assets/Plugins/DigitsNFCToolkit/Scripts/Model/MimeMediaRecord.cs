using DigitsNFCToolkit.JSON;
using System;

namespace DigitsNFCToolkit
{
	/// <summary>Record class that refers to a media file</summary>
	[Serializable]
	public class MimeMediaRecord: NDEFRecord
	{
		/// <summary>The type of the media file in mime format</summary>
		public string mimeType;

		/// <summary>The data of the media file</summary>
		public byte[] mimeData;

		public MimeMediaRecord(string mimeType, byte[] mimeData)
		{
			this.type = NDEFRecordType.MIME_MEDIA;
			this.mimeType = mimeType;
			this.mimeData = mimeData;
		}

		public MimeMediaRecord(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public override void ParseJSON(JSONObject jsonObject)
		{
			base.ParseJSON(jsonObject);

			jsonObject.TryGetString("mime_type", out mimeType);

			string mimeDataString = null;
			if(jsonObject.TryGetString("mime_data", out mimeDataString))
			{
				mimeData = Util.DecodeBase64UrlSafe(mimeDataString);
			}
		}

		public override JSONObject ToJSON()
		{
			JSONObject jsonObject = base.ToJSON();
			jsonObject.Add("mime_type", mimeType);
			jsonObject.Add("mime_data", Util.EncodeBase64UrlSafe(mimeData));

			return jsonObject;
		}
	}
}
