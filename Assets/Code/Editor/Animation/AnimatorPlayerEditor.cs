using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(AnimatorPlayer))]
	public class AnimatorPlayerEditor : Editor
	{
		private AnimatorPlayer.Params p = new AnimatorPlayer.Params();

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();
			if (!Application.isPlaying)
			{
				return;
			}
			p.stateName = EditorGUILayout.TextField("Animation Name", p.stateName);

			EditorGUI.BeginChangeCheck();
			p.normalizedTime = EditorGUILayout.Slider("Progress", p.normalizedTime, 0f, 1f);
			
			if (EditorGUI.EndChangeCheck())
			{
				var player = target as AnimatorPlayer;
				player.Play(p);
			}
		}
	
	}
} // namespace EditorTool
