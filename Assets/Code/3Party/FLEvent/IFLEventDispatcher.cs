using UnityEngine;
using System.Collections;

public interface IFLEventDispatcher
{
	void AddEventListener(string eventType, FLEventBase.FLEventHandler handler);
	void RemoveEventListener(string eventType, FLEventBase.FLEventHandler handler);
	void DispatchEvent(FLEventBase e, string type="");
}
