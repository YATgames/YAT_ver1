using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using Newtonsoft.Json;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Assets.Scripts.Managers
{
	public class ItemManager : UnitySingleton<ItemManager>
	{
        public List<Parts> PartsList { get; set; }
		public List<OriginFigure> Figures { get; set; }
		public List<DoggabiFigure> Doggabies { get; set; }
		public List<Theme> Themes { get; set; }
		public override void Initialize()
		{
			base.Initialize();
            DependuncyInjection.Inject(this);
            PartsList = new List<Parts>();
			Doggabies = new List<DoggabiFigure>();
			Figures = new List<OriginFigure>();
			Themes = new List<Theme>();
        }

		public void SettingItems(List<CatalogItem> catalogItems)
		{
			for (int i = 0; i < catalogItems.Count; i++)
			{
				var ci = catalogItems[i];
				var itemClass = ci.ItemClass;

				if (itemClass.CheckItem<Parts>())
				{
					var item = ci.ToClientItem<Parts>();
					item.CustomData = JsonConvert.DeserializeObject<PartsCustomData>(ci.CustomData);
					PartsList.Add(item);
				}
                if (itemClass.CheckItem<DoggabiFigure>())
                {
                    var item = ci.ToClientItem<DoggabiFigure>();
                    Doggabies.Add(item);
                }
				if (itemClass.CheckBundle<OriginFigure>())
				{
					var item = ci.ToBundleItem<OriginFigure>();
					Figures.Add(item);
				}
				if (itemClass.CheckItem<Theme>())
				{
					var item = ci.ToClientItem<Theme>();
                    item.CustomData = JsonConvert.DeserializeObject<ThemeCustomData>(ci.CustomData);
                    Themes.Add(item);
				}
			}
		}

		public Theme GetTheme(ThemeInstance item)
		{
			return Themes.FirstOrDefault(v => v.ID == item.ID);
		}
        public static Theme GetTheme(string id)
        {
            return Instance.Themes.FirstOrDefault(v => v.ID == id);
        }

        public static PartsCustomData GetPartsCustomData(PartsInstance instance)
		{
			var item = GetParts(instance.ID);
			if (item == null) return null;
			else return item.CustomData;
		}

		public static Parts GetParts(string id)
		{
			var parts = Instance.PartsList.FirstOrDefault(v => v.ID == id);
			if(parts == null)
			{
				UnityEngine.Debug.LogError("GetParts Error! Id = " + id);
				return null;
			}

			return parts;
        }
       
    }

}
