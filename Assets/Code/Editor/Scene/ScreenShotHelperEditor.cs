using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ScreenShotHelper))]
	public class ScreenShotHelperEditor : Editor
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

			var helper = target as ScreenShotHelper;

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
				if (!helper.cameras.IsNullOrEmpty())
				{
					EditorGUILayout.Separator();
					format = (Format)EditorGUILayout.EnumPopup("Format", format);
					if (GUILayout.Button("Captrue"))
					{
						switch (format)
						{
						case Format.JPG:
							helper.GetScreenShot(delegate(Texture2D obj) {
								ScreenShot.SaveJPG(obj, helper.savePath);
								Object.DestroyImmediate(obj);
							}, helper.cameras);
							break;
						case Format.PNG:
							helper.GetScreenShot(delegate(Texture2D obj) {
								ScreenShot.SavePNG(obj, helper.savePath);
								Object.DestroyImmediate(obj);
							}, helper.cameras);
							break;
						}
					}
				}
			}
		}
	
	}
} // namespace EditorTool
