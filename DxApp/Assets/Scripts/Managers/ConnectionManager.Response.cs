using Assets.Scripts.UI;
using Assets.Scripts.Util;
using DXApp_AppData.Enum;
using DXApp_AppData.Model;
using DXApp_AppData.Server;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Diagnostics;
using System.Resources;


namespace Assets.Scripts.Managers
{
	public partial class ConnectionManager
	{
		private void Response_CustomLogin(LoginResult response)
		{
			_entityId = response.EntityToken.Entity.Id;
			_entityType = response.EntityToken.Entity.Type;
			GetItemConfig(response);
		}

		private void Response_UpdateNickName(UpdateUserTitleDisplayNameResult response)
		{
			_playerViewModel.Account.NickName = response.DisplayName;
			LogManager.ServeResponse(response);
		}

		//Config 저장 때문에 GetStore 후 셋팅가능..
		private void CustomLogin_AfterSetting(LoginResult loginResult)
		{
			var payload = loginResult.InfoResultPayload;

			#region :::::::: Configs
			_configManager.SetTables(loginResult.InfoResultPayload.TitleData);
			#endregion

			#region :::::::: ACCOUNT
			var account = new AccountModel();
			account.NewlyCreated = loginResult.NewlyCreated;
			account.PlayFabId = loginResult.PlayFabId;
			account.SeesionTicket = loginResult.SessionTicket;
			account.LastLoginTime = loginResult.LastLoginTime;
			account.NickName = payload.AccountInfo.TitleInfo.DisplayName;
			_playerViewModel.Account = account;
			#endregion

			#region :::::::: Inventory,ItemManager,Resource Settings
			_playerViewModel.Inventory = loginResult.InfoResultPayload.UserInventory.ToInventory();

			// 리소스매니저 이미지 셋팅..
            _resourcesManager.ItemResourceSetting();
            #endregion

            #region :::::::: PLAYER
            var readData = payload.UserReadOnlyData;
			if (readData == null || readData.Count <= 0)
			{
				LogManager.Server("Response_CustomLogin : UserReadOnlyData : NULL");
				UserInit();
			}
			else
			{
				PlayerModel player = new PlayerModel();
				player = JsonConvert.DeserializeObject<PlayerModel>(readData[player.Code].Value);

                _playerViewModel.Player = player;
			}
			#endregion
		}

		private void Response_GetItemConfig(GetCatalogItemsResult response)
		{
			_itemManager.SettingItems(response.Catalog);
			LogManager.ServeResponse(response);
		}

		/// <summary>
		/// Azure Functions
		/// </summary>
		/// <param name="response"></param>
		private void Response_UserInit(Res_UserInit response)
		{
			_playerViewModel.Player = response.Player;
		}
        private void Response_DailyCheck(Res_DailyCheck response)
        {
            _playerViewModel.Account.LastLoginTime = DateTime.UtcNow;
            _playerViewModel.Player = response.Player;

			if (_playerViewModel.Inventory.Doggabis.Count == 1)
				_flowManager.AddSubPopup(PopupStyle.DailyCheck);

            _playerViewModel.ServerRespones.Invoke();
        }
        private void Response_ClearedQuest(Res_ClearedQuest response)
        {
            _playerViewModel.Player = response.Player;
			_playerViewModel.Inventory = response.InventoryModel;

            _playerViewModel.ServerRespones.Invoke();
        }

        private void Response_CreateFigure(Res_CreateFigure response)
		{
			_playerViewModel.Inventory = response.Inventory;
			_playerViewModel.ServerRespones.Invoke();
		}
        private void Response_TaggingDoggabi(Res_TaggingDoggabi response)
        {
            _playerViewModel.Inventory = response.Inventory;
			_playerViewModel.Player = response.Player;
			if(_playerViewModel.Inventory.Doggabis.Count == 1) _playerViewModel.Account.LastLoginTime = DateTime.MinValue;
            _playerViewModel.ServerRespones.Invoke();
        }
        private void Response_CreateCustomFigure(Res_CreateCustomFigure response)
		{
			if(response.Error != ServerError.Success)
			{
				switch (response.Error)
				{
					case ServerError.CreateCustomFigure_NotFoundBody: LogManager.Error("Request 몸통 없음");break;
					case ServerError.CreateCustomFigure_NotFoundHead: LogManager.Error("Request 머리 없음"); break;
					case ServerError.CreateCustomFigure_NotFoundItem: LogManager.Error("인벤토리에 없는 아이템"); break;
                }
                return;
            }
			_playerViewModel.Player = response.Player;
            _playerViewModel.Inventory = response.Inventory;

            _playerViewModel.ServerRespones.Invoke();
        }

        private void Response_DecompositionCustomFigure(Res_DecompositionCustomFigure response)
		{
			if (response.Error != ServerError.Success)
			{
				switch (response.Error)
				{
					case ServerError.CreateCustomFigure_NotFoundItem: LogManager.Error("인벤토리에 없는 아이템"); break;
				}
				return;
			}

			_playerViewModel.Inventory = response.Inventory;
			_playerViewModel.Player = response.Player;

			_playerViewModel.ServerRespones.Invoke();
        }

        private void Response_RegistCase(Res_RegistCase response)
        {
            _playerViewModel.Player = response.Player;

        }
        private void Response_ChangeFavoriteFigure(Res_ChagneFavoriteFigure response)
        {
            _playerViewModel.Player = response.Player;
            _playerViewModel.ServerRespones.Invoke();
        }

		
	}
}
