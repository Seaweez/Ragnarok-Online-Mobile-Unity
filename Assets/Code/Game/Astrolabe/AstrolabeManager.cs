using RO;
using SLua;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class AstrolabeManager : MonoBehaviour
{
	#if UNITY_EDITOR
	static AstrolabeManager _Instance;

	public static AstrolabeManager instance {
		get {
			if (_Instance == null && GameObject.Find ("Root") != null)
				_Instance = GameObject.Find ("Root").GetComponent<AstrolabeManager> ();
			return _Instance;
		}
	}

	//星盘为六边形
	public static int Sides = 6;
	//星盘最多三层
	public static int MaxLevel = 3;
	//星盘步长
	public static float PathLength = 100f;
	//星盘标准向量
	public static Vector3[] NormailizeDir = new Vector3[] {
		new Vector3 (0f, 1f, 0f),
		new Vector3 (-0.8660254f, 0.5f, 0f),
		new Vector3 (-0.8660254f, -0.5f, 0f),
		new Vector3 (0f, -1f, 0f),
		new Vector3 (0.8660254f, -0.5f, 0f),
		new Vector3 (0.8660254f, 0.5f, 0f)
	};
	//星盘维度大小枚举
	public enum AstrolabelStyle
	{
		none,
		small,
		middle,
		big
	}

	public List<Texture> astrolabeBgTextureList = new List<Texture> ();
	public List<Astrolabe> astrolabelList = new List<Astrolabe> ();
	public List<Texture> UIElementTexture = new List<Texture> ();
	public List<AstrolabeUIElement> UIElementList = new List<AstrolabeUIElement> ();
	[SerializeField]
	private Dictionary<int, Dictionary<int, int>> _StarStyleMap = new Dictionary<int, Dictionary<int, int>> ();
	[SerializeField]
	Dictionary<int, int> _CurrProfessionStarStyleMap = new Dictionary<int, int> ();
	static string _AstrolabePrefabPath = "Assets/DevelopScene/AstrolabeEditorScene/Astrolabe.prefab";

	void Update ()
	{
	}

	public string CreateAstrolabel (int id, int level, Vector3 pos)
	{
		Astrolabe _Astrolabe = GetAstrolabelByID (id);
		if (_Astrolabe == null) {
#if UNITY_EDITOR
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject> (_AstrolabePrefabPath);
			go = GameObject.Instantiate<GameObject> (go);
			go.name = "星盘" + id.ToString ();
			_Astrolabe = go.GetComponent<Astrolabe> ();

			AstrolabeManager.instance.AddAstrolabe (_Astrolabe);

			//LinkToRoot
			_Astrolabe.transform.parent = transform;
			Selection.activeGameObject = go;
			_Astrolabe.Init (id, level, pos);
			_Astrolabe.ResetStyle ();
#endif
			return "创建星盘成功!";
		} else {
			return "创建星盘失败! ID重复!";
		}
	}

	public string CreateAstrolabeByLuaTable (LuaTable table)
	{
#if UNITY_EDITOR
		GameObject go = AssetDatabase.LoadAssetAtPath<GameObject> (_AstrolabePrefabPath);
		go = GameObject.Instantiate<GameObject> (go);
		go.name = "星盘" + LuaWorker.GetFieldInt (table, "id").ToString ();
		Astrolabe _Astrolabe = go.GetComponent<Astrolabe> ();
		//LinkToRoot
		_Astrolabe.transform.parent = transform;
		_Astrolabe.Init (table);
#endif
		return "创建星盘成功!";
	}

	public Astrolabe GetAstrolabelByID (int id)
	{
		for (int i = 0; i < astrolabelList.Count; i++) {
			if (astrolabelList [i].id == id)
				return astrolabelList [i];
		}
		return null;
	}

	public Star GetStarByGid (int gid)
	{
		int aid = gid / 10000;
		int sid = gid % 10000;
		Astrolabe _Astrolabe = GetAstrolabelByID (aid);
		Star s = _Astrolabe.GetStarByIdx (sid);
		return s;
	}

	public List<Star> chooseList = new List<Star> ();

	public void OnCancelChoose ()
	{
		for (int i = 0; i < chooseList.Count; i++) {
			chooseList [i].ChangeChooseState (false);
		}
		chooseList.Clear ();
	}

	public void OnChoose (Star star)
	{
		if (chooseList.Contains (star)) {
			chooseList.Remove (star);
		} else if (chooseList.Count == 2) {
			chooseList [0].ChangeChooseState (false);
			chooseList.RemoveAt (0);
		}
		star.ChangeChooseState (true);
		chooseList.Add (star);
	}

	public bool HideStar ()
	{
		bool b = false;
		if (chooseList.Count == 1) {
			chooseList [0].UnLinkAll ();
			chooseList [0].SetActive (false);
			OnCancelChoose ();
		}
		return b;
	}

	public bool Link2Star ()
	{
		bool b = false;
		if (chooseList.Count == 2) {
			chooseList [0].TryLink (chooseList [1]);
			b = true;
		}
		return b;
	}

	public bool UnLink2Star ()
	{
		bool b = false;
		if (chooseList.Count == 2) {
			chooseList [0].TryUnLink (chooseList [1]);
			b = true;
		}
		return b;
	}

	public void AddAstrolabe (Astrolabe _Astrolabe)
	{
		if (!astrolabelList.Contains (_Astrolabe)) {
			astrolabelList.Add (_Astrolabe);
		}
	}

	public void RemoveAstrolabe (Astrolabe _Astrolabe)
	{
		if (astrolabelList.Contains (_Astrolabe)) {
			astrolabelList.Remove (_Astrolabe);
		}
	}

	public Texture GetBgTexture (AstrolabelStyle style)
	{
		return astrolabeBgTextureList [(int)style];
	}

	public Texture GetElementTexture (AstrolabeUIElement.AstrolabeUIElementStyle s)
	{
		int idx = (int)s;
		if (UIElementTexture.Count > idx)
			return UIElementTexture [idx];
		return null;
	}

	public void UpdateStarStyleMap (int profession, Dictionary<int, int> submap)
	{
		if (_StarStyleMap.ContainsKey (profession))
			_StarStyleMap [profession] = submap;
		else
			_StarStyleMap.Add ((int)profession, submap);
	}

	int GetStarStyle (int starGID)
	{
		if (_CurrProfessionStarStyleMap.ContainsKey (starGID))
			return _CurrProfessionStarStyleMap [starGID];
		//else
		//    Debug.LogError("星位配置不存在, GID = " + starGID);
		return 0;
	}

	public void ApplyProfessionStarStyle (int profession)
	{
		if (_StarStyleMap.ContainsKey (profession)) {
			_CurrProfessionStarStyleMap = _StarStyleMap [profession];
			Debug.Log ("Import: Table_Rune_" + profession.ToString () + " Success.");
		}
		else
			Debug.LogError (profession.ToString () + " Table_Rune表不存在!");
		
		for (int i = 0; i < astrolabelList.Count; i++) {
			for (int j = 0; j < astrolabelList [i].stars.Count; j++) {
				int gid = astrolabelList [i].stars [j].globalId;
				int style = GetStarStyle (gid);
				astrolabelList [i].stars [j].ResetStyle (style);
			}
		}
	}

	public void DeleteUIElement (AstrolabeUIElement ui)
	{
		if (UIElementList.Contains (ui)) {
			UIElementList.Remove (ui);
			GameObject.DestroyImmediate (ui.gameObject);
		}
	}

	#region 导入LuaTable数据到C#

	public void ParseAstrolabeTable (LuaTable table)
	{
		astrolabelList.Clear ();
		foreach (var t in table) {
			CreateAstrolabeByLuaTable (t.value as LuaTable);
		}
		Debug.Log ("ParseAstrolabeTable:" + astrolabelList.Count);
		for (int i = 0; i < astrolabelList.Count; i++) {
			astrolabelList [i].ReBuildPath ();
			astrolabelList [i].ResetStyle ();
		}
	}

	#endregion

	#endif
}