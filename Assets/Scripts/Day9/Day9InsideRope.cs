using UnityEngine;

namespace AOC22.Day9 {
	public class Day9InsideRope : MonoBehaviour {
		[SerializeField] protected Transform _headBone;

		public void LookTowards(Vector3 targetPosition) {
			transform.up = targetPosition - transform.position;
			_headBone.position = targetPosition;
		}
	}
}