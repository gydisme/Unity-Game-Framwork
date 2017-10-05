using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class MethodCaller : MonoBehaviour
{
	public UnityEvent call;

	#if !UNITY_EDITOR
	void Awake()
	{
		Destroy( this );
	}
	#endif
}
