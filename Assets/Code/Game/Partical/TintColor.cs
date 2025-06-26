using UnityEngine;
using System.Collections;

public class TintColor : MonoBehaviour {
	public Color col;
	const string TINT_COLOR = "_TintColor";
	Renderer render;
	// Use this for initialization
	void Start () {
		render = GetComponent<Renderer> ();
		col = render.material.GetColor(TINT_COLOR);
	}
	
	// Update is called once per frame
	void Update () {
		render.material.SetColor(TINT_COLOR, col);
	}
}
