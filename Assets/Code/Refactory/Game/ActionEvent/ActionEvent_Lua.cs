using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class ActionEvent_Lua : MonoBehaviour 
	{
		public void ActionEvent_ED_CMD(int cmdID)
		{
			if (null == LuaLuancher.Me)
			{
				return;
			}
			LuaLuancher.Me.Call("Command_ED", cmdID);
		}
	}
} // namespace RO
