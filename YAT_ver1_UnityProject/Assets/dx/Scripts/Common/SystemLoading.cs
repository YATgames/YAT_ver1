using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class SystemLoading : MonoBehaviour
    {
        public enum LoadingSize
        {
            Big,
            Small,
        }
        private const string _path = "Prefs/SystemLoading/{0}";
        private static List<SystemLoading> _loadingList = new List<SystemLoading>();

        private MonoBehaviour _parent; // �θ� �������̺�� ������ ������
        
        public static void Hide<T>(T root) where T : MonoBehaviour // �ε�ȭ�� �����?
        {
            var item = _loadingList.FirstOrDefault(v => v._parent.Equals(root)); // ����Ʈ���� root�� �Ķ���� ���� ���� �ִٸ� �ı��ϱ�
            if(item != null)
            {
                _loadingList.Remove(item);
                Destroy(item.gameObject);
            }
        }
        public static void Show<T>(LoadingSize size, T root)where T :MonoBehaviour
        {
            if (_loadingList.Where(v => v._parent.Equals(root)).Count() > 0)
                return;

            var instance = Instantiate(Resources.Load<SystemLoading>(string.Format(_path, size))); // �ε�ȭ�� ������Ű��
            instance._parent = root;
            if(size == LoadingSize.Small) // ���� �˾��ΰ��
            {
                instance.transform.SetParent(root.transform); // �ν��Ͻ��� �θ� ������
            }
            else
            {
                instance.transform.SetParent(PopupManager.Instance.transform); // �˾��Ŵ����� ��ġ�� ����
            }

            instance.transform.SetAsLastSibling(); // ���̾��Ű �켱���� �����ϱ� : �ش� ������Ʈ�� ������ ���������� ����.(���� ���߿� ���. ������ ����)
            var rect = instance.GetComponent<RectTransform>();

            _loadingList.Add(instance);
        }

    }
}