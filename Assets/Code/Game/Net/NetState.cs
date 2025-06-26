using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public enum NetState
{
	Disconnect = 0,
	Connecting = 1,
	Disconnecting = 5,
	Connect = 2,
	ConnectFailure = 6,
	Sending = 4,
	Error = 3,
	Timeout = 7,
}
