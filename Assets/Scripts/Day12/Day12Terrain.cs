using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AOC22.Common;
using UnityEngine;

public class Day12Terrain : MonoBehaviour {
	[SerializeField] protected MudBlock _mudBlockPrefab;
	[SerializeField] protected Day12Elf _elfPrefab;

	private static IEnumerable<Vector2Int> lookOutNeighbourDeltas { get; } = new[] {
		new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1)
	};

	private Dictionary<Vector2Int, MudBlock> blocks { get; } = new Dictionary<Vector2Int, MudBlock>();
	private Dictionary<Vector2Int, Day12Elf> elves  { get; } = new Dictionary<Vector2Int, Day12Elf>();
	public  int                              width  { get; private set; }
	public  int                              length { get; private set; }

	public bool IsInGrid(Vector2Int position) => blocks.ContainsKey(position);
	public int GetHeight(Vector2Int position) => blocks.ContainsKey(position) ? blocks[position].height : 0;

	public Vector3 GetWorldPositionAtTop(Vector2Int position) => blocks.ContainsKey(position)
		? blocks[position].GetWorldPositionAtTop()
		: new Vector3(position.x * _mudBlockPrefab.unitDimensions.x, 0, position.y * _mudBlockPrefab.unitDimensions.z);

	public Vector3 GetLowestNeighbourPosition(Vector2Int node) => lookOutNeighbourDeltas.Select(t => GetWorldPositionAtTop(node + t)).OrderBy(t => t.y).FirstOrDefault();

	public void Initialize(ICollection<(int h, int x, int y)> heightMap) {
		foreach (var block in blocks.Values) Destroy(block.gameObject);
		blocks.Clear();

		foreach ((var h, var x, var y) in heightMap) {
			var position = new Vector2Int(x, y);
			blocks.Add(position, Instantiate(_mudBlockPrefab, transform));
			blocks[position].Initialize(position, h);
		}
		width = 1 + heightMap.Max(t => t.x) - heightMap.Min(t => t.x);
		length = 1 + heightMap.Max(t => t.y) - heightMap.Min(t => t.y);
	}

	public IEnumerator ShowElf(Vector2Int position, int value, bool locked) {
		if (!elves.ContainsKey(position)) {
			elves.Add(position, Instantiate(_elfPrefab, GetWorldPositionAtTop(position), Quaternion.identity, transform));
			yield return null;
		}
		yield return StartCoroutine(elves[position].ChangePanelValue(value, locked));
	}
}