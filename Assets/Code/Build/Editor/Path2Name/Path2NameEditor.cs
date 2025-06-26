using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;
using System.IO;
using System.Xml.Serialization;

namespace EditorTool
{
	public static class Path2NameEditor
	{
		[MenuItem("Assets/ScanResource")]
		public static void CreateDataAsset ()
		{
			if (!Directory.Exists (ResStrategy.holderAssetPath))
				Directory.CreateDirectory (ResStrategy.holderAssetPath);
			ResourceID.MakeAndReadFromResource (null);
			AssetDatabase.Refresh ();
		}
	}
} // namespace EditorTool
