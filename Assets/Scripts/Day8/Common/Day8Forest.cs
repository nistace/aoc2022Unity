using System;
using System.Collections.Generic;
using System.Linq;
using AOC22.Common;
using UnityEngine;

namespace AOC22.Day8.Common {
	public class Day8Forest : MonoBehaviour {
		[SerializeField] protected AocTree        _treePrefab;
		[SerializeField] protected List<TreeLine> _treeLines = new List<TreeLine>();
		[SerializeField] protected Transform      _center;

		public int width  => _treeLines.Count;
		public int length => _treeLines.Max(t => t.count);

		public void Clean() { }

		public int CountHiddenTrees() => _treeLines.SelectMany(t => t.trees).Count(t => !t.IsVisible());
		public int CountVisibleTrees() => _treeLines.SelectMany(t => t.trees).Count(t => t.IsVisible());

		public bool TryParse(string inputText) {
			Clear();

			foreach (var line in inputText.Split(Environment.NewLine).Where(t => !string.IsNullOrEmpty(t))) {
				var treeLine = new TreeLine();
				foreach (var treeHeight in line.Trim().Select(t => t - '0')) {
					var newTree = Instantiate(_treePrefab, transform);
					newTree.transform.position = new Vector3(_treeLines.Count, 0, treeLine.count);
					newTree.SetVisible(false);
					newTree.SetHeight(treeHeight);
					treeLine.Add(newTree);
				}
				_treeLines.Add(treeLine);
			}
			_center.position = new Vector3((width - 1) * .5f, 0, (length - 1) * .5f);
			return true;
		}

		private void Clear() {
			foreach (var tree in _treeLines.SelectMany(t => t.trees)) {
				Destroy(tree.gameObject);
			}
			_treeLines.Clear();
		}

		[Serializable]
		protected class TreeLine {
			[SerializeField] protected List<AocTree> _trees = new List<AocTree>();

			public List<AocTree> trees => _trees;
			public int           count => _trees.Count;

			public void Add(AocTree tree) => _trees.Add(tree);
		}
	}
}