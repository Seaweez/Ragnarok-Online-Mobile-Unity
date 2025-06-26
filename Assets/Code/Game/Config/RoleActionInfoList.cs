using UnityEngine;
using System.Collections.Generic;

namespace RO.Config
{
	[System.Serializable,SLua.CustomLuaClassAttribute]
	public class RoleActionInfo
	{
		public string name;
		public bool showWeapon;
		public bool refreshEP;
	}
	
	public class RoleActionInfoList : ScriptableObject
	{
		public List<RoleActionInfo> actions;

		private Dictionary<string, RoleActionInfo> actionDictionary_ = null;
		public Dictionary<string, RoleActionInfo> actionDictionary
		{
			get
			{
				if (null == actionDictionary_)
				{
					actionDictionary_ = new Dictionary<string, RoleActionInfo>();
					foreach (var info in actions)
					{
						actionDictionary_.Add(info.name, info);
					}
				}
				return actionDictionary_;
			}
		}
	}
} // namespace RO.Config
