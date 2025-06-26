using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[System.Serializable, SLua.CustomLuaClassAttribute]
	public class SceneInnerToBornPointTeleportData
	{
		public int sourceID = -1;
		public int targetID = -1;
		public int nextEPID = -1;
		public float totalCost = -1;
	}

	[System.Serializable, SLua.CustomLuaClassAttribute]
	public class SceneInnerToExitPointTeleportData
	{
		public int sourceID = -1;
		public int targetID = -1;
		public float totalCost = -1;
		
		public int startEPID = -1;
		public int endBPID = -1;
	}

	[System.Serializable, SLua.CustomLuaClassAttribute]
	public class SceneInnerPlayModeTeleportData
	{
		public int ID = -1; // for raid
		public SceneInnerToBornPointTeleportData[] toBornPoints;
		public SceneInnerToExitPointTeleportData[] toExitPoints;
	}

	[SLua.CustomLuaClassAttribute]
	public class SceneInnerTeleportData : ScriptableObject, System.ICloneable
	{
		public SceneInnerPlayModeTeleportData pve;
		public SceneInnerPlayModeTeleportData pvp;
		public SceneInnerPlayModeTeleportData[] raids;

		public object Clone()
		{
			return MemberwiseClone();
		}
		
		public SceneInnerTeleportData CloneSelf()
		{
			return MemberwiseClone() as SceneInnerTeleportData;
		}
	}
} // namespace RO
