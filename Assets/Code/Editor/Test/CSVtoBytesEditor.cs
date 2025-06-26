using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using RO;
using System.Text;

namespace EditorTool
{
	public class CSVtoBytesEditor
	{
//		[MenuItem("Assets/TestLua")]
		static void TestLua ()
		{
			Object csv = Selection.activeObject;
			string path = AssetDatabase.GetAssetPath (csv);
			if (path.EndsWith (".txt")) {
				Debug.Log (path);
				string file = Path.GetFileNameWithoutExtension (path);
				string dir = Path.GetDirectoryName (path);
				//				Debug.Log (dir);
				//				Debug.Log (file);
				string newBytesfile = Path.Combine (dir, file + "_byte.txt");
				if (File.Exists (newBytesfile)) {
					File.Delete (newBytesfile);
//					File.Create (newBytesfile);
				}
				StringBuilder sb = new StringBuilder ();
				sb.AppendLine ("local TestTable = class(\"TestTable\")\n" +
					               "function TestTable:ctor()\n" +
					               "self.Datas = {}\n");

				sb.AppendLine ("TestTable.Datas = {}");
				for (int i=0; i<50000; i++) {
						sb.AppendLine (string.Format ("self.Datas[{0}] = \"Test{1}\"", i.ToString (), i.ToString ()));
//					sb.AppendLine(string.Format ("function test{0}()", i.ToString ()));
//					sb.AppendLine(string.Format ("print(\"test{0}\")", i.ToString ()));
//					sb.AppendLine("end\n");
				}
				sb.AppendLine ("end\n");
				sb.AppendLine ("function TestTable:Test(id)");
				sb.AppendLine (string.Format ("local d = self.Datas[id]"));
				sb.AppendLine ("print(d)");
				sb.AppendLine ("end\n");
				sb.AppendLine ("return TestTable");
				File.WriteAllText (newBytesfile, sb.ToString ());
				AssetDatabase.Refresh ();
			}
		}
	}
} // namespace EditorTool
