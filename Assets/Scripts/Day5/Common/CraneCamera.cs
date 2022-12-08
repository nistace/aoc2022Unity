using UnityEngine;

namespace Day5.Common {
	public class CraneCamera : MonoBehaviour {
		[SerializeField] protected Transform _crane;
		[SerializeField] protected Transform _cameraDistance;
		[SerializeField] protected float     _smoothFollow      = .5f;
		[SerializeField] protected float     _smoothDistance    = .5f;
		[SerializeField] protected float     _minDistance       = -1;
		[SerializeField] protected float     _distancePerCraneY = -.5f;
		[SerializeField] protected Vector3   _followVelocity;
		[SerializeField] protected float     _distanceVelocity;

		private void Update() {
			var position = new Vector3(0, transform.position.y, transform.position.z);
			var targetPosition = new Vector3(0, _crane.position.y, _crane.position.z);
			transform.position = Vector3.SmoothDamp(position, targetPosition, ref _followVelocity, _smoothFollow);
			var distance = Mathf.SmoothDamp(_cameraDistance.localPosition.z, _minDistance + _crane.position.y * _distancePerCraneY, ref _distanceVelocity, _smoothDistance);
			_cameraDistance.localPosition = new Vector3(0, 0, distance);
		}
	}
}