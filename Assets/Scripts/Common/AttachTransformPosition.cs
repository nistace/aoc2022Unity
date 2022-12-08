using UnityEngine;

namespace AOC22.Common {
	public class AttachTransformPosition : MonoBehaviour {
		[SerializeField] protected Transform _target;

		private void Update() => transform.position = _target.position;
	}
}