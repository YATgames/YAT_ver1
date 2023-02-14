using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class DialogueManager : UnitySingleton<DialogueManager>
    {
        // TODO : Novel_{num} 타입의 프리팹을 불러와서 대사 상황을 진행시킴
        public Novel_01 novel_01;
        public Novel_02 novel_02;

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void UnInitialize()
        {
            base.UnInitialize();
        }
        public void CreatePrefab(int num)
        {
            novel_01.gameObject.SetActive(false);
        }

    }
}