using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[SLua.CustomLuaClassAttribute]
public class FunctionsCallerInMainThread : MonoSingleton<FunctionsCallerInMainThread>
{
	public static FunctionsCallerInMainThread Ins
	{
		get
		{
			return ins;
		}
	}

	private List<Action> m_listFuncs;

	public void Call(Action func)
	{
		if (func != null)
		{
			if (m_listFuncs == null)
				m_listFuncs = new List<Action>();
			if (!m_listFuncs.Contains(func))
			{
				m_listFuncs.Add(func);
			}
		}
	}

	void Update()
	{
		if (m_listFuncs != null && m_listFuncs.Count > 0)
		{
			for(int i = 0; i < m_listFuncs.Count; i++)
			{
				Action action = m_listFuncs[i];
				if (action != null)
				{
					action();
					m_listFuncs[i] = null;
				}
			}
			m_listFuncs.Clear();
		}
	}
}
