
using System.Collections.Generic;

namespace RO.Config
{
	public static class NavMeshArea
	{
		public static readonly int NOT_WALKABLE = UnityEngine.AI.NavMesh.GetAreaFromName("Not Walkable");

		public const int MASK_ALL = UnityEngine.AI.NavMesh.AllAreas;

		public static int CalcMask(params int[] areas)
		{
			int mask = 0;
			foreach (var area in areas)
			{
				mask |= (1 << area);
			}
			return mask;
		}
	}
} // namespace RO.Config
