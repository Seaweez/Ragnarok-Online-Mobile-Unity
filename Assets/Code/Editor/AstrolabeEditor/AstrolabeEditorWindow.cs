using RO;
using SLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EditorTool
{
	public class AstrolabeEditorWindow : EditorWindow
	{
		static string _ScenePath = "Assets/DevelopScene/AstrolabeEditorScene/AstrolabeEditor.unity";
		ETopToolBar toolbar;
		int _AstrolabeID = 0;
		int _AstrolabeLv = 3;
		Vector3 _AstrolabePos = Vector3.zero;
		static List<string> _VisibleGizmosScript = new List<string> () {
			"UISprite",
			"UITexture",
			"UIPanel",
			"ShowGrid"
		};
		static string _TableClassPath = "Assets/Resources/Script/Config_Property_ZhiYe_ShuXing/Table_Class";
		static string _TableRunePath = "Assets/Resources/Script/Config_Skill_JiNeng/";
		static string TableAstrolabeName = "Table_Astrolabe";
		static string _ClientTableAstrolabePath = "Assets/Resources/Script/Refactory/Config/Astrolabe/" + TableAstrolabeName;
		static string TableAstrolabeUIName = "Table_AstrolabeUI";
		static string _ClientTableAstrolabeUIPath = "Assets/Resources/Script/Refactory/Config/Astrolabe/" + TableAstrolabeUIName;
		static string _ServerTableAstrolabePath;
		static string ClientTableExtension = ".txt";
		static string ServerTableExtension = ".lua";
		static string _AstrolabeUIElementPath = "Assets/DevelopScene/AstrolabeEditorScene/astrolabeUIElement.prefab";

		static string rune_filePrefix = "Table_Rune_";

		int m_nSelectedProfessionIdx = 1;
		int m_nSelectedUIIdx = 0;
		string[] m_arrModeStr = new string[] {
			AstrolabeUIElement.AstrolabeUIElementStyle.ditu2.ToString (),
			AstrolabeUIElement.AstrolabeUIElementStyle.ditu3.ToString (),
			AstrolabeUIElement.AstrolabeUIElementStyle.ditu4.ToString (),
			AstrolabeUIElement.AstrolabeUIElementStyle.ditu5.ToString ()
		};
		public static AstrolabeEditorWindow window;

		[MenuItem ("Window/星图编辑器")]
		public static void OpenWindow ()
		{
			window = GetWindow<AstrolabeEditorWindow> ();
			window.titleContent = new GUIContent ("星图编辑器");
			window.Init ();
			window.OpenScene (_ScenePath);
		}

		void Init ()
		{
			//if (window == null)
			//    window = GetWindow<AstrolabeEditorWindow>();
			toolbar = InitToolbar ();
			SetSelectionChangeAction (OnSelectedChanged);

			Init_ClassMenuTxt ();
            _ServerTableAstrolabePath = Application.dataPath + "/../../../client-export/Astrolabe";
        }

		List<string> class_menutxt = new List<string> ();

		void Init_ClassMenuTxt ()
		{
			class_menutxt.Clear ();

			LuaTable class_table = GetTableWorker (_TableClassPath, ClientTableExtension);

			UnityEngine.Object[] objs = AssetManager.getAllAssets (_TableRunePath);
			foreach (UnityEngine.Object obj in objs) {
				if (obj.name.StartsWith (rune_filePrefix)) {
					int classid = int.Parse (obj.name.Substring (rune_filePrefix.Length));
					LuaTable cfg = class_table [classid] as LuaTable;
					string name = LuaWorker.GetFieldString (cfg, "NameZh");
					class_menutxt.Add (name + "_" + classid);
				}
			}
		}

		void Update ()
		{
			Repaint ();   
		}

		void OnGUI ()
		{
			toolbar.Draw ();
			DrawContent ();
		}

		void OnEnable ()
		{
			Init ();
		}

		ETopToolBar InitToolbar ()
		{
			ETopToolBar bar = new ETopToolBar ();
			bar.AddToDrawQueue (new EButton ("加载星图", OnLoad));
			bar.AddToDrawQueue (new EButton ("保存当前配置", OnSave));
			bar.AddToDrawQueue (new EButton ("切换场景", OnOpenScene));
			bar.InsertFlexibleSpace ();
			bar.AddToDrawQueue (new EToggle ("线框", OnToggleGizmosVisible));
			return bar;
		}

		void DrawContent ()
		{
			EditorGUILayout.BeginHorizontal (EditorStyles.toolbar);
			//初始化Active 职业类型按钮

			if (GUILayout.Button (new GUIContent ((class_menutxt [m_nSelectedUIIdx])), EditorStyles.toolbarDropDown)) {
				Rect m_ActiveRect = EditorGUILayout.GetControlRect ();
				m_ActiveRect.y += EditorStyles.toolbarDropDown.fixedHeight;
				int count = class_menutxt.Count;
				GUIContent[] contents = new GUIContent[count];
				for (int i = 0; i < count; i++) {
					contents [i] = new GUIContent ((class_menutxt [i]).ToString ());
				}
				EditorUtility.DisplayCustomMenu (m_ActiveRect, contents, m_nSelectedProfessionIdx - 1, OnSelectProfessionClick, null);
			}
			EditorGUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("星盘ID:");
			_AstrolabeID = EditorGUILayout.IntField (_AstrolabeID);
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("星盘解锁条件:");
			GUILayout.Label ("暂未配置");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("星盘维度:");
			_AstrolabeLv = EditorGUILayout.IntField (_AstrolabeLv);
			GUILayout.EndHorizontal ();

			_AstrolabePos = EditorGUILayout.Vector3Field ("星盘中心点位置", _AstrolabePos);
			if (GUILayout.Button ("生成星盘")) {
				AstrolabeManager.instance.CreateAstrolabel (_AstrolabeID, _AstrolabeLv, _AstrolabePos);
			}

			EditorGUILayout.BeginHorizontal (EditorStyles.toolbar);
			//初始化Active UI元素按钮
			if (GUILayout.Button (new GUIContent (m_arrModeStr [m_nSelectedUIIdx]), EditorStyles.toolbarDropDown)) {
				Rect m_ActiveRect = EditorGUILayout.GetControlRect ();
				m_ActiveRect.y += EditorStyles.toolbarDropDown.fixedHeight;
				GUIContent[] contents = new GUIContent[m_arrModeStr.Length];
				for (int i = 0; i < m_arrModeStr.Length; i++) {
					contents [i] = new GUIContent (m_arrModeStr [i]);
				}
				EditorUtility.DisplayCustomMenu (m_ActiveRect, contents, m_nSelectedUIIdx, OnSelectUIElementClick, null);
			}
			if (GUILayout.Button ("生成UI元素", EditorStyles.toolbarButton)) {
				OnCreateUIElement ();
			}
			EditorGUILayout.EndHorizontal ();


            GameObject activeObj = Selection.activeGameObject;
            if (!activeObj) return;
            NGUIEditorTools.DrawSeparator();
            if (activeObj.GetComponent<Star>())
            {
                if (AstrolabeManager.instance.chooseList.Count == 1)
                {
                    if (GUILayout.Button("删除点"))
                        AstrolabeManager.instance.HideStar();
                }
                else if (AstrolabeManager.instance.chooseList.Count == 2)
                {
                    if (GUILayout.Button("删除线"))
                        AstrolabeManager.instance.UnLink2Star();
                    if (GUILayout.Button("连线"))
                        AstrolabeManager.instance.Link2Star();
                }
            }
            else if (activeObj.GetComponent<AstrolabePath>())
            {
                if (GUILayout.Button("删除线"))
                {
                    AstrolabePath path = Selection.activeGameObject.GetComponent<AstrolabePath>();
                    path.UnLink();
                }
            }
            else if (activeObj.GetComponent<Astrolabe>())
            {
                if (GUILayout.Button("删除星盘"))
                {
                    Astrolabe _Astrolabe = activeObj.GetComponent<Astrolabe>();
                    if (_Astrolabe != null)
                    {
                        _Astrolabe.UnLinkOuterConnects();
                        AstrolabeManager.instance.RemoveAstrolabe(_Astrolabe);
                        GameObject.DestroyImmediate(_Astrolabe.gameObject);
                    }
                }
            }
            else if (activeObj.GetComponent<AstrolabeUIElement>())
            {
                if (GUILayout.Button("删除UI元素"))
                    AstrolabeManager.instance.DeleteUIElement(activeObj.GetComponent<AstrolabeUIElement>());
            }
        }

		void OnLoad (EComponentBase btn, bool IsPress)
		{
			if (IsPress) {  
				OpenScene (_ScenePath);
				LuaTable table = GetTableWorker (_ClientTableAstrolabePath, ClientTableExtension);
				AstrolabeManager.instance.ParseAstrolabeTable (table);
				LuaTable uitable = GetTableWorker (_ClientTableAstrolabeUIPath, ClientTableExtension);
				ParseAstrolabeUITable (uitable);

				UnityEngine.Object[] objs = AssetManager.getAllAssets (_TableRunePath);
				foreach (UnityEngine.Object obj in objs) {
					if (obj.name.StartsWith (rune_filePrefix)) {
						int classid = int.Parse (obj.name.Substring (rune_filePrefix.Length));

						LuaTable styletable = GetTableWorker (_TableRunePath + obj.name, ClientTableExtension);
						Dictionary<int, int> _SubStarStyleMap = ParseStarAttrTable (styletable);
						AstrolabeManager.instance.UpdateStarStyleMap (classid, _SubStarStyleMap);
					}
				}
				ApplyProfessionStarStyle ();
			}
		}

		private void OnSelectUIElementClick (object userData, string[] options, int selected)
		{
			m_nSelectedUIIdx = selected;
		}

		private void OnSelectProfessionClick (object userData, string[] options, int selected)
		{
			m_nSelectedProfessionIdx = selected + 1;
			ApplyProfessionStarStyle ();
		}

		void ApplyProfessionStarStyle ()
		{
			string name = class_menutxt [m_nSelectedUIIdx];
			string[] name_splits = name.Split ('_');

			AstrolabeManager.instance.ApplyProfessionStarStyle (int.Parse (name_splits [1]));
		}

		void OnSave (EComponentBase btn, bool IsPress)
		{
			if (IsPress) {
				ExportToLua (TableAstrolabeName, _ClientTableAstrolabePath, ClientTableExtension, UnSearialize);
				ExportToLua (TableAstrolabeName, _ServerTableAstrolabePath, ServerTableExtension, UnSearialize);
				ExportToLua (TableAstrolabeUIName, _ClientTableAstrolabeUIPath, ClientTableExtension, UnSearializeTableAstrolabeUI);
			}
		}

		void OnOpenScene (EComponentBase btn, bool IsPress)
		{
			if (IsPress)
				OpenScene (_ScenePath);
		}

		void OnToggleGizmosVisible (EComponentBase toggle, bool isToggle)
		{
			ToggleGizmos (isToggle, _VisibleGizmosScript);
		}


		void OpenScene (string path)
		{
			if (string.IsNullOrEmpty (path))
				return;
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ()) {
				EditorSceneManager.OpenScene (path);
			}
		}

		public void OnSelectedChanged ()
		{
			if (AstrolabeManager.instance != null) {
				Star s = null;
				if (Selection.activeGameObject != null)
					s = Selection.activeGameObject.GetComponent<Star> ();
				if (s != null)
					AstrolabeManager.instance.OnChoose (s);
				else
					AstrolabeManager.instance.OnCancelChoose ();
			}
		}

		void SetSelectionChangeAction (Action act)
		{
			Selection.selectionChanged = act;
		}

		LuaTable GetTableWorker (string partPath, string extension)
		{
			var path = Path.ChangeExtension (partPath, extension);
			try {
				var text = File.ReadAllText (path, System.Text.Encoding.UTF8);
				var worker = LuaSvrForEditor.Me.DoString (text) as LuaTable;
				return worker;
			} catch (System.Exception e) {
				Debug.LogException (e);
			}
			return null;
		}

		void ExportToLua (string tableName, string tablePath, string extension, Func<StringBuilder> unsearializefunc)
		{
			FileStream fs = null;
			StreamWriter sw = null;
			var path = Path.ChangeExtension (tablePath, extension);
			try {
				fs = new FileStream (path, File.Exists (path) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter (fs, new UTF8Encoding (false));

				sw.Write (unsearializefunc ().ToString ());
				sw.WriteLine ("\nreturn " + tableName);

				Debug.Log ("Export " + tableName + " Lua Success");
			} catch (Exception e) {
				Debug.LogError ("Export Table_Astrolabe Lua Error: " + e.Message);
			} finally {
				if (null != sw) {
					sw.Close ();
				}
				if (null != fs) {
					fs.Close ();
				}
			}
		}

		#region 导出Table_AstrolabeUI

		StringBuilder UnSearializeTableAstrolabeUI ()
		{
			StringBuilder sb = new StringBuilder ();
			var writer = new AstrolabeWriterApdater (new AstrolabeWriter (sb), TableAstrolabeUIName);
			writer.WriteStructStart ();
			for (int i = 0; i < (int)AstrolabeUIElement.AstrolabeUIElementStyle.ditu5 + 1; i++) {
				writer.WriteMemberName (i, true, 1);
				writer.WriteStructStart ();
				int idx = 1;
				for (int j = 0; j < AstrolabeManager.instance.UIElementList.Count; j++) {
					if (i == (int)AstrolabeManager.instance.UIElementList [j].style) {
						writer.WriteMemberName (idx, true, 2);
						WriteElementInfo (writer, AstrolabeManager.instance.UIElementList [j]);
						idx++;
					}
				}
				writer.WriteStructEnd ();
			}
			writer.WriteStructEnd ();
			return sb;
		}

		void WriteElementInfo (AstrolabeWriterApdater writer, AstrolabeUIElement uielement)
		{
			writer.WriteStructStart ();
			writer.WriteMemberValue (uielement.transform.localPosition.x);
			writer.WriteMemberValue (uielement.transform.localPosition.y);
			writer.WriteMemberValue (uielement.transform.localPosition.z);
			if (uielement.transform.localEulerAngles != Vector3.zero || uielement.transform.localScale != Vector3.one) {
				writer.WriteMemberValue (uielement.transform.localEulerAngles.x);
				writer.WriteMemberValue (uielement.transform.localEulerAngles.y);
				writer.WriteMemberValue (uielement.transform.localEulerAngles.z);
				if (uielement.transform.localScale != Vector3.one) {
					writer.WriteMemberValue (uielement.transform.localScale.x);
					writer.WriteMemberValue (uielement.transform.localScale.y);
					writer.WriteMemberValue (uielement.transform.localScale.z);
				}
			}
			writer.WriteStructEnd ();
		}

		#endregion

		#region UIElement相关

		public void ParseAstrolabeUITable (LuaTable table)
		{
			AstrolabeManager.instance.UIElementList.Clear ();
			foreach (var t in table) {
				AstrolabeUIElement.AstrolabeUIElementStyle style = (AstrolabeUIElement.AstrolabeUIElementStyle)int.Parse (t.key.ToString ());
				var subTable = t.value as LuaTable;
				int idx = 1;
				LuaTable sub = null;
				do {
					sub = LuaWorker.GetFieldTable (subTable, idx);
					if (sub != null) {
						AstrolabeUIElement ele = CreateAstrolabeUIElement (style);
						ele.InitByLuaTable (sub as LuaTable);
						AstrolabeManager.instance.UIElementList.Add (ele);
						idx++;
					}
				} while (sub != null);
			}
		}

		void OnCreateUIElement ()
		{
			CreateAstrolabelUIElementByEditor (m_nSelectedUIIdx);
		}

		public void CreateAstrolabelUIElementByEditor (int style)
		{
			AstrolabeUIElement ele = CreateAstrolabeUIElement ((AstrolabeUIElement.AstrolabeUIElementStyle)style);
			AstrolabeManager.instance.UIElementList.Add (ele);
		}

		AstrolabeUIElement CreateAstrolabeUIElement (AstrolabeUIElement.AstrolabeUIElementStyle style)
		{
			AstrolabeUIElement ele = null;
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject> (_AstrolabeUIElementPath);
			go = GameObject.Instantiate<GameObject> (go);
			go.name = "UI元素" + style.ToString ();
			Selection.activeGameObject = go;
			ele = go.GetComponent<AstrolabeUIElement> ();
			//LinkToRoot
			ele.transform.parent = AstrolabeManager.instance.transform;
			ele.SetElementStyle (style);
			return ele;
		}

		#endregion

		#region 星位属性表相关

		Dictionary<int, int> ParseStarAttrTable (LuaTable table)
		{
			Dictionary<int, int> _SubStarStyleMap = new Dictionary<int, int> ();
			foreach (var t in table) {
				LuaTable attr = LuaWorker.GetFieldTable (t.value as LuaTable, "Attr");
				int gid = int.Parse (t.key.ToString ());
				int style = LuaWorker.GetFieldInt (attr, 3);
				if (!_SubStarStyleMap.ContainsKey (gid))
					_SubStarStyleMap.Add (gid, style);
				else
					Debug.LogError ("星位GID重复: " + gid);
			}
			return _SubStarStyleMap;
		}

		#endregion

		#region 导出数据到Table_Astrolabe

		StringBuilder UnSearialize ()
		{
			StringBuilder sb = new StringBuilder ();
			var writer = new AstrolabeWriterApdater (new AstrolabeWriter (sb), TableAstrolabeName);
			writer.WriteStructStart ();

			List<Astrolabe> astrolabelList = AstrolabeManager.instance.astrolabelList;
			for (int i = 0; i < astrolabelList.Count; i++) {
				if (astrolabelList [i] == null) {
					Debug.LogError (" AstrolabelList cell is null => i:" + i);
					continue;
				}
				WriteAstrolabe (writer, astrolabelList [i]);
			}

			writer.WriteStructEnd ();
			return sb;
		}

		void WriteAstrolabe (AstrolabeWriterApdater writer, Astrolabe _Astrolabe)
		{
			writer.WriteMemberName (_Astrolabe.id, true, 1);
			writer.WriteStructStart ();

			writer.WriteMemberName ("id");
			writer.WriteMemberValue (_Astrolabe.id);

			WritePosition (writer, _Astrolabe.transform.localPosition);

			writer.WriteMemberName ("unlock");
			WriteUnlockTable (writer, _Astrolabe);

			WriteStarsList (writer, _Astrolabe);

			writer.WriteStructEnd ();
		}

		void WritePosition (AstrolabeWriterApdater writer, Vector3 pos)
		{
			writer.WriteMemberName ("pos");
			writer.WriteArrayStart ();
			writer.WriteMemberValue (pos.x);
			writer.WriteMemberValue (pos.y);
			writer.WriteMemberValue (pos.z);
			writer.WriteArrayEnd ();
		}

		void WriteUnlockTable (AstrolabeWriterApdater writer, Astrolabe _Astrolabe)
		{
			writer.WriteStructStart ();
			if (_Astrolabe.unlockLv > 0) {
				writer.WriteMemberName ("lv");
				writer.WriteMemberValue (_Astrolabe.unlockLv);
			}
			if (_Astrolabe.evo > 0) {
				writer.WriteMemberName ("evo");
				writer.WriteMemberValue (_Astrolabe.evo);
			}
			writer.WriteStructEnd ();
		}

		void WriteStarsList (AstrolabeWriterApdater writer, Astrolabe _Astrolabe)
		{
			writer.WriteMemberName ("stars");
			writer.WriteStructStart ();
			for (int i = 0; i < _Astrolabe.stars.Count; i++) {
				WriteStar (writer, _Astrolabe.stars [i]);
			}
			writer.WriteStructEnd ();
		}

		void WriteStar (AstrolabeWriterApdater writer, Star star)
		{
			if (star.gameObject.activeSelf) {
				writer.WriteMemberName (star.id);
				writer.WriteStructStart ();
				writer.WriteMemberName (1);
				WriteInnerConnect (writer, star.innerConnect);
				writer.WriteMemberName (2);
				WriteOuterConnect (writer, star.outerConnect);
				writer.WriteStructEnd ();
			}
		}

		void WriteInnerConnect (AstrolabeWriterApdater writer, List<Star> list)
		{
			writer.WriteStructStart ();
			for (int i = 0; i < list.Count; i++) {
				writer.WriteMemberValue (list [i].id);
			}
			writer.WriteStructEnd ();
		}

		void WriteOuterConnect (AstrolabeWriterApdater writer, List<Star> list)
		{
			writer.WriteStructStart ();
			for (int i = 0; i < list.Count; i++) {
				writer.WriteMemberValue (list [i].globalId);
			}
			writer.WriteStructEnd ();
		}

		#endregion

		private static void ToggleGizmos (bool gizmosOn, List<string> list)
		{
			int val = gizmosOn ? 1 : 0;
			Assembly asm = Assembly.GetAssembly (typeof(Editor));
			Type type = asm.GetType ("UnityEditor.AnnotationUtility");
			if (type != null) {
				MethodInfo getAnnotations = type.GetMethod ("GetAnnotations", BindingFlags.Static | BindingFlags.NonPublic);
				MethodInfo setGizmoEnabled = type.GetMethod ("SetGizmoEnabled", BindingFlags.Static | BindingFlags.NonPublic);
				MethodInfo setIconEnabled = type.GetMethod ("SetIconEnabled", BindingFlags.Static | BindingFlags.NonPublic);
				var annotations = getAnnotations.Invoke (null, null);
				foreach (object annotation in (IEnumerable)annotations) {
					Type annotationType = annotation.GetType ();
					FieldInfo classIdField = annotationType.GetField ("classID", BindingFlags.Public | BindingFlags.Instance);
					FieldInfo scriptClassField = annotationType.GetField ("scriptClass", BindingFlags.Public | BindingFlags.Instance);
					if (classIdField != null && scriptClassField != null) {
						int classId = (int)classIdField.GetValue (annotation);
						string scriptClass = (string)scriptClassField.GetValue (annotation);
						if (list.Contains (scriptClass)) {
							setGizmoEnabled.Invoke (null, new object[] { classId, scriptClass, val });
							setIconEnabled.Invoke (null, new object[] { classId, scriptClass, val });
						}
					}
				}
			}
		}
	}
}
