using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Common;
using Assets.Scripts.UI.Popup.Sup;
using DXApp_AppData.Item;
using System.Drawing;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class AskBreakInView : MonoBehaviour
    {
        [SerializeField] private BreakAction _breakAction;

        [SerializeField] private Button _breakButton;
        [SerializeField] private Button _returnButton;

        private string _title;

        private void Start()
        {
            AddEvent();
        }
        private void AddEvent()
        {
            _breakButton.OnClickAsObservable().Subscribe(_ =>
            {
                var figureArchive = PlayerViewModel.Instance.FigureArchive;
                var customFigure = figureArchive as CustomFigureInstance;
                ConnectionManager.Instance.DecompositionCustomFigure(customFigure);
                SystemLoading.Show(SystemLoading.LoadingSize.Big, this);
            }).AddTo(gameObject);

            PlayerViewModel.Instance.ServerRespones.AsObservable().Subscribe(res =>
            {
                if (gameObject.activeSelf)
                {
                    SystemLoading.Hide(this);
                    _breakAction.BreakeAction(_title);
                    gameObject.SetActive(false);
                }
            }).AddTo(gameObject);

            _returnButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false)).AddTo(gameObject);
        }

        public void SetData(string name)
        {
            _title = name;
        }
    }
}
