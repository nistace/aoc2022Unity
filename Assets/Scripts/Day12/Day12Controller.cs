using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AOC22.Common;
using UnityEngine;
using Utils.Extensions;

namespace Day12 {
	public class Day12Controller : AbstractDayController {
		[SerializeField] protected Day12Terrain _terrain;
		[SerializeField] protected Day12Elf     _elfPrefab;
		[SerializeField] protected bool         _startFromSorAnyA;
		[SerializeField] protected Vector2Int[] _startPositions;
		[SerializeField] protected Vector2Int   _endPosition;
		[SerializeField] protected int          _step;
		[SerializeField] protected int          _endPathLength = -1;
		[SerializeField] protected AnchorCamera _camera;
		[SerializeField] protected Transform    _cameraIntroStandee;
		[SerializeField] protected Transform    _cameraSimulationAnchor;
		[SerializeField] protected Transform    _cameraOutroStandee;
		[SerializeField] protected Vector3      _cameraSimulationAnchorOffset = Vector3.up * .5f;
		[SerializeField] protected float        _cameraDistance               = .3f;

		protected override string GetResult() => _endPathLength < 0 ? "?" : $"{_endPathLength}";

		protected override int GetCurrentStep() => _step;

		protected override int GetStepCount() => _terrain.width * _terrain.length;

		protected override bool TryParse(string inputText) {
			var inputLines = inputText.Split(Environment.NewLine).Where(t => !string.IsNullOrEmpty(t)).ToArray();

			_startPositions = inputLines.Select((t, i) => (t, i)).Where(t => t.t.Contains('S')).Select(t => new Vector2Int(t.t.IndexOf('S'), t.i)).ToArray();
			_endPosition = inputLines.Select((t, i) => (t, i)).Where(t => t.t.Contains('E')).Select(t => new Vector2Int(t.t.IndexOf('E'), t.i)).First();

			var heightMap = inputLines.SelectMany((line, y) => line.Select((col, x) => (h: col switch { 'S' => 'a', 'E' => 'z', _ => col } - 'a', x, y))).ToArray();
			_terrain.Initialize(heightMap);

			_cameraIntroStandee.transform.position = _terrain.GetWorldPositionAtTop(_startPositions[0]);
			_cameraOutroStandee.transform.position = _terrain.GetWorldPositionAtTop(_endPosition);

			if (_startFromSorAnyA) {
				_startPositions = _startPositions.Union(heightMap.Where(t => t.h == 0).Select(t => new Vector2Int(t.x, t.y))).ToArray();
			}

			return true;
		}

		protected override IEnumerator DoPlaySimulation() {
			_camera.SetAnchor("Intro");
			_step = 0;
			_endPathLength = -1;
			var directions = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
			var open = _startPositions.ToList();
			var closed = new List<Vector2Int>();
			var weights = open.ToDictionary(t => t, _ => 0);
			foreach (var openPosition in open) {
				StartCoroutine(_terrain.ShowElf(openPosition, weights[openPosition], false));
			}
			for (var wait = AocTime.deltaTime; wait < 2; wait += AocTime.deltaTime) yield return null;
			_camera.SetAnchor("Simulation");
			while (open.Count > 0 && !closed.Contains(_endPosition)) {
				var node = open[0];

				var nodeWordPosition = _terrain.GetWorldPositionAtTop(node) + _cameraSimulationAnchorOffset;
				_cameraSimulationAnchor.position = _terrain.GetLowestNeighbourPosition(node).With(y: nodeWordPosition.y) + _cameraSimulationAnchorOffset / 2;
				_cameraSimulationAnchor.forward = nodeWordPosition - _cameraSimulationAnchor.position;
				_cameraSimulationAnchor.position = nodeWordPosition - _cameraDistance * _cameraSimulationAnchor.forward;

				for (var wait = AocTime.deltaTime; wait < 1f; wait += AocTime.deltaTime) yield return null;
				yield return StartCoroutine(_terrain.ShowElf(node, weights[node], true));
				open.RemoveAt(0);
				closed.Add(node);

				foreach (var openedNode in directions.Select(t => node + t).Where(t => _terrain.IsInGrid(t) && _terrain.GetHeight(t) <= _terrain.GetHeight(node) + 1).Where(t => !closed.Contains(t))
								.Where(t => !weights.ContainsKey(t) || weights[t] > weights[node] + 1)) {
					for (var wait = AocTime.deltaTime; wait < .5f; wait += AocTime.deltaTime) yield return null;
					open.Remove(openedNode);
					if (weights.ContainsKey(openedNode)) weights.Remove(openedNode);
					weights.Add(openedNode, weights[node] + 1);
					open.Insert(open.Count(t => weights[t] < weights[openedNode]), openedNode);
					yield return StartCoroutine(_terrain.ShowElf(openedNode, weights[openedNode], false));
				}

				_step++;
				_endPathLength = weights.ContainsKey(_endPosition) ? weights[_endPosition] : -1;
			}
			_step = GetStepCount();
			_endPathLength = weights[_endPosition];
			_camera.SetAnchor("Outro");
		}
	}
}