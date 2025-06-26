using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[SLua.CustomLuaClassAttribute]
public class InternetUtil : RO.SingleTonGO<InternetUtil>
{
	public class ThePing
	{
		public Ping ping;
		public float waitTime;
		public float startTime;
		public Action<bool> action;

		public ThePing(string address)
		{
			ping = new Ping(address);
		}
	}

	public static InternetUtil Ins
	{
		get
		{
			return Me;
		}
	}

	private const bool ALLOW_CARRIER_DATA_NETWORK = false;
	private const string PING_ADDRESS = "1.1.1.1";
	private List<ThePing> m_cachedPings;

	public void InternetIsValid(Action<bool> on_complete, float time_permission)
	{
		WIFIIsValid((x) => {
			if (x)
			{
				if (on_complete != null)
				{
					on_complete(true);
				}
			}
			else
			{
				CarrierIsValid(on_complete, time_permission);
			}
		}, time_permission);
	}

	private bool m_internetIsValid;
	public bool M_InternetIsValid
	{
		get
		{
			return m_internetIsValid;
		}
		set
		{
			m_internetIsValid = value;
		}
	}

	public void WIFIIsValid(Action<bool> on_complete, float time_permission)
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			ThePing thePing = new ThePing(PING_ADDRESS);
			thePing.waitTime = time_permission;
			thePing.startTime = Time.time;
			thePing.action = on_complete;
			if (m_cachedPings == null)
				m_cachedPings = new List<ThePing>();
			m_cachedPings.Add(thePing);
		}
		else
		{
			if (on_complete != null)
			{
				on_complete(false);
			}
		}
	}

	public void WIFIIsValid(Action<bool> on_complete)
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			ThePing thePing = new ThePing(PING_ADDRESS);
			thePing.waitTime = 5;
			thePing.startTime = Time.time;
			thePing.action = on_complete;
			if (m_cachedPings == null)
				m_cachedPings = new List<ThePing>();
			m_cachedPings.Add(thePing);
		}
		else
		{
			if (on_complete != null)
			{
				on_complete(false);
			}
		}
	}

	public void CarrierIsValid(Action<bool> on_complete, float time_permission = 5)
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
		{
			ThePing thePing = new ThePing(PING_ADDRESS);
			thePing.waitTime = time_permission;
			thePing.startTime = Time.time;
			thePing.action = on_complete;
			if (m_cachedPings == null)
				m_cachedPings = new List<ThePing>();
			m_cachedPings.Add(thePing);
		}
		else
		{
			if (on_complete != null)
			{
				on_complete(false);
			}
		}
	}

	void Update()
	{
		if (m_cachedPings != null && m_cachedPings.Count > 0)
		{
			for (int i = m_cachedPings.Count - 1; i >= 0; i--)
			{
				ThePing ping = m_cachedPings[i];
				Ping realPing = ping.ping;
				if (realPing.isDone)
				{
					if (ping.action != null)
					{
						ping.action(realPing.time >= 0);
					}
					m_cachedPings.Remove(ping);
					realPing.DestroyPing();
					ping = null;
				}
				else if (Time.time - ping.startTime >= ping.waitTime)
				{
					if (ping.action != null)
					{
						ping.action(false);
					}
					m_cachedPings.Remove(ping);
					realPing.DestroyPing();
					ping = null;
				}
			}
		}
	}

	public void Release()
	{
		if (m_cachedPings != null && m_cachedPings.Count > 0)
		{
			for (int i = m_cachedPings.Count - 1; i >= 0; i--)
			{
				ThePing ping = m_cachedPings[i];
				Ping realPing = ping.ping;
				realPing.DestroyPing();
				ping = null;
			}
			m_cachedPings.Clear();
		}
	}

	protected override void OnDestroy ()
	{
		Release();
		base.OnDestroy();
	}
}
