using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Common;
using DXApp_AppData.Enum;
using DXApp_AppData.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class AskCombineInView : MonoBehaviour
    {
        [SerializeField] private CombineAction _combineAction;

        [SerializeField] private Button _combineButton;
        [SerializeField] private Button _returnButton;

        [SerializeField] private Image _popupBGImage;

        [SerializeField] private Text _TicketX;
        [SerializeField] private Text _TicketCountTex;

        [SerializeField] private Sprite[] _bgSprites;

        [SerializeField] private GameObject[] _inventoryObjs;

        private string title = string.Empty;
        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            _combineButton.OnClickAsObservable().Subscribe(_ =>
            {
                var figureTypeTable = PlayerViewModel.Instance.FigureTypeTable;
                FigureType figureType = FigureType.None;
                if (figureTypeTable != null) figureType = figureTypeTable.FigureType;

                ConnectionManager.Instance.CreateCustomFigure(title, PlayerViewModel.Instance.PartsArchive, figureType);
                SystemLoading.Show(SystemLoading.LoadingSize.Big, this);
            }).AddTo(gameObject);

            PlayerViewModel.Instance.ServerRespones.AsObservable().Subscribe(res =>
            {
                SystemLoading.Hide(this);
                for (int i = 0; i < _inventoryObjs.Length; i++)
                {
                    _inventoryObjs[i].SetActive(true);
                }
                _combineAction.ClickCombineButton(title);
                gameObject.SetActive(false);
            }).AddTo(gameObject);
            _returnButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false)).AddTo(gameObject);
        }

        public void SetData(string name)
        {
            title = name;
            PlayerModel player = PlayerViewModel.Instance.Player;
            InventoryModel inventory = PlayerViewModel.Instance.Inventory;

            if (inventory.Doggabis.Count == 0 || player.CombineTicketCount == 0)
            {
                _popupBGImage.sprite = _bgSprites[0];
                _TicketX.gameObject.SetActive(false);
                _TicketCountTex.gameObject.SetActive(false);
                _combineButton.interactable = false;
            }
            else
            {
                _popupBGImage.sprite = _bgSprites[1];
                _combineButton.interactable = true;
                _TicketX.gameObject.SetActive(true);
                _TicketCountTex.gameObject.SetActive(true);
                if (player.CombineTicketCount > 0) _TicketCountTex.text = player.CombineTicketCount.ToString();
                else if (player.CombineTicketCount < 0) _TicketCountTex.text = "¡Ä";
            }
        }
    }
}
