using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameManager : UnitySingleton<GameManager>
    {
        //lic ReactiveProperty<SystemLanguage> CurrentLanguage = new ReactiveProperty<SystemLanguage>();
        public ReactiveProperty<SceneName> CurrentScene = new ReactiveProperty<SceneName>();

        //public ReactiveProperty<float> Sound = new ReactiveProperty<float>();
        //public ReactiveProperty<bool> PushAlram = new ReactiveProperty<bool>();

        private const string _saveSoundCode = "Sound";
        private const string _savePushAlramCode = "PushAlram";
        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

        }
        public override void UnInitialize()
        {
            base.UnInitialize();
            AddEvent();
            //Sound.Value = palyerprefs 에서
        }
        
        private void AddEvent()
        {
            CurrentScene.ObserveEveryValueChanged(v => v.Value).Subscribe(v =>
            {
                switch (v)
                {
                    case SceneName.MainScene:
                        // 플로우매니저에서 Change 시켜줘야함
                        break;
                }
            }).AddTo(gameObject);
        }

        public void LoadScene(SceneName name)
        {
            var loadScene = SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Single);
            loadScene.ObserveEveryValueChanged(v => v.isDone).Where(v => v).Take(1).Subscribe(_ =>
                {
                    CurrentScene.Value = name; // 씬 바꾸기
                });
        }

        /*
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DataManager.instance.p_gold = 1;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                DataManager.instance.Set_partyMember(1, 1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                DataManager.instance.SaveJson();
            }
        }*/
    }
}