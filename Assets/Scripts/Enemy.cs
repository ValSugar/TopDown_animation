using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private CharacterView _view;
	[SerializeField] private Collider _collider;

	private bool _isAlive;

	public bool IsAlive => _isAlive;

	private void Awake()
	{
		_isAlive = true;
	}

	public void OnReset()
	{
		_collider.enabled = true;
		_isAlive = true;
		_view.OnReset();
		var newPosition = Random.insideUnitSphere * 10;
		newPosition.y = 0;
		transform.position = newPosition;
	}

	public void Kill()
	{
		_collider.enabled = false;
		_isAlive = false;
		_view.KillSelf();
		StartCoroutine(RespawnCoroutine());
	}

	private IEnumerator RespawnCoroutine()
	{
		yield return new WaitForSeconds(5f);
		OnReset();
	}
}
