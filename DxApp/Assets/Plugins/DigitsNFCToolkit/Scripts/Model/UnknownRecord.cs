using DigitsNFCToolkit.JSON;
using System;

namespace DigitsNFCToolkit
{
	/// <summary>Record class that defines unknown data</summary>
	[Serializable]
	public class UnknownRecord: NDEFRecord
	{
		public UnknownRecord(string uri)
		{
			this.type = NDEFRecordType.UNKNOWN;
		}

		public UnknownRecord(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}
	}
}
