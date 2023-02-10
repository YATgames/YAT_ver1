using System;
using System.Collections.Generic;
using Assets.Scripts.Common.DataBundles;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using DXApp_AppData.Enum;
using DXApp_AppData.Item;
using DXApp_AppData.Server;
using DXApp_AppData.Table;
using PlayFab;
using PlayFab.ClientModels;

namespace Assets.Scripts.Managers
{
	public partial class ConnectionManager
	{
		public void CustomLogin(string id)
		{
			var request = new LoginWithCustomIDRequest();
			request.TitleId = _titleID;
			request.CreateAccount = true;
			request.CustomId = id;
			request.InfoRequestParameters = EditorPlayfabSetting.Instance.PlayerInfo;

			LogManager.Server("REQUEST ::: 플레이팹 커스텀로그인");

			PlayFabClientAPI.LoginWithCustomID(request,
				r =>
				{
					Response_CustomLogin(r);

					if (r.InfoResultPayload.AccountInfo.TitleInfo.DisplayName == null)
						UpdateNickName(id);
				},
				e => Error("CustomLogin", e));
		}

		public void UpdateNickName(string nickName)
		{
			LogManager.Server("REQ ::: 플레이팹 업데이트 Display");
			var request = new UpdateUserTitleDisplayNameRequest();
			request.DisplayName = nickName;

            PlayFabClientAPI.UpdateUserTitleDisplayName(request,
				r => Response_UpdateNickName(r),
				e => Error("UpdateNickName", e));
		}

		public void GetItemConfig(LoginResult keepLogin)
		{
			var request = new GetCatalogItemsRequest();
			PlayFabClientAPI.GetCatalogItems(request,
				r =>
				{
					Response_GetItemConfig(r);
					CustomLogin_AfterSetting(keepLogin);
				},
				e => Error("GetItemConfig", e));
		}

		public void UserInit()
		{
			var req = new Req_UserInit();
			SendRequest<Res_UserInit>(req, Response_UserInit);
		}
        public void DailyCheck()
        {
            var req = new Req_DailyCheck();
            SendRequest<Res_DailyCheck>(req, Response_DailyCheck);
        }
        public void ClearedQuest(QuestInfo questInfo)
        {
            var req = new Req_ClearedQuest();
			req.ClearedQuestID = questInfo.ID.ToString();
			req.QuestRewardID = questInfo.Reward;
            SendRequest<Res_ClearedQuest>(req, Response_ClearedQuest);
        }

        public void CreateFigure(string id)
		{
            var req = new Req_CreateFigure();
			req.OriginFigureID = id;
			SendRequest<Res_CreateFigure>(req, Response_CreateFigure);
		}

        public void TaggingDoggabi(string id)
        {
            var req = new Req_TaggingDoggabi();
            req.DoggabiFigureID = id;
            SendRequest<Res_TaggingDoggabi>(req, Response_TaggingDoggabi);
        }

        public void CreateCustomFigure(string name , List<PartsInstance> partsList , FigureType type)
		{
			var req = new Req_CreateCustomFigure();
			req.Name = name;
			req.PartsList = partsList;
			req.Type = type;
			SendRequest<Res_CreateCustomFigure>(req, Response_CreateCustomFigure);
		}

		public void DecompositionCustomFigure(CustomFigureInstance customFigure)
		{
			var req = new Req_DecompositionCustomFigure();
			req.CustomFigure = customFigure;
			SendRequest<Res_DecompositionCustomFigure>(req, Response_DecompositionCustomFigure);
		}

        public void RegistCase(CaseInfo caseInfo)
        {
            var req = new Req_RegistCase();
            req.Case = caseInfo;
            SendRequest<Res_RegistCase>(req, Response_RegistCase);
        }

        public void ChangeFavoriteFigure(string id)
        {
            var req = new Req_ChagneFavoriteFigure();
            req.FigureInstanceID = id;
            SendRequest<Res_ChagneFavoriteFigure>(req, Response_ChangeFavoriteFigure);
        }
    }
}
