using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AOC22.Common {
	public class AocTimeUi : MonoBehaviour {
		[SerializeField] protected Slider   _slider;
		[SerializeField] protected float    _min;
		[SerializeField] protected float    _max = 50;
		[SerializeField] protected TMP_Text _speedText;

		private void Start() {
			_slider.SetValueWithoutNotify((AocTime.coefficient - _min) / (_max - _min));
			AocTime.onCoefficientChanged.AddListener(HandleCoefficientChanged);
			_slider.onValueChanged.AddListener(HandleSliderChanged);
		}

		private void OnDestroy() {
			AocTime.onCoefficientChanged.RemoveListener(HandleCoefficientChanged);
		}

		private void HandleCoefficientChanged() => _speedText.text = $"x{AocTime.coefficient:0.0}";

		private void HandleSliderChanged(float value) => AocTime.SetCoefficient(_min + value * (_max - _min));
	}
}