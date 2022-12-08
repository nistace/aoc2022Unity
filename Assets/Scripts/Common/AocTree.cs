using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AOC22.Common {
	public class AocTree : MonoBehaviour {
		[SerializeField] protected int                _height;
		[SerializeField] protected bool               _visible = true;
		[SerializeField] protected MeshRenderer       _trunkBaseRenderer;
		[SerializeField] protected MeshRenderer       _leafRenderer;
		[SerializeField] protected Material           _visibleMaterial;
		[SerializeField] protected Material           _hiddenMaterial;
		[SerializeField] protected List<MeshRenderer> _additionalTrunkParts = new List<MeshRenderer>();
		[SerializeField] protected Vector3            _chunkLocalPosition   = new Vector3(0, 1.8f, 0);
		[SerializeField] protected Vector3            _chunkLocalScale      = new Vector3(.9f, 1f, .9f);

		[ContextMenu("Refresh Height")]
		private void RefreshHeight() => SetHeight(_height);

		public void SetHeight(int height) {
			_height = height;
			_leafRenderer.transform.SetParent(transform);
			_trunkBaseRenderer.transform.SetParent(transform);

			if (_additionalTrunkParts.Count > 0) {
				if (Application.isPlaying) Destroy(_additionalTrunkParts[0].gameObject);
				else DestroyImmediate(_additionalTrunkParts[0].gameObject);
			}
			_additionalTrunkParts.Clear();

			for (var i = 0; i < height; ++i) {
				var chunk = Instantiate(_trunkBaseRenderer, transform);
				_additionalTrunkParts.Add(chunk);
			}

			if (_additionalTrunkParts.Count > 0) SetupTrunkChunk(_additionalTrunkParts[0].transform, _trunkBaseRenderer.transform);
			for (var i = 1; i < _additionalTrunkParts.Count; ++i) SetupTrunkChunk(_additionalTrunkParts[i].transform, _additionalTrunkParts[i - 1].transform);
			_leafRenderer.transform.localScale = Vector3.one;
			_leafRenderer.transform.SetParent((_additionalTrunkParts.LastOrDefault() ?? _trunkBaseRenderer).transform);
			_leafRenderer.transform.localPosition = Vector3.zero;
			SetVisible(_visible);
		}

		public void SetVisible(bool visible) {
			_visible = visible;

			if (Application.isPlaying) {
				_trunkBaseRenderer.material = _visible ? _visibleMaterial : _hiddenMaterial;
				_leafRenderer.material = _visible ? _visibleMaterial : _hiddenMaterial;
				foreach (var additionalTrunkChunk in _additionalTrunkParts) additionalTrunkChunk.material = _visible ? _visibleMaterial : _hiddenMaterial;
			}
			else {
				_trunkBaseRenderer.sharedMaterial = _visible ? _visibleMaterial : _hiddenMaterial;
				_leafRenderer.sharedMaterial = _visible ? _visibleMaterial : _hiddenMaterial;
				foreach (var additionalTrunkChunk in _additionalTrunkParts) additionalTrunkChunk.sharedMaterial = _visible ? _visibleMaterial : _hiddenMaterial;
			}
		}

		private void SetupTrunkChunk(Transform trunkPart, Transform parentTrunkPart) {
			trunkPart.SetParent(parentTrunkPart);
			trunkPart.localScale = _chunkLocalScale;
			trunkPart.localPosition = _chunkLocalPosition;
		}

		public bool IsVisible() => _visible;
	}
}