using DXApp_AppData.Enum;
using DXApp_AppData.Item;
using DXApp_AppData.Model;
using Newtonsoft.Json;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Util
{
	public static class PlayfabDataUtil
	{
		public static Dictionary<string, string> ToModelDatas(params Model[] objs)
		{
			Dictionary<string, string> dics = new Dictionary<string, string>();
			for (int i = 0; i < objs.Length; i++)
			{
				dics.Add(objs[i].Code, JsonConvert.SerializeObject(objs[i]));
			}
			return dics;
		}

        public static InventoryModel ToInventory(this List<ItemInstance> items)
		{
			var inventory = new InventoryModel();
			for (int i = 0; i < items.Count; i++)
			{
				var item = items[i].ToItem();
                inventory.AddItem(item);
            }
            return inventory;
		}
		public static PlayfabItemInstance ToItem(this ItemInstance item)
		{
			if (item.ItemClass.CheckBundle<OriginFigure>()) return item.ToItem<OriginFigureInstance>();
			else if (item.ItemClass.CheckItem<CustomFigure>())
			{
				var customFigure = (CustomFigureInstance)item.ToItem<CustomFigureInstance>();
				customFigure.CustomData = new CustomFigureCustomData()
				{
					Name = item.CustomData["Name"],
					Parts = JsonConvert.DeserializeObject<List<string>>(item.CustomData["Parts"]),
					FigureType = (FigureType)Enum.Parse(typeof(FigureType), item.CustomData["FigureType"])
				};
				return customFigure;
			}
			else if (item.ItemClass.CheckItem<Parts>()) return item.ToItem<PartsInstance>();
			else if (item.ItemClass.CheckItem<Theme>()) return item.ToItem<ThemeInstance>();
			else if (item.ItemClass.CheckItem<DoggabiFigure>()) return item.ToItem<DoggabiFigureInstance>();
			return null;
		}

		public static PlayfabItemInstance ToItem<T>(this ItemInstance item) where T : PlayfabItemInstance, new()
		{
			return new T()
			{
				ID = item.ItemId,
				InstanceID = item.ItemInstanceId,
				PurchaseDate = item.PurchaseDate
			};
		}

		public static T ToClientItem<T>(this CatalogItem catalogItem) where T : PlayfabItem, new()
		{
			T item = new T();
			item.ID = catalogItem.ItemId;
			return item;
		}

		public static T ToBundleItem<T>(this CatalogItem item) where T : PlayfabBundleItem , new ()
		{
			T bundleItem = new T();
			bundleItem.ID = item.ItemId;
			bundleItem.Items = item.Bundle.BundledItems;
			return bundleItem;
		}

		public static bool CheckClass<T>(this string name) where T : class
		{
			return typeof(T).Name.Equals(name);
		}

        public static bool CheckItem<T>(this string name) where T : PlayfabItem
        {
            return typeof(T).Name.Equals(name);
        }

        public static bool CheckBundle<T>(this string name) where T : PlayfabBundleItem
        {
            return typeof(T).Name.Equals(name);
        }
        
    }
}
