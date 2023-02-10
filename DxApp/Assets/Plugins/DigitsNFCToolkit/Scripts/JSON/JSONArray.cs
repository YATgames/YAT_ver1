using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DigitsNFCToolkit.JSON
{
	public class JSONArray: IEnumerable<JSONValue>
	{
		private readonly List<JSONValue> values = new List<JSONValue>();

		public JSONArray()
		{
		}

		public JSONArray(JSONArray array)
		{
			values = new List<JSONValue>();
			foreach(var v in array.values)
			{
				values.Add(new JSONValue(v));
			}
		}

		public void Add(JSONValue value)
		{
			values.Add(value);
		}

		public JSONValue this[int index]
		{
			get { return values[index]; }
			set { values[index] = value; }
		}

		public int Length
		{
			get { return values.Count; }
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			foreach(var value in values)
			{
				stringBuilder.Append(value.ToString());
				stringBuilder.Append(',');
			}
			if(values.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		public IEnumerator<JSONValue> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}

		public static JSONArray Parse(string jsonString)
		{
			var tempObject = JSONObject.Parse("{ \"array\" :" + jsonString + '}');
			return tempObject == null ? null : tempObject.GetValue("array").Array;
		}

		public void Clear()
		{
			values.Clear();
		}

		public void Remove(int index)
		{
			if(index >= 0 && index < values.Count)
			{
				values.RemoveAt(index);
			}
			else
			{
				Debug.LogError("index out of range: " + index + " (Expected 0 <= index < " + values.Count + ")");
			}
		}

		public static JSONArray operator +(JSONArray lhs, JSONArray rhs)
		{
			var result = new JSONArray(lhs);
			foreach(var value in rhs.values)
			{
				result.Add(value);
			}
			return result;
		}
	}
}
