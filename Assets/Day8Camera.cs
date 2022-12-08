using System;
using UnityEngine;

public class Day8Camera : MonoBehaviour {
	public enum Anchor {
		Intro       = 0,
		Forest      = 1,
		PointOfView = 2,
		Outro       = 3
	}

	[SerializeField] protected AnchorData[] _anchors;
	[SerializeField] protected Anchor       _currentAnchor;
	[SerializeField] protected float        _smoothFollow  = .5f;
	[SerializeField] protected float        _smoothForward = .5f;
	[SerializeField] protected Vector3      _followVelocity;
	[SerializeField] protected Vector3      _forwardVelocity;

	private void Update() {
		if ((int)_currentAnchor >= 0 && (int)_currentAnchor < _anchors.Length) {
			var target = _anchors[(int)_currentAnchor].target;
			transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _followVelocity, _smoothFollow);
			transform.forward = Vector3.SmoothDamp(transform.forward, target.forward, ref _forwardVelocity, _smoothForward);
		}
	}

#if UNITY_EDITOR
	private void OnValidate() {
		for (var index = 0; index < _anchors.Length; index++) {
			_anchors[index].name = EnumUtils.Values<Anchor>()[index] + "";
		}
	}
#endif

	[Serializable] protected class AnchorData {
		[HideInInspector, SerializeField] protected string    _name;
		[SerializeField]                  protected Transform _target;

		public string name {
			get => _name;
			set => _name = value;
		}

		public Transform target => _target;
	}

	public void SetAnchor(Anchor anchor) => _currentAnchor = anchor;
}