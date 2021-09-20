using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _movementSpeed;

    private Transform _transform;
    private Vector3 _offsetPosition;

    private void Start()
    {
        _transform = transform;
        _offsetPosition = _transform.position - _target.position;
    }

    private void Update()
    {
	    _transform.position = Vector3.Lerp(_transform.position, _target.position + _offsetPosition, _movementSpeed * Time.deltaTime);
    }
}
