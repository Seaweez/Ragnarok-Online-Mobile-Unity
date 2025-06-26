using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class UILabelClickUrl : MonoBehaviour {

	public delegate void UrlCallback (string url);
	public UrlCallback callback;

	private UILabel lbl;

	// Use this for initialization
	void Start () {
		lbl = GetComponent<UILabel> ();
	}
	
	void OnClick()
	{
		if (lbl != null) {
			string url = lbl.GetUrlAtPosition(UICamera.lastHit.point);
			if (!string.IsNullOrEmpty(url))
			{
				if(callback != null)
				{
					callback(url);
				}
			}
		}
	}
}
