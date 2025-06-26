using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class UIInputSelectDelegate : UIInput {

	public delegate void SelectCallback (bool isSelected);
	public SelectCallback callback;

	protected override void OnSelect (bool isSelected)
	{
		base.OnSelect (isSelected);

		if (callback != null)
			callback (isSelected);
	}
}
