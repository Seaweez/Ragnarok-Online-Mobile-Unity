//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UISprites.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UISpriteEx), true)]
public class UISpriteExInspector : UISpriteInspector
{
	protected override void DrawCustomProperties ()
	{
		GUILayout.Space(6f);
		
		NGUIEditorTools.DrawProperty("Type", serializedObject, "typeEx", GUILayout.MinWidth(20f));
		NGUIEditorTools.DrawProperty("Anchor", serializedObject, "anchor", GUILayout.MinWidth(20f));

		if (NGUISettings.unifiedTransform)
		{
			DrawColor(serializedObject, mWidget);
		}
		else DrawInspectorProperties(serializedObject, mWidget, true);
	}
}
