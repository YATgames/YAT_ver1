using DigitsNFCToolkit.JSON;

namespace DigitsNFCToolkit
{
	public class NDEFPushResult
	{
		private bool success;
		private NDEFMessage message;

		public bool Success { get { return success; } }
		public NDEFMessage Message { get { return message; } }

		public NDEFPushResult(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public void ParseJSON(JSONObject jsonObject)
		{
			jsonObject.TryGetBoolean("success", out success);

			JSONObject messageJSON = null;
			if(jsonObject.TryGetObject("message", out messageJSON))
			{
				message = new NDEFMessage(messageJSON);
			}
			else
			{
				message = new NDEFMessage();
			}
		}
	}
}
