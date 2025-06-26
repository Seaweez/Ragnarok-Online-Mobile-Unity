using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using RO;

static public class CreateNGUIClickTween
{
	static public GameObject SelectedRoot ()
	{
		return NGUIEditorTools.SelectedRoot ();
	}

	#region Create

	[MenuItem("GameObject/NguiClick/ClickScale", false, 6)]
	static void AddNguiClickScale ()
	{
		GameObject go = Selection.activeGameObject;
		if (go != null) {
			Collider c = go.GetComponent<Collider> ();
			if (c != null) {
				UIButtonColor ubc = go.GetComponent<UIButtonColor> ();
				if (ubc != null) {
					ubc.hover = Color.white;
					ubc.pressed = Color.white;
				}
				UIButtonScale ubs = go.GetComponent<UIButtonScale> ();
				if (ubs == null) {
					ubs = go.AddComponent<UIButtonScale> ();
				}
				ubs.duration = 0.15f;
				ubs.hover = Vector3.one;
				ubs.pressed = Vector3.one * 0.95f;
			} else {
				Debug.LogErrorFormat ("{0} has no collider", go.name);
			}
		}
	}

	#endregion
}
