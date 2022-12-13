using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AOC22.Common;
using Day11.Common;
using UnityEngine;
using Utils.Extensions;

namespace AOC22.Day11 {
	public class Day11Controller : AbstractDayController {
		[SerializeField] protected AnchorCamera    _camera;
		[SerializeField] protected Day11Item[]     _itemPrefabs;
		[SerializeField] protected Monkey          _monkeyPrefab;
		[SerializeField] protected int             _targetRound = 20;
		[SerializeField] protected bool            _withRelief;
		[SerializeField] protected List<Day11Item> _items   = new List<Day11Item>();
		[SerializeField] protected List<Monkey>    _monkeys = new List<Monkey>();
		[SerializeField] protected int             _round;
		[SerializeField] protected int             _turn;

		private void Awake() {
			_camera.SetAnchor("Intro");
		}

		protected override string GetResult() => $"{_monkeys.Select(t => t.inspectedItemCount).OrderByDescending(t => t).Take(2).Aggregate(1, (x, y) => x * y)}";

		protected override int GetCurrentStep() => _round * _monkeys.Count + _turn;

		protected override int GetStepCount() => _targetRound * _monkeys.Count;

		protected override bool TryParse(string inputText) {
			var monkeyMatches = inputText.Replace(Environment.NewLine, "").Split("Monkey")
				.Select(t => Regex.Match(t, ".*Starting.*:([\\d, ]+)Operation:.*(new = [old \\+\\*\\d]+).*Test.*by *(\\d+).*true.*monkey (\\d+).*false.*monkey (\\d+)")).Where(t => t.Success).ToArray();
			_monkeys.AddRange(monkeyMatches.Select((_, i) => Instantiate(_monkeyPrefab)));
			for (var i = 0; i < monkeyMatches.Length; ++i) {
				_monkeys[i].name = $"Monkey {i}";
				_monkeys[i].transform.position = Vector3.forward.Rotate(aroundYAxis: 360f * i / _monkeys.Count);
				_monkeys[i].transform.forward = -_monkeys[i].transform.position;
				foreach (var itemWorryLevel in monkeyMatches[i].Groups[1].Value.Split(",").Select(int.Parse)) {
					_items.Add(Instantiate(_itemPrefabs.Random()));
					_monkeys[i].InstantMoveItemToStack(_items.Last());
					_items.Last().SetWorryLevel(itemWorryLevel);
				}
				_monkeys[i].operation = ParseOperation(monkeyMatches[i].Groups[2].Value);
				_monkeys[i].divider = int.Parse(monkeyMatches[i].Groups[3].Value);
				_monkeys[i].targetOnTrue = _monkeys[int.Parse(monkeyMatches[i].Groups[4].Value)];
				_monkeys[i].targetOnFalse = _monkeys[int.Parse(monkeyMatches[i].Groups[5].Value)];
				_camera.AddAnchor(new AnchorCamera.AnchorData($"Monkey {i}", _monkeys[i].cameraAnchor));
			}
			Day11Item.groupModulo = _monkeys.Aggregate(1, (t, monkey) => t * monkey.divider);
			return true;
		}

		private static Func<long, long> ParseOperation(string operation) {
			if (operation.Trim() == "new = old * old") return t => t * t;
			if (operation.Trim().StartsWith("new = old * ")) return t => t * int.Parse(operation.Trim().Substring("new = old * ".Length).Trim());
			if (operation.Trim().StartsWith("new = old + ")) return t => t + int.Parse(operation.Trim().Substring("new = old + ".Length).Trim());
			Console.WriteLine("!!! Operation not understood: " + operation);
			return t => t;
		}

		protected override IEnumerator DoPlaySimulation() {
			_camera.SetAnchor("Intro");
			yield return new WaitForSeconds(2);
			for (_round = 0; _round < _targetRound; ++_round) {
				for (_turn = 0; _turn < _monkeys.Count; _turn++) {
					_camera.SetAnchor($"Monkey {_turn}");
					var monkey = _monkeys[_turn];
					while (monkey.TryPopNextItemToThrow(out var item)) {
						yield return StartCoroutine(monkey.InspectAndThrow(item, _withRelief));
						var target = monkey.ChooseThrowTarget(item);
						yield return StartCoroutine(target.Catch(item));
					}
					StartCoroutine(monkey.StopWatchingItems());
				}
			}
			_turn = 0;
			_camera.SetAnchor("Outro");
		}
	}
}