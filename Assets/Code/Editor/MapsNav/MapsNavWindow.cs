using UnityEngine;
using UnityEditor;
using SLua;
using System;

public class MapsNavWindow : EditorWindow {
	private static MapsNavWindow window;

	private static ScriptableObject objData;
	private static SerializedObject serObjData;
	private int OriginMapID
	{
		get
		{
			return serObjData.FindProperty ("originMapID").intValue;
		}
		set
		{
			serObjData.FindProperty ("originMapID").intValue = value;
			serObjData.ApplyModifiedProperties ();
		}
	}
	private string OriginMapName
	{
		get
		{
			return serObjData.FindProperty ("originMapName").stringValue;
		}
	}
	private int TargetMapID
	{
		get
		{
			return serObjData.FindProperty ("targetMapID").intValue;
		}
		set
		{
			serObjData.FindProperty ("targetMapID").intValue = value;
			serObjData.ApplyModifiedProperties ();
		}
	}
	private string TargetMapName
	{
		get
		{
			return serObjData.FindProperty ("targetMapName").stringValue;
		}
	}
	private string Path
	{
		get
		{
			return serObjData.FindProperty ("path").stringValue;
		}
	}
	private int ConfMemorySize
	{
		get
		{
			return serObjData.FindProperty ("confMemorySize").intValue;
		}
	}

	private static AllMapsNavData allMapsNavData;
	private static AllMapsNavData.MapsNavDataForEditor mapsNavData;

	[MenuItem("RO/WorldPathFinding/MapTeleportEditor")]
	static void Init()
	{
		if (window == null) {
			window = EditorWindow.GetWindow<MapsNavWindow> ();
		}
		window.Show ();

		CreateObj ();
		CreateMapsNavData ();
	}

	void OnFocus()
	{
		if (Application.isPlaying) {
			CreateObj ();
			FireCallbackSetData (true);
			Repaint ();
		} else {
			CreateMapsNavData ();
			Repaint ();
		}
	}

	void OnGUI()
	{
		if (Application.isPlaying) {
			if (objData != null) {
				GUILayout.Label ("MapTeleport.txt Memory: " + ConfMemorySize + "kb");

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("Ori Map");
				string strOriginMapID = EditorGUILayout.TextField (OriginMapID.ToString ());
				int originMapID = 0;
				if (int.TryParse (strOriginMapID, out originMapID)) {
					OriginMapID = originMapID;
				}
				GUILayout.Label (OriginMapName);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("Tar Map");
				string strTargetMapID = EditorGUILayout.TextField (TargetMapID.ToString ());
				int targetMapID = 0;
				if (int.TryParse (strTargetMapID, out targetMapID)) {
					TargetMapID = targetMapID;
				}
				GUILayout.Label (TargetMapName);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Go")) {
					FireCallbackSetData (false);
					Repaint ();
				}
				if (GUILayout.Button ("Back")) {
					int temp = OriginMapID;
					OriginMapID = TargetMapID;
					TargetMapID = temp;
					FireCallbackSetData (false);
					Repaint ();
				}
				EditorGUILayout.EndHorizontal ();

				string path = Path;
				if (!string.IsNullOrEmpty (path)) {
					GUILayout.Label (Path);
				}
			}
		} else {
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Ori Map");
			string strOriginMapID = EditorGUILayout.TextField (mapsNavData.originMapID.ToString ());
			int originMapID = 0;
			if (int.TryParse (strOriginMapID, out originMapID)) {
				mapsNavData.originMapID = originMapID;
			}
			GUILayout.Label (mapsNavData.originMapName);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Tar Map");
			string strTargetMapID = EditorGUILayout.TextField (mapsNavData.targetMapID.ToString ());
			int targetMapID = 0;
			if (int.TryParse (strTargetMapID, out targetMapID)) {
				mapsNavData.targetMapID = targetMapID;
			}
			GUILayout.Label (mapsNavData.targetMapName);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Go")) {
				FindMapsNavData (mapsNavData.originMapID, mapsNavData.targetMapID);
				Repaint ();
			}
			if (GUILayout.Button ("Back")) {
				int temp = mapsNavData.originMapID;
				mapsNavData.originMapID = mapsNavData.targetMapID;
				mapsNavData.targetMapID = temp;
				FindMapsNavData (mapsNavData.originMapID, mapsNavData.targetMapID);
				Repaint ();
			}
			EditorGUILayout.EndHorizontal ();

			GUILayout.Label (mapsNavData.path);
		}
	}

	void OnDestroy()
	{
		if (objData != null) {
			serObjData.Dispose ();
			serObjData = null;
			ScriptableObject.DestroyImmediate (objData);
			objData = null;
		}
		mapsNavData = null;
		allMapsNavData = null;
	}

	private static void CreateObj()
	{
		if (objData == null) {
			objData = ScriptableObject.CreateInstance ("MapsNavData");
			serObjData = new UnityEditor.SerializedObject (objData);
		}
	}

	private void FireCallbackSetData(bool is_only_confmemorysize)
	{
		objData.GetType ().GetMethod ("FireCallbackSetData").Invoke (objData, new object[]{is_only_confmemorysize});
		serObjData.Update ();
	}

	private static void CreateMapsNavData()
	{
		if (allMapsNavData == null) {
			allMapsNavData = AllMapsNavData.Deserialize(Application.dataPath + "/../../../client-export/MapsNavData.xml");
		}
		if (mapsNavData == null) {
			mapsNavData = new AllMapsNavData.MapsNavDataForEditor ();
		}
	}

	private void FindMapsNavData(int origin_map_id, int target_map_id)
	{
		AllMapsNavData.MapsNavDataForEditor[] data = allMapsNavData.data;
		for (int i = 0; i < data.Length; i++) {
			AllMapsNavData.MapsNavDataForEditor tempMapsNavData = data [i];
			if (tempMapsNavData.originMapID == origin_map_id && tempMapsNavData.targetMapID == target_map_id) {
				mapsNavData.originMapName = tempMapsNavData.originMapName;
				mapsNavData.targetMapName = tempMapsNavData.targetMapName;
				mapsNavData.path = tempMapsNavData.path;
				return;
			}
		}
		mapsNavData.originMapName = null;
		mapsNavData.targetMapName = null;
		mapsNavData.path = null;
	}
}
