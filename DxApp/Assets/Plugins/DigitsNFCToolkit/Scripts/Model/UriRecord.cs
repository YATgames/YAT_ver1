using DigitsNFCToolkit.JSON;
using System;

namespace DigitsNFCToolkit
{
	/// <summary>Record class that contains an uri</summary>
	[Serializable]
	public class UriRecord: NDEFRecord
	{
		/// <summary>The full uri</summary>
		public string fullUri;

		/// <summary>The uri part of the full uri (gets determined automatically)</summary>
		public string uri;

		/// <summary>The abbreviated part of the full uri (gets determined automatically)</summary>
		public string protocol;

		public UriRecord(string fullUri)
		{
			this.type = NDEFRecordType.URI;
			this.fullUri = fullUri;
		}

		public UriRecord(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public override void ParseJSON(JSONObject jsonObject)
		{
			base.ParseJSON(jsonObject);

			jsonObject.TryGetString("full_uri", out fullUri);
			jsonObject.TryGetString("uri", out uri);
			jsonObject.TryGetString("protocol", out protocol);
		}

		public override JSONObject ToJSON()
		{
			JSONObject jsonObject = base.ToJSON();
			jsonObject.Add("full_uri", fullUri);
			jsonObject.Add("uri", uri);
			jsonObject.Add("protocol", protocol);

			return jsonObject;
		}
	}
}
