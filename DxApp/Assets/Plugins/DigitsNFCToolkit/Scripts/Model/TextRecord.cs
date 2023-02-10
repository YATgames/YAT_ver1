using DigitsNFCToolkit.JSON;
using System;

namespace DigitsNFCToolkit
{
	/// <summary>Record class that contains text</summary>
	[Serializable]
	public class TextRecord: NDEFRecord
	{
		/// <summary>Supported TextEncodings</summary>
		public enum TextEncoding { UTF8, UTF16 }

		/// <summary>The text value</summary>
		public string text;

		/// <summary>The (2 character) language code/summary>
		public string languageCode;

		/// <summary>The text encoding/summary>
		public TextEncoding textEncoding;

		public TextRecord(string text)
		{
			this.type = NDEFRecordType.TEXT;
			this.text = text;
			this.languageCode = "en"; //Default is English (see https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes for other languages). NOTE: only 2 byte/character language codes allowed
			this.textEncoding = TextEncoding.UTF8;
		}

		public TextRecord(string text, string languageCode)
		{
			this.type = NDEFRecordType.TEXT;
			this.text = text;
			this.languageCode = languageCode;
			this.textEncoding = TextEncoding.UTF8;
		}

		public TextRecord(string text, string languageCode, TextEncoding textEncoding)
		{
			this.type = NDEFRecordType.TEXT;
			this.text = text;
			this.languageCode = languageCode;
			this.textEncoding = textEncoding;
		}

		public TextRecord(JSONObject jsonObject)
		{
			ParseJSON(jsonObject);
		}

		public override void ParseJSON(JSONObject jsonObject)
		{
			base.ParseJSON(jsonObject);

			jsonObject.TryGetString("text", out text);
			jsonObject.TryGetString("language_code", out languageCode);

			int textEncodingValue;
			jsonObject.TryGetInt("text_encoding", out textEncodingValue);
			textEncoding = (TextEncoding)textEncodingValue;
		}

		public override JSONObject ToJSON()
		{
			JSONObject jsonObject = base.ToJSON();
			jsonObject.Add("text", text);
			jsonObject.Add("language_code", languageCode);
			jsonObject.Add("text_encoding", (int)textEncoding);

			return jsonObject;
		}
	}
}