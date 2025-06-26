using UnityEngine;
using System.Collections;
using System.IO;

public class ModifyKeyBoardClass
{
	const string REPLACE_FILE_PATH = "iOSSDK/KeyBoard/Keyboard.mm";
	string _filePath;

	public ModifyKeyBoardClass (string filePath)
	{
		_filePath = filePath;
	}

	//keyboard修改，修复九宫格中文输入的bug
	public void FixJiugongge ()
	{
		//#if UNITY_5_3 || UNITY_2017
		string rootpath = Application.dataPath.Replace ("Assets", "");
		string path = Path.Combine (rootpath, REPLACE_FILE_PATH);
		if (File.Exists (path)) {
			File.Copy (path, _filePath, true);
			Debug.Log("替换keyboard.mm");
		}
		//#endif
	}
}
