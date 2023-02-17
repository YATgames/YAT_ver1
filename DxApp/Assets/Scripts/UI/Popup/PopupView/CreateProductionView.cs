using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.Popup.Sup;
using UniRx;
using DG.Tweening;
using System.Collections;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.PrefabModel;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class CreateProductionView : MonoBehaviour
    {
        public ResourcesManager ResourcesManager { get; set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public ItemManager ItemManager { get; set; }
        public UIPopupCreateProduction UIPopupCreateProduction { get; set; }

        [SerializeField] private DragAndRotateCharacter _dragAndRotateCharacter;

        [SerializeField] private Image MaskImage;

        [SerializeField] private Button _okButton;

        [SerializeField] private Text _nameText;

        [SerializeField] private Text _doggabiExplainText1;
        [SerializeField] private Text _doggabiExplainText2;

        [SerializeField] private Transform _parent;

        [SerializeField] private GameObject[] BGParticles;

        private bool isDoggabi = false;
        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            _okButton.OnClickAsObservable().Subscribe(_ => UIPopupCreateProduction.Hide()).AddTo(gameObject);
        }
        private IEnumerator TimeCheck()
        {
            yield return new WaitForSeconds(1f);

            _dragAndRotateCharacter.enabled = true;
            _okButton.gameObject.SetActive(true);
            _nameText.gameObject.SetActive(true);
            if(isDoggabi)
            {
                if (PlayerViewModel.Inventory.Doggabis.Count == 1) _doggabiExplainText1.gameObject.SetActive(true);
                else if (PlayerViewModel.Inventory.Doggabis.Count == 2) _doggabiExplainText2.gameObject.SetActive(true);
            }
        }

        public void SetData(string name, string id, bool isSkip)
        {
            if (!isSkip)
            {
                MaskImage.DOColor(new Color(1, 1, 1, 0), 1f).SetEase(Ease.InExpo);
                _parent.DOLocalMoveZ(-3.5f, 1f);
                StartCoroutine(TimeCheck());
            }
            else
            {
                MaskImage.DOColor(new Color(1, 1, 1, 0), 0f).SetEase(Ease.InExpo);
                _parent.DOLocalMoveZ(-3.5f, 0f);
                _dragAndRotateCharacter.enabled = true;
                _okButton.gameObject.SetActive(true);
                _nameText.gameObject.SetActive(true);
               
            }

            // 피규어
            ResourcesManager.ModelingSetting(_parent);
            ResourcesManager.DoggabiSetting(_parent);

            BGParticles[Random.Range(0, BGParticles.Length)].SetActive(true);
            _nameText.text = name;

            // 피규어
            List<string> items = null;
            var figure = ItemManager.Figures.FirstOrDefault(v => v.ID == id);
            if (figure != null)
            {
                items = figure.Items;
            }
            else
            {
                // 도깨비
                var doggabi = ItemManager.Doggabies.FirstOrDefault(v => v.ID == id);
                items = new List<string>();
                items.Add(doggabi.ID);
                isDoggabi = true;
                if(isSkip)
                {
                    if (PlayerViewModel.Inventory.Doggabis.Count == 1) _doggabiExplainText1.gameObject.SetActive(true);
                    else if (PlayerViewModel.Inventory.Doggabis.Count == 2) _doggabiExplainText2.gameObject.SetActive(true);
                }
            }

            GameObject body = null;
            GameObject head = null;
            GameObject deco = null;

            for (int i = 0; i < items.Count; i++)
            {
                switch (items[i].Substring(0, 1))
                {
                    case "1": head = ResourcesManager.GetModel(items[i]); break;

                    case "2":
                    case "7":
                        body = ResourcesManager.GetModel(items[i]);
                        break;

                    case "3":
                    case "4":
                        deco = ResourcesManager.GetModel(items[i]);
                        break;
                }
            }

            Model_Body _body = body?.GetComponent<Model_Body>();
            Model_Head _head = head?.GetComponent<Model_Head>();
            Model_Deco _deco = deco?.GetComponent<Model_Deco>();

            try
            {
                _body?.transform.SetParent(_parent);
                _body?.ResetTransform();
                _head?.gameObject.transform.SetParent(_body.HeadPosition);
                _head?.ResetTransform();
                _deco?.gameObject.transform.SetParent(_head.DecoPosition);
                _deco?.ResetTransform();
            }
            catch
            {

            }
        }
    }
}
