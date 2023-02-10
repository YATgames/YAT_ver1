using DigitsNFCToolkit.JSON;
using System;

namespace DigitsNFCToolkit
{
	/// <summary>Record class that refers to an external type not defined by the NDEF standard</summary>
	[Serializable]
	public class ExternalTypeRecord: NDEFRecord
	{
		/// <summary>The name of the domain</summary>
		public string domainName;

		/// <summary>The type of the domain</summary>
		public string domainType;

		/// <summary>The data that needs be send to the domain</summary>
		public byte[] domainData;

		public ExternalTypeRecord(string domainName, string domainType, byte[] domainData)
		{
			this.type = NDEFRecordType.EXTERNAL_TYPE;
			this.domainName = domainName;
			this.domainType = domainType;
			this.domainData = domainData;
		}

		public ExternalTypeRecord(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public override void ParseJSON(JSONObject jsonObject)
		{
			base.ParseJSON(jsonObject);

			jsonObject.TryGetString("domain_name", out domainName);
			jsonObject.TryGetString("domain_type", out domainType);

			string domainDataString = null;
			if(jsonObject.TryGetString("domain_data", out domainDataString))
			{
				domainData = Util.DecodeBase64UrlSafe(domainDataString);
			}
		}

		public override JSONObject ToJSON()
		{
			JSONObject jsonObject = base.ToJSON();
			jsonObject.Add("domain_name", domainName);
			jsonObject.Add("domain_type", domainType);
			jsonObject.Add("domain_data", Util.EncodeBase64UrlSafe(domainData));

			return jsonObject;
		}
	}
}
