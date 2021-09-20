using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private GameObject _standartWeapon;
	[SerializeField] private GameObject _meleeWeapon;
	[SerializeField] private RagdollController _ragdoll;

	private Transform _transform;
	private Enemy _currentEnemy;

	private Action _onFinishingEnd;

	private void Awake()
	{
		_transform = transform;
	}

	public void OnReset()
	{
		_ragdoll.Disable();
		_animator.enabled = true;
		Idle();
	}

	public void Idle()
	{
		_animator.SetBool("IsMoving", false);
	}

	public void Moving(Vector3 velocity)
	{
		_animator.SetBool("IsMoving", true);
		_animator.SetFloat("MoveVelocityForward", velocity.magnitude);
		_animator.SetFloat("MoveVelocitySide", 0f);
	}

	public void Finishing(Enemy enemy, Action onFinishingEnd)
	{
		_currentEnemy = enemy;
		_onFinishingEnd = onFinishingEnd;
		_animator.SetTrigger("Finishing");
		_standartWeapon.SetActive(false);
		_meleeWeapon.SetActive(true);
	}

	public void KillSelf()
	{
		_ragdoll.Enable();
		_animator.enabled = false;
	}

	//Called from animation
	public void KillEnemy()
	{
		if (_currentEnemy == null)
			return;

		_currentEnemy.Kill();
	}

	//Called from animation
	public void FinishingEnd()
	{
		_onFinishingEnd?.Invoke();
		_standartWeapon.SetActive(true);
		_meleeWeapon.SetActive(false);
	}
}
