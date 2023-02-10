using DigitsNFCToolkit.JSON;

namespace DigitsNFCToolkit
{
	/// <summary>Enum for the types of NDEF Record</summary>
	public enum NDEFRecordType { ABSOLUTE_URI, EMPTY, EXTERNAL_TYPE, MIME_MEDIA, SMART_POSTER, TEXT, UNKNOWN, URI }

	/// <summary>Base class for NDEF Records</summary>
	public abstract class NDEFRecord: IJSONSerializable
	{
		/// <summary>The type of NDEF Record</summary>
		protected NDEFRecordType type;

		/// <summary>The type of NDEF Record</summary>
		public NDEFRecordType Type { get { return type; } }

		public virtual void ParseJSON(JSONObject jsonObject)
		{
			int typeValue;
			jsonObject.TryGetInt("type", out typeValue);
			type = (NDEFRecordType)typeValue;
		}

		public virtual JSONObject ToJSON()
		{
			JSONObject jsonObject = new JSONObject();
			jsonObject.Add("type", (int)type);

			return jsonObject;
		}
	}
}
