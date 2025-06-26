using UnityEngine;
using System.Collections.Generic;

namespace RO.Config
{
	[SLua.CustomLuaClassAttribute]
	public static class Layer
	{
		public static readonly KeyValuePair<string, int> DEFAULT;
		public static readonly KeyValuePair<string, int> UI;
		public static readonly KeyValuePair<string, int> UIModel;
		public static readonly KeyValuePair<string, int> TERRAIN;
		public static readonly KeyValuePair<string, int> STATIC_OBJECT;
		public static readonly KeyValuePair<string, int> ACCESSABLE;
		public static readonly KeyValuePair<string, int> INVISIBLE;

		static Layer()
		{
			DEFAULT = Make("Default");
			UI = Make("UI");
			UIModel = Make("UIModel");
			TERRAIN = Make("Terrain");
			STATIC_OBJECT = Make("StaticObject");
			ACCESSABLE = Make("Accessable");
			INVISIBLE = Make("InVisible");
		}

		private static KeyValuePair<string, int> Make(string name)
		{
			return new KeyValuePair<string, int>(name, LayerMask.NameToLayer(name));
		}
	}
} // namespace RO.Config
