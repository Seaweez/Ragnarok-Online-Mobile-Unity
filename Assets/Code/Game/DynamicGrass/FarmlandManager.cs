using UnityEngine;
using System.Collections;
using RO;
using System.Collections.Generic;

[SLua.CustomLuaClassAttribute]
public class FarmlandManager : SingleTonGO<FarmlandManager>
{
	public static FarmlandManager Ins
	{
		get
		{
			return FarmlandManager.Me;
		}
	}
	private List<Farmland> cachedFarmlands;
	public Farmland[] CachedFarmlands
	{
		get
		{
			return cachedFarmlands.ToArray();
		}
	}

	protected override void Awake ()
	{
		base.Awake ();
//		cachedFarmlands = new List<Farmland>();
	}

	public void Add(Farmland farmland)
	{
		if (!cachedFarmlands.Contains(farmland))
		{
			cachedFarmlands.Add(farmland);
		}
	}

	public void Remove(Farmland farmland)
	{
		cachedFarmlands.Remove(farmland);
	}

	public Farmland Get(int id)
	{
		return cachedFarmlands.Find(x => x.id == id);
	}

	protected override void OnDestroy ()
	{
//		cachedFarmlands.Clear();
//		cachedFarmlands = null;
//
//		base.OnDestroy ();
	}
}
