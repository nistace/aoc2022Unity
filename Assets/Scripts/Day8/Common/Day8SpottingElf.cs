using System;
using AOC22.Common;
using UnityEngine;
using Utils.Extensions;

namespace AOC22.Day8.Common {
	public class Day8SpottingElf : MonoBehaviour {
		[SerializeField] protected Transform  _sight;
		[SerializeField] protected bool       _spotting;
		[SerializeField] protected GameObject _lastHitObject;

		public void Update() {
			if (!_spotting) return;
			if (Physics.Raycast(new Ray(_sight.position, _sight.forward), out var hit)) {
				if (hit.collider.gameObject != _lastHitObject && hit.collider.gameObject.TryGetComponentInParent<AocTree>(out var tree)) {
					tree.SetVisible(true);
					_lastHitObject = hit.collider.gameObject;
				}
			}
			else {
				_lastHitObject = null;
			}
		}

		public void SetSpotting(bool spotting) {
			_spotting = spotting;
			_lastHitObject = null;
		}

		public bool IsSpottingSomething() => _spotting && _lastHitObject;

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			if (Physics.Raycast(new Ray(_sight.position, _sight.forward), out var hit)) {
				Gizmos.color = Color.green;
				Gizmos.DrawLine(_sight.position, hit.point);
			}
			else {
				Gizmos.color = Color.red;
				Gizmos.DrawLine(_sight.position, _sight.position + _sight.forward * 50);
			}
		}
#endif
	}
}