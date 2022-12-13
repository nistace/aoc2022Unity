using System;
using UnityEngine;
using Utils.Extensions;

namespace Day11.Common {
	public class Day11Item : MonoBehaviour {
		public static long groupModulo { get; set; } = 1;

		[SerializeField] protected Rigidbody  _rigidbody;
		[SerializeField] protected Material[] _materials;

		private long realWorryLevel { get; set; }
		public  long worryLevel     { get; private set; }

		public new Rigidbody rigidbody => _rigidbody;

		private void Reset() {
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void Awake() {
			GetComponentInChildren<MeshRenderer>().material = _materials.Random();
		}

		public bool IsDivisibleBy(int div) => worryLevel % div == 0;

		public void SetWorryLevel(long worryLevel) {
			realWorryLevel = worryLevel;
			this.worryLevel = worryLevel;
			RefreshName();
		}

		private void RefreshName() => name = $"Item {this.worryLevel}";

		public void DoOperation(Func<long, long> operation) {
			realWorryLevel = operation(worryLevel);
			worryLevel = operation(worryLevel) % groupModulo;
			RefreshName();
		}

		public void AddRelief() {
			realWorryLevel /= 3;
			worryLevel = realWorryLevel % groupModulo;
			RefreshName();
		}

		public void Attach(Transform parent) {
			transform.SetParent(parent);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			_rigidbody.isKinematic = true;
		}

		public void Release() {
			transform.SetParent(null);
			_rigidbody.isKinematic = false;
		}
	}
}