using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasView : MonoBehaviour
{
	private static CanvasView _instance;

	[SerializeField] private GameObject _finishingLabel;

	private void Awake()
	{
		_instance = this;
	}

	public static void SwitchFinishingLabel(bool flag)
	{
		_instance._finishingLabel.SetActive(flag);
	}
}
