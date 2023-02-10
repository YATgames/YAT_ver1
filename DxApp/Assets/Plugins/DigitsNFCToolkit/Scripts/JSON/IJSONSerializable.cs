namespace DigitsNFCToolkit.JSON
{
	/// <summary>Interface for classes that are JSON Serializable</summary>
	public interface IJSONSerializable
	{
		/// <summary>Initializes this class by parsing given JSONObject</summary>
		void ParseJSON(JSONObject jsonObject);

		/// <summary>Creates a JSONObject representation of this class</summary>
		JSONObject ToJSON();
	}
}
