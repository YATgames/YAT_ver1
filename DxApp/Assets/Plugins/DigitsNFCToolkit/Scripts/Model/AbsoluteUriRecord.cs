using DigitsNFCToolkit.JSON;
using System;

namespace DigitsNFCToolkit
{
	/// <summary>Record class that contains an absolute uri</summary>
	[Serializable]
	public class AbsoluteUriRecord: NDEFRecord
	{
		/// <summary>The absolute uri</summary>
		public string uri;

		public AbsoluteUriRecord(string uri)
		{
			this.type = NDEFRecordType.ABSOLUTE_URI;
			this.uri = uri;
		}

		public AbsoluteUriRecord(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public override void ParseJSON(JSONObject jsonObject)
		{
			base.ParseJSON(jsonObject);

			jsonObject.TryGetString("uri", out uri);
		}

		public override JSONObject ToJSON()
		{
			JSONObject jsonObject = base.ToJSON();
			jsonObject.Add("uri", uri);

			return jsonObject;
		}
	}
}
