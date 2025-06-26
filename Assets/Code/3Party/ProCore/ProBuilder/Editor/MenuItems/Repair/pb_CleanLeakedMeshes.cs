using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ProBuilder2.Actions
{
	public class pb_CleanLeakedMeshes : Editor
	{
		[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Repair/Clean Leaked Meshes", false, pb_Constant.MENU_REPAIR)]
		public static void CleanUp()
		{
			#if UNITY_5 || UNITY_2017
				EditorUtility.UnloadUnusedAssetsImmediate();
			#else
				EditorUtility.UnloadUnusedAssetsIgnoreManagedReferences();
			#endif
		}
	}
}