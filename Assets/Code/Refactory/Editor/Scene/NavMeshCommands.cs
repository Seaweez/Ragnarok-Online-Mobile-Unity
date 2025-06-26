using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Ghost.Utils;
using LitJson;
using RO;
using UnityEditor.SceneManagement;

namespace EditorTool
{
	public class NavMeshInfo
	{
		public float agentRadius = 0;
		public float agentHeight = 0;
		public float maxSlope = 0;
		public float stepHeight = 0;
	}

	public class NavMeshParams
	{
		public float tileSize = 0f;
		public float walkableHeight = 0f;
		public float walkableRadius = 0f;
		public float walkableClimb = 0f;
		public float cellSize = 0f;

		private static float ParseFloat(SerializedProperty prop, string name)
		{
			var value = prop.FindPropertyRelative(name);
			return null != value ? value.floatValue : 0f;
		}

		private static float ParseInt(SerializedProperty prop, string name)
		{
			var value = prop.FindPropertyRelative(name);
			return null != value ? value.intValue : 0f;
		}

		public static NavMeshParams Parse(Object asset)
		{
			var info = new SerializedObject(asset);
			var paramsInfo = info.FindProperty("m_NavMeshBuildSettings");
			if (null != paramsInfo)
			{
				NavMeshParams p = new NavMeshParams();
				p.tileSize = ParseInt(paramsInfo, "tileSize");
				p.walkableHeight = ParseFloat(paramsInfo, "agentHeight");
				p.walkableRadius = ParseFloat(paramsInfo, "agentRadius");
				p.walkableClimb = ParseFloat(paramsInfo, "agentClimb");
				p.cellSize = ParseFloat(paramsInfo, "cellSize");
				return p;
			}
			return null;
		}
	}

	public static class NavMeshCommands 
	{
		public static List<GameObject> GetAllNavMeshObjects()
		{
			var navMeshObjs = new List<GameObject>();

			var renderers = GameObject.FindObjectsOfType<MeshRenderer>();
			foreach (var obj in renderers)
			{
				if (GameObjectUtility.AreStaticEditorFlagsSet(obj.gameObject, StaticEditorFlags.NavigationStatic))
				{
					navMeshObjs.Add(obj.gameObject);
				}
			}

			return navMeshObjs;
		}

		public static void DoSetStatic()
		{
			var objs = Object.FindObjectsOfType<NavMeshStatic>();
			foreach (var obj in objs)
			{
				if (obj.editoOnly)
				{
					obj.tag = RO.Config.Tag.EDITOR_ONLY;
				}
				else
				{
					obj.tag = RO.Config.Tag.UNTAGGED;
				}
				GameObjectUtility.SetStaticEditorFlags(obj.gameObject, GameObjectUtility.GetStaticEditorFlags(obj.gameObject)|StaticEditorFlags.NavigationStatic);
				GameObjectUtility.SetNavMeshArea(obj.gameObject, obj.navMeshArea);
			}
		}

		[MenuItem("RO/NavMesh/DebugLogAllObjs")]
		static void DebugLogAllObjs()
		{
			var objs = GetAllNavMeshObjects();
			foreach (var obj in objs)
			{
				Debug.Log(string.Format("NavMesh Object: {0}", obj));
			}
		}

		[MenuItem("RO/NavMesh/ClearAllStatic")]
		static void ClearAllStatic()
		{
			var objs = GetAllNavMeshObjects();
			foreach (var obj in objs)
			{
				GameObjectUtility.SetStaticEditorFlags(
					obj, 
					GameObjectUtility.GetStaticEditorFlags(obj)&~StaticEditorFlags.NavigationStatic);
			}
		}

		[MenuItem("RO/NavMesh/SetStatic")]
		static void SetStatic()
		{
			DoSetStatic();
		}

		private static NavMeshParams GetNavMeshParams()
		{
			var sceneFolder = Path.GetDirectoryName(EditorSceneManager.GetActiveScene().path);
			var sceneName = Path.GetFileNameWithoutExtension(EditorSceneManager.GetActiveScene().name);
			var navMeshAssetPath = PathUnity.Combine(PathUnity.Combine(sceneFolder, sceneName), "NavMesh.asset");
			var navMeshAsset = AssetDatabase.LoadAssetAtPath(navMeshAssetPath, typeof(Object));
			if (null == navMeshAsset)
			{
				sceneFolder = Path.GetDirectoryName(sceneFolder);
				navMeshAssetPath = PathUnity.Combine(PathUnity.Combine(sceneFolder, sceneName), "NavMesh.asset");
				navMeshAsset = AssetDatabase.LoadAssetAtPath(navMeshAssetPath, typeof(Object));
			}
			if (null != navMeshAsset)
			{
				return NavMeshParams.Parse(navMeshAsset);
			}
			return null;
		}

		private static void WriteNavMeshInfo(WriterAdapter writer, NavMeshInfo info)
		{
			var trigangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
			var navMeshParams = GetNavMeshParams();
			
			writer.WriteStructStart ();
			
			if (null != info)
			{
				writer.WriteMemberName ("agent_radius");
				writer.WriteMemberValue(info.agentRadius);
				writer.WriteMemberName ("agent_height");
				writer.WriteMemberValue(info.agentHeight);
				writer.WriteMemberName ("max_slope");
				writer.WriteMemberValue(info.maxSlope);
				writer.WriteMemberName ("step_height");
				writer.WriteMemberValue(info.stepHeight);
			}

			if (null != navMeshParams)
			{
				writer.WriteMemberName ("tileSize");
				writer.WriteMemberValue(navMeshParams.tileSize);
				writer.WriteMemberName ("walkableHeight");
				writer.WriteMemberValue(navMeshParams.walkableHeight);
				writer.WriteMemberName ("walkableRadius");
				writer.WriteMemberValue(navMeshParams.walkableRadius);
				writer.WriteMemberName ("walkableClimb");
				writer.WriteMemberValue(navMeshParams.walkableClimb);
				writer.WriteMemberName ("cellSize");
				writer.WriteMemberValue(navMeshParams.cellSize);
			}
			
			writer.WriteMemberName ("areas");
			writer.WriteArrayStart ();
			foreach (var a in trigangulation.areas)
			{
				writer.WriteMemberValue(a);
			}
			writer.WriteArrayEnd();
			
			writer.WriteMemberName ("vertices");
			writer.WriteArrayStart ();
			foreach (var v in trigangulation.vertices)
			{
				writer.WriteStructStart();
				writer.WriteMemberName ("x");
				writer.WriteMemberValue(v.x);
				writer.WriteMemberName ("y");
				writer.WriteMemberValue(v.y);
				writer.WriteMemberName ("z");
				writer.WriteMemberValue(v.z);
				writer.WriteStructEnd();
			}
			writer.WriteArrayEnd();
			
			writer.WriteMemberName ("indices");
			writer.WriteArrayStart ();
			foreach (var i in trigangulation.indices)
			{
				writer.WriteMemberValue(i);
			}
			writer.WriteArrayEnd();
			
			writer.WriteStructEnd();
		}

		private static StringBuilder GetNavMeshTrigangulation_Json(NavMeshInfo info)
		{
			StringBuilder sb = new StringBuilder ();
			var writer = DataWriter.GetAdapter(new JsonWriter (sb));
			WriteNavMeshInfo(writer, info);
			return sb;
		}
		
		private static StringBuilder GetNavMeshTrigangulation_Lua(NavMeshInfo info)
		{
			StringBuilder sb = new StringBuilder ();
			var writer = DataWriter.GetAdapter(new LuaWriter (sb));
			WriteNavMeshInfo(writer, info);
			return sb;
		}

		public static string GetExportFilePath(string extension)
		{
			var sceneName = Path.GetFileNameWithoutExtension(EditorSceneManager.GetActiveScene().name);
			string folder = PathUnity.Combine(RO.Config.Export.SCENE_DIRECTORY, sceneName);
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}
			
			return Path.ChangeExtension(PathUnity.Combine(folder, "NavMesh.txt"), extension);
		}

		public static NavMeshInfo GetExportedNavMeshInfo()
		{
			var navMeshExportFile = NavMeshCommands.GetExportFilePath("json");
			if (!File.Exists(navMeshExportFile))
			{
				return null;
			}

			FileStream fs = null;
			StreamReader sr = null;
			try
			{
				fs = new FileStream(navMeshExportFile, FileMode.Open);
				sr = new StreamReader(fs, new UTF8Encoding(false));

				var info = new NavMeshInfo();
				JsonData jd = JsonMapper.ToObject(sr.ReadToEnd());

				info.agentRadius = (float)((double)jd["agent_radius"]);
				info.agentHeight = (float)((double)jd["agent_height"]);
				info.maxSlope = (float)((double)jd["max_slope"]);
				info.stepHeight = (float)((double)jd["step_height"]);

				return info;
			}
			catch (System.Exception)
			{
				return null;
			}
			finally
			{
				if (null != sr)
				{
					sr.Close();
				}
				if (null != fs)
				{
					fs.Close();
				}
			}
		}

		private static void ExportToJson(NavMeshInfo info)
		{
			var file = GetExportFilePath("json");
			
			FileStream fs = null;
			StreamWriter sw = null;
			try
			{
				fs = new FileStream(file, File.Exists(file) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, /*Encoding.UTF8*/new UTF8Encoding(false));
				
				sw.Write(GetNavMeshTrigangulation_Json(info).ToString());
				
				Debug.Log("Export NavMesh Json Success");
			}
			catch (System.Exception e)
			{
				Debug.LogError("Export NavMesh Json Error: "+e.Message);
			}
			finally
			{
				if (null != sw)
				{
					sw.Close();
				}
				if (null != fs)
				{
					fs.Close();
				}
			}
		}
		
		private static void ExportToLua(NavMeshInfo info)
		{
			var file = GetExportFilePath("lua");
			
			FileStream fs = null;
			StreamWriter sw = null;
			try
			{
				fs = new FileStream(file, File.Exists(file) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, new UTF8Encoding(false));
				
				sw.Write(GetNavMeshTrigangulation_Lua(info).ToString());
				
				Debug.Log("Export NavMesh Lua Success");
			}
			catch (System.Exception e)
			{
				Debug.LogError("Export NavMesh Lua Error: "+e.Message);
			}
			finally
			{
				if (null != sw)
				{
					sw.Close();
				}
				if (null != fs)
				{
					fs.Close();
				}
			}
		}

		public static void DoExport(NavMeshInfo info)
		{
			ExportToJson(info);
			ExportToLua(info);
		}

		[MenuItem("RO/NavMesh/Export")]
		static void Export()
		{
			DoExport(null);
		}
		
	}
} // namespace EditorTool
