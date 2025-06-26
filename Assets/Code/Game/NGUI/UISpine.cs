using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[RequireComponent(typeof(NGUISpine))]
	[SLua.CustomLuaClassAttribute]
	public class UISpine : UIWidget
	{
		[HideInInspector]
		[SerializeField]
		public NGUISpine
			spine;
		[HideInInspector]
		[SerializeField]
		public Material
			spineMaterial;
		Vector3[] spineVertexs;
		Color32[] spineColors;
		Vector2[] spineUVs;
	
		public override Material material {
			get {
				return spineMaterial;
			}
			set {
				base.material = value;
			}
		}

		protected override void Awake ()
		{
			base.Awake ();
			if (spine == null)
				spine = gameObject.GetComponent<NGUISpine> ();
			spine.fillDrawCall = SpineFill;
		}

		/**
		 *  ngui drawcall 0 1 2 2 3 0
		 * 
		 * spine 0 2 1 2 3 1
		 * 
		 * */

		protected void SpineFill (Vector3[] vertex, Color32[] colors, Vector2[] uvs)
		{
			spineVertexs = vertex;
			spineColors = colors;
			spineUVs = uvs;
			mChanged = true;
		}

		public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
		{
			if (spineVertexs == null || spineUVs == null || spineColors == null)
				return;

			foreach (Vector3 v in spineVertexs) {
				verts.Add (v);
			}
			foreach (Color32 c in spineColors) {
				cols.Add (c);
			}
			foreach (Vector2 uv in spineUVs) {
				uvs.Add (uv);
			}
		}
//
//		public override void ParentHasChanged ()
//		{
//			base.ParentHasChanged ();
//		}
	}
} // namespace RO
