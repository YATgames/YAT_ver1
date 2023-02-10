using System;

namespace DigitsNFCToolkit
{
	public class Util
	{
		/// <summary>Encodes given bytes to a Base64 Url safe string</summary>
		public static string EncodeBase64UrlSafe(byte[] bytes)
		{
			string base64String = Convert.ToBase64String(bytes);
			base64String = base64String.Replace("=", "").Replace("/", "_").Replace("+", "-");

			return base64String;
		}

		/// <summary>Decodes given Base64 Url safe string to a byte array</summary>
		public static byte[] DecodeBase64UrlSafe(string base64String)
		{
			base64String = base64String.PadRight(base64String.Length + (4 - base64String.Length % 4) % 4, '=').Replace("_", "/").Replace("-", "+");
			return Convert.FromBase64String(base64String);
		}
	}
}
