using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common.DI;
using DXApp_AppData.Item;
using DXApp_AppData.Table;
using Assets.Scripts.PrefabModel;
using DXApp_AppData.Enum;

namespace Assets.Scripts.Managers
{
	public class ResourcesManager : UnitySingleton<ResourcesManager>
    {
        [DependuncyInjection(typeof(ConfigManager))]
        private ConfigManager _configManager;
        [DependuncyInjection(typeof(ItemManager))]
        private ItemManager _itemManager;

        private Dictionary<string, Sprite> _imageCaches = new Dictionary<string, Sprite>();
        private Dictionary<string, GameObject> _modelCaches = new Dictionary<string, GameObject>();

        public override void Initialize()
        {
            DependuncyInjection.Inject(this);
            base.Initialize();
        }

        #region :::::::: 프리팹 로드
        public void ModelingSetting(Transform parent)
        {
            for (int i = 0; i < _itemManager.PartsList.Count; i++)
            {

                var path = string.Format("Model/GhostPartsPrefabs/{0}", _itemManager.PartsList[i].ID);
                var item = LoadAndInit(path, parent);
                if (item != null)
                {
                    var modelBase = item.GetComponent<ModelPrefabBase>();
                    modelBase.Parent = parent;
                    modelBase.gameObject.SetActive(false);

                    if (!_modelCaches.ContainsKey(_itemManager.PartsList[i].ID))
                    {
                        _modelCaches.Add(_itemManager.PartsList[i].ID, item);
                    }
                    else
                    {
                        _modelCaches[_itemManager.PartsList[i].ID] = item;
                    }
                }
            }
        }
        public void DoggabiSetting(Transform parent)
        {
            for (int i = 0; i < _itemManager.Doggabies.Count; i++)
            {

                var path = string.Format("Model/GhostPartsPrefabs/{0}", _itemManager.Doggabies[i].ID);
                var item = LoadAndInit(path, parent);
                if (item != null)
                {
                    var modelBase = item.GetComponent<ModelPrefabBase>();
                    modelBase.Parent = parent;
                    modelBase.gameObject.SetActive(false);

                    if (!_modelCaches.ContainsKey(_itemManager.Doggabies[i].ID))
                    {
                        _modelCaches.Add(_itemManager.Doggabies[i].ID, item);
                    }
                    else
                    {
                        _modelCaches[_itemManager.Doggabies[i].ID] = item;
                    }
                }
            }
        }
        public static void ResetModels(string id)
        {
            if (Instance._modelCaches.ContainsKey(id))
            {
                var parent = Instance._modelCaches[id].GetComponent<ModelPrefabBase>().Parent;
                Instance._modelCaches[id].transform.SetParent(parent);
                Instance._modelCaches[id].gameObject.SetActive(false);
            }
        }

        public static GameObject GetModel(string id)
        {
            if (!Instance._modelCaches.ContainsKey(id))
            {
                Debug.LogWarning("GetModel Problem! id = " + id);
                return null;
            }

            var item = Instance._modelCaches[id];
            item.gameObject.SetActive(true);

            return item;
        }

        public static GameObject LoadAndInit(string path, Transform parent)
        {
            var pathLoad = Load(path);
            if(pathLoad == null)
            {
                Debug.LogWarning("LoadAndInit Error! path = " + path);
                return null;
            }

            var item = Instantiate(pathLoad, parent);
            var transform = item.GetComponent<Transform>();

            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            return item;
        }

        public static GameObject Load(string path)
        {
            return Resources.Load<GameObject>(path);
        }
        #endregion

        #region :::::::::: 이미지
        public static Sprite GetImages(string id)
        {
            if(!Instance._imageCaches.ContainsKey(id))
            {
                Debug.LogError("GetPartsImage Error! Not Found Key! Key = " + id);
                return null;
            }
            return Instance._imageCaches[id];
        }

		public static Sprite GetPathImage(string subPath , string name)
		{
			if (Instance._imageCaches.ContainsKey(name) == false)
			{
				var path = string.Format("{0}{1}", subPath, name);
				var sprite = Resources.Load<Sprite>(path);
				Instance._imageCaches.Add(name, sprite);
				return sprite;
			}

			return Instance._imageCaches[name];
		}

		public static Sprite GetTheme(Theme theme)
		{
			return GetTheme(theme.ID);
		}

		public static Sprite GetCaseColor(string color)
		{
			return GetTheme(color);
		}

		public static Sprite GetTheme(string themeID)
		{
			return GetPathImage("Sprites/Themes/", themeID.ToString());
		}

		public void ItemResourceSetting()
        {
            for (int i = 0; i < _itemManager.PartsList.Count; i++)
            {
                PartsImageLoad(_itemManager.PartsList[i]);
            }

            for (int i = 0; i < _configManager.FigureTypes.Length; i++)
            {
                FigureTypeImageLoad(_configManager.FigureTypes[i]);
            }
        }

        private void FigureTypeImageLoad(FigureTypeTable figuretype)
        {
            if(_imageCaches.ContainsKey(figuretype.FigureType.ToString()))
            {
                Debug.LogWarning("FigureTypeImageLoad Problem! Already Generated Key! key = " + figuretype.FigureType.ToString());
            }

            string id = figuretype.FigureType.ToString();
            var path_ID = string.Format("Sprites/Icons/{0}","Icon_Propety" + id);
            var sprite = Resources.Load<Sprite>(path_ID);
            _imageCaches.Add(id, sprite);
        }
        private void PartsImageLoad(Parts ds)
        {
            if(_imageCaches.ContainsKey(ds.ID))
            {
                Debug.LogWarning("PartsImageLoad Problem! Already Generated Key! Key = " + ds.ID);
            }
            var path_ID = string.Format("Sprites/Items/{0}", ds.ID);
            var sprite = Resources.Load<Sprite>(path_ID);
            _imageCaches.Add(ds.ID, sprite);
        }
    }
    #endregion
}

