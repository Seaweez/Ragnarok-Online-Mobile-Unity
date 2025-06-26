using RO;
using SLua;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class Star : MonoBehaviour
{
	#if UNITY_EDITOR
	public List<Star> innerConnect = new List<Star> ();
	public List<Star> outerConnect = new List<Star> ();
	public List<AstrolabePath> pathList = new List<AstrolabePath> ();

	public int astrolabelID;

	public int globalId { get { return astrolabelID * 10000 + id; } }

	public int id;

	public int level {
		get {
			if (id > 0)
				return Mathf.FloorToInt ((id - 1) / AstrolabeManager.Sides + 1);
			else
				return 0;
		}
	}

	public UISprite sprite;

	//是否锁定星位位置, 默认锁定
	bool _bIsLock = true;
	Vector3 localPosition;
	static string _StarPrefabPath = "Assets/DevelopScene/AstrolabeEditorScene/Star.prefab";
	bool _bIsChoose = false;
	//0为dark, 1为light
	private string[] _StarStateSpriteArr = new string[2];
	static string[] _StarStyleSpriteNameArr = new string[] {
		"Rune_Locked_St", "Rune_On_St",
		"Rune_Locked_1", "Rune_On_1",
		"Rune_Locked_2", "Rune_On_2",
		"Rune_Locked_3", "Rune_On_3",
		"Rune_Locked_4", "Rune_On_4",
		"Rune_Locked_5", "Rune_On_5",
		"Rune_Locked_6", "Rune_On_6",
	};

	public Star ()
	{
		ResetStyle (0);
	}

	void Update ()
	{
		if (_bIsLock)
			transform.localPosition = localPosition;
	}

	public static Star Create (Transform parent, int aID, int selfID)
	{
		GameObject go = null;
		Transform tran = parent.Find (selfID.ToString ());
		if (tran == null) {
			go = AssetDatabase.LoadAssetAtPath<GameObject> (_StarPrefabPath);
			go = GameObject.Instantiate<GameObject> (go);
			go.name = selfID.ToString ();
		} else {
			go = tran.gameObject;
		}
		Star s = go.GetComponent<Star> ();
		s.SetID (aID, selfID);
		s.SetStarPos (parent);
		return s;
	}

	//初始化时由星盘Astrolabel类给自身的星位Star指定ID
	public void SetID (int aID, int selfID)
	{
		astrolabelID = aID;
		id = selfID;
	}

	void SetStarPos (Transform parent)
	{
		transform.SetParent (parent);
		transform.localScale = Vector3.one;
		if (id == 0) {
			transform.localPosition = Vector3.zero;
		} else {
			int idx = (id - 1) % AstrolabeManager.Sides;
			transform.localPosition = AstrolabeManager.PathLength * AstrolabeManager.NormailizeDir [idx] * level;
		}
		localPosition = transform.localPosition;
	}


	public void Init (bool visible)
	{
		sprite.depth = 10;
		//UIEventListener listener = gameObject.GetComponent<UIEventListener>();
		//listener.onClick += OnChooseClick;
		if (visible)
			LinkAll ();
		else
			UnLinkAll ();
		SetActive (visible);
	}

	public void Init (LuaTable table)
	{
		Astrolabe _Astrolabe;
		LuaTable _InTable = LuaWorker.GetFieldTable (table, 1);
		foreach (var inner in _InTable) {
			_Astrolabe = AstrolabeManager.instance.GetAstrolabelByID (astrolabelID);
			Star s = _Astrolabe.GetStarByIdx (int.Parse (inner.value.ToString ()));
			if (s != null)
				TryLink (s);
		}
		LuaTable _OutTable = LuaWorker.GetFieldTable (table, 2);
		foreach (var outer in _OutTable) {
			int gid = int.Parse (outer.value.ToString ());
			int aid = (int)(gid / 10000);
			int sid = gid - 10000 * aid;
			_Astrolabe = AstrolabeManager.instance.GetAstrolabelByID (aid);
			Star s = _Astrolabe.GetStarByIdx (sid);
			if (s != null)
				TryLink (s);
		}
		SetActive (true);
	}

	//链接上所有能链接的点
	public void LinkAll ()
	{
		Astrolabe _Astrolabe = AstrolabeManager.instance.GetAstrolabelByID (astrolabelID);
		if (_Astrolabe != null) {
			List<Star> canConnectList = _Astrolabe.GetAllCanConnectStar (this);
			for (int i = 0; i < canConnectList.Count; i++) {
				TryLink (canConnectList [i]);
			}
		}
	}

	//断开所有已链接的点
	public void UnLinkAll ()
	{
		while (innerConnect.Count > 0) {
			TryUnLink (innerConnect [0]);
		}
		while (outerConnect.Count > 0) {
			TryUnLink (outerConnect [0]);
		}
	}

	public void TryLink (Star node)
	{
		AstrolabePath path = TryGetPath (node);
		//没有到目标星位的路径则创建
		if (path == null) {
			path = AstrolabePath.TryCreatePath (this, node);
			path.InitPathSprite (this, node);
			pathList.Add (path);
		}
		//重新计算路径
		path.BuildPath ();
		//连接路径两端的星位
		path.Link ();
	}

	public void Link (Star node, AstrolabePath path)
	{
		List<Star> list;
		if (node.astrolabelID == astrolabelID)
			list = innerConnect;
		else
			list = outerConnect;
		if (!list.Contains (node))
			list.Add (node);
		if (TryGetPath (node) == null)
			pathList.Add (path);
	}

	public void TryUnLink (Star node)
	{
		AstrolabePath path = TryGetPath (node);
		//路径存在则断开路径
		if (path != null)
			path.UnLink ();
		//else
		//  尝试断开不存在的路径
	}

	public void UnLink (Star node)
	{
		List<Star> list;
		if (node.astrolabelID == astrolabelID)
			list = innerConnect;
		else
			list = outerConnect;
		if (list.Contains (node))
			list.Remove (node);
	}

	public void SetActive (bool value)
	{
		gameObject.SetActive (value);
	}

	public void ChangeChooseState (bool value)
	{
		_bIsChoose = value;
		sprite.spriteName = _bIsChoose ? _StarStateSpriteArr [1] : _StarStateSpriteArr [0];
		sprite.MarkAsChanged ();
	}

	public void ResetStyle (int style)
	{
		if (sprite == null) {
			return;
		}
		_StarStateSpriteArr [0] = _StarStyleSpriteNameArr [style * 2];
		_StarStateSpriteArr [1] = _StarStyleSpriteNameArr [style * 2 + 1];
		sprite.spriteName = _bIsChoose ? _StarStateSpriteArr [1] : _StarStateSpriteArr [0];
		sprite.MarkAsChanged ();
	}

	public AstrolabePath TryGetPath (Star node)
	{
		for (int i = 0; i < pathList.Count; i++) {
			if (pathList [i].Contains (node))
				return pathList [i];
		}
		return null;
	}
	#endif
}
