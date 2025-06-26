using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;
using Ghost.Extensions;
using Ghost.Utils;

namespace EditorTool
{
	public class CheckModel 
	{
		private static void DoCheckEffectPoint(GameObject go)
		{
			if (null == go)
			{
				return;
			}
			var eps = PointSubject.GetEffectPoints(go);
			if (!eps.IsNullOrEmpty())
			{
				foreach (var ep in eps)
				{
					if (null != ep && 0 < ep.transform.childCount)
					{
						Debug.LogErrorFormat(go, "[CheckEffectPoint Failed]: {0} has child!!!!\n{1}", ep.name, AssetDatabase.GetAssetPath(go));
					}
				}
			}
		}

		private static void DoCheckSelectionEffectPoint(Object selectionObj)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			Debug.LogFormat ("[CheckEffectPoint]: {0}", path);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = StringUtils.ConnectToString("t:", typeof(GameObject).Name);
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}
				
				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
					DoCheckEffectPoint(obj);
				}
			}
			else
			{
				DoCheckEffectPoint(selectionObj as GameObject);
			}
		}
		
		private static void DoCheckSelectionEffectPoint()
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckSelectionEffectPoint(obj);
			}
			Debug.LogFormat("[CheckEffectPoint Finished]");
		}

		private static void DoCheckHasEffectPoint(GameObject go)
		{
			if (null == go)
			{
				return;
			}
			var eps = PointSubject.GetEffectPoints(go);
			if (!eps.IsNullOrEmpty())
			{
				Debug.LogErrorFormat(go, "[CheckHasEffectPoint Failed]: ep count: {0}\n{1}", eps.Length, AssetDatabase.GetAssetPath(go));
			}
		}

		private static void DoCheckSelectionHasEffectPoint(Object selectionObj)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			Debug.LogFormat ("[CheckHasEffectPoint]: {0}", path);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = StringUtils.ConnectToString("t:", typeof(GameObject).Name);
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}
				
				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
					DoCheckHasEffectPoint(obj);
				}
			}
			else
			{
				DoCheckHasEffectPoint(selectionObj as GameObject);
			}
		}

		private static void DoCheckSelectionHasEffectPoint()
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckSelectionHasEffectPoint(obj);
			}
			Debug.LogFormat("[CheckHasEffectPoint Finished]");
		}

		private static void DoCheckSkinnedMeshRenderer(GameObject go)
		{
			if (null == go)
			{
				return;
			}
			var smr = go.FindComponentInChildren<SkinnedMeshRenderer>();
			if (null == smr)
			{
				Debug.LogErrorFormat(go, "[CheckSkinnedMeshRenderer Failed]: no SkinnedMeshRenderer\n{0}", AssetDatabase.GetAssetPath(go));
			}
			else 
			{
				if (null == smr.sharedMaterial || null == smr.sharedMaterial.mainTexture)
				{
					Debug.LogErrorFormat(go, "[CheckSkinnedMeshRenderer Failed]: no Material or mainTexture\n{0}", AssetDatabase.GetAssetPath(go));
				}
				if (smr.bones.IsNullOrEmpty())
				{
					Debug.LogErrorFormat(go, "[CheckSkinnedMeshRenderer Failed]: no bones\n{0}", AssetDatabase.GetAssetPath(go));
				}
			}
		}

		private static void DoCheckSelectionSkinnedMeshRenderer(Object selectionObj)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			Debug.LogFormat ("[CheckSkinnedMeshRenderer]: {0}", path);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = StringUtils.ConnectToString("t:", typeof(GameObject).Name);
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}
				
				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
					DoCheckSkinnedMeshRenderer(obj);
				}
			}
			else
			{
				DoCheckSkinnedMeshRenderer(selectionObj as GameObject);
			}
		}
		
		private static void DoCheckSelectionSkinnedMeshRenderer()
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckSelectionSkinnedMeshRenderer(obj);
			}
			Debug.LogFormat("[CheckSkinnedMeshRenderer Finished]");
		}

		private static void DoCheckMainRenderer(GameObject go)
		{
			if (null == go)
			{
				return;
			}
			Renderer mainRenderer = null;
			mainRenderer = go.GetComponentInChildrenTopLevel<SkinnedMeshRenderer>();
			if (null == mainRenderer)
			{
				mainRenderer = go.GetComponentInChildrenTopLevel<MeshRenderer>();
			}

			if (null != mainRenderer && mainRenderer.name != go.name) 
			{
				Debug.LogErrorFormat(go, "[CheckMainRenderer Failed]: name is not same as gameObject\n{0}", AssetDatabase.GetAssetPath(go));
			}
		}
		
		private static void DoCheckSelectionMainRenderer(Object selectionObj)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			Debug.LogFormat ("[CheckMainRenderer]: {0}", path);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = StringUtils.ConnectToString("t:", typeof(GameObject).Name);
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}
				
				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
					DoCheckMainRenderer(obj);
				}
			}
			else
			{
				DoCheckMainRenderer(selectionObj as GameObject);
			}
		}
		
		private static void DoCheckSelectionMainRenderer()
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckSelectionMainRenderer(obj);
			}
			Debug.LogFormat("[CheckMainRenderer Finished]");
		}

		private static void DoCheckRenderer(GameObject go)
		{
			if (null == go)
			{
				return;
			}
			var rs = go.FindComponentsInChildren<Renderer>();
			if (!rs.IsNullOrEmpty()) 
			{
				foreach (var r in rs)
				{
					var mats = r.sharedMaterials;
					if (!mats.IsNullOrEmpty())
					{
						foreach (var m in mats)
						{
							if (null == m)
							{
								Debug.LogErrorFormat(go, "[CheckRenderer Failed]: has none material\n{0}", AssetDatabase.GetAssetPath(go));
								return;
							}
						}
					}
				}
			}
		}
		
		private static void DoCheckSelectionRenderer(Object selectionObj)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			Debug.LogFormat ("[CheckRenderer]: {0}", path);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = StringUtils.ConnectToString("t:", typeof(GameObject).Name);
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}
				
				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
					DoCheckRenderer(obj);
				}
			}
			else
			{
				DoCheckRenderer(selectionObj as GameObject);
			}
		}
		
		private static void DoCheckSelectionRenderer()
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckSelectionRenderer(obj);
			}
			Debug.LogFormat("[CheckRenderer Finished]");
		}

		private static void DoCheckAnimation(GameObject go)
		{
			if (null == go)
			{
				return;
			}
			var a = go.GetComponent<Animator>();
			if (null != a && null != a.runtimeAnimatorController) 
			{
				var clips = a.runtimeAnimatorController.animationClips;
				if (!clips.IsNullOrEmpty())
				{
					foreach (var clip in clips)
					{
						if ("attack" == clip.name)
						{
							if (1 != clip.length)
							{
								Debug.LogErrorFormat(go, "[CheckAnimation Failed]: attack length({0}) is not 1 second\n{1}", clip.length, AssetDatabase.GetAssetPath(go));
							}
						}
					}
				}
			}
		}

		private static void DoCheckSelectionAnimation(Object selectionObj)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			Debug.LogFormat ("[CheckAnimation]: {0}", path);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = StringUtils.ConnectToString("t:", typeof(GameObject).Name);
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 

				if (guids.IsNullOrEmpty())
				{
					return;
				}

				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
					DoCheckAnimation(obj);
				}
			}
			else
			{
				DoCheckAnimation(selectionObj as GameObject);
			}
		}

		private static void DoCheckSelectionAnimation()
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckSelectionAnimation(obj);
			}
			Debug.LogFormat("[CheckAnimation Finished]");
		}

		[MenuItem("Assets/CheckModel/EffectPoint")]
		static void CheckEffectPoint()
		{
			DoCheckSelectionEffectPoint();
		}

		[MenuItem("Assets/CheckModel/HasEffectPoint")]
		static void CheckHasEffectPoint()
		{
			DoCheckSelectionHasEffectPoint();
		}

		[MenuItem("Assets/CheckModel/SkinnedMeshRenderer")]
		static void CheckSkinnedMeshRenderer()
		{
			DoCheckSelectionSkinnedMeshRenderer();
		}

		[MenuItem("Assets/CheckModel/MainRenderer")]
		static void CheckMainRenderer()
		{
			DoCheckSelectionMainRenderer();
		}

		[MenuItem("Assets/CheckModel/Renderer")]
		static void CheckRenderer()
		{
			DoCheckSelectionRenderer();
		}

		[MenuItem("Assets/CheckModel/Animation")]
		static void CheckAnimation()
		{
			DoCheckSelectionAnimation();
		}
	}
} // namespace EditorTool
