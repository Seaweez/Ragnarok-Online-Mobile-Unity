//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UITextures.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UITextureEx), true)]
public class UITextureExInspector : UITextureInspector
{
	UITexture tex;
	
	protected override void OnEnable ()
	{
		base.OnEnable();
		tex = target as UITexture;
	}

	protected override bool ShouldDrawProperties ()
	{
		if (target == null) return false;
		SerializedProperty sp = NGUIEditorTools.DrawProperty("Texture", serializedObject, "mTexture");
		NGUIEditorTools.DrawProperty("Material", serializedObject, "mMat");
		
		if (sp != null) NGUISettings.texture = sp.objectReferenceValue as Texture;
		
		if (tex != null && (tex.material == null || serializedObject.isEditingMultipleObjects))
		{
			NGUIEditorTools.DrawProperty("Shader", serializedObject, "mShader");
		}
		
		EditorGUI.BeginDisabledGroup(tex == null || tex.mainTexture == null || serializedObject.isEditingMultipleObjects);
		
//		NGUIEditorTools.DrawRectProperty("UV Rect", serializedObject, "mRect");
		
		sp = serializedObject.FindProperty("mFixedAspect");
		bool before = sp.boolValue;
		NGUIEditorTools.DrawProperty("Fixed Aspect", sp);
		if (sp.boolValue != before) (target as UIWidget).drawRegion = new Vector4(0f, 0f, 1f, 1f);
		
		if (sp.boolValue)
		{
			EditorGUILayout.HelpBox("Note that Fixed Aspect mode is not compatible with Draw Region modifications done by sliders and progress bars.", MessageType.Info);
		}
		
		EditorGUI.EndDisabledGroup();
		return true;
	}

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
