using UnityEngine;
using UnityEditor;
using Ghost.Extensions;
using System.Collections.Generic;
using System.IO;
using System;

namespace EditorTool
{
	public class ROCheckDependency
	{
		static string outputPath = Path.Combine (Application.dataPath, "DependencyErrors.errors");
		static List<string> commonFobiddens = new List<string> (){
			"Assets/DevelopEffect/",
			"Assets/DevelopScene/",
			"Assets/DevelopTest/",
			"Assets/Scene/",
			"Assets/AutomatedConfig/",
		};
		static List<string> filterExtensions = new List<string> ()
		{
			".cs",
			".shader",
		};
		static List<string> commonExceptInforbid = new List<string> (){
			"Assets/DevelopScene/Terrains/"
		};

		[MenuItem("Assets/检查依赖")]
		public static void CheckRODependency ()
		{
			if (File.Exists (outputPath)) {
				File.Delete (outputPath);
			}
			CheckFolder ("Art/Model/", new List<string> (){"Assets/Art/Public/"}, commonFobiddens, commonExceptInforbid, false);
			List<string> forbids = new List<string> ();
			forbids.AddRange (commonFobiddens);
			forbids.Add ("Art/Model/");
			CheckFolder ("Art/Public/", null, forbids, commonExceptInforbid);
			CheckFolder ("DevelopScene/Terrains/", null, commonFobiddens);
			CheckFolder ("Engine/", null, commonFobiddens);
			CheckFolder ("Resources/", new List<string>(){
				"Assets/Art/",
				"Assets/DevelopScene/Terrains/",
				"Assets/Engine/"
			});
			AssetDatabase.Refresh ();
		}
	
		//rootFolder--检测的根目录  onlyDepends--只依赖的目录  forbidDepends--禁止依赖的目录  exceptInforbid--剔除的禁止依赖子目录  allowDependInside--是否允许内部依赖
		public static void CheckFolder (string rootFolder, List<string> onlyDepends=null, List<string> forbidDepends=null, List<string> exceptInforbid=null, bool allowDependInside=true)
		{
			try {
				string thisFolder;
				string thisRootFolder = "Assets/" + rootFolder;
				string[] files = Array.FindAll (Directory.GetFiles (Path.Combine (Application.dataPath, rootFolder), "*", SearchOption.AllDirectories), (p) => {
					string extension = Path.GetExtension (p);
					return !string.IsNullOrEmpty (extension) && extension != ".meta";
				});
				string file = null;
				SDictionary<string,SDictionary<string,List<string>>> errors = new SDictionary<string, SDictionary<string, List<string>>> ();
				for (int i=0,Num = files.Length; i<Num; i++) {
					file = "Assets" + files [i].Replace (Application.dataPath, "");
					thisFolder = Path.GetDirectoryName (file);
					EditorUtility.DisplayProgressBar ("Scanning Assets", string.Format ("Scanned: {0}", file), (float)(i) / Num);
					string[] depends = AssetBundleDependency.GetDependenciesWithoutExtensions (new List<string> (){file}, null, false);
					Array.ForEach (depends, (d) => {
						string ext = Path.GetExtension (d);
						if (!filterExtensions.Contains (ext)) {
							//依赖目录不为自己所在目录
							if (!d.Contains (thisFolder)) {
								if (d.Contains (thisRootFolder)) {
									if (!allowDependInside) {
										AddError (errors, rootFolder, file, d);
										return;
									}
								} else {
									if (onlyDepends != null) {
										if (!CheckOnlyDepend (onlyDepends, d)) {
											AddError (errors, rootFolder, file, d);
											return;
										}
									}
									if (CheckForbidden (forbidDepends, exceptInforbid, d)) {
										AddError (errors, rootFolder, file, d);
										return;
									}
								}
							} 
						}
					});
				}
				DrawError (errors);
			} catch (System.Exception e) {
				throw e;
			} finally {
				EditorUtility.ClearProgressBar ();
			}
		}

		static bool CheckForbidden (List<string> forbidDepends, List<string> exceptInforbid, string path)
		{
			if (forbidDepends != null) {
				for (int i=0; i<forbidDepends.Count; i++) {
					if (path.Contains (forbidDepends [i])) {
						if (exceptInforbid == null || exceptInforbid.FindIndex ((s) => {
							return path.Contains (s);
						}) == -1)
							return true;
					}
				}
			}
			return false;
		}

		static bool CheckOnlyDepend (List<string> onlyDepends, string path)
		{
			for (int i=0; i<onlyDepends.Count; i++) {
				if (path.Contains (onlyDepends [i]))
					return true;
			}
			return false;
		}

		static bool AddError (SDictionary<string,SDictionary<string,List<string>>> map, string folder, string errorFile, string depend)
		{
			SDictionary<string,List<string>> errors = map [folder];
			if (errors == null) {
				errors = new SDictionary<string, List<string>> ();
				map [folder] = errors;
			}
			List<string> depends = errors [errorFile];
			if (depends == null) {
				depends = new List<string> ();
				errors [errorFile] = depends;
			}
			if (depends.Contains (depend) == false)
				depends.Add (depend);
			return true;
		}

		static void DrawError (SDictionary<string,SDictionary<string,List<string>>> map)
		{
			string errors = "";
			foreach (KeyValuePair<string,SDictionary<string,List<string>>> kvp in map) {
				errors += kvp.Key + " :\n";
				foreach (KeyValuePair<string,List<string>> kvp2 in kvp.Value) {
					errors += "\t" + kvp2.Key + " :\n";
					foreach (string s in kvp2.Value) {
//						errors += "\t\t<color=red>" + s + "</color>\n";
						errors += "\t\t\t--" + s + "\n";
					}
				}
			}
			Debug.Log (errors);
			if (File.Exists (outputPath) == false) {
				FileStream fs = File.Create (outputPath);
				fs.Dispose ();
				fs.Close ();
				File.WriteAllText (outputPath, errors);
			} else {
				File.AppendAllText (outputPath, errors);
			}
		}
	}
} // namespace EditorTool
