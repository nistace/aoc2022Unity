using UnityEngine;

[ExecuteAlways]
public class CopyPosition : MonoBehaviour {
	[SerializeField] protected Transform _transform;

	private void Update() {
		if (_transform) {
			transform.position = _transform.position;
		}
	}
}