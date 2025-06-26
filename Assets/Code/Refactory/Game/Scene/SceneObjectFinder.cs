using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SceneObjectFinder : LuaGameObject 
	{
		public Renderer[] sceneObjectRenderers;
		public Material[] sceneObjectMaterials{get;set;}

		public static Renderer[] FindSceneObjectRenderers(GameObject root)
		{
			var shaderManager = ShaderManager.Me;
			if (null == shaderManager)
			{
				return null;
			}
			var shaderNames = new List<string> ();
			for (int i = 0; i < shaderManager.sceneObjectShaders.Length; ++i)
			{
				var s = shaderManager.sceneObjectShaders[i];
				shaderNames.Add (s.name);
			}

			for (int i = 0; i < shaderManager.T4MShaders.Length; ++i)
			{
				var s = shaderManager.T4MShaders[i];
				shaderNames.Add (s.name);
			}
			
			if (0 >= shaderNames.Count) 
			{
				return null;
			}

			var renderers = root.GetComponentsInChildren<Renderer>();
			if (renderers.IsNullOrEmpty())
			{
				return null;
			}

			var rendererList = new List<Renderer>();
			for (int i = 0; i < renderers.Length; ++i) 
			{
				var r = renderers[i];
				if (r.sharedMaterials.IsNullOrEmpty ()) 
				{
					continue;
				}

				for (int j = 0; j < r.sharedMaterials.Length; ++j) 
				{
					var m = r.sharedMaterials [j];
					if (null == m) 
					{
						ROLogger.LogFormat (r, "<color=red>[SceneObject Material is NULL]: </color>{0}", r.name);
						continue;
					}
					if (shaderNames.Contains (m.shader.name)) 
					{
						rendererList.Add(r);
						break;
					}
				}
			}

			return rendererList.ToArray();
		}

		public static Material[] GetSceneObjectMaterials(Renderer[] renderers)
		{
			if (renderers.IsNullOrEmpty())
			{
				return null;
			}

			var shaderManager = ShaderManager.Me;
			if (null == shaderManager)
			{
				return null;
			}
			var shaderNames = new List<string> ();
			for (int i = 0; i < shaderManager.sceneObjectShaders.Length; ++i)
			{
				var s = shaderManager.sceneObjectShaders[i];
				shaderNames.Add (s.name);
			}
			
			for (int i = 0; i < shaderManager.T4MShaders.Length; ++i)
			{
				var s = shaderManager.T4MShaders[i];
				shaderNames.Add (s.name);
			}
			
			if (0 >= shaderNames.Count) 
			{
				return null;
			}

			var materialList = new List<Material>();
			for (int i = 0; i < renderers.Length; ++i) 
			{
				var r = renderers[i];
				if (r.sharedMaterials.IsNullOrEmpty ()) 
				{
					continue;
				}
				
				for (int j = 0; j < r.sharedMaterials.Length; ++j) 
				{
					var m = r.sharedMaterials [j];
					if (null == m) 
					{
						ROLogger.LogFormat (r, "<color=red>[SceneObject Material is NULL]: </color>{0}", r.name);
						continue;
					}

					#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
					if (shaderNames.Contains(m.shader.name))
					{
						m = r.materials[j];
						ShaderManager.HandleSceneObjectMaterial(m);
						materialList.Add(m);
					}
					#else
					if (shaderNames.Contains (m.shader.name) && !materialList.Contains (m)) 
					{
						ShaderManager.HandleSceneObjectMaterial (m);
						materialList.Add (m);
					}
					#endif
				}
			}

			return materialList.ToArray();
		}

		#region behaviour
		void Reset()
		{
//			sceneObjectRenderers = FindSceneObjectRenderers(gameObject);
		}

		protected override void Awake ()
		{
//			sceneObjectMaterials = GetSceneObjectMaterials(sceneObjectRenderers);
			sceneObjectMaterials = null;
			base.Awake ();
		}
		#endregion behaviour
	}
} // namespace RO
