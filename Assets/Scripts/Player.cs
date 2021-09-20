using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private CharacterView _view;
	[SerializeField] private Transform _spineBone;
	[SerializeField] private float _distanceThresholdToSpineRotation;
	[SerializeField] private float _rotationSpeed;
	[SerializeField] private float _movementSpeed;
	[SerializeField] private Vector3 _offsetRotation;
	[SerializeField] private float _distanceToObjectForFinishing;
	[SerializeField] private float _movementForFinishingSpeed;

	private Transform _transform;
	private Transform _aimPoint;

	private float _inputHorizontal;
	private float _inputVertical;
	private Vector3 _relativeVelocity;

	private bool _isLockControls;
	
	private Enemy _nearEnemy;

	private void Awake()
	{
		_transform = transform;
		_aimPoint = new GameObject("PlayerAimPoint").transform;
	}

	private void Update()
	{
		if (_nearEnemy != null && Input.GetKeyDown(KeyCode.Space))
		{
			_isLockControls = true;
			_transform.LookAt(_nearEnemy.transform);
			StartCoroutine(MoveToObjectForFinish(_nearEnemy.transform, () =>
			{
				_transform.LookAt(_nearEnemy.transform);
				_view.Finishing(_nearEnemy, () => _isLockControls = false);
				_nearEnemy = null;
				CanvasView.SwitchFinishingLabel(false);
			}));
			return;
		}

		if (_isLockControls)
			return;

		_inputHorizontal = Input.GetAxis("Horizontal");
		_inputVertical = Input.GetAxis("Vertical");

		if (_inputHorizontal == 0 && _inputVertical == 0)
		{
			_view.Idle();
			return;
		}

		_relativeVelocity = _inputHorizontal * Vector3.right + _inputVertical * Vector3.forward;

		var lookRotation = Quaternion.LookRotation(_relativeVelocity);
		_transform.rotation = Quaternion.RotateTowards(_transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);

		_view.Moving(_relativeVelocity);

		var movementVector = _relativeVelocity * Time.deltaTime * _movementSpeed;
		_characterController.Move(movementVector);
	}

	private void LateUpdate()
	{
		if (_isLockControls)
			return;

		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out RaycastHit hitInfo);

		var newPosition = hitInfo.point;
		newPosition.y = _transform.position.y;
		_aimPoint.position = newPosition;

		var directional = _aimPoint.position - _transform.position;
		var lookRotationToAim = Quaternion.LookRotation(directional, -Vector3.right);
		var eulerAnglesWithOffset = lookRotationToAim.eulerAngles + _offsetRotation;
		var correctLookRotation = Quaternion.Euler(
			eulerAnglesWithOffset.x,
			eulerAnglesWithOffset.y,
			_aimPoint.position.z < _transform.position.z ? eulerAnglesWithOffset.z : -eulerAnglesWithOffset.z);

		_spineBone.rotation = correctLookRotation;
	}

	private IEnumerator MoveToObjectForFinish(Transform toObject, Action callBack)
	{
		var waitForEndOfFrame = new WaitForEndOfFrame();
		var direction = _transform.position - toObject.position;
		direction.y = 0f;
		var suitablePosition = toObject.position + ((direction / direction.magnitude) * _distanceToObjectForFinishing);
		var t = 0f;
		while (t < 1f)
		{
			t +=  _movementForFinishingSpeed * Time.deltaTime;
			yield return waitForEndOfFrame;
			var newPosition = Vector3.Lerp(_transform.position, suitablePosition, t);
			_transform.position = newPosition;
		}

		callBack?.Invoke();
	}

	private void OnTriggerEnter(Collider col)
	{
		if (!col.TryGetComponent(out Enemy enemy))
			return;

		if (!enemy.IsAlive)
			return;

		_nearEnemy = enemy;
		CanvasView.SwitchFinishingLabel(true);
	}

	private void OnTriggerExit(Collider col)
	{
		if (!col.TryGetComponent(out Enemy enemy))
			return;

		if (enemy != _nearEnemy)
			return;

		_nearEnemy = null;
		CanvasView.SwitchFinishingLabel(false);
	}
}
