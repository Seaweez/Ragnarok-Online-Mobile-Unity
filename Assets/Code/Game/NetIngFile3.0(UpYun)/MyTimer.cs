using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MyTimer : MonoBehaviour
{
	private float m_deltaTime;
	private List<Action> m_listAction;
	private bool m_b;

	public void Initialize(float delta_time)
	{
		m_deltaTime = delta_time;
	}

	public void Register(Action action)
	{
		if (m_listAction == null)
			m_listAction = new List<Action>();
		m_listAction.Add(action);
	}

	public void Start()
	{
		StartCoroutine(Dida());
	}

	public void Stop()
	{
		StopCoroutine(Dida());
	}

	IEnumerator Dida()
	{
		yield return new WaitForSeconds(m_deltaTime);
		for (int i = 0; i < m_listAction.Count; i++)
		{
			Action action = m_listAction[i];
			if (action != null) action();
		}
	}
}
