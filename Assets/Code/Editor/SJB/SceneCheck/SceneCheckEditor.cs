using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using EditorTool;
using System.IO;
using System.Text;

public struct SceneMeshInfo
{
	public int vertCounts;
	public int trisCounts;
	public int subMeshCounts;

	public bool missingMesh;

	public string name;

	public int meshInstanceID;

	public void Add (SceneMeshInfo info)
	{
		this.vertCounts += info.vertCounts;
		this.trisCounts += info.trisCounts;
		this.subMeshCounts += info.subMeshCounts;
	}
}

public struct STextureInfo
{
	public int index;
	public string name;
	public int width;
	public int height;
	public int mipmapCount;
	public int InstanceID;

	public TextureFormat format;

	int _memSize;
	int _Size;

	public int Size {
		get {
			if (_Size == 0) {
				_Size = width * height;
			}
			return _Size;
		}
	}

	public int MemSize {
		get { 
			if (_memSize == 0) {
				int bitsPerPixel = CheckUIAtlas.GetBitsPerPixel (this.format);
				int mipMapCount = mipmapCount;
				int mipLevel = 1;
				_memSize = 0;
				int w = width;
				int h = height;
				while (mipLevel <= mipMapCount) {
					_memSize += w * h * bitsPerPixel / 8;
					w = w / 2;
					h = h / 2;
					mipLevel++;
				}
			}
			return _memSize;
		}
	}
}

public struct SceneTextureInfo
{
	public int count;
	public long memSize;
}

public class CheckSceneInfo
{
	public string sceneName;
	public string scenePath;
	SceneMeshInfo _totalMeshInfo = new SceneMeshInfo ();
	SceneTextureInfo _totalTextureInfo = new SceneTextureInfo ();

	public CheckSceneInfo (string path)
	{
		scenePath = path;
		sceneName = Path.GetFileNameWithoutExtension (scenePath);
	}

	public SceneTextureInfo totalTextureInfo {
		get {
			return _totalTextureInfo;
		}
	}

	public int totalVertCounts {
		get { 
			return _totalMeshInfo.vertCounts;
		}
	}

	public int totalTrisCounts {
		get { 
			return _totalMeshInfo.trisCounts;
		}
	}

	private List<SceneMeshInfo> _objectInfos = new List<SceneMeshInfo> ();
	private List<SceneMeshInfo> _warnningInfos = new List<SceneMeshInfo> ();
	private Dictionary<string,bool> _warnningInfosMap = new Dictionary<string,bool> ();

	private Dictionary<string,bool> _matMap = new Dictionary<string,bool> ();

	private Dictionary<TextureFormat,List<STextureInfo>> _textureMap = new Dictionary<TextureFormat,List<STextureInfo>> ();

	public Dictionary<TextureFormat, List<STextureInfo>> textureMap {
		get {
			return _textureMap;
		}
	}

	public int totalMeshCount {
		get { 
			return _objectInfos.Count;
		}
	}

	public int totalMaterialCount {
		get { 
			return _matMap.Count;
		}
	}

	public List<SceneMeshInfo> warnningInfos {
		get {
			return _warnningInfos;
		}
	}

	#region mesh

	public void CollectMeshes (MeshFilter[] mfs)
	{
		if (mfs != null && mfs.Length > 0) {
			for (int j = 0; j < mfs.Length; j++) {
				_CollectMesh (mfs [j]);
			}
		}
	}

	void _CollectMesh (MeshFilter mf)
	{
		if (mf != null) {
			SceneMeshInfo info = new SceneMeshInfo ();
			if (mf.sharedMesh != null) {
				int tris = mf.sharedMesh.triangles.Length / 3;  
				int verts = mf.sharedMesh.vertexCount;  
				int submeshs = mf.sharedMesh.subMeshCount;

				info.trisCounts += tris;
				info.vertCounts += verts;
				info.subMeshCounts = submeshs;
				info.meshInstanceID = mf.sharedMesh.GetInstanceID ();

				if (tris >= 1000 || submeshs >= 4) {
					if (_warnningInfosMap.ContainsKey (mf.sharedMesh.GetInstanceID ().ToString ()) == false) {
						SceneMeshInfo warn = new SceneMeshInfo ();
						warn.trisCounts = tris;
						warn.vertCounts = verts;
						warn.subMeshCounts = submeshs;
						warn.name = mf.sharedMesh.name;
						warn.meshInstanceID = mf.sharedMesh.GetInstanceID ();
						_warnningInfos.Add (warn);
						_warnningInfosMap.Add (warn.meshInstanceID.ToString (), true);
					}
				}

				_totalMeshInfo.Add (info);

				info.name = mf.gameObject.name;
				_objectInfos.Add (info);
			} else {
				if (_warnningInfosMap.ContainsKey (mf.name) == false) {
					SceneMeshInfo warn = new SceneMeshInfo ();
					warn.trisCounts = -1;
					warn.vertCounts = -1;
					warn.subMeshCounts = -1;
					warn.missingMesh = true;
					warn.name = mf.name;
					_warnningInfosMap.Add (mf.name, true);
					_warnningInfos.Add (warn);
				}
			}
		}
	}

	void _SortWarnMeshInfos ()
	{
		_warnningInfos.Sort ((a, b) => {
			return b.trisCounts.CompareTo (a.trisCounts);
		});
	}

	#endregion

	#region collect material & texture

	public void CollectMaterial (string path)
	{
		if (!_matMap.ContainsKey (path)) {
			_matMap.Add (path, true);
		}
	}

	public void CollectTexture (string path)
	{
		Texture2D texture = AssetDatabase.LoadMainAssetAtPath (path) as Texture2D;
		if (texture != null) {
			_totalTextureInfo.count++;
			TextureFormat format = texture.format;
			List<STextureInfo> list = null;
			if (!_textureMap.TryGetValue (format, out list)) {
				list = new List< STextureInfo> ();
				_textureMap.Add (format, list);
			}
			STextureInfo info = new STextureInfo ();
			info.index = list.Count + 1;
			info.format = texture.format;
			info.width = texture.width;
			info.height = texture.height;
			info.mipmapCount = texture.mipmapCount;
			info.name = texture.name;
			info.InstanceID = texture.GetInstanceID ();

			_totalTextureInfo.memSize += info.MemSize;

			list.Add (info);
		}
	}

	void _SortTextureInfos ()
	{
		foreach (KeyValuePair<TextureFormat,List<STextureInfo>> kvp in(_textureMap)) {
			kvp.Value.Sort ((a, b) => {
				if (a.MemSize == b.MemSize) {
					if (a.Size == b.Size) {
						return a.index.CompareTo (b.index);
					}
					return b.Size.CompareTo (a.Size);
				} else
					return b.MemSize.CompareTo (a.MemSize);
			});
		}
	}

	#endregion

	public void AnalysisDatas ()
	{
		_SortWarnMeshInfos ();
		_SortTextureInfos ();
	}

	public void ShowInfo ()
	{
		Debug.Log (string.Format ("场景{0}中\n" +
		"顶点数:{1}万\n" +
		"三角形数:{2}万\n" +
		"总的mesh数量:{3}\n", this.sceneName, (float)this.totalVertCounts / 10000, (float)this.totalTrisCounts / 10000, this._objectInfos.Count));

		string warnInfo = "";

		SceneMeshInfo info;
		for (int i = 0; i < _warnningInfos.Count; i++) {
			info = _warnningInfos [i];
			if (info.missingMesh) {
				warnInfo += string.Format ("名字:{0}  mesh missing", info.name);
			} else {
				warnInfo += string.Format ("名字:{0}  顶点数:{1} 三角形数:{2} submesh:{3}\n", info.name, info.vertCounts, info.trisCounts, info.subMeshCounts);
			}
		}
		if (_warnningInfos.Count > 0) {
			Debug.Log (string.Format ("需要注意的Mesh:\n" +
			"数量:{0}\n" +
			"信息:\n{1}", _warnningInfos.Count, warnInfo));
		}

		if (_matMap.Count > 0) {
			Debug.Log (string.Format ("共有{0}个材质", _matMap.Count));
		}
		AnalysisDatas ();
		if (_textureMap.Count > 0) {
			StringBuilder sb = new StringBuilder ();
			foreach (KeyValuePair<TextureFormat,List<STextureInfo>> kvp in(_textureMap)) {
				sb.Append (kvp.Key);
				sb.Append ("(数量 ");
				sb.Append (kvp.Value.Count);
				sb.Append (")");
				sb.AppendLine (":");
				for (int i = 0; i < kvp.Value.Count; i++) {
					sb.Append ("	");
					sb.Append (" name:");
					sb.Append (kvp.Value [i].name);
					sb.Append (" width:");
					sb.Append (kvp.Value [i].width);
					sb.Append (" height:");
					sb.Append (kvp.Value [i].height);
					sb.Append (" mem:");
					sb.AppendLine (CheckUIAtlas.humanReadableByteCount (kvp.Value [i].MemSize));
				}
			}
			Debug.Log (string.Format ("共有{0}个贴图,总内存大小{1}", _totalTextureInfo.count, CheckUIAtlas.humanReadableByteCount (_totalTextureInfo.memSize)));
			Debug.Log (sb.ToString ());
		}
	}
}

public class SceneCheckInfoCell
{
	public CheckSceneInfo data;

	static Color32 _SelectedColor = new Color32 (62, 85, 150, 255);
	static GUIStyle _TitleStyle = new GUIStyle ();
	static GUIStyle _TitleRedStyle;
	static GUIStyle _TitleYellowStyle;
	private ELabel _TitleLabel = null;

	public SceneCheckInfoCell (CheckSceneInfo d)
	{
		InitStyle ();
		Reset (d);
	}

	public void Reset (CheckSceneInfo d)
	{
		data = d;
		InitTitleLabel ();
	}

	private void InitStyle ()
	{
		_TitleStyle.alignment = TextAnchor.MiddleLeft;
		_TitleStyle.normal.textColor = Color.white;
		_TitleStyle.fontSize = 12;

		_TitleRedStyle = new GUIStyle (_TitleStyle);
		_TitleRedStyle.normal.textColor = Color.red;
		_TitleYellowStyle = new GUIStyle (_TitleStyle);
		_TitleYellowStyle.normal.textColor = Color.yellow;
	}

	private void InitTitleLabel ()
	{
		if (_TitleLabel == null)
			_TitleLabel = new ELabel ();
		_TitleLabel.content.text = data.sceneName;
	}

	public void DrawTitle ()
	{
		Rect r = EditorGUILayout.GetControlRect ();
		GUIStyle style = _TitleStyle;
		if (r.Contains (Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
			SceneCheckInspectorWindow.selected = this;
		if (this == SceneCheckInspectorWindow.selected)
			EditorGUI.DrawRect (new Rect (r), _SelectedColor);
		string title = data.sceneName;
		if (data.totalTrisCounts >= 350000) {
			title += " 【三角形过多】";
			style = _TitleRedStyle;
		} else if (data.totalTrisCounts >= 200000) {
			title += " 【三角形略多】";
			style = _TitleYellowStyle;
		}
		if (data.totalVertCounts >= 700000) {
			title += " 【顶点过多】";
			style = _TitleRedStyle;
		} else if (data.totalVertCounts >= 500000) {
			title += " 【顶点略多】";
			if (style == _TitleStyle) {
				style = _TitleYellowStyle;
			}
		}

		_TitleLabel.content.text = title;
		_TitleLabel.DrawGUI (r, style);
	}
}

public class SceneCheckEditor
{
	static List<SceneCheckInfoCell> _Cells = new List<SceneCheckInfoCell> ();

	public static List<SceneCheckInfoCell> Cells {
		get {
			return _Cells;
		}
	}

	[MenuItem ("Assets/测试select场景的信息")]
	public static void CheckSelect ()
	{
		UnityEngine.Object select = Selection.activeObject;
		if (select != null) {
			string[] depends = null;
			CheckSceneInfo info = null;
			string path = AssetDatabase.GetAssetPath (select);
			depends = AssetDatabase.GetDependencies (path);
			info = new CheckSceneInfo (path);
			HandleSceneAndItsDepends (info, path, depends);
			info.ShowInfo ();
		}
	}

	//	[MenuItem ("Assets/测试All场景的信息")]
	public static void Menu_ScanAllBuildSettingScenes ()
	{
		List<CheckSceneInfo> infos = _ScanAllBuildSettingScenes ();
		for (int i = 0; i < infos.Count; i++) {
			infos [i].ShowInfo ();
		}
	}

	public static void HandleScene (string path)
	{
		SceneCheckInfoCell found = _Cells.Find ((cell) => {
			return cell.data.scenePath == path;
		});
		CheckSceneInfo info = new CheckSceneInfo (path);
		string[] depends = AssetDatabase.GetDependencies (path);
		HandleSceneAndItsDepends (info, path, depends);
		info.AnalysisDatas ();
		if (found == null) {
			found = new SceneCheckInfoCell (info);
			_Cells.Add (found);
			_Cells.Sort ((a, b) => {
				return b.data.totalTrisCounts.CompareTo (a.data.totalTrisCounts);
			});
		} else {
			found.data = info;
		}

	}

	public static void ScanAllBuildSettingScenes ()
	{
		_Cells.Clear ();
		SceneCheckInfoCell cell = null;
		try {
			List<CheckSceneInfo> infos = _ScanAllBuildSettingScenes (true);
			for (int i = 0; i < infos.Count; i++) {
				cell = new SceneCheckInfoCell (infos [i]);
				_Cells.Add (cell);
			}
			_Cells.Sort ((a, b) => {
				return b.data.totalTrisCounts.CompareTo (a.data.totalTrisCounts);
			});
		} catch (System.Exception e) {
			Debug.LogError (e);
		} finally {
			EditorUtility.ClearProgressBar ();
		}
	}

	public static void DrawAllTitle ()
	{
		for (int i = 0; i < _Cells.Count; i++) {
			_Cells [i].DrawTitle ();
		}
	}

	static List<CheckSceneInfo> _ScanAllBuildSettingScenes (bool showProgess = false)
	{
		List<string> sceneExcepts = new List<string> () {
			"Scene/testformonster",
			"Scene/testmap1",
			"Scene/Launch"
		}; 
		List<string> scenes = GetLevelsFromBuildSettings ().FindAll ((p) => {
			foreach (string except in sceneExcepts) {
				if (p.Contains (except))
					return false;
			}
			return true;
		});

		string[] depends = null;
		List<CheckSceneInfo> infos = new List<CheckSceneInfo> ();
		CheckSceneInfo info = null;
		for (int i = 0; i < scenes.Count; i++) {
			if (showProgess) {
				EditorUtility.DisplayProgressBar ("检查场景", string.Format ("{0}/{1}", i + 1, scenes.Count), (float)(i + 1) / scenes.Count);
			}
			depends = AssetDatabase.GetDependencies (scenes [i]);
			info = new CheckSceneInfo (scenes [i]);
			HandleSceneAndItsDepends (info, scenes [i], depends);
			info.AnalysisDatas ();
			infos.Add (info);
		}
		return infos;
	}

	static void HandleSceneAndItsDepends (CheckSceneInfo info, string path, string[] depends)
	{
		string depend = null;
		AssetType dependAssetType;
		for (int i = 0; i < depends.Length; i++) {
			depend = depends [i];
			Path.GetExtension (depend);
			dependAssetType = GetAssetType (depend);
			if (dependAssetType == AssetType.Prefab && depend.StartsWith ("Assets/Scene")) {
				GameObject perf = AssetDatabase.LoadMainAssetAtPath (depend) as GameObject;
				if (perf != null) {
					MeshFilter[] mfs = perf.GetComponentsInChildren<MeshFilter> ();
					//collect mesh info
					info.CollectMeshes (mfs);
				}
			} else if (dependAssetType == AssetType.Mat) {
				info.CollectMaterial (depend);
			} else if (dependAssetType == AssetType.Img) {
				info.CollectTexture (depend);
			}
		}
	}

	static List<string> GetLevelsFromBuildSettings ()
	{
		List<string> levels = new List<string> ();
		for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i) {
			if (EditorBuildSettings.scenes [i].enabled)
				levels.Add (EditorBuildSettings.scenes [i].path);
		}

		return levels;
	}

	static AssetType GetAssetType (string extension)
	{
		if (extension.Contains (Path.DirectorySeparatorChar.ToString ()))
			extension = Path.GetExtension (extension);
		switch (extension) {
		case ".prefab":
			return AssetType.Prefab;
		case ".shader":
			return AssetType.Shader;
		case ".cs":
			return AssetType.Csharp;
		case ".asset":
			return AssetType.Asset;
		case ".mat":
			return AssetType.Mat;
		case ".png":
		case ".jpg":
		case ".exr":
		case ".tga":
			return AssetType.Img;
		}
		return AssetType.UnKnown;
	}
}


public class SceneCheckInspectorWindow:EditorWindow
{
	public static SceneCheckInspectorWindow window = null;
	public static SceneCheckInfoCell selected;

	static Vector2 m_ScrollPos = Vector2.zero;

	public SceneCheckInspectorWindow ()
	{
		window = this;
	}

	void Update ()
	{
		Repaint ();
	}

	void OnGUI ()
	{
		m_ScrollPos = EditorGUILayout.BeginScrollView (m_ScrollPos);
		if (selected != null)
			DrawInspector ();

		EditorGUILayout.EndScrollView ();
		if (selected != null) {
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("重新检测"))
				ReCheck (selected.data.scenePath);
			EditorGUILayout.EndHorizontal ();
		}
	}

	void DrawInspector ()
	{
		GUILayout.Label (string.Format ("场景名：{0}", selected.data.sceneName));
		GUILayout.Label (string.Format ("	顶点数：{0}  三角面：{1}  mesh数：{2}", selected.data.totalVertCounts, selected.data.totalTrisCounts, selected.data.totalMeshCount));

		SceneMeshInfo info;
		GUILayout.Label ("需要注意的Mesh:");
		GUILayout.Label ("	数量:【" + selected.data.warnningInfos.Count + "】");
		GUILayout.Label ("\t信息:");
		for (int i = 0; i < selected.data.warnningInfos.Count; i++) {
			info = selected.data.warnningInfos [i];
			if (info.missingMesh) {
				GUILayout.Label (string.Format ("\t\t名字:{0}  mesh missing", info.name));
			} else {
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("\t\t名字:", GUILayout.ExpandWidth (false));
				if (GUILayout.Button (info.name, GUILayout.ExpandWidth (false))) {
					Selection.activeInstanceID = info.meshInstanceID;
				}
				GUILayout.Label (string.Format ("  顶点数:{0} 三角形数:{1} submesh:{2}", info.vertCounts, info.trisCounts, info.subMeshCounts));
				GUILayout.EndHorizontal ();
			}
		}
		GUILayout.Label ("材质信息:");
		GUILayout.Label ("	数量:【" + selected.data.totalMaterialCount + "】");

		GUILayout.Label ("贴图信息:");
		GUILayout.Label ("	数量:【" + selected.data.totalTextureInfo.count + "】" + " 总内存:" + CheckUIAtlas.humanReadableByteCount (selected.data.totalTextureInfo.memSize));
		GUILayout.Label ("\t信息:");

		Dictionary<TextureFormat,List<STextureInfo>> map = selected.data.textureMap;

		foreach (KeyValuePair<TextureFormat,List<STextureInfo>> kvp in map) {
			GUILayout.Label (string.Format ("\t{0} (数量 {1})", kvp.Key, kvp.Value.Count));
			for (int i = 0; i < kvp.Value.Count; i++) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (string.Format ("\t\tmem:{0}  name", CheckUIAtlas.humanReadableByteCount (kvp.Value [i].MemSize)), GUILayout.ExpandWidth (false));
				if (GUILayout.Button (kvp.Value [i].name, GUILayout.ExpandWidth (false))) {
					Selection.activeInstanceID = kvp.Value [i].InstanceID;
				}
				GUILayout.Label (string.Format (" ({0}x{1})", kvp.Value [i].width, kvp.Value [i].height));
				GUILayout.EndHorizontal ();
			}
		}

	}

	void ReCheck (string path)
	{
		SceneCheckEditor.HandleScene (path);
	}
}