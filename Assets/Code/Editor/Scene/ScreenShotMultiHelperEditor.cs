using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ScreenShotMultiHelper))]
	public class ScreenShotMultiHelperEditor : Editor
	{
		enum Format
		{
			JPG,
			PNG
		}
		private Format format = Format.JPG;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			var helper = target as ScreenShotMultiHelper;

			EditorGUILayout.BeginHorizontal();
			helper.savePath = EditorGUILayout.TextField("Save", helper.savePath);
			if (GUILayout.Button("..."))
			{
				switch (format)
				{
				case Format.JPG:
					helper.savePath = EditorUtility.SaveFilePanel("Save Screen Shot", "", "ScreenShot", "jpg");
					break;
				case Format.PNG:
					helper.savePath = EditorUtility.SaveFilePanel("Save Screen Shot", "", "ScreenShot", "png");
					break;
				}
			}
			EditorGUILayout.EndHorizontal();

			if (Application.isPlaying)
			{
				if (!helper.cameraList.IsNullOrEmpty())
				{
					EditorGUILayout.Separator();
					format = (Format)EditorGUILayout.EnumPopup("Format", format);
					if (GUILayout.Button("Captrue"))
					{
						switch (format)
						{
						case Format.JPG:
							foreach (var camerasInfo in helper.cameraList)
							{
								var path = string.Format("{0}_{1}", helper.savePath, camerasInfo.suffix);
								helper.GetScreenShot(delegate(Texture2D obj) {
									ScreenShot.SaveJPG(obj, path);
									Object.DestroyImmediate(obj);
										}, camerasInfo.cameras);
							}
							break;
						case Format.PNG:
							foreach (var camerasInfo in helper.cameraList)
							{
								var path = string.Format("{0}_{1}", helper.savePath, camerasInfo.suffix);
								helper.GetScreenShot(delegate(Texture2D obj) {
									ScreenShot.SavePNG(obj, path);
									Object.DestroyImmediate(obj);
										}, camerasInfo.cameras);
							}
							break;
						}
					}
				}
			}
		}
	
	}
} // namespace EditorTool
