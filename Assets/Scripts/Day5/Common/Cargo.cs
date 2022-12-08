using System;
using System.Collections.Generic;
using System.Linq;
using AOC22.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Day5.Common {
	public class Cargo : MonoBehaviour {
		[SerializeField] protected Crate           _cratePrefab;
		[SerializeField] protected List<CratePile> _cratePiles = new List<CratePile>();

		[Serializable] protected class CratePile {
			[SerializeField] protected List<Crate> _crates = new List<Crate>();

			public List<Crate> crates => _crates;
			public int         count  => _crates.Count;
			public Crate       last   => _crates.LastOrDefault();

			public void Add(Crate crate) => _crates.Add(crate);
			public void RemoveLast() => _crates.RemoveAt(_crates.Count - 1);
		}

		private void Clear() {
			foreach (var crate in _cratePiles.SelectMany(t => t.crates)) {
				Destroy(crate.gameObject);
			}
			_cratePiles.Clear();
		}

		public bool TryParse(string input) {
			Clear();

			var lines = input.Split(Environment.NewLine).ToList();
			var stackNumberLine = lines.Last();
			lines.RemoveAt(lines.Count - 1);
			lines.Reverse();
			for (var i = 0; i < (stackNumberLine.Length + 1) / 4; ++i) {
				_cratePiles.Add(new CratePile());
				for (var j = 0; j < lines.Count && lines[j][i * 4 + 1] != ' '; ++j) {
					var newCrate = Object.Instantiate(_cratePrefab, new Vector3(0, j * .1f, i * .3f), Quaternion.identity);
					newCrate.SetCharacter(lines[j][i * 4 + 1]);
					_cratePiles[i].Add(newCrate);
				}
			}
			return true;
		}

		public Vector3 GetLanePosition(int lane) => new Vector3(0, 0, (lane - 1) * .3f);

		public float GetHeight(int lane, bool plusOne) => (_cratePiles[lane - 1].count + (plusOne ? 1 : 0)) * .1f;

		public Crate Take(int lane) {
			var crate = _cratePiles[lane - 1].last;
			_cratePiles[lane - 1].RemoveLast();
			return crate;
		}

		public void Add(int lane, Crate crate) {
			_cratePiles[lane - 1].Add(crate);
			crate.transform.SetParent(null);
		}

		public string GetResult() => string.Join("", _cratePiles.Select(t => t.last?.character ?? ' '));
	}
}