using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using RO;

static public class CreatNGUIEditor
{
	static public GameObject SelectedRoot ()
	{
		return NGUIEditorTools.SelectedRoot ();
	}
	#region Create

	[MenuItem("GameObject/NGUI/Panel", false, 6)]
	static void AddPanel ()
	{
		UIPanel panel = NGUISettings.AddPanel (SelectedRoot ());
		Selection.activeGameObject = (panel == null) ? NGUIEditorTools.SelectedRoot (true) : panel.gameObject;
	}

	[MenuItem("GameObject/NGUI/Widget &#w", false, 6)]
	static public void AddWidget ()
	{
		GameObject go = NGUIEditorTools.SelectedRoot (true);
		
		if (go != null) {
			Selection.activeGameObject = NGUISettings.AddWidget (go).gameObject;
		} else
			Debug.Log ("You must select a game object first.");
	}

	[MenuItem("GameObject/NGUI/Image &#s", false, 6)]
	static public void AddSprite ()
	{
		GameObject go = NGUIEditorTools.SelectedRoot (true);
		
		if (go != null) {
			Selection.activeGameObject = NGUISettings.AddSprite (go).gameObject;
		} else
			Debug.Log ("You must select a game object first.");
	}
	
	[MenuItem("GameObject/NGUI/Label &#l", false, 6)]
	static public void AddLabel ()
	{
		GameObject go = NGUIEditorTools.SelectedRoot (true);
		
		if (go != null) {
			Selection.activeGameObject = NGUISettings.AddLabel (go).gameObject;
		} else
			Debug.Log ("You must select a game object first.");
	}

	[MenuItem("GameObject/NGUI/Button",false,6)]
	static public void AddButton ()
	{
		GameObject go = SelectedRoot ();
		
		int depth = NGUITools.CalculateNextDepth (go);
		go = NGUITools.AddChild (go);
		go.name = "Button";
			
		UISprite bg = NGUITools.AddWidget<UISprite> (go);
		bg.type = UISprite.Type.Simple;
		bg.name = "Background";
		bg.depth = depth;
		bg.atlas = NGUISettings.atlas;
		bg.spriteName = "button";
		bg.width = 200;
		bg.height = 50;
		bg.MakePixelPerfect ();
			
		if (NGUISettings.ambigiousFont != null) {
			UILabel lbl = NGUITools.AddWidget<UILabel> (go);
			lbl.ambigiousFont = NGUISettings.ambigiousFont;
			lbl.text = go.name;
			lbl.AssumeNaturalSize ();
		}
			
		// Add a collider
		NGUITools.AddWidgetCollider (go);
			
		// Add the scripts
		go.AddComponent<UIButton> ().tweenTarget = bg.gameObject;
		go.AddComponent<UIPlaySound> ();
			
		Selection.activeGameObject = go;
	}

	[MenuItem("GameObject/NGUI/Texture &#t", false, 6)]
	static public void AddTexture ()
	{
		GameObject go = NGUIEditorTools.SelectedRoot (true);
		
		if (go != null) {
			Selection.activeGameObject = NGUISettings.AddTexture (go).gameObject;
		} else
			Debug.Log ("You must select a game object first.");
	}

	[MenuItem("GameObject/NGUI/Scroll View", false, 6)]
	static void AddScrollView ()
	{
		UIPanel panel = NGUISettings.AddPanel (SelectedRoot ());
		if (panel == null)
			panel = NGUIEditorTools.SelectedRoot (true).GetComponent<UIPanel> ();
		panel.clipping = UIDrawCall.Clipping.SoftClip;
		panel.name = "Scroll View";
		panel.gameObject.AddComponent<UIScrollView> ();
		Selection.activeGameObject = panel.gameObject;
	}

    [MenuItem("GameObject/NGUI/RO Scroll View", false, 6)]
    static void AddROScrollView()
    {
        UIPanel panel = NGUISettings.AddPanel(SelectedRoot());
        if (panel == null)
            panel = NGUIEditorTools.SelectedRoot(true).GetComponent<UIPanel>();
        panel.clipping = UIDrawCall.Clipping.SoftClip;
        panel.name = "RO Scroll View";
        panel.gameObject.AddComponent<ROUIScrollView>();
        Selection.activeGameObject = panel.gameObject;
    }
	
	[MenuItem("GameObject/NGUI/Grid", false, 6)]
	static void AddGrid ()
	{
		Add<UIGrid> ();
	}
	
	static T Add<T> () where T : MonoBehaviour
	{
		T t = NGUITools.AddChild<T> (SelectedRoot ());
		Selection.activeGameObject = t.gameObject;
		return t;
	}
	
	[MenuItem("NGUI/Create/2D UI", true)]
	[MenuItem("Assets/NGUI/Create 2D UI", true, 1)]
	static bool Create2Da ()
	{
		if (UIRoot.list.Count == 0 || UICamera.list.size == 0)
			return true;
		foreach (UICamera c in UICamera.list)
			#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
			if (NGUITools.GetActive(c) && c.camera.isOrthoGraphic)
				#else
			if (NGUITools.GetActive (c) && c.GetComponent<Camera> ().orthographic)
					#endif
				return false;
		return true;
	}
	
	[MenuItem("NGUI/Create/3D UI", false, 6)]
	[MenuItem("Assets/NGUI/Create 3D UI", false, 1)]
	static void Create3D ()
	{
		UICreateNewUIWizard.CreateNewUI (UICreateNewUIWizard.CameraType.Advanced3D);
	}
	
	[MenuItem("NGUI/Create/3D UI", true)]
	[MenuItem("Assets/NGUI/Create 3D UI", true, 1)]
	static bool Create3Da ()
	{
		if (UIRoot.list.Count == 0 || UICamera.list.size == 0)
			return true;
		foreach (UICamera c in UICamera.list)
			#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
			if (NGUITools.GetActive(c) && !c.camera.isOrthoGraphic)
				#else
			if (NGUITools.GetActive (c) && !c.GetComponent<Camera> ().orthographic)
					#endif
				return false;
		return true;
	}
	
	#endregion
}
