using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorTool;
using System.Collections.Generic;

public class SceneCheckWindow : EditorWindow
{
	static Vector2 m_ScrollPos = Vector2.zero;
	static SceneCheckInspectorWindow _InspectorWindow = null;
	public static SceneCheckWindow window = null;

	public SceneCheckWindow ()
	{
		window = this;
	}

	[MenuItem ("RO/CheckAssets/检查场景Mesh")]
	public static void Open ()
	{
		ShowWindow ();
	}

	static void ShowWindow ()
	{
		window = GetWindow<SceneCheckWindow> ();
		window.titleContent.text = "场景列表";
		window.position.size.Set (1000f, 800f);
	}

	void Init ()
	{
		InitDocker ();
	}

	void OnGUI ()
	{
		m_ScrollPos = EditorGUILayout.BeginScrollView (m_ScrollPos);
		var drops = DrawDropZone ();
		if (drops != null && drops.Count > 0) {
			SceneAsset scene = null;
			try {
				for (int i = 0; i < drops.Count; i++) {
					EditorUtility.DisplayProgressBar ("检查场景", string.Format ("{0}/{1}", i + 1, drops.Count), (float)(i + 1) / drops.Count);
					scene = drops [i] as SceneAsset;
					if (scene) {
						SceneCheckEditor.HandleScene (AssetDatabase.GetAssetPath (scene));
					}
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			} finally {
				EditorUtility.ClearProgressBar ();
			}
		}
		EditorGUILayout.EndScrollView ();
		if (GUILayout.Button ("检测所有BuildSettings场景"))
			CheckBuildSettingScenes ();
	}

	void _DrawDropLabel()
	{
		GUILayout.FlexibleSpace ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label ("拖动场景到这来");
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.FlexibleSpace ();
	}

	List<Object> DrawDropZone ()
	{
		if (DragAndDrop.objectReferences.Length == 0) {
			if (SceneCheckEditor.Cells.Count == 0)
				_DrawDropLabel ();
			else
				SceneCheckEditor.DrawAllTitle ();
		} else
			_DrawDropLabel ();

		Rect rect = new Rect (0, 0, 200, Screen.height);
		if (rect.Contains (Event.current.mousePosition)) {
			switch (Event.current.type) {
			case EventType.DragUpdated:
				if (IsValidDragPayload ())
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				else
					DragAndDrop.visualMode = DragAndDropVisualMode.None;
				break;

			case EventType.DragExited:
				var droppedObjectsList = new List<Object> ();
				for (int i = 0; i < DragAndDrop.objectReferences.Length; ++i) {
					var type = DragAndDrop.objectReferences [i].GetType ();
					if (type == typeof(SceneAsset)) {
						droppedObjectsList.Add (DragAndDrop.objectReferences [i]);
					} else if (type == typeof(UnityEditor.DefaultAsset) && System.IO.Directory.Exists (DragAndDrop.paths [i])) {
						droppedObjectsList.AddRange (AddScenesInPath (DragAndDrop.paths [i]));
					}
				}
				Repaint ();
				return droppedObjectsList;
			}
		}
		return null;
	}

	// recursively find textures in path
	List<Object> AddScenesInPath (string path)
	{
		List<Object> localObjects = new List<Object> ();
		foreach (var q in System.IO.Directory.GetFiles(path)) {
			string f = q.Replace ('\\', '/');
			System.IO.FileInfo fi = new System.IO.FileInfo (f);
			if (fi.Extension.ToLower () == ".meta" || fi.FullName.ToLower().Contains("config/"))
				continue;

			Object obj = AssetDatabase.LoadAssetAtPath (f, typeof(SceneAsset));
			if (obj != null)
				localObjects.Add (obj);
		}
		foreach (var q in System.IO.Directory.GetDirectories(path)) {
			string d = q.Replace ('\\', '/');
			localObjects.AddRange (AddScenesInPath (d));
		}

		return localObjects;
	}

	bool IsValidDragPayload ()
	{
		int idx = 0;
		foreach (var v in DragAndDrop.objectReferences) {
			var type = v.GetType ();

			if (type == typeof(SceneAsset)) {
				return true;
			} else if (type == typeof(UnityEditor.DefaultAsset) && System.IO.Directory.Exists (DragAndDrop.paths [idx])) {
				return true;
			}
			++idx;
		}
		return false;
	}

	void CheckBuildSettingScenes ()
	{
		SceneCheckEditor.ScanAllBuildSettingScenes ();
	}

	private void InitDocker ()
	{
		if (_InspectorWindow == null) {
			_InspectorWindow = GetWindow<SceneCheckInspectorWindow> ("场景信息");
			Rect r = new Rect (0f, 0f, 200f, position.height);
			EDocker.Dock (window, _InspectorWindow, r, 1);
		}
	}

	void Update ()
	{
		Repaint ();
	}

	void OnDisable ()
	{
		window = null;
	}

	void OnEnable ()
	{
		ShowWindow ();
		Init ();
	}
}

