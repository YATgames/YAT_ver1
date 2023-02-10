using PlayFab.ClientModels;
using UnityEngine;

namespace Assets.Scripts.Common.DataBundles
{
	[CreateAssetMenu(fileName = "EditorPlayfabSetting", menuName = "DataBundle/EditorPlayfabSetting", order = 1)]
	public class EditorPlayfabSetting : Singleton<EditorPlayfabSetting>
	{
		public GetPlayerCombinedInfoRequestParams PlayerInfo;
		public override void Initialize()
		{
			base.Initialize();
		}
	}
}
