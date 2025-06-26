using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class IndependentTest : SingleTonGO<IndependentTest> 
	{
		public static IndependentTest Instance {
			get {
				return Me;
			}
		}

		protected override void Awake ()
		{
			if (null == Me)
			{
				base.Awake();
			}
			else
			{
				GameObject.Destroy(gameObject);
			}
		}
	
	}
} // namespace RO
