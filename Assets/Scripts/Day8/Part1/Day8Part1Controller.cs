using System.Collections;
using System.Collections.Generic;
using AOC22.Common;
using AOC22.Day8.Common;
using UnityEngine;

namespace AOC22.Day8.Part1 {
	public class Day8Part1Controller : AbstractDayController {
		[SerializeField] protected Day8Camera      _camera;
		[SerializeField] protected CrateMover      _crateMover;
		[SerializeField] protected Day8Forest      _forest;
		[SerializeField] protected Day8SpottingElf _spottingElf;
		[SerializeField] protected int             _step;
		[SerializeField] protected float           _distanceToTheTree   = 1;
		[SerializeField] protected float           _minCraneHeight      = 1;
		[SerializeField] protected float           _craneHeightStep     = 1;
		[SerializeField] protected List<Vector3>   _crateMoverPositions = new List<Vector3>();

		protected override string GetResult() => $"{_forest?.CountVisibleTrees() ?? 0}";
		protected override int GetCurrentStep() => _step;
		protected override int GetStepCount() => _crateMoverPositions.Count;

		protected override bool TryParse(string inputText) {
			_crateMoverPositions.Clear();
			if (!_forest.TryParse(inputText)) return false;
			var width = _forest.width;
			var length = _forest.length;
			for (var x = 0; x < width; ++x) _crateMoverPositions.Add(new Vector3(x, 0, -_distanceToTheTree));
			for (var y = 0; y < length; ++y) _crateMoverPositions.Add(new Vector3(width + _distanceToTheTree - 1, 0, y));
			for (var x = width - 1; x >= 0; --x) _crateMoverPositions.Add(new Vector3(x, 0, length + _distanceToTheTree - 1));
			for (var y = width - 1; y >= 0; --y) _crateMoverPositions.Add(new Vector3(-_distanceToTheTree, 0, y));

			return true;
		}

		protected override IEnumerator DoPlaySimulation() {
			_camera.SetAnchor(Day8Camera.Anchor.Intro);
			yield return new WaitForSeconds(2);
			_camera.SetAnchor(Day8Camera.Anchor.Forest);
			for (_step = 0; _step < _crateMoverPositions.Count; _step++) {
				yield return StartCoroutine(_crateMover.MoveTo(_crateMoverPositions[_step], _crateMoverPositions[_step] - _crateMover.transform.position));
				var forwardVector = Vector3.back;
				if (Mathf.Approximately(_crateMoverPositions[_step].x, -_distanceToTheTree)) forwardVector = Vector3.right;
				else if (Mathf.Approximately(_crateMoverPositions[_step].x, _forest.length + _distanceToTheTree - 1)) forwardVector = Vector3.left;
				else if (Mathf.Approximately(_crateMoverPositions[_step].z, -_distanceToTheTree)) forwardVector = Vector3.forward;
				yield return StartCoroutine(_crateMover.MoveTo(_crateMoverPositions[_step], forwardVector));
				yield return StartCoroutine(_crateMover.MoveCrane(_minCraneHeight, false));
				_camera.SetAnchor(Day8Camera.Anchor.PointOfView);
				_spottingElf.SetSpotting(true);
				var height = _minCraneHeight;
				do {
					yield return StartCoroutine(_crateMover.MoveCrane(height, false));
					height += _craneHeightStep;
				} while (_spottingElf.IsSpottingSomething());
				_spottingElf.SetSpotting(false);
				_camera.SetAnchor(Day8Camera.Anchor.Forest);
				yield return StartCoroutine(_crateMover.MoveCrane(_minCraneHeight, false));
			}
			_camera.SetAnchor(Day8Camera.Anchor.Outro);
		}
	}
}