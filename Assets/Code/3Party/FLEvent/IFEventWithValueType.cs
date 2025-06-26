using UnityEngine;
using System.Collections;

public class IFEventWithValueType : FLEventBase
{
	public object translateValue;
	public string eventType;
	
	public IFEventWithValueType (string type):base(type)
	{

	}
}
