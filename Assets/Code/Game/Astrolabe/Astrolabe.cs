using RO;
using SLua;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Astrolabe : MonoBehaviour
{
	#if UNITY_EDITOR
	Star center;
	public List<Star> stars;
	static int _DefaultUnlockLv = 40;
	public int unlockLv = -1;
	public int evo = -1;
	public int id;

	public int level {
		get {
			for (int i = stars.Count - 1; i >= 0; i--) {
				if (stars [i].gameObject.activeSelf)
					return stars [i].level;
			}
			return 0;
		}
	}

	public UITexture bg;

	void Update ()
	{
		if (transform.hasChanged)
			SyncLinkOuterConnects ();
	}

	public void Init (int ID, int level, Vector3 pos)
	{
		unlockLv = _DefaultUnlockLv;
		id = ID;
		transform.localScale = Vector3.one;
		transform.localPosition = pos;
		for (int i = 0; i <= AstrolabeManager.Sides * AstrolabeManager.MaxLevel; i++) {
			stars.Add (Star.Create (transform, id, i));
		}
		for (int i = 0; i < stars.Count; i++) {
			stars [i].Init (i <= level * AstrolabeManager.Sides);
		}
	}

	LuaTable _StarsTable = null;

	public void Init (LuaTable table)
	{
		id = LuaWorker.GetFieldInt (table, "id");
		transform.localScale = Vector3.one;
		LuaTable _UnlockTable = LuaWorker.GetFieldTable (table, "unlock");
		unlockLv = LuaWorker.GetFieldInt (_UnlockTable, "lv");
		if (unlockLv <= 0)
			unlockLv = _DefaultUnlockLv;
		evo = LuaWorker.GetFieldInt (_UnlockTable, "evo");
		LuaTable _PosTable = LuaWorker.GetFieldTable (table, "pos");
		Vector3 pos = new Vector3 (LuaWorker.GetFieldFloat (_PosTable, 1), LuaWorker.GetFieldFloat (_PosTable, 2), LuaWorker.GetFieldFloat (_PosTable, 3));
		transform.localPosition = pos;

		_StarsTable = LuaWorker.GetFieldTable (table, "stars");
			
		for (int i = 0; i <= AstrolabeManager.Sides * AstrolabeManager.MaxLevel; i++) {
			stars.Add (Star.Create (transform, id, i));
		}
		for (int i = 0; i < stars.Count; i++) {
			stars [i].Init (false);
		}

		AstrolabeManager.instance.AddAstrolabe (this);
	}

	public void ResetStyle ()
	{
		bg.mainTexture = AstrolabeManager.instance.GetBgTexture ((AstrolabeManager.AstrolabelStyle)level);
		bg.MakePixelPerfect ();
	}

	public void ReBuildPath ()
	{
		if (_StarsTable != null) {
			foreach (var item in _StarsTable) {
				int index = int.Parse (item.key.ToString ());
				Star s = GetStarByIdx (index);
				s.Init (item.value as LuaTable);
			}
		}
	}

	public List<Star> GetAllCanConnectStar (Star star)
	{
		int Sides = AstrolabeManager.Sides;
		List <Star> list = new List<Star> ();
		switch (star.level) {
		case 0:
			list = stars.GetRange (1, Sides);
			break;
		case 1:
		case 2:
		case 3:
                //在当前圈的索引1-6
			int idx;
                //在当前星盘内的ID
			int _ID;
                //left
			_ID = (star.id + 1);
			if (_ID > star.level * Sides)
				_ID = (star.level - 1) * Sides + 1;
			list.Add (stars [_ID]);
                //right
			_ID = (star.id - 1);
			if (_ID < (star.level - 1) * Sides + 1)
				_ID = star.level * Sides;
			list.Add (stars [_ID]);
                //top
			idx = star.id + Sides;
			if (idx <= AstrolabeManager.MaxLevel * Sides)
				list.Add (stars [idx]);
                //bottom
			idx = star.id - Sides;
			if (idx <= 0)
				list.Add (stars [0]);
			else
				list.Add (stars [idx]);
			break;
		default:
			break;
		}
		return list;
	}

	public void UnLinkOuterConnects ()
	{
		for (int i = 0; i < stars.Count; i++) {
			for (int j = 0; j < stars [i].outerConnect.Count; j++) {
				stars [i].TryUnLink (stars [i].outerConnect [j]);
			}
		}
	}

	public void SyncLinkOuterConnects ()
	{
		for (int i = 0; i < stars.Count; i++) {
			for (int j = 0; j < stars [i].outerConnect.Count; j++) {
				if (!stars [i].outerConnect [j].transform.parent.hasChanged)
					stars [i].TryLink (stars [i].outerConnect [j]);
			}
		}
	}

	public Star GetStarByIdx (int idx)
	{
		for (int i = 0; i < stars.Count; i++) {
			if (stars [i].id == idx)
				return stars [i];
		}
		return null;
	}

	public void ModiffyUnlock (int evoValue, int lv)
	{
		if (lv > 0)
			unlockLv = lv;
		if (evoValue > 0)
			evo = evoValue;
	}

	void OnDestroy ()
	{
		AstrolabeManager.instance.RemoveAstrolabe (this);
	}
	#endif
}
