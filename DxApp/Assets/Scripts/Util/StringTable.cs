using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Util
{
	[ExecuteInEditMode]
	[Serializable]
	public static class StringTable
	{
		private const string SaveLanguageCode = "Language";
		public static SystemLanguage CurrentLanguage { get; private set; }

		private readonly static Dictionary<SystemLanguage, StringTableMapper> _dics = new Dictionary<SystemLanguage, StringTableMapper>
		{
			{ SystemLanguage.Korean , new StringTableMapper() },
			{ SystemLanguage.English , new StringTableMapper() }
		};

		public static void Init()
		{
            var data = Resources.Load("Tables/string_table") as TextAsset;

            var csvFile = CSVReader.Read(data.text);

            for (int i = 0; i < csvFile.Count; i++)
            {
                var key = csvFile[i]["Key"].ToString();
                _dics[SystemLanguage.English].AddString(key, csvFile[i]["English"].ToString());
                _dics[SystemLanguage.Korean].AddString(key, csvFile[i]["Korean"].ToString());
            }

            CurrentLanguage = GetLanguage();
        }

		public static void SetLanguage(SystemLanguage sys_lan)
		{
			if (sys_lan == CurrentLanguage) return;

			CurrentLanguage = sys_lan;
			PlayerPrefs.SetString(SaveLanguageCode, sys_lan.ToString());
		}

		private static SystemLanguage GetLanguage()
		{
			var lan_str = PlayerPrefs.GetString(SaveLanguageCode, SystemLanguage.Korean.ToString());
			var lan = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), lan_str);
			return lan;
		}

		public static string FromStringTable(this string str, params object[] args)
		{
			return _dics.ContainsKey(CurrentLanguage) 
				? _dics[CurrentLanguage].GetString(str, args) 
				: string.Format(str , args);
		}

		public static string ToCommaValue(this long value)
		{
			return value.ToString("###,###");
		}
		public static string ToCommaValue(this int value)
		{
			return value.ToString("###,###");
		}
	}

	public class StringTableMapper
	{
		private readonly Dictionary<string, string> _stringDic = new Dictionary<string, string>();

		public void AddString(string key, string value)
		{
			_stringDic.Add(key, value);
		}

		public string GetString(string key, params object[] args)
		{
			try
			{
				return _stringDic.ContainsKey(key) ?
					string.Format(_stringDic[key], args) :
					string.Format(key, args);
			}
			catch
			{
				var str = string.Format(key, args);
				LogManager.Error("Dic = {0} , {1}" , _stringDic == null ? "NULL" : "TRUE" , str);
				return str;
			}
		}
	}
}