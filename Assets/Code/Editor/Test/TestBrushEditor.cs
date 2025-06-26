using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;
using RO.Test;

namespace EditorTool
{
	[CustomEditor(typeof(TestBrush))]
	public class TestBrushEditor : Editor
	{
		enum Format
		{
			JPG,
			PNG
		}
		private Format format = Format.JPG;
		private TextureFormat textureFormat = TextureFormat.ARGB32;

		private void Save(TestBrush brush, string path, System.Action<Texture2D, string> saveFunc)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			var canvas = brush.canvas;
			var oldRT = RenderTexture.active;
			RenderTexture.active = canvas;

			var rect = new Rect(0, 0, canvas.width, canvas.height);
			var texture = new Texture2D((int)rect.width, (int)rect.height, textureFormat, false);
			texture.ReadPixels(rect, 0, 0);
			texture.Apply();

			RenderTexture.active = oldRT;

			saveFunc(texture, path);
			Texture2D.DestroyImmediate(texture);
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying)
			{
				var brush = target as TestBrush;

				EditorGUILayout.Separator();
				if (GUILayout.Button("Clear"))
				{
					brush.Clear();
				}

				EditorGUILayout.Separator();
				EditorGUI.BeginDisabledGroup(false);
				EditorGUILayout.ObjectField("Canvas", brush.canvas, typeof(RenderTexture), false);
				EditorGUI.EndDisabledGroup();
				if (GUILayout.Button("Rebuild Canvas"))
				{
					brush.RebuildCanvas(false);
				}

				EditorGUILayout.Separator();
				format = (Format)EditorGUILayout.EnumPopup("Format", format);
				textureFormat = (TextureFormat)EditorGUILayout.EnumPopup("Texture Format", textureFormat);
				if (GUILayout.Button("Captrue"))
				{
					switch (format)
					{
					case Format.JPG:
						Save (brush, EditorUtility.SaveFilePanel("Save Paint", "", "Paint", "jpg"), ScreenShot.SaveJPG);
						break;
					case Format.PNG:
						Save (brush, EditorUtility.SaveFilePanel("Save Paint", "", "Paint", "png"), ScreenShot.SavePNG);
						break;
					}
				}

			}
		}
	
	}
} // namespace EditorTool
