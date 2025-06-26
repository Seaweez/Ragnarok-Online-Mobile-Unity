using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace RO
{
	//	[System.Serializable]
	public class PathId2Name
	{
		[XmlAttribute ("name")] 
		public string name;

		[XmlAttribute ("path")] 
		public string path;

		public List<PathId2Name>
			children;
		[XmlIgnoreAttribute]
		public SDictionary<string, string>
			fileToFull = new SDictionary<string, string> ();


		public string this [string file] {
			get {
				string v = null;
				fileToFull.TryGetValue (file, out v);
				return v;
			}
		}

		public void MakeScriptFullPath (string path)
		{
			if (children == null) {
				children = new List<PathId2Name> ();
			}
			path = path.Replace (ResourceID.ResFolder, "");
			string extension = Path.GetExtension (path);
			if (path.Contains ("Script") && extension == ".txt") {
				string file = Path.GetFileNameWithoutExtension (path);
				path = path.Replace (extension, "");
				#if UNITY_EDITOR
				path = path.Replace (System.IO.Path.DirectorySeparatorChar, '/');
				#endif
				PathId2Name pn = new PathId2Name ();
				pn.name = file;
				pn.path = path;
				children.Add (pn);
			}
		}

		public PathId2Name ()
		{
		}

		public void MapFullPath ()
		{
			PathId2Name path;
			if (children != null) {
				for (int i = 0; i < children.Count; i++) {
					path = children [i];
					fileToFull [path.name] = path.path;
				}
			}
		}
	}
}
// namespace RO
