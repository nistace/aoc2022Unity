using System.Collections;
using AOC22.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	[SerializeField] protected AnchorCamera _camera;
	[SerializeField] protected Canvas       _ui;

	private void Awake() {
		_camera.SetAnchor("First");
		_ui.enabled = false;
	}

	private void Start() {
		StartCoroutine(DoAnimation());

		MenuCalendarDay.onDayClicked.AddListener(LoadScene);
	}

	private IEnumerator DoAnimation() {
		yield return new WaitForSeconds(2);
		_camera.SetAnchor("Second");
		yield return new WaitForSeconds(1);
		_camera.SetAnchor("Third");
		yield return new WaitForSeconds(1);
		_ui.enabled = true;
	}

	private static void LoadScene(int day, int part) {
		SceneManager.LoadSceneAsync($"Day{day}Part{part}");
	}
}