using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class PolygonSprite : MonoBehaviour
	{
		public bool isGradient = true;
		public float defaultLength = 100;
		public Shader useShader;
		public Material mat;
		[HideInInspector]
		[SerializeField]
		protected float
			_vertexColorLength = 100;
		[HideInInspector]
		[SerializeField]
		protected bool
			_useVertexColor;
		[SerializeField]
		protected MeshFilter
			meshFilter;
		[SerializeField]
		protected Mesh
			mesh;
		[SerializeField]
		protected MeshRenderer
			meshRender;
		[HideInInspector]
		[SerializeField]
		[Range(3,12)]
		protected int
			mSideNum = 3;
		[HideInInspector]
		[SerializeField]
		protected Color
			mgradientOutside = Color.white;
		[HideInInspector]
		[SerializeField]
		protected Color
			mgradientInside = Color.black;

		public float vertexColorLength {
			get {
				return _vertexColorLength;
			}
			set {
				value = Mathf.Max (1, value);
				if (_vertexColorLength != value) {
					_vertexColorLength = value;
					ColorChange ();
				}
			}
		}

		public bool useVertexColor {
			get {
				return _useVertexColor;
			}
			set {
				if (_useVertexColor != value) {
					_useVertexColor = value;
					ColorChange ();
				}
			}
		}

		public Color gradientOutside {
			get{ return mgradientOutside;}
			set {
				if (mgradientOutside != value) {
					mgradientOutside = value;
					ColorChange ();
				}
			}
		}

		public Color gradientInside {
			get{ return mgradientInside;}
			set {
				if (mgradientInside != value) {
					mgradientInside = value;
					ColorChange ();
				}
			}
		}

		public int SideNum {
			get{ return mSideNum;}
			set {
				if (mSideNum != value) {
					mSideNum = value;
					ReBuildPolygon ();
				}
			}
		}

		public List<float> lengths {
			get{ return polygonLengths;}
		}

		[HideInInspector]
		[SerializeField]
		protected List<float>
			polygonLengths = new List<float> ();
		[HideInInspector]
		[SerializeField]
		protected List<Vector3>
			polygonVerts = new List<Vector3> ();
		[HideInInspector]
		[SerializeField]
		protected List<Vector2>
			polygonUVs = new List<Vector2> ();
		[HideInInspector]
		[SerializeField]
		protected List<Color32>
			polygonColors = new List<Color32> ();
		[HideInInspector]
		[SerializeField]
		protected List<int>
			indices = new List<int> ();

		void Awake ()
		{
			Reset ();
		}

		void Reset ()
		{
			if (meshFilter == null) {
				meshFilter = GetComponent<MeshFilter> ();
			}
			if (meshFilter.sharedMesh == null) {
				mesh = newMesh ();
				meshFilter.sharedMesh = mesh;
			} else
				mesh = meshFilter.sharedMesh;
			if (meshRender == null) {
				meshRender = GetComponent<MeshRenderer> ();
			}
			if (meshRender.sharedMaterial == null) {
				meshRender.sharedMaterial = GetMat ();
			}
			if (polygonVerts.Count == 0)
				ReBuildPolygon ();
		}

		Material GetMat ()
		{
			GetShader ();
			if (this.mat == null && this.useShader != null) {
				this.mat = new Material (this.useShader);
			}
			return this.mat;
		}

		Shader GetShader ()
		{
			if (this.useShader == null) {
				this.useShader = Shader.Find ("Custom/SolidColor");
			}
			return this.useShader;
		}

		Mesh newMesh ()
		{
			Mesh mesh = new Mesh ();
			mesh.name = "polygonMesh";
//			mesh.hideFlags = HideFlags.HideAndDontSave;
			mesh.MarkDynamic ();
			return mesh;
		}

		[ContextMenu("Execute")]
		public void ReBuildPolygon ()
		{
			Clear ();
			mSideNum = Mathf.Max (3, mSideNum);
			Quaternion q = Quaternion.Euler (0, 0, -360 / mSideNum);
			Vector3 start = new Vector3 (0, defaultLength, 0);
			Vector3 center = this.center;
			polygonVerts.Add (center);
			polygonColors.Add (mgradientInside);
			for (int i=0; i<mSideNum; i++) {
				polygonLengths.Add (defaultLength);
				if (i != 0)
					start = q * start;
				//vertex
				polygonVerts.Add (start);
				//colors
				polygonColors.Add (mgradientOutside);
			}
			for (int i=0; i<mSideNum; i++) {
				//indices
				indices.Add (0);
				indices.Add (i + 1);
				if (i == mSideNum - 1)
					indices.Add (1);
				else
					indices.Add (i + 2);
			}
			if(mesh != null)
			{
				mesh.Clear ();
				mesh.vertices = polygonVerts.ToArray ();
				mesh.colors32 = polygonColors.ToArray ();
				mesh.triangles = indices.ToArray ();
			}
			
			//			mesh.UploadMeshData();
//			TestPrint<Vector3> (polygon		Verts);
//			TestPrint<int> (indices);
//			TestPrint<Vector3> (polygonVerts);
		}

		void TestPrint<T> (BetterList<T> list)
		{
			for (int i=0; i<list.size; i++) {
				print (list [i]);
			}
		}

		Vector3 center {
			get{ return Vector3.zero;}
		}

		public void SetLength (int index, float length)
		{
			if (index >= 0 && index < polygonLengths.Count) {
				polygonLengths [index] = length;
				Vector3 start = new Vector3 (0, length, 0);
				if (index > 0) {
					Quaternion q = Quaternion.Euler (0, 0, (-360 / mSideNum) * index);
					start = q * start;
				}
				polygonVerts [index + 1] = start;
				if(mesh != null)
					mesh.vertices = polygonVerts.ToArray ();
			}
			if (_useVertexColor)
				ColorChange ();
		}

		void ColorChange ()
		{
			polygonColors [0] = mgradientInside;
			if (_useVertexColor) {
				for (int i=1; i<polygonColors.Count; i++) {
					polygonColors [i] = Color.Lerp (mgradientInside, mgradientOutside, Mathf.Clamp (polygonLengths [i-1] / _vertexColorLength, 0, 1));
				}
			} else {
				for (int i=1; i<polygonColors.Count; i++)
					polygonColors [i] = mgradientOutside;
			}
			if(mesh != null)
				mesh.colors32 = polygonColors.ToArray ();
		}

		void Clear ()
		{
			polygonLengths.Clear ();
			polygonVerts.Clear ();
			polygonUVs.Clear ();
			polygonColors.Clear ();
			indices.Clear ();
		}

		protected void GradientColor (BetterList<Color32> cols)
		{
			int count = cols.size;
			count /= 4;
			cols.Clear ();

		}

		protected void HAddColor (BetterList<Color32> cols, Color left, Color right)
		{
			cols.Add (left);
			cols.Add (left);
			cols.Add (right);
			cols.Add (right);
		}

		protected void VAddColor (BetterList<Color32> cols, Color top, Color bottom)
		{
			cols.Add (bottom);
			cols.Add (top);
			cols.Add (top);
			cols.Add (bottom);
		}
	}
} // namespace RO