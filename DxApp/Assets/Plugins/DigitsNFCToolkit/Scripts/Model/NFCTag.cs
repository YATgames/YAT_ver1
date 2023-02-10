using DigitsNFCToolkit.JSON;
using System;

namespace DigitsNFCToolkit
{
	/// <summary>Enum for the types of NFC Technology</summary>
	public enum NFCTechnology
	{
		UNKNOWN,
		ISO_DEP,
		MIFARE_CLASSIC,
		MIFARE_ULTRALIGHT,
		NDEF,
		NDEF_FORMATABLE,
		NFC_A,
		NFC_B,
		NFC_BARCODE,
		NFC_F,
		NFC_V
	}

	/// <summary>Class for information about a NFC Tag</summary>
	public class NFCTag: IJSONSerializable
	{
		/// <summary>The id of the tag</summary>
		private String id;

		/// <summary>List of technologies supported by the tag</summary>
		private NFCTechnology[] technologies;

		/// <summary>Name of the manufacturer</summary>
		private String manufacturer;

		/// <summary>Indicates if this tag is writable</summary>
		private bool writable;

		/// <summary>The max write size (in bytes) if writable</summary>
		private int maxWriteSize;

		/// <summary>The id of the tag</summary>
		public String ID { get { return id; } }

		/// <summary>List of technologies supported by the tag</summary>
		public NFCTechnology[] Technologies { get { return technologies; } }

		/// <summary>Name of the manufacturer</summary>
		public String Manufacturer { get { return manufacturer; } }

		/// <summary>Indicates if this tag is writable</summary>
		public bool Writable { get { return writable; } }

		/// <summary>The max write size (in bytes) if writable</summary>
		public int MaxWriteSize { get { return maxWriteSize; } }

		public NFCTag(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public void ParseJSON(JSONObject jsonObject)
		{
			jsonObject.TryGetString("id", out id);

			JSONArray technologiesJSON = new JSONArray();
			if(jsonObject.TryGetArray("technologies", out technologiesJSON))
			{
				int length = technologiesJSON.Length;
				technologies = new NFCTechnology[length];
				for(int i = 0; i < length; i++)
				{
					int technologyValue = technologiesJSON[i].Integer;
					technologies[i] = (NFCTechnology)technologyValue;
				}
			}
			else
			{
				technologies = new NFCTechnology[0];
			}

			jsonObject.TryGetString("manufacturer", out manufacturer);
			jsonObject.TryGetBoolean("writable", out writable);
			jsonObject.TryGetInt("max_write_size", out maxWriteSize);
		}

		public JSONObject ToJSON()
		{
			JSONObject jsonObject = new JSONObject();
			jsonObject.Add("id", id);

			JSONArray technologiesJSON = new JSONArray();
			int length = technologies.Length;
			for(int i = 0; i < length; i++)
			{
				technologiesJSON.Add((int)technologies[i]);
			}
			jsonObject.Add("technologies", technologiesJSON);

			jsonObject.Add("manufacturer", manufacturer);
			jsonObject.Add("writable", writable);
			jsonObject.Add("max_write_size", maxWriteSize);

			return jsonObject;
		}
	}
}
