using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Ghost.Config;
using Ghost.Utils;

namespace EditorTool
{
	public class ExportUtils 
	{
		public static void ExportBoolean(WriterAdapter writer, bool b)
		{
			writer.WriteMemberValue(b);
		}

		public static void ExportVector(WriterAdapter writer, Vector2 v)
		{
			writer.WriteArrayStart();
			writer.WriteMemberValue(v.x);
			writer.WriteMemberValue(v.y);
			writer.WriteArrayEnd();
		}

		public static void ExportVector(WriterAdapter writer, Vector3 v)
		{
			writer.WriteArrayStart();
			writer.WriteMemberValue(v.x);
			writer.WriteMemberValue(v.y);
			writer.WriteMemberValue(v.z);
			writer.WriteArrayEnd();
		}

		public static void ExportColor(WriterAdapter writer, Color v)
		{
			writer.WriteArrayStart();
			writer.WriteMemberValue(v.r);
			writer.WriteMemberValue(v.g);
			writer.WriteMemberValue(v.b);
			writer.WriteMemberValue(v.a);
			writer.WriteArrayEnd();
		}

		public static void ExportEnum(WriterAdapter writer, System.Enum v)
		{
			writer.WriteMemberValue(System.Convert.ToInt32(v));
		}
	
		public static void ExportAsset(WriterAdapter writer, Object v, string rootPath = null)
		{
			if (null == rootPath)
			{
				rootPath = "Assets/Resources/";
			}
			var path = AssetDatabase.GetAssetPath(v);
			if (path.StartsWith(rootPath))
			{
				path = path.Substring(rootPath.Length);
			}
			path = Path.ChangeExtension(path, null);
			writer.WriteMemberValue(path);
		}

		public static void DoSaveAsset(string path, string content, Object oldAsset)
		{
			FileStream fs = null;
			StreamWriter sw = null;
			try
			{
				fs = new FileStream(path, File.Exists(path) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, new UTF8Encoding(false));
				
				sw.Write(content);
			}
			catch (System.Exception e)
			{
				Debug.LogError("SaveAsseta Error: "+e.Message);
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

				if (null != oldAsset)
				{
					EditorUtility.SetDirty(oldAsset);
				}
				AssetDatabase.SaveAssets();
			}
		}

		public static bool SaveAsset(string path, string content)
		{
			var oldAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
			path = PathUnity.Combine(PathConfig.DIRECTORY_ASSETS, path);
			if (File.Exists(path))
			{
				if (EditorUtility.DisplayDialog("Export Assets", string.Format("Asset Already Exist!!!\n{0}", path), "Override", "Cancel"))
				{
					DoSaveAsset(path, content, oldAsset);
					return true;
				}
			}
			else
			{
				DoSaveAsset(path, content, oldAsset);
				return true;
			}
			return false;
		}
		
	}
} // namespace EditorTool
