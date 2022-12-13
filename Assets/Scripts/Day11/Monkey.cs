using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AOC22.Common;
using Day11.Common;
using TMPro;
using UnityEngine;
using Utils.Extensions;

public class Monkey : MonoBehaviour {
	[SerializeField] protected Transform       _itemStack;
	[SerializeField] protected Transform       _grabTransform;
	[SerializeField] protected Transform       _handItemAnchor;
	[SerializeField] protected RotateTowards   _headLook;
	[SerializeField] protected RotateTowards   _handLook;
	[SerializeField] protected AnimationCurve  _itemFlyHeightCurve;
	[SerializeField] protected float           _itemFlyHeightCurveCoefficient = 3;
	[SerializeField] protected float           _itemFlyTime                   = 2;
	[SerializeField] protected List<Day11Item> _items                         = new List<Day11Item>();
	[SerializeField] protected Monkey          _targetOnTrue;
	[SerializeField] protected Monkey          _targetOnFalse;
	[SerializeField] protected int             _inspectedItemCount;
	[SerializeField] protected float           _armSpeed = 1;
	[SerializeField] protected Vector3         _handRestPosition;
	[SerializeField] protected Vector3         _lookItemHandPosition;
	[SerializeField] protected Vector3         _shakeItemHandPosition;
	[SerializeField] protected float           _shakeSpeed = 3;
	[SerializeField] protected int             _shakeTime  = 1;
	[SerializeField] protected TMP_Text        _worryLevelLabel;
	[SerializeField] protected float           _waitTime = 1;
	[SerializeField] protected RotateTowards   _cameraAnchor;

	public Monkey targetOnTrue {
		get => _targetOnTrue;
		set => _targetOnTrue = value;
	}

	public Monkey targetOnFalse {
		get => _targetOnFalse;
		set => _targetOnFalse = value;
	}

	public int              divider            { get; set; }
	public int              inspectedItemCount => _inspectedItemCount;
	public Func<long, long> operation          { get; set; }
	public Transform        cameraAnchor       => _cameraAnchor.transform;

	private void Start() {
		StartCoroutine(StopWatchingItems());
	}

	public IEnumerator Catch(Day11Item item) {
		var targetOnGround = _itemStack.position.OnGround();
		var originOnGround = item.transform.position.OnGround();
		var originY = item.transform.position.y;
		var targetY = _itemStack.position.y;
		var lerp = 0f;

		while (lerp < 1) {
			lerp += AocTime.deltaTime / _itemFlyTime;
			item.transform.position = new Vector3(Mathf.Lerp(originOnGround.x, targetOnGround.x, lerp),
				Mathf.Lerp(originY, targetY, lerp) + _itemFlyHeightCurve.Evaluate(lerp) * _itemFlyHeightCurveCoefficient, Mathf.Lerp(originOnGround.y, targetOnGround.y, lerp));
			yield return null;
		}
		InstantMoveItemToStack(item);
	}

	public IEnumerator InspectAndThrow(Day11Item item, bool withRelief) {
		_headLook.target = item.transform;
		_handLook.target = _headLook.transform;
		_cameraAnchor.target = item.transform;
		if (item.transform.position.y < -2) {
			item.rigidbody.velocity = Vector3.zero;
			item.rigidbody.angularVelocity = Vector3.zero;
			item.transform.position = _itemStack.position;
		}
		while (_grabTransform.position != item.transform.position) {
			_grabTransform.position = Vector3.MoveTowards(_grabTransform.position, item.transform.position, AocTime.deltaTime * _armSpeed);
			yield return null;
		}
		item.Attach(_handItemAnchor);
		yield return null;
		var previousWorryLevel = item.worryLevel;
		_worryLevelLabel.enabled = true;
		_worryLevelLabel.text = $"{previousWorryLevel}";
		while (_grabTransform.localPosition != _lookItemHandPosition) {
			_grabTransform.localPosition = Vector3.MoveTowards(_grabTransform.localPosition, _lookItemHandPosition, AocTime.deltaTime * _armSpeed);
			yield return null;
		}
		for (var wait = 0f; wait < _waitTime; wait += AocTime.deltaTime) yield return null;
		item.DoOperation(operation);
		if (withRelief) item.AddRelief();
		var shakeTarget = _shakeItemHandPosition;
		var shakeTime = 0f;
		while (shakeTime < 1) {
			while (_grabTransform.localPosition != shakeTarget) {
				_grabTransform.localPosition = Vector3.MoveTowards(_grabTransform.localPosition, shakeTarget, AocTime.deltaTime * _shakeSpeed);
				_worryLevelLabel.text = $"{Mathf.Lerp(previousWorryLevel, item.worryLevel, shakeTime):0}";
				shakeTime += AocTime.deltaTime / _shakeTime;
				yield return null;
			}
			shakeTarget = shakeTarget == _shakeItemHandPosition ? _lookItemHandPosition : _shakeItemHandPosition;
		}
		for (var wait = 0f; wait < _waitTime; wait += AocTime.deltaTime) yield return null;
		_worryLevelLabel.text = $"{item.worryLevel} % {divider}<br><sprite name=\".notdef\">";
		for (var wait = 0f; wait < _waitTime; wait += AocTime.deltaTime) yield return null;
		_worryLevelLabel.text = $"{item.worryLevel} % {divider}<br>{(item.IsDivisibleBy(divider) ? "<sprite name=\"Smiling face with smiling eyes\">" : "<sprite name=\"1f606\">")}";
		for (var wait = 0f; wait < _waitTime; wait += AocTime.deltaTime) yield return null;
		_worryLevelLabel.enabled = false;
		_handLook.target = null;
		_inspectedItemCount++;
	}

	public IEnumerator StopWatchingItems() {
		_headLook.target = null;
		_handLook.target = null;
		_cameraAnchor.target = _headLook.transform;
		_worryLevelLabel.enabled = false;
		while (_grabTransform.localPosition != _handRestPosition) {
			_grabTransform.localPosition = Vector3.MoveTowards(_grabTransform.localPosition, _handRestPosition, AocTime.deltaTime * _armSpeed);
			yield return null;
		}
	}

	public Monkey ChooseThrowTarget(Day11Item item) => item.IsDivisibleBy(divider) ? targetOnTrue : targetOnFalse;

	public bool TryPopNextItemToThrow(out Day11Item item) {
		item = _items.FirstOrDefault();
		if (item) _items.Remove(item);
		return item;
	}

	public void InstantMoveItemToStack(Day11Item item) {
		item.transform.position = _itemStack.position;
		item.Release();
		_items.Add(item);
	}
}