using System;
using UnityEngine;
using Utils.Extensions;

namespace AOC22.Common {
	[ExecuteAlways]
	public class RotateTowards : MonoBehaviour {
		protected enum Direction {
			Forward = 0,
			Up      = 1,
			Right   = 2,
			Back    = 3,
			Down    = 4,
			Left    = 5
		}

		protected enum NoTargetBehavior {
			None               = 0,
			SetDefaultAsLocal  = 1,
			SetDefaultAsGlobal = 2,
		}

		[SerializeField] protected Transform        _target;
		[SerializeField] protected Direction        _direction;
		[SerializeField] protected NoTargetBehavior _noTargetBehavior;
		[SerializeField] protected Quaternion       _defaultRotation;
		[SerializeField] protected bool             _lookForwardY;

		public Transform target {
			get => _target;
			set => _target = value;
		}

		private void Update() {
			var targetPosition = _lookForwardY ? _target.position.With(y: transform.position.y) : _target.position;

			if (_target) {
				switch (_direction) {
					case Direction.Forward:
						transform.forward = targetPosition - transform.position;
						break;
					case Direction.Up:
						transform.up = targetPosition - transform.position;
						break;
					case Direction.Right:
						transform.right = targetPosition - transform.position;
						break;
					case Direction.Back:
						transform.forward = transform.position - targetPosition;
						break;
					case Direction.Down:
						transform.up = transform.position - targetPosition;
						break;
					case Direction.Left:
						transform.right = transform.position - targetPosition;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else {
				switch (_noTargetBehavior) {
					case NoTargetBehavior.SetDefaultAsLocal:
						transform.localRotation = _defaultRotation;
						break;
					case NoTargetBehavior.SetDefaultAsGlobal:
						transform.rotation = _defaultRotation;
						break;
					case NoTargetBehavior.None: break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}