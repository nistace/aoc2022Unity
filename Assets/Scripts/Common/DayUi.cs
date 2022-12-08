using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AOC22.Common {
	public class DayUi : MonoBehaviour {
		[SerializeField] protected TMP_Text   _stepCount;
		[SerializeField] protected Image      _progressBar;
		[SerializeField] protected TMP_Text   _tempResultText;
		[SerializeField] protected GameObject _resultPanel;
		[SerializeField] protected TMP_Text   _resultText;
		[SerializeField] protected Button     _goToMenuButton;

		public UnityEvent onGoToMenuClicked => _goToMenuButton.onClick;

		public void Refresh(int instructionIndex, int instructionCount, string result) {
			_stepCount.text = $"{instructionIndex}/{instructionCount}";
			_progressBar.fillAmount = instructionCount == 0 ? 0f : (float)instructionIndex / instructionCount;
			_tempResultText.text = result;
		}

		public void SetVisible(bool visible) => GetComponent<Canvas>().enabled = visible;

		public void ShowFinalResult(string finalResult) {
			_resultText.text = $"... And this is how we found out that <#9DF>{finalResult}</color> was the answer!<br><br>It was probably the optimal way to handle this!";
			_resultPanel.SetActive(true);
		}

		public void HideFinalResult() => _resultPanel.SetActive(false);
	}
}