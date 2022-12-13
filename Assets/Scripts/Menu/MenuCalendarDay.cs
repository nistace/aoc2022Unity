using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuCalendarDay : MonoBehaviour {
	public class DayClicked : UnityEvent<int, int> { }

	[SerializeField] protected int      _day;
	[SerializeField] protected TMP_Text _dayText;
	[SerializeField] protected Button   _part1Button;
	[SerializeField] protected Button   _part2Button;

	public static DayClicked onDayClicked { get; } = new DayClicked();

	private void Start() {
		_part1Button.onClick.AddListener(() => onDayClicked.Invoke(_day, 1));
		_part2Button.onClick.AddListener(() => onDayClicked.Invoke(_day, 2));
	}

#if UNITY_EDITOR
	public void Build(int day, bool part1, bool part2) {
		_day = day;
		_dayText.text = $"Day {day}";
		_part1Button.gameObject.SetActive(part1);
		_part2Button.gameObject.SetActive(part2);
		EditorUtility.SetDirty(this);
		EditorUtility.SetDirty(_dayText);
		EditorUtility.SetDirty(_part1Button);
		EditorUtility.SetDirty(_part2Button);
	}
#endif
}