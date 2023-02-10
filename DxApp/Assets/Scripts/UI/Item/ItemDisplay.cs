using Assets.Scripts.Common.Models;
using Assets.Scripts.Common;
using Assets.Scripts.Managers;
using Assets.Scripts.PrefabModel;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using DXApp_AppData.Table;
using System;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Common.DI;
using DG.Tweening;

namespace Assets.Scripts.UI.Item
{
    public class ItemDisplay : MonoBehaviour
    {
        public int Number { get { return _caseNumber; } }

        [SerializeField] private float _intervalTime = 5f;
        [SerializeField] private int _caseNumber;

        [SerializeField] private Button _button;
        [SerializeField] private GameObject _empty;
        [SerializeField] private Transform _testFigureParent;
        [SerializeField] private Animator _animator;
        //[SerializeField] private GameObject _img; // testFigureText상위객체
        //[SerializeField] private Text _testFigureText;

        public PlayerViewModel PlayerViewModel { get; set; }
        public OnEventTrigger<int> OnClick { get; set; }
        public ResourcesManager _ResourcesManager { get; set; }
        private void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(_intervalTime)).Where(v => _testFigureParent.gameObject.activeSelf == true).Subscribe(_ =>
            {
                LogManager.Log("메인 화면에서 일정 주기로 피규어가 액션을 취함 ::: Interval Time {0}초 ::: {1} 케이스", _intervalTime.ToString(".##"), _caseNumber);

            }).AddTo(gameObject);
            _button.OnClickAsObservable().Subscribe(_ => OnClick?.Invoke(_caseNumber)).AddTo(gameObject);
        }

        public void SetCase(CaseInfo data)
        {
            if (string.IsNullOrEmpty(data.ThemeID))
            {
                _button.image.sprite = ResourcesManager.GetCaseColor(data.CaseColor);
                _empty.SetActive(true);
            }
            else
            {
                _button.image.sprite = ResourcesManager.GetTheme(data.ThemeID);
                _empty.SetActive(false);
            }
        }

        public void SetEmpty()
        {
            _testFigureParent.gameObject.SetActive(false);
            //_img.SetActive(false);
        }

        public void SetData(CustomFigureInstance data)
        {
            _testFigureParent.gameObject.SetActive(true);
            //_testFigureText.text = string.Format("CustomFigure \n\n ID : {0} \n\n InstanceID : {1}", data.ID, data.InstanceID);

            ResetModels();
            LoadModeling(data);
        }

        public void SetData(OriginFigureInstance data)
        {
            _testFigureParent.gameObject.SetActive(true);
            //_testFigureText.text = string.Format("OriginFigure \n\n ID : {0} \n\n InstanceID : {1}", data.ID, data.InstanceID);
            ResetModels();
            LoadModeling(data);
        }


        #region ###FigureAnimation
        // body에 있는 애니메이터 할당하는 방법 찾아야함.
        enum Anim
        {
            IDLE,
            COLLAPSE,
            DANCE,
            ELATED,
            FIRE,
            JUMPTURN,
            LIEFLAT,
            LOWJUMP,
            PANIC,
            SHUDDER,
            STARTLING,
            WALK
        }
        /// <summary>
        /// 모델링 애니메이션
        /// </summary>
        /// <param name="anim"> 재생할 애니메이션 및 모션 </param>
        private void ModelingMotion(Anim anim)
        {
            switch (anim)
            {
                case Anim.IDLE:
                    _animator.SetTrigger("Idle"); break;
                case Anim.COLLAPSE:
                    _animator.SetTrigger("Collapse"); break;
                case Anim.DANCE:
                    _animator.SetTrigger("Dance"); break;
                case Anim.ELATED:
                    _animator.SetTrigger("Elated"); break;
                case Anim.FIRE:
                    _animator.SetTrigger("Fire"); break;
                case Anim.JUMPTURN:
                    _animator.SetTrigger("JumpTurn"); break;
                case Anim.LIEFLAT:
                    _animator.SetTrigger("LieFlat"); break;
                case Anim.LOWJUMP:
                    _animator.SetTrigger("LowJump"); break;
                case Anim.PANIC:
                    _animator.SetTrigger("Panic"); break;
                case Anim.SHUDDER:
                    _animator.SetTrigger("Shudder"); break;
                case Anim.STARTLING:
                    _animator.SetTrigger("Startling"); break;
                case Anim.WALK:
                    _animator.SetTrigger("Walk"); break;
                default:
                    Debug.LogError("<color=red> 없는 애니메이션 입니다.에니메이터를 확인해주세요</color>"); break;
            }
        }
        #endregion
        private void ResetModels()
        {
            for (int i = 0; i < ItemManager.Instance.PartsList.Count; i++)
            {
                ResourcesManager.ResetModels(ItemManager.Instance.PartsList[i].ID);
            }
        }
        public void loadResource(ResourcesManager resourceManager)
        {
            resourceManager.ModelingSetting(_testFigureParent);
        }
        private void LoadModeling(CustomFigureInstance customFigure)
        {
            //var items = ItemManager.Instance.Figures.FirstOrDefault(v => v.ID == customFigure.ID).Items;

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
            }
            catch
            {

            }
        }
        private void LoadModeling(OriginFigureInstance originFigure)
        {
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
        }
    }
}