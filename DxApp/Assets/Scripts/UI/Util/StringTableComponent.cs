using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Util
{
	[RequireComponent(typeof(Text))]
	public class StringTableComponent : MonoBehaviour
	{
		public string Key { get { return _key; } }
		public string[] Args { get { return _args; } }

		[SerializeField] private string _key;
		[SerializeField] private string[] _args;

		private Text _text;

		private void Start()
		{
			GameManager.Instance.CurrentLanguage
				.ObserveEveryValueChanged(v => v.Value)
				.Subscribe(v =>
				{
					SetData(_key.FromStringTable(_args));
				})
				.AddTo(gameObject);
		}

		public void SetData(string txt)
		{
			if (_text == null) _text = GetComponent<Text>();

			_text.text = txt;
		}
	}
}
