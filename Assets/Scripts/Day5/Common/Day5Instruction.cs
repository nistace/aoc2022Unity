using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Day5.Common {
	[Serializable]
	public class Day5Instruction {
		[SerializeField] protected int _amount;
		[SerializeField] protected int _fromLane;
		[SerializeField] protected int _toLane;

		public int amount   => _amount;
		public int fromLane => _fromLane;
		public int toLane   => _toLane;

		public Day5Instruction(int amount, int fromLane, int toLane) {
			_amount = amount;
			_fromLane = fromLane;
			_toLane = toLane;
		}

		private const string regexTemplate = "move (\\d+) from (\\d+) to (\\d+)";

		public static bool TryParse(string line, out Day5Instruction day5Instruction) {
			day5Instruction = default;
			var match = Regex.Match(line, regexTemplate);
			if (!match.Success) return false;
			day5Instruction = new Day5Instruction(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
			return true;
		}

		public static bool TryParse(IEnumerable<string> lines, out List<Day5Instruction> instructions) {
			instructions = new List<Day5Instruction>();
			foreach (var instructionLine in lines) {
				if (TryParse(instructionLine, out var instruction)) {
					instructions.Add(instruction);
				}
			}
			return true;
		}
	}
}