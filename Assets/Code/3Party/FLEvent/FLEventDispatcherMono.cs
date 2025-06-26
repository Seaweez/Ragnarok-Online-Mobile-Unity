using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SLua.CustomLuaClassAttribute]
public class FLEventDispatcherMono : BaseBehaviour, IFLEventDispatcher
{
		private FLEventDispatcher _eventDispatcher;

		private FLEventDispatcher eventDispatcher {
				get {
						if (_eventDispatcher == null)
								_eventDispatcher = new FLEventDispatcher ();
						return _eventDispatcher;
				}
		}

		public void passEvent (FLEventBase e, string t = "")
		{
				eventDispatcher.DispatchEvent (e, t);
		}

		public void DispatchEvent (FLEventBase e, string t = "")
		{
				if (e.target != null) {
						// This event is dispatched from other target
						e = e.Clone ();
				}
				e.target = this;
				eventDispatcher.DispatchEvent (e, t);
		}

		public void AddEventListener (string eventType, FLEventBase.FLEventHandler handler)
		{
				eventDispatcher.AddEventListener (eventType, handler);
		}

		public void RemoveEventListener (string eventType, FLEventBase.FLEventHandler handler)
		{
				eventDispatcher.RemoveEventListener (eventType, handler);
		}
}

