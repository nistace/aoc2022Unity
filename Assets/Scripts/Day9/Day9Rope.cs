using System.Collections;
using System.Collections.Generic;
using AOC22.Common;
using UnityEngine;

namespace AOC22.Day9 {
	public class Day9Rope : MonoBehaviour {
		[SerializeField] protected float                _offsetY               = .1f;
		[SerializeField] protected float                _yDistanceBetweenKnots = .9f;
		[SerializeField] public    int                  _length;
		[SerializeField] protected Transform            _head;
		[SerializeField] protected Transform            _tailKnot;
		[SerializeField] protected Day9InsideRope       _tailRope;
		[SerializeField] protected Transform            _insideKnotPrefab;
		[SerializeField] protected List<Transform>      _insideKnots = new List<Transform>();
		[SerializeField] protected Day9InsideRope       _insideRopePrefab;
		[SerializeField] protected List<Day9InsideRope> _insideRopes   = new List<Day9InsideRope>();
		[SerializeField] protected float                _knotMoveSpeed = 2;

		private List<Transform>      allKnots       { get; } = new List<Transform>();
		private List<Day9InsideRope> allInsideRopes { get; } = new List<Day9InsideRope>();

		public void SetLength(int count) {
			_length = Mathf.Max(2, count);
			allKnots.Clear();
			allInsideRopes.Clear();

			while (_insideKnots.Count < _length - 2) _insideKnots.Add(Instantiate(_insideKnotPrefab, transform));
			while (_insideKnots.Count > _length - 2) {
				Destroy(_insideKnots[0].gameObject);
				_insideKnots.RemoveAt(0);
			}

			while (_insideRopes.Count < _length - 2) _insideRopes.Add(Instantiate(_insideRopePrefab, transform));
			while (_insideRopes.Count > _length - 2) {
				Destroy(_insideRopes[0].gameObject);
				_insideRopes.RemoveAt(0);
			}

			allKnots.Add(_head);
			allKnots.AddRange(_insideKnots);
			allKnots.Add(_tailKnot);

			allInsideRopes.Add(null);
			allInsideRopes.AddRange(_insideRopes);
			allInsideRopes.Add(_tailRope);

			_head.position = ToKnotPosition(0, Vector2Int.zero);
			_tailKnot.position = ToKnotPosition(_length - 1, Vector2Int.zero);
			_tailRope.LookTowards(ToKnotPosition(_length - 2, Vector2Int.zero));

			for (var i = 0; i < allKnots.Count; ++i) {
				allKnots[i].position = ToKnotPosition(i, Vector2Int.zero);
			}

			for (var i = 1; i < allInsideRopes.Count; ++i) {
				allInsideRopes[i].transform.position = ToKnotPosition(i, Vector2Int.zero);
				allInsideRopes[i].LookTowards(allKnots[i - 1].position);
			}
		}

		private Vector3 ToKnotPosition(int knotIndex, Vector2Int planePosition) => new Vector3(planePosition.x, _offsetY + (_length - knotIndex - 1) * _yDistanceBetweenKnots, planePosition.y);

		public IEnumerator MoveKnot(int knotIndex, Vector2Int planePosition) {
			var targetPosition = ToKnotPosition(knotIndex, planePosition);
			if (allKnots[knotIndex].position == targetPosition) yield break;
			allKnots[knotIndex].forward = targetPosition - allKnots[knotIndex].position;

			while (allKnots[knotIndex].position != targetPosition) {
				allKnots[knotIndex].position = Vector3.MoveTowards(allKnots[knotIndex].position, targetPosition, _knotMoveSpeed * AocTime.deltaTime);
				if (allInsideRopes[knotIndex]) {
					allInsideRopes[knotIndex].transform.position = allKnots[knotIndex].position;
					allInsideRopes[knotIndex].LookTowards(allKnots[knotIndex - 1].position);
				}
				if (knotIndex < allInsideRopes.Count - 1) {
					allInsideRopes[knotIndex + 1].LookTowards(allKnots[knotIndex].position);
				}
				yield return null;
			}
		}
	}
}