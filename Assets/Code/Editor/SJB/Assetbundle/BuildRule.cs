using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace EditorTool
{
	public enum EBuildRule
	{
		//指定名字
		SpecifyName,
		//文件夹
		FolderName,
		//所在文件夹
		FileFolder,
		//所在文件夹
		FileFolderNoDirectory,
		//自己文件名
		SelfName,
		//sub文件夹
		SubFolderName,
	}
	
	public class BuildRule
	{
		public EBuildRule rule { get; private set; }
		
		public string bundleName { get; private set; }
		
		public string checkFolder { get; private set; }

		List<BuildRule> _rules;

		public List<BuildRule> subRules {
			get {
				return _rules;
			}
		}

		public BuildRule SubMatch(string name)
		{
			if (_rules != null) {
				for(int i=0;i<_rules.Count;i++)
				{
					if(name.Contains(_rules[i].checkFolder))
						return _rules[i];
				}
			}
			return null;
		}

		public BuildRule (string folderName, string specifiedBundleName, EBuildRule rule)
		{
			this.rule = rule;
			bundleName = specifiedBundleName;
			if (folderName.EndsWith ("/"))
				folderName = folderName.Remove (folderName.Length - 1);
			checkFolder = folderName;
		}

		public void AddRule(BuildRule rule)
		{
			if (_rules == null)
				_rules = new List<BuildRule> ();
			_rules.Add (rule);
		}

		public string GetBundleName (string path)
		{
			string bundleName = path;
			string[] directories = null;
			string directory = null;
			switch (rule) {
			case EBuildRule.FolderName:
				bundleName = checkFolder;
				break;
			case EBuildRule.FileFolder:
				directory = Path.GetDirectoryName (path);
				directories = directory.Split (Path.DirectorySeparatorChar);
				bundleName = Path.Combine (directory, directories [directories.Length - 1]);
				break;
			case EBuildRule.FileFolderNoDirectory:
				bundleName = Path.GetDirectoryName (path);
				break;
			case EBuildRule.SelfName:
				break;
			case EBuildRule.SpecifyName:
				bundleName = this.bundleName;
				break;
			case EBuildRule.SubFolderName:
				path = path.Replace (this.checkFolder, "");
				directory = Path.GetDirectoryName (path);
                directory = directory.Replace(Path.DirectorySeparatorChar, '/');
                if (directory [0] == '/') {
					directory = directory.Remove (0, 1);
				}
                
                directories = directory.Split ('/');
				if (directories.Length > 0) {
					bundleName = Path.Combine (checkFolder, directories [0]);
				}
				break;
			}
			return bundleName;
		}
	}
} // namespace EditorTool
