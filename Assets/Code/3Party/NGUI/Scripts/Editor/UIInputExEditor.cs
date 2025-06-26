using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
#if UNITY_3_5
[CustomEditor(typeof(UIInputEx))]
#else
[CustomEditor(typeof(UIInputEx), true)]
#endif
public class UIInputExEditor : UIInputEditor {

	private bool ignoreFoldout = false;

	private void OnInspectorGUI_Ignore(UIInputEx input, UIInputEx.Ignore i, string label)
	{
		var oldIgnore = UIInputEx.CheckIgnore(i, input.ignores);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(20);
		bool newIgnore = EditorGUILayout.ToggleLeft(label, oldIgnore);
		EditorGUILayout.EndHorizontal();

		if (newIgnore != oldIgnore)
		{
			if (newIgnore)
			{
				input.AddIgnore(i);
			}
			else
			{
				input.RemoveIgnore(i);
			}
		}
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		var input = target as UIInputEx;
		input.DBCCaseLimit = EditorGUILayout.IntField("DBC Case Limit", input.DBCCaseLimit);

		ignoreFoldout = EditorGUILayout.Foldout(ignoreFoldout, "Ignore Character");
		if (ignoreFoldout)
		{
			OnInspectorGUI_Ignore(input, UIInputEx.Ignore.Emoji, "Emoji");
			OnInspectorGUI_Ignore(input, UIInputEx.Ignore.Punctuation, "Punctuation");
			OnInspectorGUI_Ignore(input, UIInputEx.Ignore.Symbol, "Symbol");
			OnInspectorGUI_Ignore(input, UIInputEx.Ignore.Separator, "Separator");
			OnInspectorGUI_Ignore(input, UIInputEx.Ignore.Custom, "Custom");
		}
	}

}
