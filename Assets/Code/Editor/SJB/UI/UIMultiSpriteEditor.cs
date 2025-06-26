using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(UIMultiSprite))]
	public class UIMultiSpriteEditor:UISpriteInspector
	{
		protected override bool ShouldDrawProperties ()
		{
			GUILayout.BeginHorizontal ();
			if (NGUIEditorTools.DrawPrefixButton ("Atlas"))
				ComponentSelector.Show<UIAtlas> (OnSelectAtlas);
			SerializedProperty atlas = NGUIEditorTools.DrawProperty ("", serializedObject, "mAtlas", GUILayout.MinWidth (20f));
			
			if (GUILayout.Button ("Edit", GUILayout.Width (40f))) {
				if (atlas != null) {
					UIAtlas atl = atlas.objectReferenceValue as UIAtlas;
					NGUISettings.atlas = atl;
					NGUIEditorTools.Select (atl.gameObject);
				}
			}
			GUILayout.EndHorizontal ();
			
			SerializedProperty sp = serializedObject.FindProperty ("mSpriteName");
			NGUIEditorTools.DrawAdvancedSpriteField (atlas.objectReferenceValue as UIAtlas, sp.stringValue, SelectSprite, false);
			return true;
		}

		void SelectSprite (string spriteName)
		{
			serializedObject.Update ();
			SerializedProperty sp = serializedObject.FindProperty ("mSpriteName");
			sp.stringValue = spriteName;
			serializedObject.ApplyModifiedProperties ();
			NGUITools.SetDirty (serializedObject.targetObject);
			NGUISettings.selectedSprite = spriteName;
		}

		void OnSelectAtlas (Object obj)
		{
			serializedObject.Update ();
			SerializedProperty sp = serializedObject.FindProperty ("mAtlas");
			sp.objectReferenceValue = obj;
			serializedObject.ApplyModifiedProperties ();
			NGUITools.SetDirty (serializedObject.targetObject);
			NGUISettings.atlas = obj as UIAtlas;
		}

		protected override void DrawCustomProperties ()
		{
			base.DrawCustomProperties ();
		
			if (NGUIEditorTools.DrawHeader ("States", "States", false, true)) {
				NGUIEditorTools.BeginContents (true);
				EditorGUI.BeginDisabledGroup (serializedObject.isEditingMultipleObjects);
				{
					UIMultiSprite sprite = target as UIMultiSprite;
					SerializedObject sobj = new SerializedObject (sprite);
					sobj.Update ();
					SerializedProperty splst = sobj.FindProperty ("_stateSpriteList");
					for (int i = 0; i<splst.arraySize; i++) {
						int k = i;
						GUILayout.BeginHorizontal ();
						{
							GUILayout.Label ("State " + k.ToString () + ":");
							SerializedProperty atlasMapSprite = splst.GetArrayElementAtIndex (k);
							SerializedProperty tempatlas = atlasMapSprite.FindPropertyRelative ("atlas");
							SerializedProperty tempspriteName = atlasMapSprite.FindPropertyRelative ("spriteName");
							if (GUILayout.Button (((UIAtlas)tempatlas.objectReferenceValue).name)) {
								ComponentSelector.Show<UIAtlas> ((o) => {
									sobj.Update ();
									tempatlas.objectReferenceValue = o;

									UIAtlas oatlas = o as UIAtlas;
									UISpriteData s = oatlas.GetSprite (tempspriteName.stringValue);
									if (s == null) {
										tempspriteName.stringValue = "";
										NGUISettings.selectedSprite = "";
									}
									NGUISettings.atlas = o as UIAtlas;

									sobj.ApplyModifiedProperties ();
								});
							}
							if (GUILayout.Button (tempspriteName.stringValue)) {
								SpriteSelector.Show ((sname) => {
									sobj.Update ();
									NGUISettings.selectedSprite = sname.ToString ();
									tempspriteName.stringValue = sname.ToString ();
									sobj.ApplyModifiedProperties ();
								});
							}
							if (GUILayout.Button ("Remove", GUILayout.Width (76f))) {
								sobj.Update ();
								splst.DeleteArrayElementAtIndex (k);
								sobj.ApplyModifiedProperties ();
							}
						}
						GUILayout.EndHorizontal ();
					}
					if (GUILayout.Button ("Add", GUILayout.Width (76f))) {
						ComponentSelector.Show<UIAtlas> ((o) => {
							sobj.Update ();
							int k = splst.arraySize;
							splst.InsertArrayElementAtIndex (k);
							SerializedProperty sp = splst.GetArrayElementAtIndex (k);
							SerializedProperty sa = sp.FindPropertyRelative ("atlas");
							sa.objectReferenceValue = o;
							NGUISettings.atlas = o as UIAtlas;
							sobj.ApplyModifiedProperties ();
						});
					}
				}
				EditorGUI.EndDisabledGroup ();
				NGUIEditorTools.EndContents ();
			}
		}

		public override void OnPreviewGUI (Rect rect, GUIStyle background)
		{
			UIMultiSprite sprite = target as UIMultiSprite;
			if (sprite == null || !sprite.isValid)
				return;
			
			Texture2D tex = sprite.mainTexture as Texture2D;
			if (tex == null)
				return;
			
			UISpriteData sd = sprite.atlas.GetSprite (sprite.spriteName);
			NGUIEditorTools.DrawSprite (tex, rect, sd, sprite.color);
		}
	}
} // namespace EditorTool
