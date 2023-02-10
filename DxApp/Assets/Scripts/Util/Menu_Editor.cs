#if UNITY_EDITOR
using Assets.Scripts.UI.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
// 임시 빌드용 막
namespace Assets.Scripts.Util
{
	public class Menu_Editor
	{
		[MenuItem("Util/Define/DEV")]
		static void SetDEV()
		{
			SetDefines("DEV");
		}

		[MenuItem("Util/Define/QA")]
		static void SetQA()
		{
			SetDefines("QA");
		}

		[MenuItem("Util/Define/LIVE")]
		static void SetLIVE()
		{
			SetDefines("LIVE");
		}

		static void SetDefines(string symbols)
		{
			PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);

		}

		[MenuItem("Util/StringTable/Set_Kr")]
		static void SetStringTable_Kr()
		{
			SetStringTable(SystemLanguage.Korean);
		}

		[MenuItem("Util/StringTable/Set_En")]
		static void SetStringTable_En()
		{
			SetStringTable(SystemLanguage.English);
		}

		static void SetStringTable(SystemLanguage len)
		{
			var data = Resources.Load("string_table") as TextAsset;
			var csvFile = CSVReader.Read(data.text);

			var dic = new Dictionary<SystemLanguage, StringTableMapper>
			{
				{SystemLanguage.Korean , new StringTableMapper()},
				{SystemLanguage.English , new StringTableMapper()}
			};
			for (int i = 0; i < csvFile.Count; i++)
			{
				var key = csvFile[i]["Key"].ToString();
				dic[SystemLanguage.English].AddString(key, csvFile[i]["English"].ToString());
				dic[SystemLanguage.Korean].AddString(key, csvFile[i]["Korean"].ToString());
			}

			var items = Resources.FindObjectsOfTypeAll(typeof(StringTableComponent));
			foreach (var item in items)
			{
				var c = item as StringTableComponent;

				var str = dic.ContainsKey(len) ? dic[len].GetString(c.Key, c.Args) : string.Format(c.Key , c.Args);
				c.SetData(str);
			}


			var imageItems = Resources.FindObjectsOfTypeAll(typeof(ImageTableComponenet));
			foreach(var item in imageItems)
			{
				var i = item as ImageTableComponenet;
				i.SetData(len);
			}
		}

	}
}
#endif
