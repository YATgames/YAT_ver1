using System;
using DigitsNFCToolkit.JSON;

namespace DigitsNFCToolkit
{
    public enum NDEFReadError { NONE, HAS_UNREADABLE_RECORDS, UNKNOWN, USER_CANCEL, TIMEOUT }

    public class NDEFReadResult
    {
        private bool success;
        private NDEFReadError error;
        private NDEFMessage message;
        private string tagID;

        public bool Success{ get { return success; }}
        public NDEFReadError Error{ get { return error; }}
        public NDEFMessage Message{ get { return message; }}
        public string TagID{ get { return tagID; }}

        public NDEFReadResult(JSONObject jsonObject)
        {
            ParseJSON(jsonObject);
        }

        public void ParseJSON(JSONObject jsonObject)
        {
            jsonObject.TryGetBoolean("success", out success);

            int errorValue;
            jsonObject.TryGetInt("error", out errorValue);
            error = (NDEFReadError)errorValue;

            JSONObject messageJSON = null;
            if(jsonObject.TryGetObject("message", out messageJSON))
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
