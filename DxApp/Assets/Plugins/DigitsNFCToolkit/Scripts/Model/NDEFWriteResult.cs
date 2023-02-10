using System;
using DigitsNFCToolkit.JSON;

namespace DigitsNFCToolkit
{
    public enum NDEFWriteError { NONE, NOT_WRITABLE, INSUFFICIENT_SPACE, UNKNOWN, USER_CANCEL, TIMEOUT }

    public class NDEFWriteResult
    {
        private bool success;
        private NDEFWriteError error;
        private NDEFMessage message;
        private string tagID;

        public bool Success { get { return success; } }
        public NDEFWriteError Error { get { return error; } }
        public NDEFMessage Message { get { return message; } }
        public string TagID { get { return tagID; } }

        public NDEFWriteResult(JSONObject jsonObject)
        {
            ParseJSON(jsonObject);
        }

        public void ParseJSON(JSONObject jsonObject)
        {
            jsonObject.TryGetBoolean("success", out success);

            int errorValue;
            jsonObject.TryGetInt("error", out errorValue);
            error = (NDEFWriteError)errorValue;

            JSONObject messageJSON = null;
            if (jsonObject.TryGetObject("message", out messageJSON))
            {
                message = new NDEFMessage(messageJSON);
            }
            else
            {
                message = new NDEFMessage();
            }

            jsonObject.TryGetString("tag_id", out tagID);
        }
    }
}
