using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	public class GetAssetInfo {
		
		// type
		[MenuItem("Assets/GetInfo/Type")]
		static void GetTypeInfo () 
		{
			var selectedAssets = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
			foreach (Object obj in selectedAssets) 
			{
				Debug.Log(string.Format("{0}\ntype: {1}\n", AssetDatabase.GetAssetPath(obj), obj.GetType().ToString()));
			}
		}
		
		// dependence
		private static void DebugLogDependencies(string[] depends)
		{
			if (null != depends && 0 < depends.Length)
			{
				var dependsMap = new Dictionary<string, List<string>>(); 
				foreach (var path in depends)
				{
					var key = Path.GetExtension(path);
					List<string> list = null;
					if (!dependsMap.ContainsKey(key))
					{
						list = new List<string>();
						dependsMap.Add(key, list);
					}
					else
					{
						list = dependsMap[key];
					}
					
					list.Add(path);
				}
				foreach (var list in dependsMap.Values)
				{
					list.MakeUnique();
				}
				foreach(var key_value in dependsMap)
				{
					string log = string.Format("\t{0}: \n\n", key_value.Key);
					var list = key_value.Value;
					foreach (var path in list)
					{
						log += string.Format("{0}\n", path);
					}
					Debug.Log (log);
				}
			}
		}
		
		private static void DoGetDependenciesEach (SelectionMode mode)
		{
			var selectedAssets = Selection.GetFiltered (typeof(Object), mode);
			foreach (Object obj in selectedAssets) 
			{
				Debug.Log (string.Format("{0}\ndepends on:", AssetDatabase.GetAssetPath(obj)));	
				string[] depends = AssetDatabase.GetDependencies(new string[]{AssetDatabase.GetAssetPath(obj)});
				DebugLogDependencies(depends);
			}
		}
		
		private static void DoGetDependenciesAll (SelectionMode mode)
		{
			var selectedAssets = Selection.GetFiltered (typeof(Object), mode);
			List<string> selectedAssetsPath = new List<string>();
			foreach (var obj in selectedAssets)
			{
				selectedAssetsPath.Add(AssetDatabase.GetAssetPath(obj));
			}
			Debug.Log ("selected all\ndepends on:");	
			string[] depends = AssetDatabase.GetDependencies(selectedAssetsPath.ToArray());
			DebugLogDependencies(depends);
		}
		
		[MenuItem("Assets/GetInfo/DependenciesEach")]
		static void GetDependenciesEach () { DoGetDependenciesEach (SelectionMode.Assets); }
		
		[MenuItem("Assets/GetInfo/DependenciesEach(Deep)")]
		static void GetDependenciesEachDeep () { DoGetDependenciesEach (SelectionMode.DeepAssets); }
		
		[MenuItem("Assets/GetInfo/DependenciesAll")]
		static void GetDependenciesAll () { DoGetDependenciesAll (SelectionMode.Assets); }
		
		[MenuItem("Assets/GetInfo/DependenciesAll(Deep)")]
		static void GetDependenciesAllDeep () { DoGetDependenciesAll (SelectionMode.DeepAssets); }

		// dependence level
		private static void DoGetDependenceLevelEach (SelectionMode mode)
		{
			var selectedAssets = Selection.GetFiltered (typeof(Object), mode);
			foreach (Object obj in selectedAssets) 
			{
				var path = AssetDatabase.GetAssetPath(obj);
				Debug.Log (string.Format("{0}\ndepends level: {1}", path, AssetDependenceLevel.GetAssetLevel(obj, path)));
			}
		}

		[MenuItem("Assets/GetInfo/DependenceLevelEach")]
		static void GetDependenceLevelEach () { DoGetDependenceLevelEach (SelectionMode.Assets); }
		
		[MenuItem("Assets/GetInfo/DependenceLevelEach(Deep)")]
		static void GetDependenceLevelEachDeep () { DoGetDependenceLevelEach (SelectionMode.DeepAssets); }

		[MenuItem("Assets/GetInfo/TextureDependenceEach(Deep)")]
		static void GetTextureDependenceEachDeep () 
		{ 
			var selectedAssets = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
			var dependenceMap = new SortedDictionary<string, List<string>>();
			foreach (Object obj in selectedAssets) 
			{
				var path = AssetDatabase.GetAssetPath(obj);
				string[] depends = AssetDatabase.GetDependencies(new string[]{path});
				foreach (var d in depends)
				{
					var texture = AssetDatabase.LoadAssetAtPath(d, typeof(Texture)) as Texture;
					if (null == texture)
					{
						continue;
					}
					List<string> list = null;
					if (!dependenceMap.ContainsKey(d))
					{
						list = new List<string>();
						dependenceMap.Add(d, list);
					}
					else
					{
						list = dependenceMap[d];
					}
					
					list.Add(path);
				}
			}
			foreach(var key_value in dependenceMap)
			{
				var texture = AssetDatabase.LoadAssetAtPath(key_value.Key, typeof(Texture)) as Texture;
				string log = string.Format("\t{0}: \n\n", key_value.Key);
				var list = key_value.Value;
				foreach (var path in list)
				{
					log += string.Format("{0}\n", path);
				}
				Debug.LogFormat (texture, log);
			}
		}

		// bounds
		private static void DoGetRendererBoundsEach (SelectionMode mode)
		{
			var selectedAssets = Selection.GetFiltered (typeof(GameObject), mode);
			foreach (var obj in selectedAssets)
			{
				var log = new System.Text.StringBuilder();
				log.Append(AssetDatabase.GetAssetPath(obj));

				var go = obj as GameObject;

				var renderers = new List<Renderer>();
				for (int i = 0; i < go.transform.childCount; ++i)
				{
					var renderer = go.transform.GetChild(i).GetComponent<Renderer>();
					if (null != renderer)
					{
						renderers.Add(renderer);
					}
				}

				if (0 < renderers.Count)
				{
					log.AppendFormat("\nrenderers counts: {0}", renderers.Count);
					foreach (var r in renderers)
					{
						var sr = r as SkinnedMeshRenderer;
						if (null != sr)
						{
							log.AppendFormat("\n{0} local bounds: {1}, size: {2}", sr.name, sr.localBounds, sr.localBounds.size);
						}
						else
						{
							log.AppendFormat("\n{0} bounds: {1}, size: {2}", r.name, r.bounds, r.bounds.size);
						}
					}
				}

				log.AppendLine();
				Debug.Log(log.ToString());
			}
		}
		
		[MenuItem("Assets/GetInfo/RendererBoundsEach")]
		static void GetRendererBoundsEach () { DoGetRendererBoundsEach (SelectionMode.DeepAssets); }

		#region referenced by

		private static void DoGetReferencedBy(Material mat)
		{
			var matPath =  AssetDatabase.GetAssetPath(mat);
			Debug.LogFormat("{0}\n<color=green>GetReferencedBy Begin</color>", matPath);
			var guids = AssetDatabase.FindAssets("t:Model");
			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var go = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;
				var renderers = go.FindComponentsInChildren<Renderer>();
				foreach (var r in renderers)
				{
					if (null != r && null != r.sharedMaterials && ArrayUtility.Contains(r.sharedMaterials, mat))
					{
						Debug.LogFormat(path);
						break;
					}
				}
			}
			Debug.LogFormat("{0}\n<color=green>GetReferencedBy End</color>", matPath);
		}

		[MenuItem("Assets/GetInfo/ReferencedBy", true)]
		static bool GetReferencedByValidFunc () 
		{ 
			var objs = Selection.objects;
			if (null == objs || 1 != objs.Length)
			{
				return false;
			}

			var obj = objs[0];

			return obj is Material;
		}

		[MenuItem("Assets/GetInfo/ReferencedBy")]
		static void GetReferencedBy () 
		{ 
			var obj = Selection.activeObject;

			var mat = obj as Material;
			if (null != mat)
			{
				DoGetReferencedBy(mat);
			}
		}

		[MenuItem("Assets/GetInfo/PrefabsCount")]
		static void GetPrefabsCount () 
		{ 
			var obj = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath(obj);
			if (AssetDatabase.IsValidFolder(path))
			{
				var guids = AssetDatabase.FindAssets("t:Prefab", new string[]{path});
				Debug.LogFormat("prefabs count:{0}\n{1}", guids.Length, path);
			}
		}

		[MenuItem("Assets/GetInfo/RuntimeMemorySize")]
		static void GetRuntimeMemorySize () 
		{ 
			foreach (var obj in Selection.objects)
			{
				Debug.LogFormat(
					obj, 
					"RuntimeMemorySize: {0}\n{1}", 
					EditorUtility.FormatBytes(UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(obj)),
					AssetDatabase.GetAssetPath(obj));
			}
		}

		#endregion referenced by
		
	}
}// namespace Ghost.Editor
