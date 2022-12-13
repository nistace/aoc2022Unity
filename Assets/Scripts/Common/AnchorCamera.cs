using System;
using System.Linq;
using UnityEngine;

namespace AOC22.Common {
	public class AnchorCamera : MonoBehaviour {
		[SerializeField] protected AnchorData[] _anchors;
		[SerializeField] protected AnchorData   _currentAnchor;
		[SerializeField] protected float        _smoothFollow  = .5f;
		[SerializeField] protected float        _smoothForward = .5f;
		[SerializeField] protected Vector3      _followVelocity;
		[SerializeField] protected Vector3      _forwardVelocity;

		private void Awake() {
			_currentAnchor = _anchors.FirstOrDefault();
		}

		private void Update() {
			if (_currentAnchor != null && _currentAnchor.target) {
				transform.position = Vector3.SmoothDamp(transform.position, _currentAnchor.target.position, ref _followVelocity, _smoothFollow);
				transform.forward = Vector3.SmoothDamp(transform.forward, _currentAnchor.target.forward, ref _forwardVelocity, _smoothForward);
			}
		}

		public void SetAnchor(string anchor) {
			_currentAnchor = _anchors.FirstOrDefault(t => t.name == anchor);
			if (_currentAnchor == null) Debug.LogWarning($"No anchor named {anchor} for the camera");
		}

		public void AddAnchor(AnchorData anchorData) => _anchors = _anchors.Union(new[] { anchorData }).ToArray();

		[Serializable] public class AnchorData {
			[SerializeField] protected string    _name;
			[SerializeField] protected Transform _target;

			public AnchorData(string name, Transform target) {
				_name = name;
				_target = target;
			}

			public string name => _name;

			public Transform target => _target;
		}
	}
}