using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SkyboxCamera : LuaGameObject 
	{
		public Skybox skybox;
		public Material skyboxMaterial{get;set;}

		#region behaviour
		protected override void Awake ()
		{
			var mat = skybox.material;
			if (null != mat)
			{
				skyboxMaterial = Object.Instantiate<Material>(mat);
			}
			else
			{
				skyboxMaterial = new Material(Shader.Find("RO/Clear"));
			}
			skybox.material = skyboxMaterial;
			base.Awake ();
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			Material.Destroy(skyboxMaterial);
		}
		#endregion behaviour
	}
} // namespace RO
