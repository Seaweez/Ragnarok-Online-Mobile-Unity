using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnySDKCallbacks
{
	public enum E_CallbackCommand
	{
		InitializeSuccess,
		InitializeFail,
		LoginSuccess,
		LoginFail,
		LoginNetError,
		LoginCancel,
		LogoutSuccess,
		LogoutFail,
		PaySuccess,
		PayFail,
		PayTimeout,
		PayCancel,
		PayProductIllegal,
		PayPaying,
	}

	public delegate void AnySDKCallback(string message);

	private Dictionary<E_CallbackCommand, AnySDKCallback> m_callbacks = new Dictionary<E_CallbackCommand, AnySDKCallback>();

	public void Register(E_CallbackCommand command, AnySDKCallback callback)
	{
		if (m_callbacks.ContainsKey(command))
		{
			m_callbacks[command] = callback;
		}
		else
		{
			m_callbacks.Add(command, callback);
		}
	}

	public void Remove(E_CallbackCommand command, AnySDKCallback callback)
	{
		if (m_callbacks.ContainsKey(command))
		{
			m_callbacks.Remove(command);
		}
	}

	public void Call(E_CallbackCommand command, string message)
	{
		if (m_callbacks.ContainsKey(command))
		{
			AnySDKCallback callback = m_callbacks[command];
			if (callback != null)
			{
				callback(message);
			}
		}
	}
}
