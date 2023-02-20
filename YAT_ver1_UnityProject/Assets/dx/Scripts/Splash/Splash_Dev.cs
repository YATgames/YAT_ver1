using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.UI;
using System.Threading;
using System.Collections;
using Assets.Scripts.Common.Models;
using UnityEditor.Build;

namespace Assets.Scripts.Splash
{
    public class Splash_Dev : MonoBehaviour
    {
        // �� _Dev �κ��� ���� ���� ����
        /*
         * SystemLoading.Show�� �ϴµ� root.trasform���� �����ȴܸ���
         * �ٵ� �̰� canvas�ܿ��� �Ǿ��ϱ⶧���� �߰������� ��ũ��Ʈ �۾��� �ʿ���
         */

        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;
        [DependuncyInjection(typeof(PopupManager))]
        private PopupManager _popupManager;



        [SerializeField] private InputField _idField;
        [SerializeField] private Button _loginButton;

        private void Start()
        {
            DependuncyInjection.Inject(this);
            _idField.text = "DevLogin";
            _loginButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (string.IsNullOrEmpty(_idField.text))
                    return;

                _idField.text = "idField";
                Debug.Log("Splash ���� ���۹�ư ����");
                SystemLoading.Show(SystemLoading.LoadingSize.Big, this); // �� ��ü�� �����Ǵ� ū �ε�
                DataManager.Instance.ChagneValue();
            }).AddTo(gameObject);

            //  PlayerViewModelModel ���� �ν��Ͻ� ������ ������
            DataManager.Instance
                .ObserveEveryValueChanged(v => v._nullObject)
                .Where(v => v != null) // �����ڵ�� PlayerModel�� �������� ������ null�� ���ü��� ����
                // �������� ������Ű���� ���ٰ� ä������ ������� �ؾߵɵ�
                .Subscribe(player =>
                {
                    GameManager.Instance.LoadScene(SceneName.MainScene);
                    SystemLoading.Hide(this);
                    //FlowManager.Instance.AddSubPopup(PopupStyle.Main,null); // �̰� ������ null �Ķ���� ����� �ѹ� �غ���
                    FlowManager.Instance.AddSubPopup(PopupStyle.Main); 
                }).AddTo(gameObject);
            
        }
        private void SetButton() // dx�� Splash_DEV ��ũ��Ʈ �κ��� �Լ��� �Ҵ��Ŵ
        {

        }
    }
}