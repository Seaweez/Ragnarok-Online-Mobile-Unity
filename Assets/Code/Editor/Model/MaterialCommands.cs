using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ghost.Extensions;
using Ghost.Utils;
using Ghost.Config;

namespace EditorTool
{
	public static class MaterialCommands 
	{
		[MenuItem("Assets/Material/CheckShader", true)]
		static bool CheckShaderValidFunc()
		{
			var objs = Selection.objects;
			if (objs.IsNullOrEmpty())
			{
				return false;
			}
			
			foreach (var obj in objs)
			{
				var objPath = AssetDatabase.GetAssetPath(obj);
				if (AssetDatabase.IsValidFolder(objPath))
				{
					return true;
				}
				
				if (obj is Material)
				{
					return true;
				}
			}
			
			return false;
		}

		[MenuItem("Assets/Material/CheckShader")]
		static void CheckShader()
		{
			var objs = Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets);
			if (objs.IsNullOrEmpty())
			{
				Debug.LogFormat("<color=yellow>No Selected Assets</color>");
				return;
			}
			try
			{
				var shaderMap = new Dictionary<string, List<string>>();

				var progressTitle = "Check Shader";
				EditorUtility.DisplayProgressBar(progressTitle, "", 0);
				for (int i = 0; i < objs.Length; ++i)
				{
					var obj = objs[i];
					var objPath = AssetDatabase.GetAssetPath(obj);
					
					var material = obj as Material;
					var shaderName = (null != material.shader) ? material.shader.name : "Null";

					List<string> list;
					if (!shaderMap.TryGetValue(shaderName, out list))
					{
						list = new List<string>();
						shaderMap.Add(shaderName, list);
					}
					list.Add(objPath);
					
					EditorUtility.DisplayProgressBar(progressTitle, string.Format("[{0}]: {1}", material.name, objPath), (float)i/objs.Length);
//					Debug.LogFormat(obj, "<color=green>Check Shader({0}): </color>{1}", objPath);
				}

				foreach (var key_value in shaderMap)
				{
					var sb = new StringBuilder();
					sb.Append(key_value.Key);
					foreach (var str in key_value.Value)
					{
						sb.AppendFormat("\n{0}", str);
					}
					var shader = Shader.Find(key_value.Key);
					Debug.LogFormat(shader, sb.ToString());
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		[MenuItem("Assets/Material/CheckT4M", true)]
		static bool CheckT4MValidFunc()
		{
			var objs = Selection.objects;
			if (objs.IsNullOrEmpty())
			{
				return false;
			}
			
			foreach (var obj in objs)
			{
				var objPath = AssetDatabase.GetAssetPath(obj);
				if (AssetDatabase.IsValidFolder(objPath))
				{
					return true;
				}
				
				if (obj is Material)
				{
					return true;
				}
			}
			
			return false;
		}
		
		[MenuItem("Assets/Material/CheckT4M")]
		static void CheckT4M()
		{
			var objs = Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets);
			if (objs.IsNullOrEmpty())
			{
				Debug.LogFormat("<color=yellow>No Selected Assets</color>");
				return;
			}
			try
			{
				var progressTitle = "Check Texture";
				EditorUtility.DisplayProgressBar(progressTitle, "", 0);
				for (int i = 0; i < objs.Length; ++i)
				{
					var obj = objs[i];
					var objPath = AssetDatabase.GetAssetPath(obj);

					Vector2 maxSize = Vector2.zero;
					var material = obj as Material;
					for (int j = 0; j < 6; ++j)
					{
						var texName = "_Splat"+j;
						if (!material.HasProperty(texName))
						{
							break;
						}
						var tex = material.GetTexture(texName);
						if (null != tex)
						{
							var scale = material.GetTextureScale(texName);
							var size = new Vector2(tex.width, tex.height);
							size = size.Multiply(scale);
							maxSize = Vector2.Max(maxSize, size);
						}
					}
					EditorUtility.DisplayProgressBar(
						progressTitle, 
						string.Format("[{0}]: {1}", material.name, objPath), (float)i/objs.Length);
					if (2048 < maxSize.x || 2048 < maxSize.y)
					{
						Debug.LogFormat(obj, "Check T4M: sizeScaled=<color=red>{1}</color>\n{0}", 
						                objPath, 
						                maxSize);
					}
					else
					{
						Debug.LogFormat(obj, "Check T4M: sizeScaled=<color=green>{1}</color>\n{0}", 
						                objPath, 
						                maxSize);
					}
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		class TexInfo
		{
			public Texture2D tex;
			public Vector2 tilling;
			public Vector2 size;
			public Vector2 sizeScaled;
			public Color[] colors;

			public TexInfo(Material mat, string name, int i)
			{
				Init(mat, name+i);
			}

			public TexInfo(Material mat, string texName)
			{
				Init(mat, texName);
			}

			private void Init(Material mat, string texName)
			{
				if (mat.HasProperty(texName))
				{
					tex = mat.GetTexture(texName) as Texture2D;
					SetTextureReadable(true);
				}
				else
				{
					tex = null;
				}
				if (null != tex)
				{
					tilling = mat.GetTextureScale(texName);
					size = new Vector2(tex.width, tex.height);
					sizeScaled = size.Multiply(tilling);
					colors = tex.GetPixels();
				}
				else
				{
					tilling = Vector2.zero;
					size = Vector2.zero;
					sizeScaled = Vector2.zero;
					colors = null;
				}
			}

			public Color GetPixelBilinear(float fx, float fy)
			{
				if (null == tex)
				{
					return Color.black;
				}
				fx *= tilling.x;
				fx -= (int)fx;

				fy *= tilling.y;
				fy -= (int)fy;

//				return tex.GetPixel(fx, fy);
				
				fx *= size.x;
				fy *= size.y;

				int x1 = (int)fx;
				int x2 = (int)Mathf.Clamp(Mathf.CeilToInt(fx), 0, size.x-1);

				int y1 = (int)fy;
				int y2 = (int)Mathf.Clamp(Mathf.CeilToInt(fy), 0, size.y-1);

				var c11 = colors[x1+y1*(int)size.x];
				var c12 = colors[x1+y2*(int)size.x];
				var c21 = colors[x2+y1*(int)size.x];
				var c22 = colors[x2+y2*(int)size.x];

				var c1 = Color.Lerp(c11, c21, fx-x1);
				var c2 = Color.Lerp(c12, c22, fx-x1);
				return Color.Lerp(c1, c2, fy-y1);
			}

			public void SetTextureReadable(bool readable)
			{
				if (null == tex)
				{
					return;
				}
				var path = AssetDatabase.GetAssetPath(tex);
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				TextureImporterSettings texSettings = new TextureImporterSettings();
				ti.ReadTextureSettings(texSettings);
				if (texSettings.readable != readable)
				{
					texSettings.readable = readable;
					ti.SetTextureSettings(texSettings);
					AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
				}
			}
		}
		
		[MenuItem("Assets/Material/BakeT4M", true)]
		static bool TerrainMatFilter()
		{
			return Selection.activeObject && Selection.activeObject is Material;
		}

		[MenuItem("Assets/Material/BakeT4M")]
		static void MergeTex()
		{
			var mat = Selection.activeObject as Material;
			if (!mat.HasProperty("_Splat0") || !mat.HasProperty("_Splat1") || !mat.HasProperty("_Control"))
			{
				Debug.LogError(string.Format("{0} is not a T4M material!", mat.name));
				return;
			}

			float albedoScale = 1f;
			if (mat.HasProperty("_AlbedoScale"))
			{
				albedoScale = mat.GetFloat("_AlbedoScale");
			}
			
			int targetWidth = 0;
			int targetHeight = 0;

			var texArray = new TexInfo[6];
			for (int i = 0; i < texArray.Length; ++i)
			{
				var info = new TexInfo(mat, "_Splat", i);
				targetWidth = Mathf.Max(targetWidth, (int)info.sizeScaled.x);
				targetHeight = Mathf.Max(targetHeight, (int)info.sizeScaled.y);
				info.SetTextureReadable(true);
				texArray[i] = info;
			}
			var ctlTexArray = new TexInfo[2] {
				new TexInfo(mat, "_Control"), 
				new TexInfo(mat, "_Control2")
			};
			for (int i = 0; i < ctlTexArray.Length; ++i)
			{
				ctlTexArray[i].SetTextureReadable(true);
			}
			
			// 限制Texture的最大长宽为4096
			targetWidth = Mathf.Clamp(targetWidth, 0, 2048);
			targetHeight = Mathf.Clamp(targetHeight, 0, 2048);
			
			Color[] targetColors = new Color[Mathf.CeilToInt(targetWidth * targetHeight)];

			var pTotal = targetColors.Length;
			var pCur = 0.0f;
			try
			{
				EditorUtility.DisplayProgressBar("MergeTex", string.Format("{0}/{1}", pCur, pTotal), pCur/pTotal);

				var factor = Vector2.zero;
				var colorArray = new Color[6];
				var ctlColorArray = new Color[2];

				for (int y = 0; y < targetHeight; ++y)
				{
					for (int x = 0; x < targetWidth; ++x)
					{

						factor.Set((float)x/targetWidth, (float)y/targetHeight);

						for (int i = 0; i < colorArray.Length; ++i)
						{
							colorArray[i] = texArray[i].GetPixelBilinear(factor.x, factor.y);
						}
						for (int i = 0; i < ctlColorArray.Length; ++i)
						{
							ctlColorArray[i] = ctlTexArray[i].GetPixelBilinear(factor.x, factor.y);
						}

						var c = colorArray[0] * ctlColorArray[0].r + colorArray[1] * ctlColorArray[0].g + colorArray[2] * ctlColorArray[0].b + colorArray[3] * ctlColorArray[0].a + colorArray[4] * ctlColorArray[1].r + colorArray[5] * ctlColorArray[1].g;
						targetColors[y * targetWidth + x] = c * albedoScale / 0.78f; // 0.78 fix SceneObject-Lit shader's bug!!!

						++pCur;
					}
					EditorUtility.DisplayProgressBar("MergeTex", string.Format("{0:0,0}/{1:0,0}, {2:#.00%}", pCur, pTotal, pCur/pTotal), pCur/pTotal);
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
			Texture2D resTex = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, true);
			resTex.SetPixels(targetColors);
			resTex.Apply();
			
			byte[] data = resTex.EncodeToPNG();
			string path = AssetDatabase.GetAssetPath(mat).Replace(".mat", "_Merged.png");
			File.WriteAllBytes(path, data);

			for (int i = 0; i < texArray.Length; ++i)
			{
				texArray[i].SetTextureReadable(false);
			}
			for (int i = 0; i < ctlTexArray.Length; ++i)
			{
				ctlTexArray[i].SetTextureReadable(false);
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

	}
} // namespace EditorTool
