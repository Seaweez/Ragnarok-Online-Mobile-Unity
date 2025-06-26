using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SLua.CustomLuaClassAttribute]
public class RolesOnCurrentMap : Singleton<RolesOnCurrentMap>
{
	public static RolesOnCurrentMap Ins
	{
		get
		{
			return ins;
		}
	}

	private class Role
	{
		public Vector3 pos;
	}

	private Dictionary<int, Role> roles;

	public bool Exist(int roleID)
	{
		if (roles != null)
		{
			return roles.ContainsKey(roleID);
		}
		return false;
	}

	public void SetPos(int roleID, Vector3 pos)
	{
		if (Exist(roleID))
		{
			roles[roleID].pos = pos;
		}
		else
		{
			if (roles == null)
				roles = new Dictionary<int, Role>();
			Role role = new Role();
			role.pos = pos;
			roles.Add(roleID, role);
		}
	}

	public Vector3 GetPosOfRole(int roleID)
	{
		if (Exist(roleID))
		{
			return roles[roleID].pos;
		}
		return Vector3.zero;
	}

	public void Reset()
	{
		if (roles != null)
		{
			roles.Clear();
		}
	}
}
