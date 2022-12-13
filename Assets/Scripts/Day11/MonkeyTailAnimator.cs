using System;
using System.Linq;
using UnityEngine;
using Utils.Extensions;
using Random = UnityEngine.Random;

public class MonkeyTailAnimator : MonoBehaviour {
	[SerializeField] protected Bone[] _bones;
	[SerializeField] protected float  _speed = 2;
	[SerializeField] protected float  _offset;

	private void Awake() {
		_offset = Random.value * 10;
	}

	private void Reset() {
		_bones = transform.GetChild(0).GetComponentsInChildren<Transform>().Select(t => new Bone(t, t.localRotation)).ToArray();
	}

	public void Update() {
		var lerp = (Mathf.Sin((Time.time + _offset) * _speed) + 1) * .5f;
		_bones.ForEach(t => t.Animate(lerp));
	}

	[ContextMenu("Save rotations as min")] private void SaveRotationsAsMin() => _bones.ForEach(t => t.SaveRotationsAsMin());
	[ContextMenu("Save rotations as max")] private void SaveRotationsAsMax() => _bones.ForEach(t => t.SaveRotationsAsMax());

	[Serializable]
	protected class Bone {
		[SerializeField] protected Transform  _bone;
		[SerializeField] protected Quaternion _rotationMin;
		[SerializeField] protected Quaternion _rotationMax;

		public Bone(Transform bone, Quaternion defaultRotation) {
			_bone = bone;
			_rotationMin = defaultRotation;
			_rotationMax = defaultRotation;
		}

		public void Animate(float lerp) {
			_bone.localRotation = Quaternion.Slerp(_rotationMin, _rotationMax, lerp);
		}

		public void SaveRotationsAsMin() => _rotationMin = _bone.localRotation;
		public void SaveRotationsAsMax() => _rotationMax = _bone.localRotation;
	}
}