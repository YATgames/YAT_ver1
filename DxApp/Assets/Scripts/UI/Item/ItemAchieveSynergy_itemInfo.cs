using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
{
    public class ItemAchieveSynergy_itemInfo : MonoBehaviour
    {
        [SerializeField] Text itemText;
        [SerializeField] Image itemImg;

        public Text ItemText { get { return itemText; } set{ value = itemText; } }
        public Image ItemImg { get { return itemImg; } set { value = itemImg; } }
    }
}
