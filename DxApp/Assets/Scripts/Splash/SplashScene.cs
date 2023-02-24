using Assets.Scripts.ManageObject;
using Assets.Scripts.Util;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Splash
{
	public class SplashScene : MonoBehaviour
	{
		[SerializeField] private Image _xosoftLogo;
		[SerializeField] private Image _titletLogo;

		[SerializeField] private SplashMain _main;
		[SerializeField] private Splash_DEV _dev;
		[SerializeField] private Splash_Live _live;

		private IEnumerator Start()
		{
			Application.targetFrameRate = 60;
			Application.runInBackground = true;

			ServiceSetting_Start();
			_main.gameObject.SetActive(false);

			DOTween.Init();
			ManageObjectFacade.Initialize();

			yield return new WaitForSecondsRealtime(1f);

			yield return StartCoroutine(PlayAni());
			ServiceSetting_End();
		}

		private IEnumerator PlayAni()
		{
#if LIVE
			yield return StartCoroutine(_xosoftLogo.Co_FadeInOut(1f));
			yield return StartCoroutine(_titletLogo.Co_FadeInOut(1f));
#endif

			_main.gameObject.SetActive(true);

			yield return StartCoroutine(_main.PlayAni());
		}

		#region :::::::: Service Settings
		private void ServiceSetting_Start()
		{
			_dev.gameObject.SetActive(false);
            _live.gameObject.SetActive(false);
        }
		private void ServiceSetting_End()
		{
#if LIVE
			_live.gameObject.SetActive(true);
#elif DEV
            _dev.gameObject.SetActive(true);
#elif QA
			_dev.gameObject.SetActive(true);
#endif
		}
		#endregion

	}
}




//using Assets.Scripts.ManageObject;
//using DG.Tweening;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Assets.Scripts.Splash
//{
//    public class SplashScene : MonoBehaviour
//    {
//       // [SerializeField] private Reporter _logReporter;

//        [SerializeField] private Image _xosoftLogo;
//        [SerializeField] private Image _titletLogo;

//        [SerializeField] private SplashMain _main;
//        [SerializeField] private Splash_DEV _dev;

//        private IEnumerator Start()
//        {
//            ServiceSetting_Start();
//            _main.gameObject.SetActive(false);

//            DOTween.Init();
//            ManageObjectFacade.Initialize();

//            yield return new WaitForSecondsRealtime(1f);

//            yield return StartCoroutine(PlayAni());
//            ServiceSetting_End();
//        }

//        private IEnumerator PlayAni()
//        {
//#if LIVE
//			yield return StartCoroutine(_xosoftLogo.Co_FadeInOut(2f));

//			yield return StartCoroutine(_titletLogo.Co_FadeInOut(2f));
//#endif

//            _main.gameObject.SetActive(true);

//            yield return StartCoroutine(_main.PlayAni());
//        }

//        #region :::::::: Service Settings
//        private void ServiceSetting_Start()
//        {
//            //_logReporter.Hide();
//            _dev.gameObject.SetActive(false);

//#if LIVE
//			Destroy(_logReporter.gameObject);
//#endif

//#if DEV
//           // DontDestroyOnLoad(_logReporter);
//#endif

//#if QA
//			//DontDestroyOnLoad(_logReporter);
//#endif
//        }
//        private void ServiceSetting_End()
//        {
//#if LIVE
//#endif

//#if DEV
//            _dev.gameObject.SetActive(true);
//#endif

//#if QA
//			_dev.gameObject.SetActive(true);
//#endif
//        }
//        #endregion

//    }
//}
