using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using DXApp_AppData.Server;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.CloudScriptModels;
using System;
namespace Assets.Scripts.Managers
{
	public partial class ConnectionManager : UnitySingleton<ConnectionManager>
	{
		[DependuncyInjection(typeof(FlowManager))]
		private FlowManager _flowManager;
		[DependuncyInjection(typeof(GameManager))]
		private GameManager _gameManager;
		[DependuncyInjection(typeof(ConfigManager))]
		private ConfigManager _configManager;
		[DependuncyInjection(typeof(ItemManager))]
		private ItemManager _itemManager;
		[DependuncyInjection(typeof(PlayerViewModel))]
		private PlayerViewModel _playerViewModel;
        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourcesManager;
        private string _titleID { get { return PlayFabSettings.TitleId; } }
		private string _entityId;
		private string _entityType;

		public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);
		}

		#region ::::::: Send & Recive
		public void SendRequest<T>(Req req_data, Action<T> resAction) where T : Res
		{
			var req = new ExecuteFunctionRequest();
			req.Entity = new EntityKey() { Id = _entityId, Type = _entityType };
			req.FunctionName = req_data.FunctionName;
			req.FunctionParameter = req_data;
			req.GeneratePlayStreamEvent = true;
			LogManager.ServeRequst(req_data);

			PlayFabCloudScriptAPI.ExecuteFunction(req
				, result => Result(result, resAction)
				, e => Error(req.FunctionName, e));
		}

		private void Result<T>(ExecuteFunctionResult r, Action<T> resAction) where T : Res
		{
			var result = GetResult<T>(r.FunctionResult);

			LogManager.ServeResponse(result);
			resAction?.Invoke(result);
		}

		private void Error(string functuonName, PlayFabError e)
		{
			//ItemSystemWindow.ShowError(e.Error); -- 에러 공용 팝업 나중에 제작되면 추가할 것
			LogManager.ServerError("{0} : {1}", functuonName, e.GenerateErrorReport());
		}
		#endregion

		private T GetResult<T>(object result)
		{
			return JsonConvert.DeserializeObject<T>(result.ToString());
		}
	}

}
