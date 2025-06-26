using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FLEventDispatcher : BaseClass, IFLEventDispatcher
{
	private Dictionary<string, List<FLEventBase.FLEventHandler>> _handlers;
	
	public void DispatchEvent (FLEventBase e, string type="")
	{
		if (type != "")
			e.Type = type;
		if (e.target == null)
			e.target = this;
		if (_handlers != null && _handlers.ContainsKey (e.Type)) {
			List<FLEventBase.FLEventHandler> eventHandlers = _handlers [e.Type];
			int i = 0;
			while (i < eventHandlers.Count) {
				eventHandlers [i] (e);
				i++;
			}
		}
	}

	public void AddEventListener (string eventType, FLEventBase.FLEventHandler handler)
	{
		if (_handlers == null)
			_handlers = new Dictionary<string, List<FLEventBase.FLEventHandler>> ();

		List<FLEventBase.FLEventHandler> eventHandlers;
		if (!_handlers.ContainsKey (eventType))
			_handlers [eventType] = eventHandlers = new List<FLEventBase.FLEventHandler> ();
		else
			eventHandlers = _handlers [eventType];

		if (eventHandlers.IndexOf (handler) == -1)
			eventHandlers.Add (handler);
	}

	public void RemoveEventListener (string eventType, FLEventBase.FLEventHandler handler)
	{
		if (_handlers != null && _handlers.ContainsKey (eventType)) {
			List<FLEventBase.FLEventHandler> eventHandlers = _handlers [eventType];
			eventHandlers.Remove (handler);
		}
	}

}
