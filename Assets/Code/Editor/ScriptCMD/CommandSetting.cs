using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using RO;

namespace EditorTool
{
	public class CommandSetting : ScriptableObject
	{
		public string protobufCmdPath;
		private static CommandSetting _instance;

		public static CommandSetting Instance {
			get {
				#if !SLUA_STANDALONE
				if (_instance == null) {
					var folder = "Assets/Resources/AutoGenerate/";

					_instance = Resources.Load<CommandSetting> ("AutoGenerate/CommandSetting");

					#if UNITY_EDITOR
					if (_instance == null) {
						if (!Directory.Exists (folder)) {
							Directory.CreateDirectory (folder);
						}
						_instance = CommandSetting.CreateInstance<CommandSetting> ();
						AssetDatabase.CreateAsset (_instance, folder + "CommandSetting.asset");
					}
					#endif

				}
				#endif
				return _instance;
			}
		}

		#if UNITY_EDITOR
		[MenuItem ("RO/Commands/Setting")]
		public static void Open ()
		{
			Selection.activeObject = Instance;
		}
		#endif

		private string _GetPath (Object obj)
		{
			var path = AssetDatabase.GetAssetPath (obj);
			if (AssetDatabase.IsValidFolder (path)) {
				return path;
			}
			return null;
		}

		public void CallCmd (string cmd, params string[] args)
		{
			if (File.Exists (cmd)) {
				if (Path.GetExtension (cmd) == ".py") {
					Debug.LogFormat ("<color=yellow>执行脚本{0}</color>", cmd);
					//python
					string cmdWithArgs = cmd;
					if (args != null && args.Length > 0) {
						for (int i = 0; i < args.Length; i++) {
							cmdWithArgs += " " + args [i];
						}
					}
					EditorTool.CommandHelper.ExcutePython (cmdWithArgs);
				}
			} else {
				Debug.LogErrorFormat ("脚本未找到！{0}", cmd);
			}
		}

		public void CallGenProtobuf ()
		{
			string cmd = string.IsNullOrEmpty (protobufCmdPath) ? "../../../server/trunk/server/Proto/client/gen_service_client.py" : protobufCmdPath;
			CallCmd (cmd,Application.dataPath);
		}

	}

	[CustomEditor (typeof(CommandSetting))]
	public class CommandSettingEditor : Editor
	{
		CommandSetting setting;
		// 子弹类型
		public SerializedProperty protobuf;

		private void OnEnable ()
		{
			protobuf = serializedObject.FindProperty ("protobufCmdPath");  
		}

		private void CustomDrawInspector ()
		{
			DrawCmd (protobuf, "protobuf脚本位置:", true);
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();  
			setting = target as CommandSetting;

			EditorGUILayout.BeginVertical ();
			CustomDrawInspector ();
			EditorGUILayout.EndVertical ();
			
			EditorGUILayout.Separator ();
			if (GUILayout.Button ("生成协议")) {
				EditorApplication.delayCall += setting.CallGenProtobuf;
			}
			if (GUI.changed) {  
				EditorUtility.SetDirty (target);  
			}  

			serializedObject.ApplyModifiedProperties ();  
		}

		private void DrawCmd (SerializedProperty sprop, string tip, bool canModify)
		{
			EditorGUILayout.BeginHorizontal ();
			if (canModify) {
				if (GUILayout.Button ("选择", GUILayout.ExpandWidth (false))) {
					string path = EditorUtility.OpenFilePanelWithFilters ("选择脚本", Application.dataPath, new string[] {
						"sh",
						"py"
					});
					sprop.stringValue = path;
					Debug.LogFormat ("选择路径:{0}", path);
					AssetDatabase.SaveAssets ();
				}
			}
			DrawFolderField (tip, string.IsNullOrEmpty (sprop.stringValue) ? "无" : sprop.stringValue);
			EditorGUILayout.EndHorizontal ();
		}

		private void DrawFolderField (string desc, string path)
		{
			GUILayout.Label (desc, GUILayout.ExpandWidth (true));
			GUILayout.Label (path, GUILayout.ExpandWidth (false));
		}
	}
}