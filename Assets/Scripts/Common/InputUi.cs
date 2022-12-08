using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AOC22.Common {
	public class InputUi : MonoBehaviour {
		[SerializeField] protected TMP_InputField _inputField;
		[SerializeField] protected Button         _startButton;

		public UnityEvent onStartClicked       => _startButton.onClick;
		public string     inputText            => _inputField.text;

		public void SetVisible(bool visible) => GetComponent<Canvas>().enabled = visible;

		public void SetInput(string assetText) => _inputField.text = assetText;
	}
}