using System;

namespace DigitsNFCToolkit.JSON
{
	public enum JSONValueType
	{
		String,
		Number,
		Object,
		Array,
		Boolean,
		Null
	}

	public class JSONValue
	{
		public JSONValue(JSONValueType type)
		{
			Type = type;
		}

		public JSONValue(string str)
		{
			Type = JSONValueType.String;
			String = str;
		}

		public JSONValue(double number)
		{
			Type = JSONValueType.Number;
			Double = number;
		}

		public JSONValue(JSONObject obj)
		{
			if(obj == null)
			{
				Type = JSONValueType.Null;
			}
			else
			{
				Type = JSONValueType.Object;
				Object = obj;
			}
		}

		public JSONValue(JSONArray array)
		{
			Type = JSONValueType.Array;
			Array = array;
		}

		public JSONValue(bool boolean)
		{
			Type = JSONValueType.Boolean;
			Boolean = boolean;
		}

		public JSONValue(JSONValue value)
		{
			Type = value.Type;
			switch(Type)
			{
				case JSONValueType.String:
					String = value.String;
					break;

				case JSONValueType.Boolean:
					Boolean = value.Boolean;
					break;

				case JSONValueType.Number:
					Double = value.Double;
					break;

				case JSONValueType.Object:
					if(value.Object != null)
					{
						Object = new JSONObject(value.Object);
					}
					break;

				case JSONValueType.Array:
					Array = new JSONArray(value.Array);
					break;
			}
		}

		public JSONValueType Type { get; private set; }
		public string String { get; set; }
		public double Double { get; set; }
		public float Float { get { return (float)Double; } set { Double = value; } }
		public int Integer { get { return (int)Double; } set { Double = value; } }
		public JSONObject Object { get; set; }
		public JSONArray Array { get; set; }
		public bool Boolean { get; set; }
		public JSONValue Parent { get; set; }

		public static implicit operator JSONValue(string str)
		{
			return new JSONValue(str);
		}

		public static implicit operator JSONValue(double number)
		{
			return new JSONValue(number);
		}

		public static implicit operator JSONValue(JSONObject obj)
		{
			return new JSONValue(obj);
		}

		public static implicit operator JSONValue(JSONArray array)
		{
			return new JSONValue(array);
		}

		public static implicit operator JSONValue(bool boolean)
		{
			return new JSONValue(boolean);
		}

		public override string ToString()
		{
			switch(Type)
			{
				case JSONValueType.Object: return Object.ToString();
				case JSONValueType.Array: return Array.ToString();
				case JSONValueType.Boolean: return Boolean ? "true" : "false";
				case JSONValueType.Number: return Double.ToString();
				case JSONValueType.String: return "\"" + String + "\"";
				case JSONValueType.Null: return "null";
			}
			return "null";
		}
	}
}
