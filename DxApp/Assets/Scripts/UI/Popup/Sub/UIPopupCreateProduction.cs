using Assets.Scripts.Common.DI;
using UnityEngine;
using Assets.Scripts.UI.Popup.PopupView;
using Assets.Scripts.Managers;
using System;

namespace Assets.Scripts.UI.Popup.Sup
{
    public class UIPopupCreateProduction : PopupSub
    {
        [DependuncyInjection(typeof(ItemManager))]
        private ItemManager _itemManager;
        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourcesManager;

        [SerializeField] private CreateProductionView _createProductionView;

        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _createProductionView.UIPopupCreateProduction = this;
            _createProductionView.ItemManager = _itemManager;
            _createProductionView.ResourcesManager = _resourcesManager;
        }

        public override void Show(params object[] data)
        {
            base.Show(data);
            string title = data[0].ToString();
            string id = data[1].ToString();
            bool isSkip = Convert.ToBoolean(data[2].ToString());
            _createProductionView.SetData(title, id, isSkip);
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
