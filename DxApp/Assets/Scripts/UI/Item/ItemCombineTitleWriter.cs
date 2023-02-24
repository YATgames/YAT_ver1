using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Popup.PopupView;
using Assets.Scripts.Util;
using DXApp_AppData.Enum;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
{
    public class ItemCombineTitleWriter : MonoBehaviour
    {
        [SerializeField] private AskCombineInView _askCombineInView;

        [SerializeField] private Button _writeNickNameButton;
        [SerializeField] private Button _cancelNickNameButton;

        [SerializeField] private Text _nickPlaceholder;
        [SerializeField] private InputField _nickNameField;

        private char[] _first_char;
        private char[] _second_char;
        private char[] _third_char;

        private void Awake()
        {
            #region ::::::SettingRandomChar
            _first_char = new char[] {'±¸' , 'ÀÚ' , 'Ã¶' , '¼®' , '±Í'
                    , '°­' , 'Á¶' , 'À±' , 'Àå' , 'ÀÓ'
                    , '¿À' , 'ÇÑ' , '½Å' , '¼­' , '±Ç'
                    , 'È²' , '¾È' , '¼Û' , 'À¯' , 'È«'
                    , 'Àü' , '°í' , '¹®' , '¼Õ' , '¾ç'
                    , '¹è' , 'Á¶' , '¹é' , 'Çã' , '³²'};

            _second_char = new char[] {'¹¦' , '°£' , 'µ¿' , 'ÀÎ' , 'Áö'
                    , '°©' , '¸é' , '¿ì' , '°Ç' , 'ÁØ'
                    , '½Â' , '¿µ' , '¼º' , 'Áø' , 'ÁØ'
                    , 'Á¤' , '¸²' , '±¤' , '¿µ' , 'È£'
                    , 'Áß' , 'ÈÆ' , 'ÈÄ' , '¿ì' , '»ó'
                    , '¿¬' , 'Ã¶' , '¾Æ' , 'À±' , 'Àº'};

            _third_char = new char[] {'ÁÖ' , '¹Ì' , 'ÀÚ' , '¼º' , '»ó'
                    , '³²' , '½Ä' , 'ÀÏ' , 'Ã¶' , 'º´'
                    , 'Çý' , '¿µ' , '¹Ì' , 'È¯' , '½Ä'
                    , '¼÷' , 'ÀÚ' , 'Èñ' , '¼ø' , 'Áø'
                    , '¼­' , 'ºó' , 'Á¤' , 'Áö' , 'ÇÏ'
                    , '¿¬' , '¼º' , '°ø' , '¾È' , '¿ø'};
            #endregion
        }

        private void OnEnable()
        {
            _nickNameField.text = string.Empty;
            _nickPlaceholder.text = MixNickName(_first_char, _second_char, _third_char) + "±Í";
        }

        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            _writeNickNameButton.OnClickAsObservable("Button_Click").Subscribe(_ =>
            {
                string title = (_nickNameField.text == string.Empty) ? _nickPlaceholder.text : _nickNameField.text;
                _askCombineInView.gameObject.SetActive(true);
                _askCombineInView.SetData(title);
            }).AddTo(gameObject);
        }

        private string MixNickName(char[] first, char[] second, char[] third)
        {
            int nickNameLengh = Random.Range(1, 5);
            string nickName = null;

            #region ::::::CreateRandomNickName
            for (int i = 0; i < nickNameLengh; i++)
            {
                int random = Random.Range(0, 3);
                switch(random)
                {
                    case 0:
                        nickName += first[Random.Range(0, first.Length)].ToString();
                        continue;
                    case 1:
                        nickName += second[Random.Range(0, second.Length)].ToString();
                        continue;
                    case 2:
                        nickName += third[Random.Range(0, third.Length)].ToString();
                        continue;
                }
            }
            #endregion

            return nickName;
        }
    }
}
