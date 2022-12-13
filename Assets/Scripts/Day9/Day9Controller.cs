using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOC22.Common;
using UnityEngine;

namespace AOC22.Day9 {
	public class Day9Controller : AbstractDayController {
		[SerializeField] protected int              _ropeLength        = 2;
		[SerializeField] protected List<Vector2Int> _instructions      = new List<Vector2Int>();
		[SerializeField] protected List<Vector2Int> _ropeKnotPositions = new List<Vector2Int>();
		[SerializeField] protected Day9Rope         _rope;
		[SerializeField] protected Transform        _highlightCellPrefab;
		[SerializeField] protected List<Transform>  _highlightCells = new List<Transform>();
		[SerializeField] protected int              _step;
		[SerializeField] protected AnchorCamera     _camera;
		[SerializeField] protected Transform        _outroCameraAnchor;
		[SerializeField] protected float            _outroCameraAnchorHeightMultiplier = 1.5f;

		private HashSet<Vector2Int> highlightedCellPositions { get; } = new HashSet<Vector2Int>();

		protected override string GetResult() => $"{highlightedCellPositions.Count}";

		protected override int GetCurrentStep() => _step;

		protected override int GetStepCount() => _instructions.Count;

		protected override bool TryParse(string inputText) {
			_instructions.Clear();
			foreach (var highlightedCell in _highlightCells) Destroy(highlightedCell.gameObject);
			_highlightCells.Clear();
			highlightedCellPositions.Clear();
			_rope.SetLength(_ropeLength);
			_ropeKnotPositions.Clear();
			_ropeKnotPositions.AddRange(Enumerable.Repeat(Vector2Int.zero, _ropeLength));
			_instructions.AddRange(inputText.Split(Environment.NewLine).Select(t => Regex.Match(t, "(R|L|U|D) *(\\d+)")).Where(t => t.Success).Select(t => (move: t.Groups[1].Value switch {
				"R" => Vector2Int.right,
				"L" => Vector2Int.left,
				"U" => Vector2Int.up,
				"D" => Vector2Int.down,
				_   => Vector2Int.zero
			}, times: int.Parse(t.Groups[2].Value))).SelectMany(t => Enumerable.Repeat(t.move, t.times)));
			_camera.SetAnchor("Intro");
			return true;
		}

		protected override IEnumerator DoPlaySimulation() {
			_camera.SetAnchor("Intro");
			yield return new WaitForSeconds(2);
			_camera.SetAnchor("Elf");
			for (_step = 0; _step < _instructions.Count; ++_step) {
				RefreshAllKnotPositions(_instructions[_step]);
				for (var i = 0; i < _ropeKnotPositions.Count; ++i) {
					yield return StartCoroutine(_rope.MoveKnot(i, _ropeKnotPositions[i]));
					yield return null;
				}
				if (!highlightedCellPositions.Contains(_ropeKnotPositions.Last())) {
					_highlightCells.Add(Instantiate(_highlightCellPrefab, new Vector3(_ropeKnotPositions.Last().x, 0, _ropeKnotPositions.Last().y), Quaternion.identity));
					highlightedCellPositions.Add(_ropeKnotPositions.Last());
				}
			}
			var minX = highlightedCellPositions.Min(t => t.x);
			var maxX = highlightedCellPositions.Max(t => t.x);
			var minY = highlightedCellPositions.Min(t => t.y);
			var maxY = highlightedCellPositions.Max(t => t.y);
			_outroCameraAnchor.transform.position = new Vector3((minX + maxX) / 2f, _outroCameraAnchorHeightMultiplier * Mathf.Max(maxX - minX, maxY - minY), (minY + maxY) / 2f);

			_camera.SetAnchor("Outro");
		}

		private void RefreshAllKnotPositions(Vector2Int move) {
			_ropeKnotPositions[0] += move;
			for (var i = 1; i < _ropeKnotPositions.Count; ++i) {
				var current = _ropeKnotPositions[i];
				var previous = _ropeKnotPositions[i - 1];
				if (GridDistance(previous, current) == 2 && (previous.x == current.x || previous.y == current.y) || GridDistance(previous, current) > 2) {
					_ropeKnotPositions[i] += new Vector2Int(Mathf.Clamp(previous.x - current.x, -1, 1), Mathf.Clamp(previous.y - current.y, -1, 1));
				}
			}
		}

		private static int GridDistance(Vector2Int first, Vector2Int other) => Math.Abs(other.x - first.x) + Math.Abs(other.y - first.y);
	}
}