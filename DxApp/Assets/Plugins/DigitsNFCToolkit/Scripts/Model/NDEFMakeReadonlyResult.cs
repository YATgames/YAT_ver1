using DigitsNFCToolkit.JSON;

namespace DigitsNFCToolkit
{
	public enum NDEFMakeReadonlyError { NONE, NOT_ALLOWED, UNKNOWN }

	public class NDEFMakeReadonlyResult
	{
		private bool success;
		private NDEFMakeReadonlyError error;
		private string tagID;

		public bool Success { get { return success; } }
		public NDEFMakeReadonlyError Error { get { return error; } }
		public string TagID { get { return tagID; } }

		public NDEFMakeReadonlyResult(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public void ParseJSON(JSONObject jsonObject)
		{
			jsonObject.TryGetBoolean("success", out success);

			int errorValue;
			jsonObject.TryGetInt("error", out errorValue);
			error = (NDEFMakeReadonlyError)errorValue;

			jsonObject.TryGetString("tag_id", out tagID);
		}
	}
}
