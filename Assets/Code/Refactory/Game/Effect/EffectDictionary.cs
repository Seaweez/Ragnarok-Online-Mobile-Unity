using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[System.Serializable]
	public class EffectInfo
	{
		[SerializeField]
		public int ID = 0;
		[SerializeField]
		public string path = null;
	}

	[SLua.CustomLuaClassAttribute]
	public class EffectDictionary : ScriptableObject, System.ICloneable
	{
		#region read from table
		[SerializeField]
		public EffectInfo[] effectInfos;
		#endregion read from table
		
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		public EffectDictionary CloneSelf()
		{
			return MemberwiseClone() as EffectDictionary;
		}
		
		private Dictionary<int, EffectInfo> infos = null;
		public void Init()
		{
			if (!effectInfos.IsNullOrEmpty())
			{
				infos = new Dictionary<int, EffectInfo>();
				for (int i = 0; i < effectInfos.Length; ++i)
				{
					var effectInfo = effectInfos[i];
					infos.Add(effectInfo.ID, effectInfo);
				}
			}
			else
			{
				infos = null;
			}
		}
		public EffectInfo GetEffectInfo(int effectID)
		{
			EffectInfo info;
			if (infos.TryGetValue(effectID, out info))
			{
				return info;
			}
			return null;
		}

		private static EffectDictionary global_ = null;
		public static EffectDictionary global
		{
			get
			{
				if (null == global_)
				{
					global_ = ResourceManager.Loader.SLoad<EffectDictionary>(ResourcePathHelper.IDEffectDictionary());
					if (null != global_)
					{
						global_.Init();
					}
				}
				return global_;
			}
		}
	}
} // namespace RO
