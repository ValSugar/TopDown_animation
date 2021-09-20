using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
	[SerializeField] private Rigidbody[] _rigidbodyes;

	private void Awake()
	{
		if (_rigidbodyes.Length > 0)
			return;

		_rigidbodyes = GetComponentsInChildren<Rigidbody>();
	}

	public void Enable()
	{
		foreach (var rb in _rigidbodyes)
			rb.isKinematic = false;
	}

	public void Disable()
	{
		foreach (var rb in _rigidbodyes)
			rb.isKinematic = true;
	}
}
