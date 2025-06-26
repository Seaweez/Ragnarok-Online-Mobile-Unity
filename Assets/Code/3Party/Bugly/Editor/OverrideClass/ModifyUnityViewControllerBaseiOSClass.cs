using UnityEngine;
using System.Collections;

public class ModifyUnityViewControllerBaseiOSClass
{
	string _filePath;
	public ModifyUnityViewControllerBaseiOSClass (string filePath)
	{
		_filePath = filePath;
	}

	public void WriteSwitchEdgeProtectCode ()
	{
		ROXClass keyboard = new ROXClass (_filePath);
		keyboard.Replace ("    return res;", "return UIRectEdgeAll;");

	}
}