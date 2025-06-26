using UnityEngine;
using System.Collections;

public class TextureOffset : MonoBehaviour {

	public Renderer objRenderer = null;
	public string propertyName = string.Empty;
	public Vector2 offset;

	void Reset()
	{
		if (null == objRenderer)
		{
			objRenderer = GetComponent<Renderer> ();
		}
	}

	void Start () 
	{
		if (null == objRenderer)
		{
			objRenderer = GetComponent<Renderer> ();
		}
	}

	void Update () 
	{
		if (null != objRenderer)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				objRenderer.material.mainTextureOffset = offset;
			}
			else
			{
				objRenderer.material.SetTextureOffset(propertyName, offset);
			}
		}
	}
}
