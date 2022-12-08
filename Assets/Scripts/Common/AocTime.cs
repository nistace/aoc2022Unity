using UnityEngine;
using UnityEngine.Events;

namespace AOC22.Common {
	public static class AocTime {
		public static float coefficient { get; private set; } = 1;
		public static float deltaTime   => Time.deltaTime * coefficient;

		public static UnityEvent onCoefficientChanged { get; } = new UnityEvent();

		public static void SetCoefficient(float coefficient) {
			AocTime.coefficient = coefficient;
			onCoefficientChanged.Invoke();
		}
	}
}