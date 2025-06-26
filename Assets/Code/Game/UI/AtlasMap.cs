using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class AtlasMap : ScriptableObject 
	{
		private static AtlasMap Global_ = null;
		public static AtlasMap Global
		{
			get
			{
				if (null == Global_)
				{
					#if UNITY_EDITOR
						Global_ = Resources.Load<AtlasMap>(ResourcePathHelper.IDGUIAtlasMap());
					#else
						Global_ = ResourceManager.Me.SLoad<AtlasMap>(ResourcePathHelper.IDGUIAtlasMap());
					#endif
					if (null != Global_)
					{
						Global_.Init();
					}
				}
				return Global_;
			}
		}

		public static UIAtlas GetAtlas(string name)
		{
			if (null == AtlasMap.Global)
			{
				return null;
			}
			return AtlasMap.Global.TryGetAtlas(name);
		}

		public UIAtlas[] atlas = null;
		private Dictionary<string, UIAtlas> atlasMap = null;

		private void Init()
		{
			atlasMap = new Dictionary<string, UIAtlas>();
			if (atlas.IsNullOrEmpty())
			{
				return;
			}
			foreach (var a in atlas)
			{
				atlasMap.Add(a.name, a);
			}
		}

		public UIAtlas TryGetAtlas(string name)
		{
			UIAtlas a;
			if (!atlasMap.TryGetValue(name, out a))
			{
				return null;
			}
			return a;
		}
	
	}
} // namespace RO
