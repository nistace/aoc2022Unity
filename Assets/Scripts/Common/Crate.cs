using System;
using TMPro;
using UnityEngine;

namespace AOC22.Common {
	public class Crate : MonoBehaviour {
		[SerializeField] private   TMP_Text[] _characterTexts;
		[SerializeField] protected char       _character = 'A';

		public char character => _character;

		public void SetCharacter(char c) {
			_character = c;
			foreach (var characterText in _characterTexts)
				characterText.text = $"{_character}";
		}

#if UNITY_EDITOR
		private void OnValidate() => SetCharacter(_character);
#endif
	}
}