using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ghost.Utils;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ChildrenTransformExporter))]
	public class ChildrenTransformExporterEditor : Editor
	{
		private void WriteChildrenTransformInfo(ChildrenTransformExporter exporter, WriterAdapter writer)
		{
			writer.WriteStructStart();

			var childCount = exporter.transform.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				var child = exporter.transform.GetChild(i);
				writer.WriteMemberNameAsIndex(child.name);
				writer.WriteStructStart();

				writer.WriteMemberName("localPosition");
				writer.WriteArrayStart();
				writer.WriteMemberValue(child.transform.localPosition.x);
				writer.WriteMemberValue(child.transform.localPosition.y);
				writer.WriteMemberValue(child.transform.localPosition.z);
				writer.WriteArrayEnd();

				writer.WriteMemberName("localRotation");
				writer.WriteArrayStart();
				writer.WriteMemberValue(child.transform.localRotation.x);
				writer.WriteMemberValue(child.transform.localRotation.y);
				writer.WriteMemberValue(child.transform.localRotation.z);
				writer.WriteMemberValue(child.transform.localRotation.w);
				writer.WriteArrayEnd();

				writer.WriteMemberName("localScale");
				writer.WriteArrayStart();
				writer.WriteMemberValue(child.transform.localScale.x);
				writer.WriteMemberValue(child.transform.localScale.y);
				writer.WriteMemberValue(child.transform.localScale.z);
				writer.WriteArrayEnd();

				writer.WriteStructEnd();
			}

			writer.WriteStructEnd();
		}

		private StringBuilder GetChildrenTransformInfo(ChildrenTransformExporter exporter, string name)
		{
			StringBuilder sb = new StringBuilder ();
			var writer = DataWriter.GetAdapter(new LuaWriter (sb));

			(writer as LuaWriterApdater).rootName = name;
			WriteChildrenTransformInfo(exporter, writer);
			return sb;
		}

		private void Export(ChildrenTransformExporter exporter)
		{
			if (null == exporter.config)
			{
				return;
			}
			var configPath = AssetDatabase.GetAssetPath(exporter.config);
			FileStream fs = null;
			StreamWriter sw = null;
			try
			{
				fs = new FileStream(configPath, File.Exists(configPath) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, new UTF8Encoding(false));
				
				sw.Write(GetChildrenTransformInfo(exporter, Path.GetFileNameWithoutExtension(configPath)).ToString());
				
				Debug.Log("Export Children Transform Success");
			}
			catch (System.Exception e)
			{
				Debug.LogError("Export Children Transform Error: "+e.Message);
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
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (!Application.isPlaying && GUILayout.Button("Export"))
			{
				Export(target as ChildrenTransformExporter);
			}
		}
	
	}
} // namespace EditorTool
