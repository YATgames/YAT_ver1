using UnityEngine;

namespace Assets.Scripts.Common.DataBundles
{
	[CreateAssetMenu(fileName = "EditorSettings" , menuName = "DataBundle/EditorSettings" , order = 0)]
	public class EditorSettings : Singleton<EditorSettings>
	{
		public override void Initialize()
		{
			base.Initialize();
		}
	}
}
