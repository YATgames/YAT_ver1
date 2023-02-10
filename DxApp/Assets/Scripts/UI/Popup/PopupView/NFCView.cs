using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Common;
using Assets.Scripts.Util;
using DigitsNFCToolkit;
using DXApp_AppData.Item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
	public class NFCView : MonoBehaviour
	{
		public ConnectionManager ConnectionManager { get; set; }
        public PlayerViewModel PlayerViewModel { get; set; }

        [SerializeField] private CreateAction _createAction;

        [SerializeField] private Dropdown _originFigureDropDown;

        [SerializeField] private Button _createButton;
        [SerializeField] private Button _startReadButton;

        [SerializeField] private GameObject _duplicateCheckInView;

        [SerializeField] private Text _testLabel;

        private string _keepCreateFigureName;
        private string _id;

		private List<OriginFigure> _datas;
        private List<DoggabiFigure> _doggabieDatas;

        private const string TAG_INFO_FORMAT = "ID: {0}\nTechnologies: {1}\nManufacturer: {2}\nWritable: {3}\nMaxWriteSize: {4}";
        private void Start()
		{
			AddEvent();
        }

        private void AddEvent()
        {
            _startReadButton.OnClickAsObservable().Subscribe(_ => OnStartNFCReadClick()).AddTo(gameObject);

            _createButton.OnClickAsObservable().Subscribe(_ =>
            {
                var index = _originFigureDropDown.value;

                if(index- _datas.Count >= 0)
                {
                    //신비 금비
                    _keepCreateFigureName = _doggabieDatas[index -_datas.Count].Name.FromStringTable();

                    _id = _doggabieDatas[index - _datas.Count].ID;
                    bool isCheck = null != PlayerViewModel.Inventory.Doggabis.FirstOrDefault(v => v.ID == _id) ;

                    if (isCheck)
                    {
                        _duplicateCheckInView.gameObject.SetActive(true);
                    }
                    else
                    {
                        ConnectionManager.TaggingDoggabi(_id);

                        SystemLoading.Show(SystemLoading.LoadingSize.Big, this);
                    }
                }
                else
                {
                    // 피규어 태깅
                    _keepCreateFigureName = _datas[index].Name.FromStringTable();

                    _id = _datas[index].ID;

                    ConnectionManager.CreateFigure(_datas[index].ID);

                    SystemLoading.Show(SystemLoading.LoadingSize.Big, this);
                }
            }).AddTo(gameObject);

            PlayerViewModel.ServerRespones.AsObservable().Subscribe(res =>
            {
                SystemLoading.Hide(this);

                _createAction.IsTagging(_keepCreateFigureName, _id);
            }).AddTo(gameObject);
        }

        #region :::::::::Settings
        private void OnEnable()
        {
#if (!UNITY_EDITOR)
			NativeNFCManager.AddNFCTagDetectedListener(OnNFCTagDetected);
			NativeNFCManager.AddNDEFReadFinishedListener(OnNDEFReadFinished);

			Debug.Log("NFC Tag Info Read Supported: " + NativeNFCManager.IsNFCTagInfoReadSupported());
			Debug.Log("NDEF Read Supported: " + NativeNFCManager.IsNDEFReadSupported());
			Debug.Log("NFC Enabled: " + NativeNFCManager.IsNFCEnabled());
#endif

#if (!UNITY_EDITOR) && !UNITY_IOS
			NativeNFCManager.Enable();
#elif UNITY_IOS
			_startReadButton.gameObject.SetActive(true);
#else
            _startReadButton.gameObject.SetActive(false);
#endif
        }

        private void OnDisable()
        {
#if (!UNITY_EDITOR)
			NativeNFCManager.RemoveNFCTagDetectedListener(OnNFCTagDetected);
			NativeNFCManager.RemoveNDEFReadFinishedListener(OnNDEFReadFinished);
#endif
#if (!UNITY_EDITOR) && !UNITY_IOS
			NativeNFCManager.Disable();
#endif
        }
        #endregion

        public void SetData(List<OriginFigure> datas)
		{
			for (int i = 0; i < datas.Count; i++)
			{
				_originFigureDropDown.options.Add(new Dropdown.OptionData(datas[i].Name.FromStringTable()));
			}
			_datas = datas;
		}
        public void SetData(List<DoggabiFigure> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                _originFigureDropDown.options.Add(new Dropdown.OptionData(datas[i].Name.FromStringTable()));
            }
            _doggabieDatas = datas;
        }
        #region ::::::::NFC_Method
        public void OnStartNFCReadClick()
        {
#if (!UNITY_EDITOR)
			NativeNFCManager.ResetOnTimeout = true;
            NativeNFCManager.Enable();
#endif
        }

        public void OnNFCTagDetected(NFCTag tag)
        {
            StopAllCoroutines();
            IEnumerator _resetCorutine = UpdateTagInfo(tag);
            StartCoroutine(_resetCorutine);
        }

        public void OnNDEFReadFinished(NDEFReadResult result)
        {
            string readResultString = string.Empty;
            if (result.Success)
            {
                readResultString = string.Format("NDEF Message was read successfully from tag {0}", result.TagID);
            }
            else
            {
                readResultString = string.Format("Failed to read NDEF Message from tag {0}\nError: {1}", result.TagID, result.Error);
            }
            Debug.Log(readResultString);
        }

        private IEnumerator UpdateTagInfo(NFCTag tag)
        {
            bool CheckSerial = true;

            string technologiesString = string.Empty;
            NFCTechnology[] technologies = tag.Technologies;
            int length = technologies.Length;
            for (int i = 0; i < length; i++)
            {
                if (i > 0)
                {
                    technologiesString += ", ";
                }

                technologiesString += technologies[i].ToString();
            }

            string maxWriteSizeString = string.Empty;
            if (tag.MaxWriteSize > 0)
            {
                maxWriteSizeString = tag.MaxWriteSize + " bytes";
            }
            else
            {
                maxWriteSizeString = "Unknown";
            }

            string tagInfo = string.Format(TAG_INFO_FORMAT, tag.ID, technologiesString, tag.Manufacturer, tag.Writable, maxWriteSizeString);

            _testLabel.text = tag.ID;
            // 서버에서 TagNumber 있으면 아이템 분배
            if (CheckSerial)
            {
                // 소환귀 등록 아이템 분배
               
                // 애니메이션 재생 후 서버 태그 넘버 다시 찍어도 못쓰게 하기.
            }

#if UNITY_IOS
            NativeNFCManager.Enable();
#endif

            yield break;
        }
        #endregion
    }
}
