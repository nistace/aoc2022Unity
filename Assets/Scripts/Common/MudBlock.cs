using UnityEngine;

namespace AOC22.Common {
	public class MudBlock : MonoBehaviour {
		[SerializeField] protected MeshRenderer _mud;
		[SerializeField] protected MeshRenderer _grass;
		[SerializeField] protected int          _height;
		[SerializeField] protected Vector3      _unitDimensions;

		public int     height         => _height;
		public Vector3 unitDimensions => _unitDimensions;

		public void Initialize(Vector2Int gridPosition, int height) {
			_height = height;

			transform.position = new Vector3(gridPosition.x * _unitDimensions.x, 0, gridPosition.y * _unitDimensions.z);
			_mud.transform.localScale = new Vector3(1, height, 1);
			_grass.transform.localPosition = new Vector3(0, _height * _unitDimensions.y, 0);
			_mud.material.mainTextureScale = new Vector2(1, height);
		}

		public Vector3 GetWorldPositionAtTop() => _grass.transform.position;
	}
}