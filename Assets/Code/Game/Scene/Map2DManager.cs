using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class Map2DManager : SingleTonGO<Map2DManager> 
	{
		public static Map2DManager Instance {
			get {
				return Me;
			}
		}
		public GameObject monoGameObject
		{
			get
			{
				return gameObject;
			}
		}

		private Dictionary<int, Map2D> map2Ds_ = new Dictionary<int, Map2D>();
		private Vector3 currentP = Vector3.zero;

		private Map2D currentMap2D_ = null;
		private Map2D currentMap2D
		{
			get
			{
				return currentMap2D_;
			}
			set
			{
				if (value == currentMap2D)
				{
					return;
				}
				currentMap2D_ = value;

				#if OBSOLETE
				var player = Player.Me;
				var e = CoreEventSingleton<SceneEventMap2DChanged>.Me;
				e.currentMap2D = currentMap2D;
				player.SceneEventHandler(e);
#endif
			}
		}

		public void SetCurrentMap2D(Vector3 p)
		{
			currentP = p;
			foreach (var m in map2Ds_.Values)
			{
				if (m.Contains(currentP.XZ()))
				{
					currentMap2D = m;
					return;
				}
			}
			currentMap2D = null;
		}
		
		public Dictionary<int, Map2D> GetAllMap2Ds ()
		{
			return map2Ds_;
		}
		
		public bool Add(Map2D map2D)
		{
			if (map2Ds_.ContainsKey(map2D.ID))
			{
				return false;
			}
			map2Ds_.Add(map2D.ID, map2D);
			if (map2D.Contains(currentP.XZ ()))
			{
				currentMap2D = map2D;
			}
			return true;
		}
		
		public void Remove(Map2D map2D)
		{
			if (!map2Ds_.ContainsKey(map2D.ID))
			{
				return;
			}
			map2Ds_.Remove(map2D.ID);
			if (currentMap2D == map2D)
			{
				SetCurrentMap2D(currentP);
			}
		}

		public Map2D GetMap2D()
		{
			if (null != currentMap2D)
			{
				return currentMap2D;
			}
			return GetMap2D(0);
		}
		
		public Map2D GetMap2D(int ID)
		{
			Map2D map2D;
			if (map2Ds_.TryGetValue(ID, out map2D))
			{
				return map2D;
			}
			return null;
		}
	}
} // namespace RO
