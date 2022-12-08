using UnityEngine;

namespace AOC22.Common {
	public class LookTowards : MonoBehaviour {
		[SerializeField] protected Transform _target;

		private void Update() => transform.forward = _target.position - transform.position;
	}
}