using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	[CustomEditor(typeof(RenderPictures))]
	public class E_RenderPicturesEditor : Editor
	{
		private static string[] GetGUIDs(Object folder, out string folderPath)
		{
			folderPath = AssetDatabase.GetAssetPath(folder);
			if (AssetDatabase.IsValidFolder(folderPath))
			{
				return AssetDatabase.FindAssets("t:Prefab t:Model", new string[]{folderPath});
			}
			return null;
		}

		private static GameObject LoadObject(string guid, out string objPath)
		{
			objPath = AssetDatabase.GUIDToAssetPath(guid);
			return AssetDatabase.LoadAssetAtPath<GameObject>(objPath);
		}

		private static bool SetAnimatorController(GameObject obj, string objPath)
		{
			var animator = obj.GetComponent<Animator>();
			if (null == animator)
			{
				return false;
			}
			var objFolderPath = Path.GetDirectoryName(objPath);
			var guids = AssetDatabase.FindAssets("t:AnimatorController AC_", new string[]{objFolderPath});
			if (guids.IsNullOrEmpty())
			{
				return false;
			}
			var acPath = AssetDatabase.GUIDToAssetPath(guids[0]);
			UnityEditor.Animations.AnimatorController ac = AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(acPath);
			if (null == ac)
			{
				return false;	
			}
			animator.runtimeAnimatorController = ac;
			return true;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying)
			{
				EditorGUILayout.Separator();

				var render = target as RenderPictures;
				if (!render.running)
				{
					if (GUILayout.Button("Start"))
					{
						var path = EditorUtility.SaveFolderPanel("Save Render Pictures", "", "RenderPicture");
						if (!string.IsNullOrEmpty(path))
						{
							render.StartRender(
								path, 
								GetGUIDs,
								LoadObject,
								SetAnimatorController,
								EditorUtility.DisplayCancelableProgressBar,
								EditorUtility.ClearProgressBar);
						}
					}
				}
			}
		}
	}
} // namespace Ghost.EditorTool
