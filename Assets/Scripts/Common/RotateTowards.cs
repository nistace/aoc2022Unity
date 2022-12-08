using UnityEngine;

namespace AOC22.Common {
	[ExecuteInEditMode]
	public class RotateTowards : MonoBehaviour {
		[SerializeField] protected Transform _target;

		private void Update() {
			if (_target) {
				transform.forward = _target.position - transform.position;
			}
		}
	}
}