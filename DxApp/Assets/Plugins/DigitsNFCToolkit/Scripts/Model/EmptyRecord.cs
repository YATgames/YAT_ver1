using DigitsNFCToolkit.JSON;
using System;

namespace DigitsNFCToolkit
{
	/// <summary>Record class that is empty and does not hold any content</summary>
	[Serializable]
	public class EmptyRecord: NDEFRecord
	{
		public EmptyRecord(string uri)
		{
			this.type = NDEFRecordType.EMPTY;
		}

		public EmptyRecord(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}
	}
}
