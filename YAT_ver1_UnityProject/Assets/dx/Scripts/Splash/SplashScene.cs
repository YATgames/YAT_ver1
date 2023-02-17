using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Assets.Scripts.Common.Models;
using Assets.Scripts.ManageObject;
using Assets.Scripts.Common;
using Assets.Scripts.UI;

namespace Assets.Scripts.Splash
{
    public class SplashScene : MonoBehaviour
    {
        private IEnumerator Start()
        {
            Application.targetFrameRate = 60;
            Application.runInBackground = true;

            DOTween.Init();
            ManageObjectFacade.Initialize();
            yield return new WaitForSeconds(1f);

            // 버튼 활성화 애니메이부분
        }
        private void ServiceSetting()
        {

        }
    }
}