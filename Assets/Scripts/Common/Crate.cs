using TMPro;
using UnityEngine;

namespace AOC22.Common {
	public class Crate : MonoBehaviour {
		[SerializeField] private TMP_Text[] _characterTexts;

		public char character { get; private set; }

		public void SetCharacter(char c) {
			character = c;
			foreach (var characterText in _characterTexts)
				characterText.text = $"{c}";
		}
	}
}