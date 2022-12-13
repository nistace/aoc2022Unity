using System;
using System.Collections;
using System.Collections.Generic;
using AOC22.Common;
using Day5.Common;
using UnityEngine;

namespace AOC22.Day5.Part1 {
	public class Day5Part1Controller : AbstractDayController {
		[SerializeField] protected CrateMover            _crateMover;
		[SerializeField] protected Cargo                 _cargo;
		[SerializeField] protected List<Day5Instruction> _instructions;
		[SerializeField] protected int                   _instructionIndex;

		protected override string GetResult() => _cargo?.GetResult() ?? string.Empty;
		protected override int GetCurrentStep() => _instructionIndex;
		protected override int GetStepCount() => _instructions?.Count ?? 0;

		protected override bool TryParse(string inputText) {
			var parts = inputText.Split($"{Environment.NewLine}{Environment.NewLine}");
			if (parts.Length < 2) return false;
			if (!_cargo.TryParse(parts[0])) return false;
			if (!Day5Instruction.TryParse(parts[1].Trim().Split(Environment.NewLine), out _instructions)) return false;
			return true;
		}

		protected override IEnumerator DoPlaySimulation() {
			var maneuverToCrateForward = _crateMover.handleCrateOffset - _crateMover.maneuverOffset;
			for (_instructionIndex = 0; _instructionIndex < _instructions.Count; _instructionIndex++) {
				var day5Instruction = _instructions[_instructionIndex];
				var fromLanePosition = _cargo.GetLanePosition(day5Instruction.fromLane);
				var fromManeuverPosition = fromLanePosition + _crateMover.maneuverOffset;
				var fromCratePosition = fromLanePosition + _crateMover.handleCrateOffset;
				var toLanePosition = _cargo.GetLanePosition(day5Instruction.toLane);
				var toManeuverPosition = toLanePosition + _crateMover.maneuverOffset;
				var toCratePosition = toLanePosition + _crateMover.handleCrateOffset;
				for (var moveStep = 0; moveStep < day5Instruction.amount; ++moveStep) {
					if (Vector3.SqrMagnitude(fromManeuverPosition - _crateMover.transform.position) > .001f) {
						yield return StartCoroutine(_crateMover.MoveTo(fromManeuverPosition, fromManeuverPosition - _crateMover.transform.position));
					}
					yield return StartCoroutine(_crateMover.MoveTo(fromManeuverPosition, maneuverToCrateForward));
					yield return StartCoroutine(_crateMover.MoveCrane(_cargo.GetHeight(day5Instruction.fromLane, false), true));
					yield return StartCoroutine(_crateMover.MoveTo(fromCratePosition, maneuverToCrateForward));
					yield return StartCoroutine(_crateMover.MoveCrane(_cargo.GetHeight(day5Instruction.fromLane, false), false));
					var crate = _cargo.Take(day5Instruction.fromLane);
					_crateMover.AttachToCrane(crate.transform);
					yield return null;
					yield return StartCoroutine(_crateMover.MoveCrane(_cargo.GetHeight(day5Instruction.fromLane, true), true));
					yield return StartCoroutine(_crateMover.MoveTo(fromManeuverPosition, maneuverToCrateForward));
					yield return StartCoroutine(_crateMover.MoveTo(toManeuverPosition, toManeuverPosition - _crateMover.transform.position));
					yield return StartCoroutine(_crateMover.MoveTo(toManeuverPosition, maneuverToCrateForward));
					yield return StartCoroutine(_crateMover.MoveCrane(_cargo.GetHeight(day5Instruction.toLane, true), true));
					yield return StartCoroutine(_crateMover.MoveTo(toCratePosition, maneuverToCrateForward));
					yield return StartCoroutine(_crateMover.MoveCrane(_cargo.GetHeight(day5Instruction.toLane, true), false));
					_cargo.Add(day5Instruction.toLane, crate);
					yield return null;
					yield return StartCoroutine(_crateMover.MoveCrane(_cargo.GetHeight(day5Instruction.toLane, false), true));
					yield return StartCoroutine(_crateMover.MoveTo(toManeuverPosition, maneuverToCrateForward));
				}
			}
		}
	}
}