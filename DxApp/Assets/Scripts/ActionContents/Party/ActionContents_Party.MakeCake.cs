using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Party : MonoBehaviour
    {
        [Header("MakeCake")]
        [SerializeField] private Transform _selectionParent;
        [SerializeField] private Transform[] _cakeSplats;
        [SerializeField] private Image _crows;
        [SerializeField] private GameObject _mixAura;
        [SerializeField] private GameObject _smokeVFX;
        [SerializeField] private GameObject _cake;
        [SerializeField] private GameObject _angryMark;

        private IEnumerator InitMakeCake()
        {
            for (int i = 0; i < _selectionParent.childCount; i++)
            {
                _selectionParent.GetChild(i).gameObject.SetActive(false);
                _selectionParent.GetChild(i).localScale = Vector3.zero;
            }

            /*StartCoroutine(TextBoxAppear("달달한게 땡기네"));
            yield return new WaitForSeconds(4f);
            StartCoroutine(TextBoxAppear("어떤 걸 만들어볼까~"));
            yield return new WaitForSeconds(4f);*/

            _mixAura.SetActive(true);
            yield return new WaitForSeconds(1f);

            _selectionParent.GetChild(0).gameObject.SetActive(true);
            _selectionParent.GetChild(0).DOScale(1f, 0.5f).SetEase(Ease.OutElastic);
            yield return new WaitForSeconds(0.35f);
            _selectionParent.GetChild(1).gameObject.SetActive(true);
            _selectionParent.GetChild(1).DOScale(1f, 0.5f).SetEase(Ease.OutElastic);

            yield return new WaitForSeconds(1f);

            _selectionParent.GetChild(0).GetComponent<Outline>().DOFade(0.75f, 1.2f).SetLoops(-1, LoopType.Yoyo);
            _selectionParent.GetChild(1).GetComponent<Outline>().DOFade(0.75f, 1.2f).SetLoops(-1, LoopType.Yoyo);

            int buttonCount = 2;
            for (int i = 0; i < buttonCount; i++)
            {
                _selectionParent.GetChild(i).GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
                {
                    StartCoroutine(AddIngredient());
                }).AddTo(gameObject);
            }
            yield return null;
        }

        private int addedIngredientInt;

        private IEnumerator AddIngredient()
        {
            Transform imageTF = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                imageTF = hit.collider.transform;
            }
            imageTF.GetComponent<Button>().enabled = false;
            imageTF.GetComponent<Outline>().enabled = false;
            imageTF.DOMove(_mixAura.transform.position, 2f).SetEase(Ease.OutExpo);
            imageTF.DOScale(0f, 0.5f).SetEase(Ease.InQuart);
            yield return new WaitForSeconds(1f);


            _mixAura.transform.DOScale(_mixAura.transform.localScale.x + (addedIngredientInt + 1) * 0.2f, 1f);
            yield return new WaitForSeconds(1f);

            addedIngredientInt++;
            if (addedIngredientInt == 2)
                StartCoroutine(StartMixCoroutine());
        }

        private IEnumerator StartMixCoroutine()
        {
            /*StartCoroutine(TextBoxAppear("뭔가 만들어지는 중..!"));
            yield return new WaitForSeconds(4f);*/

            // 공통 로직

            yield return new WaitForSeconds(1f);
            _mixAura.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            _smokeVFX.SetActive(true);
            StartCoroutine(TextBoxAppear("방구 냄새가 나는 딸기케익 완성!"));
            yield return new WaitForSeconds(1f);
            _cake.SetActive(true);
            yield return new WaitForSeconds(3f);

            StartCoroutine(TextBoxAppear("으악! 이게 무슨 냄새야!"));
            _anim.SetTrigger("Panic");
            yield return new WaitForSeconds(4f);

            _anim.SetTrigger("Idle");
            GameObject.Find("Button_Case").GetComponent<Image>().DOColor(new Color(0.2f, 0.2f, 0.2f), 0.25f);
            yield return new WaitForSeconds(0.25f);

            _angryMark.transform.position = _figureModel.GetComponent<PrefabModel.Model_Body>().HeadPosition.position + new Vector3(0.85f, 1.15f, -5f);
            _angryMark.transform.localScale = Vector3.zero;
            _angryMark.SetActive(true);
            _angryMark.transform.DOScale(1f, 0.25f);

            yield return new WaitForSeconds(1f);

            _angryMark.transform.DOScale(0f, 0.25f);
            _cake.transform.DOLocalMoveY(300f, 1.5f);
            yield return new WaitForSeconds(2f);
            _cake.transform.DOLocalMoveY(-200f, 0.15f);
            _cake.transform.DOLocalRotate(new Vector3(0f, 0f, 180f), 0.15f);

            for (int i = 0; i < _cakeSplats.Length; i++)
            {
                _cakeSplats[i].gameObject.SetActive(true);
                _cakeSplats[i].DOScale(1f, 0.15f);
            }

            yield return new WaitForSeconds(0.5f);
            _crows.gameObject.SetActive(true);
            _crows.DOFade(1f, 1f);
            yield return new WaitForSeconds(0.5f);
            _anim.SetTrigger("Elated");
        }
    }
}
