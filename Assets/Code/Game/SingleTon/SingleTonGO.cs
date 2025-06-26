using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SingleTonGO<T> : MonoBehaviour
		where T : SingleTonGO<T>, new()
	{
		public static T Me{get;private set;}

		public virtual bool forceResetMe
		{
			get
			{
				return false;
			}
		}

		protected void RegisterMe()
		{
			if (this == Me)
			{
				return;
			}
			if (null != Me)
			{
				if (!forceResetMe)
				{
					GameObject.Destroy(gameObject);
					return;
				}
				GameObject.Destroy(Me.gameObject);
			}
			Me = this as T;
			OnRegisteredMe();
		}

		protected void UnregisterMe()
		{
			if (this != Me)
			{
				return;
			}
			Me = null;
			OnUnregisteredMe();
		}

		protected virtual void OnRegisteredMe()
		{

		}

		protected virtual void OnUnregisteredMe()
		{
			
		}

		protected virtual void Awake ()
		{
			RegisterMe();
		}
		
		protected virtual void OnDestroy ()
		{
			UnregisterMe();
		}
	}
} // namespace RO
