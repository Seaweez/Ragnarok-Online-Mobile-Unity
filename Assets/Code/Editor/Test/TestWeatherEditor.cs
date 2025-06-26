using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;
using RO.Test;

namespace EditorTool
{
	[CustomEditor(typeof(TestWeather))]
	public class TestWeatherEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying && GUILayout.Button("Change"))
			{
				var testWeather = target as TestWeather;
				testWeather.Apply();
			}
		}
	
	}
} // namespace EditorTool
