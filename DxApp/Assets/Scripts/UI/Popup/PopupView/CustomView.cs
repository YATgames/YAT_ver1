using Assets.Scripts.Common;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Util;
using DXApp_AppData.Table;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DXApp_AppData.Item;
using System.Resources;
using Assets.Scripts.PrefabModel;
using Assets.Scripts.Util;
using DG.Tweening;
using UnityEngine.Rendering;

namespace Assets.Scripts.UI.Popup.PopupView
{
	public class CustomView : MonoBehaviour
	{
		public FlowManager FlowManager { get; set; }
		public PlayerViewModel PlayerViewModel { get; set; }
		public PopupManager PopupManager { get; set; }

		public int CaseNumber { get; set; }

		[SerializeField] private Animator _buttonsAni;

		//[SerializeField] private Button _buttonsButton;

		[SerializeField] private Button _figureActionButton;
		[SerializeField] private Button _temmaButton;
		[SerializeField] private Button _figureInvenButton;

        [SerializeField] private RectTransform _inputRange;
		//[SerializeField] private Text _themaNameText;
		[SerializeField] private Image _temmaImage;
		[SerializeField] private GameObject _figureParent;

		//[SerializeField] private GameObject _caseImage; // testFigureParent -> _caseImage
		//[SerializeField] private Text _testFigureText; // 임시 (피규어 정보 가지고있음)

		[SerializeField] private DropItem _dropItem;

		private int _caseNumber;
		private bool _isOpen = false;

        [Space(10)]
        [Header("ActionFigure")]
		[SerializeField] private Transform _actionContentParents;
		[SerializeField] private GameObject _partyContnets;
		[SerializeField] private GameObject _circusContnets;
		[SerializeField] private GameObject _bathroomContnets;
		[SerializeField] private Transform _characterTransform;
        private float _figureScale = 700f;
        private GameObject _actionFigure;

		private enum ActionContents
		{
			PARTY,
			CIRCUS,
			BATHROOM,
			NONE,
		}
		private ActionContents _actionContents;
        public ResourcesManager ResourcesManager { get; set; }

        public void Initialize()
		{
            AddEvent();
            IntroButotnAni();
        }
        private void IntroButotnAni()
        {
            _inputRange.sizeDelta = new Vector2(Screen.width, Screen.height);
            _isOpen = true;
            _buttonsAni.SetBool("Open", true);
        }
        private void AddEvent()
        {
            /*
             * 피드백 : 처음 생성될때 애니메이션 재생되고 유지되도록(버튼 인터렉션 해제)
            _buttonsButton.OnClickAsObservable().Subscribe(_ =>
            {
                _isOpen = !_isOpen;
                _buttonsAni.SetBool("Open", _isOpen);
            }).AddTo(gameObject);
            */

            _temmaButton.OnClickAsObservable().Subscribe(_ =>
            {
                FlowManager.AddSubPopup(PopupStyle.CaseInven, CaseNumber);
            }).AddTo(gameObject);

            _figureInvenButton.OnClickAsObservable().Subscribe(_ =>
            {
                FlowManager.AddSubPopup(PopupStyle.DisplayInven, CaseNumber);
            }).AddTo(gameObject);

            _dropItem.OnDrop.AsObservable().Subscribe(_ =>
            {
                LogManager.Log("먼지 제거 액션 동작");
            }).AddTo(gameObject);

            _figureActionButton.OnClickAsObservable().Subscribe(_ =>
            {
                // 검사 후 정보에 맞는 컨텐츠 재생
                AddActionContents();
            }).AddTo(gameObject);

            ResourcesManager.ModelingSetting(_characterTransform);
        }
        public void SetData(CaseInfo data)
		{
			// 테마 확인
			if (string.IsNullOrEmpty(data.ThemeID))
			{
				//_themaNameText.text = string.Empty;
				_temmaImage.sprite = ResourcesManager.GetCaseColor(data.CaseColor);
            }
            else
			{
				var thema = PlayerViewModel.Inventory.Themes.FirstOrDefault(v => v.ID == data.ThemeID);
				if(thema != null)
				{
					//_themaNameText.text = thema.Name.FromStringTable();
				}
				_temmaImage.sprite = ResourcesManager.GetTheme(data.ThemeID);
            }

            // 피규어 확인
            if (string.IsNullOrEmpty(data.FigureInstanceID))
			{
				//_caseImage.SetActive(false);
			}
			else
			{
				//_caseImage.SetActive(true);
				var customFigure = PlayerViewModel.Inventory.CustomFigures.FirstOrDefault(v => v.InstanceID == data.FigureInstanceID);
				var originFigure = PlayerViewModel.Inventory.OriginFigures.FirstOrDefault(v => v.InstanceID == data.FigureInstanceID);

                if (customFigure != null)
				{
					//_testFigureText.text = string.Format("CustomFigure \n\n ID : {0} \n\n InstanceID : {1}", customFigure.ID, customFigure.InstanceID);
                    #region LoadCustomFigure
                    ResetModels();
                    var items = customFigure.CustomData.Parts;
                    GameObject body = null;
                    GameObject head = null;
                    GameObject deco = null;

                    for (int i = 0; i < items.Count; i++)
                    {
                        /// 1 - Head , 2 - Body , 3 - PostDeco , 4 - PreDeco
                        var item = items[i];
                        switch (item.Substring(0, 1))
                        {
                            case "1": head = ResourcesManager.GetModel(item); break;
                            case "2": body = ResourcesManager.GetModel(item); break;
                            case "3": deco = ResourcesManager.GetModel(item); break;
                            case "4": deco = ResourcesManager.GetModel(item); break;
                        }
                    }

                    Model_Body _body = body?.GetComponent<Model_Body>();
                    Model_Head _head = head?.GetComponent<Model_Head>();
                    Model_Deco _deco = deco?.GetComponent<Model_Deco>();


                    try
                    {
                        _body.transform.localPosition = Vector3.zero;
                        _head?.gameObject.transform.SetParent(_body.HeadPosition);
                        _head?.ResetTransform();

                        switch (_deco.DecoType)
                        {
                            case DecoType.OverHead:
                                _deco.gameObject.transform.SetParent(_head.DecoPosition);
                                break;

                            case DecoType.Wing:
                                _deco.gameObject.transform.SetParent(_body.WingPos);
                                break;

                            case DecoType.Tail:
                                _deco.gameObject.transform.SetParent(_body.TailPos);
                                break;
                        }

                        _deco.ResetTransform();
                        GetFigure();
                    }
                    catch
                    {

                    }
                    #endregion

                }
                else if (originFigure != null)
				{
					//_testFigureText.text = string.Format("OriginFigure \n\n ID : {0} \n\n InstanceID : {1}", originFigure.ID, originFigure.InstanceID);
                    #region LoadOriginFigure
                    ResetModels();

                    var items = ItemManager.Instance.Figures.FirstOrDefault(v => v.ID == originFigure.ID).Items;

                    GameObject body = null;
                    GameObject head = null;
                    GameObject deco = null;

                    for (int i = 0; i < items.Count; i++)
                    {
                        /// 1 - Head , 2 - Body , 3 - PostDeco , 4 - PreDeco
                        var item = items[i];
                        switch (item.Substring(0, 1))
                        {
                            case "1": head = ResourcesManager.GetModel(item); break;
                            case "2": body = ResourcesManager.GetModel(item); break;
                            case "3": deco = ResourcesManager.GetModel(item); break;
                            case "4": deco = ResourcesManager.GetModel(item); break;
                        }
                    }

                    Model_Body _body = body?.GetComponent<Model_Body>();
                    Model_Head _head = head?.GetComponent<Model_Head>();
                    Model_Deco _deco = deco?.GetComponent<Model_Deco>();
                    GetFigure();
                    //UnityEngine.Debug.Log("Body : " + body.name);
                    _actionFigure = body.gameObject;
                    try
                    {
                        _body.transform.localPosition = Vector3.zero;
                        _head?.gameObject.transform.SetParent(_body.HeadPosition);
                        _head?.ResetTransform();
                        switch (_deco.DecoType)
                        {
                            case DecoType.OverHead:
                                _deco.gameObject.transform.SetParent(_head.DecoPosition);
                                break;

                            case DecoType.Wing:
                                _deco.gameObject.transform.SetParent(_body.WingPos);
                                break;

                            case DecoType.Tail:
                                _deco.gameObject.transform.SetParent(_body.TailPos);
                                break;
                        }

                        _deco.ResetTransform();
                    }
                    catch
                    {

                    }
                    #endregion
                }
                //else
					//_caseImage.SetActive(false);

            }
            CheckContents(data);
		}
        private void ResetModels()
        {
            for (int i = 0; i < ItemManager.Instance.PartsList.Count; i++)
            {
                ResourcesManager.ResetModels(ItemManager.Instance.PartsList[i].ID);
            }
        }
        private void GetFigure()
        {
                
        }
        private void CheckContents(CaseInfo data)
        {
			// 케이스 유무 검사
            if (string.IsNullOrEmpty(data.ThemeID))
            {
                _actionContents = ActionContents.NONE;
            }
            else
            {
                switch (data.ThemeID)
                {
                    case "60001":
                        _actionContents = ActionContents.CIRCUS;
                        break;
                    case "60002":
                        _actionContents = ActionContents.PARTY;
                        break;
                    case "60003":
                        _actionContents = ActionContents.BATHROOM;
                        break;
                    default:
                        UnityEngine.Debug.LogError("<color=red> CaseInfo.ThemeID 를 제대로 가져오지 못했다 <color>");
                        _actionContents = ActionContents.NONE;
                        break;
                }
            }
        }

        private void AddActionContents()
        {
            // 케이스가 없다면 동작하지 않는다
            if (_actionContents == ActionContents.NONE)
            {
                return;
            }
            else
            {
                _buttonsAni.SetBool("Open", false);
                //_buttonsButton.image.raycastTarget = false;
                //_themaNameText.gameObject.SetActive(false);
                GameObject contents = null;
                switch (_actionContents)
                {
                    case ActionContents.PARTY:
                        FigureScaleSetting();
                        contents = Instantiate(_partyContnets, _actionContentParents.transform);
                        break;
                    case ActionContents.CIRCUS:
                        FigureScaleSetting();
                        contents = Instantiate(_circusContnets, _actionContentParents.transform); break;
                    case ActionContents.BATHROOM:
                        FigureScaleSetting();
                        contents = Instantiate(_bathroomContnets, _actionContentParents.transform);
                        contents.GetComponent<ActionContents_Bathroom>().GetFigure(_actionFigure);
                        break;
                    default:
                        contents = Instantiate(_partyContnets, _actionContentParents.transform);
                        UnityEngine.Debug.LogError("<color=red> CaseInfo.ThemeID 를 제대로 가져오지 못했다</color>");
                        break;
                }
            }
        }
        // 피규어 확대 하고나서 기본 사이즈 상태로 조정
        private void FigureScaleSetting()
        {
            _characterTransform.DOScale(_figureScale, 0.5f).SetEase(Ease.Linear);
            _characterTransform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.Linear);
            this.GetComponent<DragAndRotateCharacter>().enabled = false;
            this.GetComponent<PinchObject>().enabled = false;
        }

    }
}
