using System.Collections;
using UnityEngine;

namespace AOC22.Common {
	public class CrateMover : MonoBehaviour {
		[SerializeField] protected Vector3   _handleCrateOffset = new Vector3(.262f, 0, 0);
		[SerializeField] protected Vector3   _maneuverOffset    = new Vector3(.6f, 0, 0);
		[SerializeField] protected Transform _movementLever;
		[SerializeField] protected Transform _craneLever;
		[SerializeField] protected Transform _crane;
		[SerializeField] protected float     _craneHigherOffset = 1.5f;
		[SerializeField] protected float     _craneCrateOffset  = 1.12f;
		[SerializeField] protected float     _speed             = .5f;
		[SerializeField] protected float     _rotationSpeed     = 1;
		[SerializeField] protected float     _craneSpeed        = 10;

		public Vector3 handleCrateOffset => _handleCrateOffset;
		public Vector3 maneuverOffset    => _maneuverOffset;

		public IEnumerator MoveTo(Vector3 destination, Vector3 forwardVector) {
			_movementLever.rotation = Quaternion.Euler(Vector3.SignedAngle(transform.forward, forwardVector, Vector3.up) < 0 ? -10 : 10, 0, 0);
			while (Mathf.Abs(Vector3.SignedAngle(transform.forward, forwardVector, Vector3.up)) > .1f) {
				transform.forward = Vector3.RotateTowards(transform.forward, forwardVector, _rotationSpeed * AocTime.deltaTime, 1);
				yield return null;
			}
			_movementLever.rotation = Quaternion.Euler(0, 0, Mathf.Abs(Vector3.SignedAngle(transform.forward, destination - transform.position, Vector3.up)) < 10f ? 10 : -10);
			while (Vector3.SqrMagnitude(transform.position - destination) > .000001f) {
				transform.position = Vector3.MoveTowards(transform.position, destination, _speed * AocTime.deltaTime);
				_movementLever.rotation = Quaternion.Euler(0, 0, 10);
				yield return null;
			}
		}

		public IEnumerator MoveCrane(float height, bool higher) {
			var fixedHeight = height + (higher ? _craneHigherOffset : _craneCrateOffset);
			_craneLever.rotation = Quaternion.Euler(0, 0, fixedHeight > _craneLever.position.y ? 10 : -10);
			while (!Mathf.Approximately(fixedHeight, _crane.position.y)) {
				_crane.position = new Vector3(_crane.position.x, Mathf.MoveTowards(_crane.position.y, fixedHeight, _craneSpeed * AocTime.deltaTime), _crane.position.z);
				yield return null;
			}
		}

		public void AttachToCrane(Transform item) => item.SetParent(_crane.transform);

		public void ReleaseAllFromCrane() {
			for (var i = 0; i < transform.childCount; ++i) {
				transform.GetChild(i).SetParent(null);
			}
		}
	}
}