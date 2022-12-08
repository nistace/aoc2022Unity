using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AOC22.Common {
	public abstract class AbstractDayController : MonoBehaviour {
		[SerializeField] private   InputUi   _inputUi;
		[SerializeField] private   DayUi     _ui;
		[SerializeField] protected TextAsset _defaultInputAsset;

		private void Reset() {
			_inputUi = GetComponentInChildren<InputUi>();
			_ui = GetComponentInChildren<DayUi>();
		}

		private void Start() {
			_ui.SetVisible(false);
			_ui.HideFinalResult();
			_inputUi.SetVisible(true);
			_inputUi.SetInput(_defaultInputAsset.text);
			_inputUi.onStartClicked.AddListener(PlaySimulation);
			_ui.onGoToMenuClicked.AddListener(GoToMenu);
		}

		private static void GoToMenu() => SceneManager.LoadScene("Menu");

		private void PlaySimulation() {
			if (TryParse(_inputUi.inputText)) {
				StartCoroutine(PlaySimulationAndShowResult());
				_inputUi.SetVisible(false);
				_ui.SetVisible(true);
			}
			else {
				_inputUi.SetVisible(true);
				_ui.SetVisible(false);
			}
		}

		private IEnumerator PlaySimulationAndShowResult() {
			yield return StartCoroutine(DoPlaySimulation());
			_ui.ShowFinalResult(GetResult());
		}

		private void Update() {
			_ui.Refresh(GetCurrentStep(), GetStepCount(), GetResult());
		}

		protected abstract string GetResult();
		protected abstract int GetCurrentStep();
		protected abstract int GetStepCount();
		protected abstract bool TryParse(string inputText);
		protected abstract IEnumerator DoPlaySimulation();
	}
}