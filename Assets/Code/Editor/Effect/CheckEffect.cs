using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using Ghost.Extensions;
using Ghost.Utils;

namespace EditorTool
{
	public class CheckEffect 
	{
		private static void DoCheckProjectorObject(GameObject obj, float farclip = -1)
		{
			if (null == obj)
			{
				return;
			}
			var projs = obj.FindComponentsInChildren<Projector>();
			if (!projs.IsNullOrEmpty())
			{
				foreach (var p in projs)
				{
					Debug.LogFormat(obj, "Projector: {0}->{1}", obj.name, p.name);
					if (0 < farclip)
					{
						p.farClipPlane = farclip;
					}
				}
			}
		}

		private static void DoCheckProjector(Object selectionObj, float farclip = -1)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			Debug.LogFormat ("[CheckEffectProjector]: {0}", path);
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
					DoCheckProjectorObject(obj, farclip);
				}
			}
			else
			{
				DoCheckProjectorObject(selectionObj as GameObject, farclip);
			}
		}

		private static void DoCheckProjector(float farclip = -1)
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckProjector(obj, farclip);
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("[CheckProjector Finished]");
		}

		[MenuItem("Assets/CheckEffect/Projector")]
		static void CheckProjector()
		{
			DoCheckProjector();
		}

		[MenuItem("Assets/CheckEffect/Projector_SetFarClip_1")]
		static void SetProjectorFarClip_1()
		{
			DoCheckProjector(1);
		}

		[MenuItem("Assets/CheckEffect/Projector_SetFarClip_2")]
		static void SetProjectorFarClip_2()
		{
			DoCheckProjector(2);
		}

		[MenuItem("Assets/CheckEffect/Projector_SetFarClip_3")]
		static void SetProjectorFarClip_3()
		{
			DoCheckProjector(3);
		}

		[MenuItem("Assets/CheckEffect/Projector_SetFarClip_4")]
		static void SetProjectorFarClip_4()
		{
			DoCheckProjector(4);
		}

		[MenuItem("Assets/CheckEffect/Projector_SetFarClip_5")]
		static void SetProjectorFarClip_5()
		{
			DoCheckProjector(5);
		}

		public static void DoCheckParticalSystem(GameObject obj, string objPath)
		{
			if (null == obj)
			{
				return;
			}
			var renders = obj.FindComponentsInChildren<ParticleSystemRenderer>();
			if (!renders.IsNullOrEmpty())
			{
				var sameOrders = new Dictionary<int, HashSet<Material>>();
				var differentOrders = new Dictionary<Material, HashSet<int>>();

				var hasOrder0 = false;
				var hasSameOrder = false;
				var hasDifferentOrder = false;
				var hasNullMaterial = false;

				foreach (var r in renders)
				{
					if (0 == r.sortingOrder)
					{
						hasOrder0 = true;
					}

					if (null == r.sharedMaterial)
					{
						hasNullMaterial = true;
						continue;
					}

					HashSet<Material> mats;
					if (!sameOrders.TryGetValue(r.sortingOrder, out mats))
					{
						mats = new HashSet<Material>();
						sameOrders.Add(r.sortingOrder, mats);
					}
					mats.Add(r.sharedMaterial);
					if (!hasSameOrder)
					{
						hasSameOrder = 1 < mats.Count;
					}

					HashSet<int> orders;
					if (!differentOrders.TryGetValue(r.sharedMaterial, out orders))
					{
						orders = new HashSet<int>();
						differentOrders.Add(r.sharedMaterial, orders);
					}
					orders.Add(r.sortingOrder);
					if (!hasDifferentOrder)
					{
						hasDifferentOrder = 1 < orders.Count;
					}
				}


				if (hasNullMaterial || hasSameOrder || hasDifferentOrder || hasOrder0)
				{
					StringBuilder sb = new StringBuilder();
					if (hasNullMaterial)
					{
						sb.Append("<color=red>Has Null Materials! </color>");
					}
					if (hasSameOrder)
					{
						sb.Append("<color=red>Several Materials Has Same Order! </color>");
					}
					if (hasDifferentOrder)
					{
						sb.Append("<color=red>One Material Has Different Orders! </color>");
					}
					if (hasOrder0)
					{
						sb.Append("<color=yellow>Has 0 Order! </color>");
					}
					sb.AppendFormat("\n{0}", objPath);

					Debug.LogFormat(obj, sb.ToString());
				}
			}
		}

		public static void DoCheckParticalSystem(Object selectionObj)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			if (AssetDatabase.IsValidFolder(path))
			{
				var guids = AssetDatabase.FindAssets("t:Prefab", new string[]{path}); 
				
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
						DoCheckParticalSystem(obj, objPath);
					}
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}
				
			}
			else
			{
				DoCheckParticalSystem(selectionObj as GameObject, path);
			}
		}
		
		public static void DoCheckParticalSystem()
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoCheckParticalSystem(obj);
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("[DoCheckParticalSystem Finished]");
		}
		
		[MenuItem("Assets/CheckEffect/ParticalSystem")]
		static void CheckParticalSystem()
		{
			DoCheckParticalSystem();
		}
	
	}
} // namespace EditorTool
