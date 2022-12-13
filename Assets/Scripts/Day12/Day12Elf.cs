using System.Collections;
using AOC22.Common;
using TMPro;
using UnityEngine;

public class Day12Elf : MonoBehaviour {
	[SerializeField] protected RotateTowards _rotateTowards;
	[SerializeField] protected Transform     _panel;
	[SerializeField] protected TMP_Text      _panelLabel;
	[SerializeField] protected Vector3       _panelLockedPosition;
	[SerializeField] protected Vector3       _panelUnlockedPosition;
	[SerializeField] protected float         _speed = 1;

	private void Start() {
		_rotateTowards.target = Camera.main.transform;
		_panel.localPosition = _panelUnlockedPosition;
		_panelLabel.text = string.Empty;
	}

	public IEnumerator ChangePanelValue(int value, bool lockValue) {
		while (_panel.localPosition != _panelLockedPosition) {
			_panel.localPosition = Vector3.MoveTowards(_panel.localPosition, _panelLockedPosition, _speed * AocTime.deltaTime);
			yield return null;
		}
		_panelLabel.text = value + (lockValue ? "" : "?");
		if (!lockValue) {
			while (_panel.localPosition != _panelUnlockedPosition) {
				_panel.localPosition = Vector3.MoveTowards(_panel.localPosition, _panelUnlockedPosition, _speed * AocTime.deltaTime);
				yield return null;
			}
		}
	}
}