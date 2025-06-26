using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	public static class ShaderCommands
	{
		static void ShaderToShader(string label, string from, string to)
		{
			var fromShader = Shader.Find(from);
			if (null == fromShader)
			{
				Debug.LogFormat("<color=red>{0} Failed: </color> No {1} shader", label, from);
				return;
			}

			var toShader = Shader.Find(to);
			if (null == toShader)
			{
				Debug.LogFormat("<color=red>{0} Failed: </color> No {1} shader", label, to);
				return;
			}

			var objs = Object.FindObjectsOfType<Renderer>();

			var totalCount = null != objs ? objs.Length : 0;
			var changedCount = 0;
			foreach (var obj in objs)
			{
				if (obj.sharedMaterials.IsNullOrEmpty())
				{
					continue;
				}
				foreach (var m in obj.sharedMaterials)
				{
					if (null != m && m.shader == fromShader)
					{
						m.shader = toShader;
						++changedCount;
					}
				}
			}

			Debug.LogFormat("<color=green>{0} Finished: </color>{1}/{2}", label, changedCount, totalCount);
		}

		static void SetT4MShader(bool ambientOn)
		{
			var t4mShaders = new Shader[]{
				Shader.Find("RO/T4M/2 Textures"),
				Shader.Find("RO/T4M/3 Textures"),
				Shader.Find("RO/T4M/4 Textures"),
				Shader.Find("RO/T4M/5 Textures"),
				Shader.Find("RO/T4M/6 Textures"),
			};

			var objs = Object.FindObjectsOfType<Renderer>();

			var totalCount = null != objs ? objs.Length : 0;
			var changedCount = 0;
			foreach (var obj in objs)
			{
				if (obj.sharedMaterials.IsNullOrEmpty())
				{
					continue;
				}
				foreach (var m in obj.sharedMaterials)
				{
					if (null != m && ArrayUtility.Contains(t4mShaders, m.shader))
					{
						if (ambientOn)
						{
							m.EnableKeyword("_Ambient_ON");
						}
						else
						{
							m.DisableKeyword("_Ambient_ON");
						}
					}
				}
			}

			Debug.LogFormat("<color=green>SetT4MShader Finished: </color>{0}/{1}", changedCount, totalCount);
		}

		[MenuItem("RO/Shader/Standard-->SceneObject")]
		static void StandardToSceneObject()
		{
			ShaderToShader("Standard-->SceneObject", "Standard", "RO/SceneObject/Lit");
			SetT4MShader(true);
		}

		[MenuItem("RO/Shader/Standard<--SceneObject")]
		static void SceneObjectToStandard()
		{
			ShaderToShader("Standard<--SceneObject", "RO/SceneObject/Lit", "Standard");
			SetT4MShader(false);
		}

		[MenuItem("RO/Shader/ResetMaterials")]
		static void ResetMaterials()
		{
			var sceneObjectShaders = new Shader[]{
				Shader.Find("RO/SceneObject/Lit"),
				Shader.Find("RO/SceneObject/Unlit"),
				Shader.Find("RO/SceneObject/Lit-Wind"),
				Shader.Find("RO/SceneObject/Unlit-Wind"),
			};
			var effectShaders = new Shader[]{
				Shader.Find("RO/Effect/Transparent"),
				Shader.Find("RO/Effect/Transparent-AlwaysFront"),
			};

			var guids = AssetDatabase.FindAssets("t:Material");
			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var m = AssetDatabase.LoadMainAssetAtPath(path) as Material;
				if (null != m)
				{
					if (ArrayUtility.Contains(sceneObjectShaders, m.shader))
					{
						SceneObjectShaderGUI.MaterialChanged(m);
					}
					else if (ArrayUtility.Contains(effectShaders, m.shader))
					{
						EffectShaderGUI.MaterialChanged(m);
					}
				}
			}
		}

		[MenuItem("RO/Shader/Change All Shaders to RO/T4M/4 Textures")]
		public static void ChangeShaders()
		{
			// ตัวแปรสำหรับนับจำนวนของ materials ที่ถูกเปลี่ยน
			int changedCount = 0;

			// ค้นหา GUID ของทุก material ในโปรเจ็ค
			string[] guids = AssetDatabase.FindAssets("t:Material");

			foreach (string guid in guids)
			{
				// แปลง GUID เป็น asset path
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);

				// โหลด material จาก asset path
				Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

				if (mat != null && mat.shader != null)
				{
					// ตรวจสอบว่า shader ปัจจุบันของ material คืออะไร
					if (mat.shader.name == "Hidden/InternalErrorShader")
					{
						// ค้นหา shader ใหม่ที่ต้องการตั้งค่า
						Shader newShader = Shader.Find("RO/T4M/4 Textures");

						if (newShader != null)
						{
							// เปลี่ยน shader ของ material
							mat.shader = newShader;

							// เพิ่มจำนวนของ materials ที่ถูกเปลี่ยน
							changedCount++;
						}
						else
						{
							Debug.LogWarning("Shader 'RO/T4M/4 Textures' not found. Make sure it's imported and available in the project.");
						}
					}
				}
			}

			// แสดงผลจำนวนของ materials ที่ถูกเปลี่ยน
			Debug.Log("Shader replacement completed. Changed " + changedCount + " materials.");

		}

		[MenuItem("RO/Shader/RevertRenderers")]
		static void RevertRenderers()
		{
			var objs = Object.FindObjectsOfType<Renderer>();

			foreach (var obj in objs)
			{
				var prefab = PrefabUtility.GetPrefabParent(obj.gameObject) as GameObject;
				if (null != prefab)
				{
					var originRenderer = prefab.GetComponent<Renderer>();
					if (null != originRenderer && ComponentUtility.CopyComponent(originRenderer))
					{
						ComponentUtility.PasteComponentValues(obj);
					}
				}
			}
		}
	}
} // namespace EditorTool
