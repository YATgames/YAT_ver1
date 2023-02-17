using UnityEngine;
using Assets.Scripts.Util;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Managers;
using UniRx;

namespace Assets.Scripts.Splash
{
    public class SplashMain : MonoBehaviour
    {
        [SerializeField] private Image _splashCase;
        [SerializeField] private Image _splashFigure;
        [SerializeField] private Image _splashFigureOne;
        [SerializeField] private Image _splashXLigth;
        [SerializeField] private Image _splashX;
        [SerializeField] private Image _shinbiLogo;
        [SerializeField] private Image _figureLogoLight;
        [SerializeField] private Transform _figureLogo;

        [SerializeField] private float _firstTime = 1f;

        private void Start()
        {
            _figureLogo.gameObject.SetActive(false);
        }

        public IEnumerator PlayAni()
        {
            yield return new WaitForEndOfFrame();
            SoundManager.Instance.PlayBGM("Splash_BGM");

            _splashCase.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), _firstTime);
            _splashCase.DOColor(new Color(1, 1, 1, 0.25f), 1f);

            _splashFigure.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), _firstTime);
            _splashFigure.DOColor(new Color(1,1,1,0.1f), _firstTime);

            _splashFigureOne.transform.DOScale(new Vector3(1f, 1f, 1f), _firstTime);
            _splashFigureOne.DOColor(Color.white, _firstTime);

            _splashX.transform.DOScale(new Vector3(1f, 1f, 1f), _firstTime);
            _splashX.DOColor(Color.white, 2f);

            _figureLogo.DoLocalFrom(new Vector3(0, 500, 0), _firstTime);
            _figureLogo.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);

            _shinbiLogo.DOColor(Color.white, 0.5f);

            _splashXLigth.gameObject.SetActive(true);
            _figureLogoLight.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

        }
    }
}
