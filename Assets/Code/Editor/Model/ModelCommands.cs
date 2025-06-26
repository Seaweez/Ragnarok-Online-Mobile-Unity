using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;
using Ghost.Utils;
using Ghost.Config;

namespace EditorTool
{
	public static class ModelCommands 
	{
		private static bool IsModelFBX(string path)
		{
			var fileName = Path.GetFileName(path);
			return string.Equals("fbx", Path.GetExtension(fileName).TrimStart('.').ToLower());
		}

		[MenuItem("Assets/Model/PickUpMesh", true)]
		static bool PickUpValidFunc()
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
				
				if (obj is GameObject && IsModelFBX(objPath))
				{
					return true;
				}
			}
			
			return false;
		}

		[MenuItem("Assets/Model/PickUpMesh")]
		static void PickUpMesh()
		{
			var objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			if (objs.IsNullOrEmpty())
			{
				Debug.LogFormat("<color=yellow>No Selected Assets</color>");
				return;
			}
			try
			{
				var progressTitle = "Pick Up Mesh";
				EditorUtility.DisplayProgressBar(progressTitle, "", 0);
				for (int i = 0; i < objs.Length; ++i)
				{
					var obj = objs[i];
					if (!(obj is GameObject))
					{
						continue;
					}
					var objPath = AssetDatabase.GetAssetPath(obj);
					if (!IsModelFBX(objPath))
					{
						continue;
					}
					
					var newMesh = AssetDatabase.LoadAssetAtPath(objPath, typeof(Mesh)) as Mesh;
					if (null == newMesh)
					{
						continue;
					}
					
					var savePath = Path.ChangeExtension(PathUnity.Combine(Path.GetDirectoryName(objPath), newMesh.name), PathConfig.EXTENSION_ASSET);
					var saveMesh = AssetDatabase.LoadAssetAtPath<Mesh>(savePath);
					string op = null;
					if (null == saveMesh)
					{
						saveMesh = new Mesh();
						AssetDatabase.CreateAsset(saveMesh, savePath);
						op = "New";
					}
					else
					{
						op = "Replace";
					}

					EditorUtility.CopySerialized(newMesh, saveMesh);
					
					EditorUtility.DisplayProgressBar(progressTitle, string.Format("[{0}, {1}]: {2}", newMesh.name, op, objPath), (float)i/objs.Length);
					Debug.LogFormat(obj, "<color=green>Pick Up Mesh({0}): </color>{1}", op, objPath);
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		[MenuItem("Assets/Model/Reimport")]
		static void Reimport()
		{
			var objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			if (objs.IsNullOrEmpty())
			{
				Debug.LogFormat("<color=yellow>No Selected Assets</color>");
				return;
			}
			try
			{
				var progressTitle = "Reimport Model FBX";
				EditorUtility.DisplayProgressBar(progressTitle, "", 0);
				for (int i = 0; i < objs.Length; ++i)
				{
					var obj = objs[i];
					if (!(obj is GameObject))
					{
						continue;
					}
					var objPath = AssetDatabase.GetAssetPath(obj);
					if (!IsModelFBX(objPath))
					{
						continue;
					}
					
					AssetDatabase.ImportAsset(objPath);
					
					EditorUtility.DisplayProgressBar(progressTitle, objPath, (float)i/objs.Length);
					Debug.LogFormat(obj, "<color=green>{0}: </color>{1}", progressTitle, objPath);
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		public static void DoModel(Object selectionObj, System.Action<GameObject, ModelImporter> doFunc)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = "t:Model";
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}
				
				var totalCount = guids.Length;
				try
				{
					int i = 0;
					foreach (var guid in guids)
					{
						var objPath = AssetDatabase.GUIDToAssetPath(guid);
						var obj = AssetDatabase.LoadAssetAtPath<GameObject>(objPath);
						
						++i;
						EditorUtility.DisplayProgressBar(
							string.Format("Proccessing Folder: {0}", path), 
							string.Format("{0}/{1}, {2}", i, totalCount, obj.name), 
							(float)i/totalCount);
						doFunc(obj, ModelImporter.GetAtPath(objPath) as ModelImporter);
					}
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}
				
			}
			else
			{
				doFunc(selectionObj as GameObject, ModelImporter.GetAtPath(path) as ModelImporter);
			}
		}
		
		public static void DoModels(System.Action<GameObject, ModelImporter> doFunc)
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoModel(obj, doFunc);
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("<color=green>[DoModels Finished]</color>");
		}

		[MenuItem("Assets/Model/FindBoneCountTop10")]
		static void FindBoneCountTop10()
		{
			var objList = new List<GameObject>();
			DoModels(delegate(GameObject obj, ModelImporter importer) {
				var smr = obj.FindComponentInChildren<SkinnedMeshRenderer>();
				if (null != smr && null != smr.bones)
				{
					objList.Add(obj);
				}
			});
			objList.Sort(delegate(GameObject x, GameObject y) {
				var smr1 = x.FindComponentInChildren<SkinnedMeshRenderer>();
				var smr2 = y.FindComponentInChildren<SkinnedMeshRenderer>();
				return smr2.bones.Length - smr1.bones.Length;
			});
			var count = Mathf.Min(10, objList.Count);
			for (int i = 0; i < count; ++i)
			{
				var obj = objList[i];
				var smr = obj.FindComponentInChildren<SkinnedMeshRenderer>();
				Debug.LogFormat(obj, "bone count: {0}\n{1}", smr.bones.Length, AssetDatabase.GetAssetPath(obj));
			}
		}

		[MenuItem("Assets/Model/FindVerticesCountTop10")]
		static void FindVerticesCountTop10()
		{
			var objList = new List<GameObject>();
			DoModels(delegate(GameObject obj, ModelImporter importer) {
				var smr = obj.FindComponentInChildren<SkinnedMeshRenderer>();
				if (null != smr && null != smr.sharedMesh)
				{
					objList.Add(obj);
				}
			});
			objList.Sort(delegate(GameObject x, GameObject y) {
				var smr1 = x.FindComponentInChildren<SkinnedMeshRenderer>();
				var smr2 = y.FindComponentInChildren<SkinnedMeshRenderer>();
				return smr2.sharedMesh.vertices.Length - smr1.sharedMesh.vertices.Length;
			});
			var count = Mathf.Min(10, objList.Count);
			for (int i = 0; i < count; ++i)
			{
				var obj = objList[i];
				var smr = obj.FindComponentInChildren<SkinnedMeshRenderer>();
				Debug.LogFormat(obj, "vertices count: {0}\n{1}", smr.sharedMesh.vertices.Length, AssetDatabase.GetAssetPath(obj));
			}
		}

		[MenuItem("Assets/Model/Shutdown_OptimizeGameObjects")]
		static void ShutdownOptimizeGameObjects()
		{
			DoModels(delegate(GameObject obj, ModelImporter importer) {
				if (importer.optimizeGameObjects)
				{
					Debug.LogFormat(obj, "Shutdown OptimizeGameObjects: {0}", importer.assetPath);
					importer.optimizeGameObjects = false;
					importer.SaveAndReimport();
				}
			});
			AssetDatabase.SaveAssets();
		}

		[MenuItem("Assets/Model/Open_OptimizeGameObjects")]
		static void OpenOptimizeGameObjects()
		{
			DoModels(delegate(GameObject obj, ModelImporter importer) {
				if (!importer.optimizeGameObjects)
				{
					Debug.LogFormat(obj, "Open OptimizeGameObjects: {0}", importer.assetPath);
					importer.optimizeGameObjects = true;
					
					var pointNames = new HashSet<string>();
					var points = GameObjectHelper.GetPoints(obj, "EP_");
					if (!points.IsNullOrEmpty())
					{
						foreach (var p in points)
						{
							if (null != p)
							{
								pointNames.Add(p.name);
							}
						}
					}
					points = GameObjectHelper.GetPoints(obj, "CP_");
					if (!points.IsNullOrEmpty())
					{
						foreach (var p in points)
						{
							if (null != p)
							{
								pointNames.Add(p.name);
							}
						}
					}
					var pointInfos = GameObjectHelper.GetPointsWithID(obj, "ECP_");
					if (!pointInfos.IsNullOrEmpty())
					{
						foreach (var info in pointInfos)
						{
							pointNames.Add(info.transform.name);
						}
					}
					
					importer.extraExposedTransformPaths = pointNames.ToArray();
					importer.SaveAndReimport();
				}
			});
			AssetDatabase.SaveAssets();
		}
	
	}
} // namespace EditorTool
