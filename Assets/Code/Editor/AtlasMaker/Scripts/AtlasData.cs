using System;
using UnityEditor;
using System.Collections.Generic;
using EditorTool;

[Serializable]
public class AtlasData
{
	public string atlasName;
	public List<string> resourcePathList;
	public string iconWidth;
	public string iconHeight;
	public string atlasPath;
	public string targetPfbPath;
	public string targetMatPath;
	public string targetTexPath;
	public int defaultFormat = -3;
	public int iosFormat;
	public int androidFormat;
	public int maxSize = 2048;
	public bool forceSquare = false;
    public bool splitPath = true;

    public AtlasData ()
	{
        resourcePathList = new List<string>();
        iconWidth = "0";
        iconHeight = "0";
        atlasPath = string.Empty;
        targetPfbPath = string.Empty;
        targetMatPath = string.Empty;
        targetTexPath = string.Empty;
    }

	public void Destroy()
	{
		if(!string.IsNullOrEmpty(targetPfbPath))
		{
			AssetDatabase.DeleteAsset (targetPfbPath);
			targetPfbPath = string.Empty;
		}
		if(!string.IsNullOrEmpty(targetMatPath))
		{
			AssetDatabase.DeleteAsset (targetMatPath);
			targetMatPath = string.Empty;
		}
		if(!string.IsNullOrEmpty(targetTexPath))
		{
			AssetDatabase.DeleteAsset (targetTexPath);
			targetTexPath = string.Empty;
		}
	}
}

