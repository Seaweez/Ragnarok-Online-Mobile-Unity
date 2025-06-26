using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using RO;
using RO.Test;
using System;

namespace EditorTool
{
#if RESOURCE_LOAD
    [CustomEditor(typeof(LuaTestStartGame))]
	public class LuaTestStartGameEditor:SJBLuaTestEditor
	{

	}

	[CustomEditor(typeof(SJBLuaTest))]
	public class SJBLuaTestEditor:LuaBehaviourEditor
	{
		SJBLuaTest _test;
		
		public override void OnInspectorGUI ()
		{
			_test = target as SJBLuaTest;
			serializedObject.Update ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("lua"));
			base.OnInspectorGUI ();
			serializedObject.ApplyModifiedProperties ();
			string path = AssetDatabase.GetAssetPath (_test.lua);
			if (string.IsNullOrEmpty (path) == false) {
				int index = path.IndexOf ("Resources/");
				if (index > -1)
					path = path.Remove (0, index).Replace ("Resources/", "").Replace (".txt", "");
				_test.LuaFile = path;
			}
		}
	}
#endif
	[CustomEditor(typeof(LuaUIBehaviour))]
	public class LuaUIBehaviourEditor:LuaBehaviourEditor
	{
		LuaUIBehaviour _test;
		
		public override void OnInspectorGUI ()
		{
			_test = target as LuaUIBehaviour;
			serializedObject.Update ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("lua"));
			base.OnInspectorGUI ();
			serializedObject.ApplyModifiedProperties ();
			string path = AssetDatabase.GetAssetPath (_test.lua);
			if (string.IsNullOrEmpty (path) == false) {
				int index = path.IndexOf ("Resources/");
				if (index > -1)
					path = path.Remove (0, index).Replace ("Resources/", "").Replace (".txt", "");
				_test.LuaFile = path;
			}
			_test.layer = (UILayer)EditorGUILayout.EnumPopup ("层级", _test.layer);
		}

		protected override void TryCreate (Action<string> createHandler= null)
		{
			base.TryCreate (AfterCreate);
		}

		void AfterCreate (string path)
		{
			TextAsset t = AssetDatabase.LoadMainAssetAtPath (path) as TextAsset;
			_test.lua = t;
		}
	}

	[CustomEditor(typeof(LuaBehaviour), true)]
	public class LuaBehaviourEditor:Editor
	{
		static string extensions = ".txt";
		protected LuaBehaviour _lua;

		public override void OnInspectorGUI ()
		{
			_lua = target as LuaBehaviour;
			EditorGUILayout.BeginVertical ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Lua路径:", GUILayout.Width (50));
			string luaFile = EditorGUILayout.TextField (_lua.LuaFile);
			bool changed = false;
			if (luaFile != _lua.LuaFile) {
				_lua.LuaFile = luaFile;
				_lua.LuaFile = _lua.LuaFile.Replace (".",  "/");
				changed = true;
			}
			string[] luas = Collector.Me.luas;
			int index = ArrayUtility.IndexOf (luas, _lua.LuaFile);
			if (index < 0 && string.IsNullOrEmpty (_lua.LuaFile) == false) {
				TryCreate ();
			}
			EditorGUILayout.EndHorizontal ();
			PopupLuas (changed);
			if (GUILayout.Button ("ReLoad Lua Scripts")) {
				Collector.Me.Refresh ();
			}
			EditorGUILayout.EndVertical ();
		}

		virtual protected void TryCreate (Action<string> createHandler= null)
		{
			if (GUILayout.Button ("Create")) {
				string path = ResourceID.ResFolder + _lua.LuaFile + ".txt";
				if (Directory.Exists (Path.GetDirectoryName (path)) == false) {
					Directory.CreateDirectory (Path.GetDirectoryName (path));
				}
				File.Create (path);
				AssetDatabase.Refresh ();
				Collector.Me.Refresh ();
				if (createHandler != null)
					createHandler (path);
			}
		}

		void PopupLuas (bool changed)
		{
			EditorGUILayout.BeginHorizontal ();
			string[] luas = Collector.Me.luas;
			int index = ArrayUtility.IndexOf (luas, _lua.LuaFile);
			int Select = EditorGUILayout.Popup ("Lua Scripts", index, luas);
			if (Select != index) {
				_lua.LuaFile = luas [Select];
			}
			EditorGUILayout.EndHorizontal ();
		}
	
		private class Collector
		{
			public string[] luas{ get; private set; }
			
			public static Collector Me = new Collector ();

			private Collector ()
			{
				Refresh ();
			}
			
			public void Refresh ()
			{
//				List<string> findLuas;
				try {
					luas = Directory.GetFiles (ResourceID.ResFolder + ResourceID.LuaFolder, "*" + extensions, SearchOption.AllDirectories);
					for (int i=0,Num = luas.Length; i<Num; i++) {
						luas [i] = luas [i].Replace (ResourceID.ResFolder, "").Replace (extensions, "");
						EditorUtility.DisplayProgressBar ("Loading Lua Scripts", string.Format ("Loaded: {0}", luas [i]), i / Num);
					}
				} catch (System.Exception e) {
					throw e;
				} finally {
					EditorUtility.ClearProgressBar ();
				}
			}
		}
	}
} // namespace EditorTool
