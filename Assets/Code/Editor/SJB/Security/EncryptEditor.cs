using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using RO.Config;
using System;
using RO;
using System.Text;

namespace EditorTool
{
	public class EncryptEditor
	{
		[MenuItem("Assets/加密选中文件")]
		public static void ZipSelect ()
		{
			UnityEngine.Object select = Selection.activeObject;
			if (select != null) {
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();  
				sw.Start ();
				string path = "/" + AssetDatabase.GetAssetPath (select);
				path = path.Replace ("/Assets/", "");
				path = Path.Combine (Application.dataPath, path);
				string extension = Path.GetExtension (path);
				string entryptFilePath = ROPathConfig.TrimExtension (path) + "_entrypted" + extension;
				Debug.Log (path);
				Debug.Log (entryptFilePath);

				byte[] datas = File.ReadAllBytes (path);
				AESSecurity.DebugLog = true;
				byte[] encrypts = AESSecurity.EncryptBytes (datas);
				if (encrypts != null) {
					if (File.Exists (entryptFilePath)) {
						File.Delete (entryptFilePath);
					}
					File.WriteAllBytes (entryptFilePath, encrypts);
				}
				AssetDatabase.Refresh ();
				sw.Stop ();  
				TimeSpan ts2 = sw.Elapsed;
				Debug.Log (string.Format ("encrypt耗时{0}秒", ts2.TotalMilliseconds / 1000));
			}
		}

		public static void AESEncrypt (string filePath)
		{
			if (File.Exists (filePath)) {
				AESSecurity.DebugLog = true;
				byte[] datas = File.ReadAllBytes (filePath);
				Debug.Log (string.Format ("尝试encrypt {0}",filePath));
				byte[] encrypts = AESSecurity.EncryptBytes (datas);
				if (encrypts != null) {
					File.Delete (filePath);
					File.WriteAllBytes (filePath, encrypts);
				}
			}
		}

		[MenuItem("AssetBundle/Encrypt/LogSKeyBytes")]
		public static void LogSKeyBytes ()
		{
			string saltString = "XDKongmingsuzhitaicha";
			string pWDString = "XDkmsuzhizuicha";
			byte[] salt = Encoding.UTF8.GetBytes (saltString);
			byte[] salt2 = Encoding.UTF8.GetBytes (pWDString);
			string t = "";
			for(int i=0;i<salt.Length;i++)
			{
				t += salt[i].ToString() + ",";
			}
			Debug.Log(t);
			t = "";
			for(int i=0;i<salt2.Length;i++)
			{
				t += salt2[i].ToString() + ",";
			}
			Debug.Log(t);
		}
	}
} // namespace EditorTool
