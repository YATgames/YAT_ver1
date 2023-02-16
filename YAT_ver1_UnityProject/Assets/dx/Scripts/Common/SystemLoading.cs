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

        private MonoBehaviour _parent; // 부모를 모노비헤이비어 형으로 가져옴
        
        public static void Hide<T>(T root) where T : MonoBehaviour // 로딩화면 숨기기?
        {
            var item = _loadingList.FirstOrDefault(v => v._parent.Equals(root)); // 리스트에서 root로 파라미터 넣은 값이 있다면 파괴하기
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

            var instance = Instantiate(Resources.Load<SystemLoading>(string.Format(_path, size))); // 로딩화면 생성시키기
            instance._parent = root;
            if(size == LoadingSize.Small) // 작은 팝업인경우
            {
                instance.transform.SetParent(root.transform); // 인스턴스의 부모를 설정함
            }
            else
            {
                instance.transform.SetParent(PopupManager.Instance.transform); // 팝업매니저의 위치에 생성
            }

            instance.transform.SetAsLastSibling(); // 하이어라키 우선순위 설정하기 : 해당 오브젝트의 순위를 마지막으로 변경.(가장 나중에 출력. 앞으로 보임)
            var rect = instance.GetComponent<RectTransform>();

            _loadingList.Add(instance);
        }

    }
}