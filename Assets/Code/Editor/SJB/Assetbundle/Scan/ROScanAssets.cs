using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

namespace EditorTool
{
	public class ROScanAssets
	{
		[MenuItem("Assets/ScanFolderPrint")]
		static void ScaneFolder ()
		{
			UnityEngine.Object selected = Selection.activeObject;
			if (selected != null) {
				string path = AssetDatabase.GetAssetPath (selected);
				string[] files = Array.FindAll (Directory.GetFiles (path, "*", SearchOption.AllDirectories), (p) => {
					string extension = Path.GetExtension (p);
					return !string.IsNullOrEmpty (extension) && extension != ".meta";
				});
				string text = "";
				string text1 = "";
				for (int i=0; i<files.Length; i++) {
					text = files [i];
					text = Path.GetFileNameWithoutExtension (text);
					text1 += "\"" + text + "\",\n";
				}
				Debug.Log (text1);
			}
		}

		public static void ScanDependencies (string folder, List<string> extensionsLimit, List<string> excepts, Action<string> call)
		{
			ScanDependenciesWithSearchPattern (folder, extensionsLimit, excepts, "*", call);
		}

		public static void ScanDependciesByFiles (List<string> files, List<string> extensions, bool includeSelf, Action<string> call)
		{
			ScanDependciesByFiles (files.ToArray (), extensions, includeSelf, call);
		}

		public static void ScanDependciesByFiles (string[] files, List<string> extensions, bool includeSelf, Action<string> call)
		{
			try {
				List<string> listFiles = new List<string> ();
				for (int i=0; i<files.Length; i++)
					files [i] = RetrimPath (files [i]);
				listFiles.AddRange (files);
				string[] depends = AssetBundleDependency.GetDependenciesWithoutExtensions (listFiles, extensions, includeSelf);
				if (call != null) {
					int Num = depends.Length;
					for (int i=0; i<Num; i++) {
						EditorUtility.DisplayProgressBar ("Scanning Depends", string.Format ("Scanned: {0}", depends [i]), (float)(i) / Num);
						call (depends [i]);
					}
				}
			} catch (System.Exception e) {
				throw e;
			} finally {
				EditorUtility.ClearProgressBar ();
			}
		}

		public static void ScanDependenciesWithSearchPattern (string folder, List<string> extensionsLimit, List<string> excepts, string pattern = "*", Action<string> call = null)
		{
			try {
				if (folder.Contains ("Assets/"))
					folder = folder.Replace ("Assets/", "");
				folder = Path.Combine (Application.dataPath, folder);
				string[] files = null;
				if (string.IsNullOrEmpty (Path.GetExtension (folder)) == false)
					files = new string[]{folder};
				else {
					files = Array.FindAll (Directory.GetFiles (folder, pattern, SearchOption.AllDirectories), (p) => {
						if (excepts != null) {
							for (int i=0; i<excepts.Count; i++) {
								if (p.Contains (excepts [i]))
									return false;
							}
						}
						string extension = Path.GetExtension (p);
						if (!string.IsNullOrEmpty (extension) && extension != ".meta") {
							if (extensionsLimit != null)
								return extensionsLimit.Contains (extension);
							return true;
						}
						return false;
					});
				}
				List<string> listFiles = new List<string> ();
				for (int i=0; i<files.Length; i++)
					files [i] = RetrimPath (files [i]);
				listFiles.AddRange (files);
				string[] depends = AssetBundleDependency.GetDependenciesWithoutExtensions (listFiles, null, true);
				if (call != null) {
					int Num = depends.Length;
					for (int i=0; i<Num; i++) {
						EditorUtility.DisplayProgressBar ("Scanning Depends", string.Format ("Scanned: {0}", depends [i]), (float)(i) / Num);
						call (depends [i]);
					}
				}
			} catch (System.Exception e) {
				throw e;
			} finally {
				EditorUtility.ClearProgressBar ();
			}
		}

		static string RetrimPath (string path)
		{
			return path.Replace (Application.dataPath, "Assets");
		}
	}
} // namespace EditorTool
