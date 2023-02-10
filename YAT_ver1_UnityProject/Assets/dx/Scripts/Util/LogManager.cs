using Newtonsoft.Json;
using UnityEngine;
// using DXApp_AppData.Server; // 서버는 아직 안불러올거임

namespace Assets.Scripts.Util
{
    public class LogManager
    {
        public static void KeepServer(string format, params object[] args)
        {
            var str = string.Format("<color=#EE82EE>KeepServer\t{0}</color>", format);
            Log(str, args);
        }

        public static void Server(string format, params object[] args)
        {
            var str = string.Format("<color=#FFFF00>Server\t{0}</color>", format);
            Log(str, args);
        }

        public static void ServerKeepLog(string format, params object[] args)
        {
            var str = string.Format("<color=#A52A2A>ServerKEEP\t{0}</color>", format);
            Log(format, args);
        }

        public static void ServerError(string format, params object[] args)
        {
            var str = string.Format("<color=#A52A2A>ServerError\t{0}</color>", format);
            Log(str, args);
        }

        /*
        public static void ServeRequst(Req req)
        {
            var str = string.Format("<color=#00FFFF>REQ\t{0}</color>", req.FunctionName);
            Log(str);
        }*/

        public static void ServeResponse(object response)
        {
            var str = string.Format("<color=#00FFFF>RES\t{0}</color>", JsonConvert.SerializeObject(response));
            Log(str);
        }
        /*
        public static void ServeResponse(Res response)
        {
            var str = string.Format("<color=#00FFFF>RES\t{0}\t{1}</color>", response.FunctionName, JsonConvert.SerializeObject(response));
            Log(str);
        }*/

        public static void Log(string log)
        {
            Debug.Log(log);
        }

        public static void Log(string format, params object[] args)
        {
            Debug.LogFormat(format, args);
        }

        public static void Error(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }

    }
}
