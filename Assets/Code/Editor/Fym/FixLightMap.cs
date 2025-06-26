using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
public class FixLightMap : Editor
{
	[MenuItem("RO/Fym/修复地图撕裂",false,0)]
	static void FixGameObjectCopm ()
	{
		var renderers = GameObject.FindObjectsOfType<MeshRenderer>();
		foreach (var renderer in renderers)
		{
			if (ComponentUtility.CopyComponent(renderer))
			{
				var obj_Renderer= renderer.gameObject;
				Component.DestroyImmediate(renderer);
				ComponentUtility.PasteComponentAsNew(obj_Renderer);
			}
		}
	}

}

