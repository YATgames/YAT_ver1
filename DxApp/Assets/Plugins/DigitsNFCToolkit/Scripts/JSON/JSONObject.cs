using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DigitsNFCToolkit.JSON
{
	public class JSONObject: IEnumerable<KeyValuePair<string, JSONValue>>
	{
		private enum JSONParsingState
		{
			Object,
			Array,
			EndObject,
			EndArray,
			Key,
			Value,
			KeyValueSeparator,
			ValueSeparator,
			String,
			Number,
			Boolean,
			Null
		}

		private readonly IDictionary<string, JSONValue> values = new Dictionary<string, JSONValue>();
		private static readonly Regex unicodeRegex = new Regex(@"\\u([0-9a-fA-F]{4})");
		private static readonly byte[] unicodeBytes = new byte[2];

		public JSONObject()
		{
		}

		public JSONObject(JSONObject other)
		{
			values = new Dictionary<string, JSONValue>();

			if(other != null)
			{
				foreach(var keyValuePair in other.values)
				{
					values[keyValuePair.Key] = new JSONValue(keyValuePair.Value);
				}
			}
		}

		public bool ContainsKey(string key)
		{
			return values.ContainsKey(key);
		}

		public JSONValue GetValue(string key)
		{
			JSONValue value;
			values.TryGetValue(key, out value);
			return value;
		}

		public ICollection<string> GetKeys()
		{
			return values.Keys;
		}

		public string GetString(string key)
		{
			var value = GetValue(key);
			if(value == null)
			{
				Debug.LogError(key + "(string) == null");
				return string.Empty;
			}
			return value.String;
		}

		public bool TryGetString(string key, out string stringValue)
		{
			var value = GetValue(key);
			if(value != null)
			{
				stringValue = value.String;
				return true;
			}
			else
			{
				stringValue = default(string);
				return false;
			}
		}

		public double GetNumber(string key)
		{
			var value = GetValue(key);
			if(value == null)
			{
				Debug.LogError(key + " == null");
				return double.NaN;
			}
			return value.Double;
		}

		public bool TryGetDouble(string key, out double doubleValue)
		{
			var value = GetValue(key);
			if(value != null)
			{
				doubleValue = value.Double;
				return true;
			}
			else
			{
				doubleValue = default(double);
				return false;
			}
		}

		public bool TryGetFloat(string key, out float floatValue)
		{
			var value = GetValue(key);
			if(value != null)
			{
				floatValue = value.Float;
				return true;
			}
			else
			{
				floatValue = default(float);
				return false;
			}
		}

		public bool TryGetInt(string key, out int intValue)
		{
			var value = GetValue(key);
			if(value != null)
			{
				intValue = value.Integer;
				return true;
			}
			else
			{
				intValue = default(int);
				return false;
			}
		}

		public JSONObject GetObject(string key)
		{
			var value = GetValue(key);
			if(value == null)
			{
				Debug.LogError(key + " == null");
				return null;
			}
			return value.Object;
		}

		public bool TryGetObject(string key, out JSONObject objectValue)
		{
			var value = GetValue(key);
			if(value != null)
			{
				objectValue = value.Object;
				return true;
			}
			else
			{
				objectValue = default(JSONObject);
				return false;
			}
		}

		public bool GetBoolean(string key)
		{
			var value = GetValue(key);
			if(value == null)
			{
				Debug.LogError(key + " == null");
				return false;
			}
			return value.Boolean;
		}

		public bool TryGetBoolean(string key, out bool boolValue)
		{
			var value = GetValue(key);
			if(value != null)
			{
				boolValue = value.Boolean;
				return true;
			}
			else
			{
				boolValue = default(bool);
				return false;
			}
		}

		public JSONArray GetArray(string key)
		{
			var value = GetValue(key);
			if(value == null)
			{
				Debug.LogError(key + " == null");
				return null;
			}
			return value.Array;
		}

		public bool TryGetArray(string key, out JSONArray arrayValue)
		{
			var value = GetValue(key);
			if(value != null)
			{
				arrayValue = value.Array;
				return true;
			}
			else
			{
				arrayValue = default(JSONArray);
				return false;
			}
		}

		public JSONValue this[string key]
		{
			get { return GetValue(key); }
			set { values[key] = value; }
		}

		public void Add(string key, JSONValue value)
		{
			values[key] = value;
		}

		public void Add(KeyValuePair<string, JSONValue> pair)
		{
			values[pair.Key] = pair.Value;
		}

		public static JSONObject Parse(string jsonString)
		{
			if(string.IsNullOrEmpty(jsonString))
			{
				return null;
			}

			JSONValue currentValue = null;

			var keyList = new Stack<string>();
			var state = JSONParsingState.Object;

			for(var startPosition = 0; startPosition < jsonString.Length; ++startPosition)
			{
				startPosition = SkipWhitespace(jsonString, startPosition);

				switch(state)
				{
					case JSONParsingState.Object:
						if(jsonString[startPosition] != '{')
						{
							return Fail('{', startPosition);
						}

						JSONValue newObj = new JSONObject();
						if(currentValue != null)
						{
							newObj.Parent = currentValue;
						}
						currentValue = newObj;

						state = JSONParsingState.Key;
						break;
					case JSONParsingState.EndObject:
						if(jsonString[startPosition] != '}')
						{
							return Fail('}', startPosition);
						}

						if(currentValue.Parent == null)
						{
							return currentValue.Object;
						}

						switch(currentValue.Parent.Type)
						{

							case JSONValueType.Object:
								currentValue.Parent.Object.values[keyList.Pop()] = new JSONValue(currentValue.Object);
								break;

							case JSONValueType.Array:
								currentValue.Parent.Array.Add(new JSONValue(currentValue.Object));
								break;

							default:
								return Fail("valid object", startPosition);

						}
						currentValue = currentValue.Parent;

						state = JSONParsingState.ValueSeparator;
						break;
					case JSONParsingState.Key:
						if(jsonString[startPosition] == '}')
						{
							--startPosition;
							state = JSONParsingState.EndObject;
							break;
						}

						var key = ParseString(jsonString, ref startPosition);
						if(key == null)
						{
							return Fail("key string", startPosition);
						}
						keyList.Push(key);
						state = JSONParsingState.KeyValueSeparator;
						break;
					case JSONParsingState.KeyValueSeparator:
						if(jsonString[startPosition] != ':')
						{
							return Fail(':', startPosition);
						}
						state = JSONParsingState.Value;
						break;
					case JSONParsingState.ValueSeparator:
						switch(jsonString[startPosition])
						{

							case ',':
								state = currentValue.Type == JSONValueType.Object ? JSONParsingState.Key : JSONParsingState.Value;
								break;

							case '}':
								state = JSONParsingState.EndObject;
								--startPosition;
								break;

							case ']':
								state = JSONParsingState.EndArray;
								--startPosition;
								break;

							default:
								return Fail(", } ]", startPosition);
						}
						break;
					case JSONParsingState.Value:
						{
							var c = jsonString[startPosition];
							if(c == '"')
							{
								state = JSONParsingState.String;
							}
							else if(char.IsDigit(c) || c == '-')
							{
								state = JSONParsingState.Number;
							}
							else
								switch(c)
								{

									case '{':
										state = JSONParsingState.Object;
										break;

									case '[':
										state = JSONParsingState.Array;
										break;

									case ']':
										if(currentValue.Type == JSONValueType.Array)
										{
											state = JSONParsingState.EndArray;
										}
										else
										{
											return Fail("valid array", startPosition);
										}
										break;

									case 'f':
									case 't':
										state = JSONParsingState.Boolean;
										break;


									case 'n':
										state = JSONParsingState.Null;
										break;

									default:
										return Fail("beginning of value", startPosition);
								}

							--startPosition; //To re-evaluate this char in the newly selected state
							break;
						}
					case JSONParsingState.String:
						var str = ParseString(jsonString, ref startPosition);
						if(str == null)
						{
							return Fail("string value", startPosition);
						}

						switch(currentValue.Type)
						{

							case JSONValueType.Object:
								currentValue.Object.values[keyList.Pop()] = new JSONValue(str);
								break;

							case JSONValueType.Array:
								currentValue.Array.Add(str);
								break;

							default:
								Debug.LogError("Fatal error, current JSON value not valid");
								return null;
						}

						state = JSONParsingState.ValueSeparator;
						break;
					case JSONParsingState.Number:
						var number = ParseNumber(jsonString, ref startPosition);
						if(double.IsNaN(number))
						{
							return Fail("valid number", startPosition);
						}

						switch(currentValue.Type)
						{

							case JSONValueType.Object:
								currentValue.Object.values[keyList.Pop()] = new JSONValue(number);
								break;

							case JSONValueType.Array:
								currentValue.Array.Add(number);
								break;

							default:
								Debug.LogError("Fatal error, current JSON value not valid");
								return null;
						}

						state = JSONParsingState.ValueSeparator;

						break;
					case JSONParsingState.Boolean:
						if(jsonString[startPosition] == 't')
						{
							if(jsonString.Length < startPosition + 4 ||
								jsonString[startPosition + 1] != 'r' ||
								jsonString[startPosition + 2] != 'u' ||
								jsonString[startPosition + 3] != 'e')
							{
								return Fail("true", startPosition);
							}

							switch(currentValue.Type)
							{

								case JSONValueType.Object:
									currentValue.Object.values[keyList.Pop()] = new JSONValue(true);
									break;

								case JSONValueType.Array:
									currentValue.Array.Add(new JSONValue(true));
									break;

								default:
									Debug.LogError("Fatal error, current JSON value not valid");
									return null;
							}

							startPosition += 3;
						}
						else
						{
							if(jsonString.Length < startPosition + 5 ||
								jsonString[startPosition + 1] != 'a' ||
								jsonString[startPosition + 2] != 'l' ||
								jsonString[startPosition + 3] != 's' ||
								jsonString[startPosition + 4] != 'e')
							{
								return Fail("false", startPosition);
							}

							switch(currentValue.Type)
							{

								case JSONValueType.Object:
									currentValue.Object.values[keyList.Pop()] = new JSONValue(false);
									break;

								case JSONValueType.Array:
									currentValue.Array.Add(new JSONValue(false));
									break;

								default:
									Debug.LogError("Fatal error, current JSON value not valid");
									return null;
							}

							startPosition += 4;
						}

						state = JSONParsingState.ValueSeparator;
						break;
					case JSONParsingState.Array:
						if(jsonString[startPosition] != '[')
						{
							return Fail('[', startPosition);
						}

						JSONValue newArray = new JSONArray();
						if(currentValue != null)
						{
							newArray.Parent = currentValue;
						}
						currentValue = newArray;

						state = JSONParsingState.Value;
						break;
					case JSONParsingState.EndArray:
						if(jsonString[startPosition] != ']')
						{
							return Fail(']', startPosition);
						}

						if(currentValue.Parent == null)
						{
							return currentValue.Object;
						}

						switch(currentValue.Parent.Type)
						{

							case JSONValueType.Object:
								currentValue.Parent.Object.values[keyList.Pop()] = new JSONValue(currentValue.Array);
								break;

							case JSONValueType.Array:
								currentValue.Parent.Array.Add(new JSONValue(currentValue.Array));
								break;

							default:
								return Fail("valid object", startPosition);
						}
						currentValue = currentValue.Parent;

						state = JSONParsingState.ValueSeparator;
						break;
					case JSONParsingState.Null:
						if(jsonString[startPosition] == 'n')
						{
							if(jsonString.Length < startPosition + 4 ||
								jsonString[startPosition + 1] != 'u' ||
								jsonString[startPosition + 2] != 'l' ||
								jsonString[startPosition + 3] != 'l')
							{
								return Fail("null", startPosition);
							}

							switch(currentValue.Type)
							{

								case JSONValueType.Object:
									currentValue.Object.values[keyList.Pop()] = new JSONValue(JSONValueType.Null);
									break;

								case JSONValueType.Array:
									currentValue.Array.Add(new JSONValue(JSONValueType.Null));
									break;

								default:
									Debug.LogError("Fatal error, current JSON value not valid");
									return null;
							}

							startPosition += 3;
						}
						state = JSONParsingState.ValueSeparator;
						break;
				}
			}
			Debug.LogError("Unexpected end of string");
			return null;
		}

		private static int SkipWhitespace(string str, int pos)
		{
			for(; pos < str.Length && char.IsWhiteSpace(str[pos]); ++pos) ;
			return pos;
		}

		private static string ParseString(string str, ref int startPosition)
		{
			if(str[startPosition] != '"' || startPosition + 1 >= str.Length)
			{
				Fail('"', startPosition);
				return null;
			}

			var endPosition = str.IndexOf('"', startPosition + 1);
			if(endPosition <= startPosition)
			{
				Fail('"', startPosition + 1);
				return null;
			}

			while(str[endPosition - 1] == '\\')
			{
				endPosition = str.IndexOf('"', endPosition + 1);
				if(endPosition <= startPosition)
				{
					Fail('"', startPosition + 1);
					return null;
				}
			}

			var result = string.Empty;

			if(endPosition > startPosition + 1)
			{
				result = str.Substring(startPosition + 1, endPosition - startPosition - 1);
			}

			startPosition = endPosition;

			// Parse Unicode characters that are escaped as \uXXXX
			while(true)
			{
				Match m = unicodeRegex.Match(result);
				if(!m.Success)
				{
					break;
				}

				string s = m.Groups[1].Captures[0].Value;
				unicodeBytes[1] = byte.Parse(s.Substring(0, 2), NumberStyles.HexNumber);
				unicodeBytes[0] = byte.Parse(s.Substring(2, 2), NumberStyles.HexNumber);
				s = Encoding.Unicode.GetString(unicodeBytes, 0, unicodeBytes.Length);

				result = result.Replace(m.Value, s);
			}

			result = SanitizeJSONString(result);

			return result;
		}

		private static string SanitizeJSONString(string jsonStringValue)
		{
			StringBuilder stringBuilder = new StringBuilder(jsonStringValue);
			stringBuilder.Replace("\\\"", "\"");
			stringBuilder.Replace("\\\\", "\\");
			stringBuilder.Replace("\\/", "/");
			stringBuilder.Replace("\\\b", "\b");
			stringBuilder.Replace("\\\f", "\f");
			stringBuilder.Replace("\\\n", "\n");
			stringBuilder.Replace("\\\r", "\r");
			stringBuilder.Replace("\\\t", "\t");
			return stringBuilder.ToString();
		}

		private static double ParseNumber(string str, ref int startPosition)
		{
			if(startPosition >= str.Length || (!char.IsDigit(str[startPosition]) && str[startPosition] != '-'))
			{
				return double.NaN;
			}

			var endPosition = startPosition + 1;

			for(; endPosition < str.Length && str[endPosition] != ',' && str[endPosition] != ']' && str[endPosition] != '}'; ++endPosition) ;

			double result;
			if(!double.TryParse(str.Substring(startPosition, endPosition - startPosition), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result))
			{
				return double.NaN;
			}
			startPosition = endPosition - 1;
			return result;
		}

		private static JSONObject Fail(char expected, int position)
		{
			return Fail(new string(expected, 1), position);
		}

		private static JSONObject Fail(string expected, int position)
		{
			Debug.LogError("Invalid json string, expecting " + expected + " at " + position);
			return null;
		}

		/// <returns>String representation of this JSONObject</returns>
		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append('{');

			foreach(var pair in values)
			{
				stringBuilder.Append("\"" + pair.Key + "\"");
				stringBuilder.Append(':');
				stringBuilder.Append(pair.Value.ToString());
				stringBuilder.Append(',');
			}
			if(values.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		public IEnumerator<KeyValuePair<string, JSONValue>> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}

		/// <summary>
		/// Empty this JSONObject of all values.
		/// </summary>
		public void Clear()
		{
			values.Clear();
		}

		/// <summary>
		/// Remove the JSONValue attached to the given key.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(string key)
		{
			if(values.ContainsKey(key))
			{
				values.Remove(key);
			}
		}
	}
}