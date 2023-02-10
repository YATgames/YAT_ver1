using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.Common;
using DXApp_AppData.Enum;
using DXApp_AppData.Table;
using Newtonsoft.Json;

namespace Assets.Scripts.Managers
{
	public class ConfigManager : UnitySingleton<ConfigManager>
	{
		private Tables _tables { get; set; }

		public FigureTypeTable[] FigureTypes { get { return _tables.FigureTypeTables; } }
		public QuestInfo[] Quests { get { return _tables.QuestTables; } }

		public void SetTables(Dictionary<string, string> titleData)
		{
			_tables = new Tables();

			var FTCode = Tables.FigureTypeTables_Code;
			_tables.FigureTypeTables = JsonConvert.DeserializeObject<FigureTypeTable[]>(titleData[FTCode]);

			var QTCode = Tables.QuestTables_Code;
			_tables.QuestTables = JsonConvert.DeserializeObject<QuestInfo[]>(titleData[QTCode]);
        }

		public FigureTypeTable GetFigureTypeTable(FigureType figureType)
		{
			var figureTypeTable = _tables.FigureTypeTables.FirstOrDefault(v => v.FigureType == figureType);
			if(figureTypeTable == null)
			{
				UnityEngine.Debug.LogError("GetFigureTypeTable Error! FigureType = "+ figureType.ToString());
				return null;
			}

			return figureTypeTable;
        }
	}
}
