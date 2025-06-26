using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CoreDataManager : ScriptableObject 
{
	public List<AtlasData> atlasDataList = new List<AtlasData>();

	public void RemoveData (AtlasData data)
	{
		atlasDataList.Remove (data);
		Save ();
	}

	public void Save()
	{
		EditorUtility.SetDirty (this);
	}
}