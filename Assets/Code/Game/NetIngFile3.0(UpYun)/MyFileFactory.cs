using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SLua.CustomLuaClassAttribute]
public class MyFileFactory : Singleton<MyFileFactory>
{
	public static MyFileFactory Ins
	{
		get {return ins;}
	}

	private List<MyFile> m_cachedMyFiles = new List<MyFile>();

	public MyFile GetMyFile(string path)
	{
		for (int i = 0; i < m_cachedMyFiles.Count; i++)
		{
			MyFile itemMyFile = m_cachedMyFiles[i];
			string itemPath = itemMyFile.Path;
			if (string.Equals(itemPath, path))
			{
				return itemMyFile;
			}
		}
		MyFile myFile = new MyFile(path);
		m_cachedMyFiles.Add(myFile);
		return myFile;
	}
}
