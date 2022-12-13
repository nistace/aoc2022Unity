using System;
using UnityEditor;
using UnityEngine;

public class MenuCalendar : MonoBehaviour {
	[SerializeField] private EnabledScene[] _enabledScenes;

	[Serializable] protected class EnabledScene {
		[HideInInspector, SerializeField] protected string _name;
		[SerializeField]                  protected bool   _part1;
		[SerializeField]                  protected bool   _part2;

		public string name {
			get => _name;
			set => _name = value;
		}

		public bool part1 => _part1;
		public bool part2 => _part2;
	}

#if UNITY_EDITOR

	[ContextMenu("Build")]
	private void Build() {
		var dayNumber = 1;
		foreach (var day in GetComponentsInChildren<MenuCalendarDay>()) {
			var enabledScene = _enabledScenes.Length >= dayNumber ? _enabledScenes[dayNumber - 1] : null;
			day.Build(dayNumber, enabledScene?.part1 ?? false, enabledScene?.part2 ?? false);
			dayNumber++;
		}
		EditorUtility.SetDirty(this);
	}

	private void OnValidate() {
		for (var i = 0; i < _enabledScenes.Length; ++i) {
			_enabledScenes[i].name = $"Day {i + 1}: {(_enabledScenes[i].part1 ? _enabledScenes[i].part2 ? "1 & 2" : "1" : _enabledScenes[i].part2 ? "2" : "-")}";
		}
	}
#endif
}