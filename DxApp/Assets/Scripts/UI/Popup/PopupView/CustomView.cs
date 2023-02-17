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
using Assets.Scripts.UI.Common;
using System;
using System.Linq.Expressions;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class CustomView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public PopupManager PopupManager { get; set; }
        public ConnectionManager ConnectionManager { get; set; }

        public int CaseNumber { get; set; }

        [SerializeField] DragAndRotateCharacter _dragAndRotateCharacter;

        [SerializeField] private Animator _buttonsAni;

        [SerializeField] private Button _teminateGhost;
        [SerializeField] private Button _teminateTemma;

        [SerializeField] private Button _figureActionButton;
        [SerializeField] private Button _temmaButton;
        [SerializeField] private Button _figureInvenButton;

        [SerializeField] private RectTransform _inputRange;

        [SerializeField] private Image _caseImage; // 케이스 이미지
        [SerializeField] private Image _temmaImage; // 테마 이미지
        [SerializeField] private GameObject _figureParent;

        [SerializeField] private DropItem _dropItem;
        private bool _isOpen = false;
        public Image temmaImage
        {
            get
            {
                return _temmaImage;
            }
        }
        [Space(10)]
        [Header("ActionFigure")]
        [SerializeField] private Transform _actionContentParents;
        [SerializeField] private GameObject _partyContnets;
        [SerializeField] private GameObject _circusContnets;
        [SerializeField] private GameObject _bathroomContnets;
        [SerializeField] private Transform _characterTransform;

        [SerializeField] private Image _contentsExitImage; // 컨텐츠 자동종료 FadeOut을 위해서 생성
        private float _figureScale = 700f;
        private GameObject _actionFigure;
        private GameObject _contents; // 플레이중인 액션컨텐츠


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
        public void IntroButotnAni()
        {
            _contentsExitImage.gameObject.SetActive(false);
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
            _teminateGhost.OnClickAsObservable().Subscribe(_ =>
            {
                var caseinfo = PlayerViewModel.Player.CaseList.FirstOrDefault(v => v.Number == CaseNumber);
                if (string.IsNullOrEmpty(caseinfo.FigureInstanceID)) return;
                else
                {
                    caseinfo.FigureInstanceID = string.Empty;
                    ConnectionManager.RegistCase(caseinfo);
                    SystemLoading.Show(SystemLoading.LoadingSize.Big, this);
                }
            }).AddTo(gameObject);

            _teminateTemma.OnClickAsObservable().Subscribe(_ =>
            {
                var caseinfo = PlayerViewModel.Player.CaseList.FirstOrDefault(v => v.Number == CaseNumber);
                if (string.IsNullOrEmpty(caseinfo.ThemeID)) return;
                else
                {
                    caseinfo.ThemeID = string.Empty;
                    ConnectionManager.RegistCase(caseinfo);
                    SystemLoading.Show(SystemLoading.LoadingSize.Big, this);
                }
            }).AddTo(gameObject);

            PlayerViewModel.ServerRespones.AsObservable().Subscribe(_ =>
            {
                SystemLoading.Hide(this);
            }).AddTo(gameObject);

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
                _teminateTemma.gameObject.SetActive(false);
                _temmaImage.gameObject.SetActive(false);
                _caseImage.sprite = ResourcesManager.GetCaseColor(data.CaseColor); // 추가
            }
            else
            {
                var thema = PlayerViewModel.Inventory.Themes.FirstOrDefault(v => v.ID == data.ThemeID);
                if (thema != null)
                {
                    //_themaNameText.text = thema.Name.FromStringTable();

                    _teminateTemma.gameObject.SetActive(true);
                    _temmaImage.gameObject.SetActive(true);
                    _temmaImage.sprite = ResourcesManager.GetTheme(data.ThemeID);
                    _caseImage.sprite = ResourcesManager.GetCaseColor(data.CaseColor); // 추가
                }

            }

            // 피규어 확인
            if (string.IsNullOrEmpty(data.FigureInstanceID))
            {
                //_caseImage.SetActive(false);
                _teminateGhost.gameObject.SetActive(false);
                if (_actionFigure != null) _actionFigure.SetActive(false);
            }
            else
            {
                _teminateGhost.gameObject.SetActive(true);
                _dragAndRotateCharacter.enabled = true;
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
                _teminateGhost.gameObject.SetActive(false);
                _teminateTemma.gameObject.SetActive(false);
                _buttonsAni.SetBool("Open", false);
                switch (_actionContents)
                {
                    case ActionContents.PARTY:
                        FigureScaleSetting();
                        _contents = Instantiate(_partyContnets, _actionContentParents.transform);
                        break;
                    case ActionContents.CIRCUS:
                        FigureScaleSetting();
                        _contents = Instantiate(_circusContnets, _actionContentParents.transform); 
                        _contents.GetComponent<ActionContents_Circus>().GetFigure(_actionFigure);
                        _contents.GetComponent<ActionContents_Circus>().GetCustomView(this);
                        break;
                    case ActionContents.BATHROOM:
                        FigureScaleSetting();
                        _contents = Instantiate(_bathroomContnets, _actionContentParents.transform);
                        _contents.GetComponent<ActionContents_Bathroom>().GetFigure(_actionFigure);
                        _contents.GetComponent<ActionContents_Bathroom>().GetCustomView(this);
                        break;
                    default:
                        _contents = Instantiate(_partyContnets, _actionContentParents.transform);
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

        /// <summary>
        /// 액션컨텐츠 끝내기
        /// </summary>
        /// <param name="delayTime">검정 배경 나오기까지 딜레이시간</param>
        /// <param name="delayTime2">검정 배경 사라지기까지 딜레이시간</param>
        public void ContentsExit(float delayTime, float delayTime2)
        {
            Observable.Timer(TimeSpan.FromSeconds((double)delayTime))
                .Subscribe(_ =>
                {
                    _contentsExitImage.gameObject.SetActive(true);
                    _contentsExitImage.DOFade(1f, 1f).From(0f).SetEase(Ease.Linear).OnComplete(()=>
                    {
                        _actionFigure.GetComponent<Animator>().SetTrigger("Idle");
                        Destroy(_contents);
                    });
                    _contentsExitImage.DOFade(0f, 1f).SetEase(Ease.Linear).SetDelay(delayTime2)
                        .OnComplete(() =>
                        {
                            IntroButotnAni();
                            this.GetComponent<DragAndRotateCharacter>().enabled = true;
                            this.GetComponent<PinchObject>().enabled = true;
                        });
                });
        }
    }
}
