using UnityEngine;
using System.Collections;
using RO;
using System.Collections.Generic;

public class NavMesh2Texture
{
	private static Camera m_camera;
	private static Map2D m_map2D;
	private const float RIM_WIDTH = 0.5F;
	private const int TEXTURE_WIDTH = 512;
	private const int TEXTURE_HEIGHT = 512;

	private static string PNGPath {
		get{ return Application.persistentDataPath + "/NavMeshMap_" + m_map2D.ID + ".png";}
	}

	private static List<GameObject> m_listCachedDelegateDraw = new List<GameObject> ();
	private static System.Action<Texture> m_action;

	public static void Ready (Map2D map2D)
	{
		m_map2D = map2D;
		CreateCamera ();
		GenerateDelegateDraws ();
	}

	private static void CreateCamera ()
	{
		GameObject goCamera = new GameObject ("NavMeshCamera");
		m_camera = goCamera.AddComponent<Camera> ();
		Quaternion quaternion = m_camera.transform.rotation;
		m_camera.orthographic = true;
		m_camera.clearFlags = CameraClearFlags.SolidColor;
		m_camera.backgroundColor = Color.clear;
		m_camera.cullingMask = 1 << 17;
		Vector3 posMap2D = m_map2D.transform.position;
		m_camera.transform.position = posMap2D;
		Vector3 euler = quaternion.eulerAngles;
		euler.x = 90;
		quaternion.eulerAngles = euler;
		m_camera.transform.rotation = quaternion;
		float width = m_map2D.size.x;
		float height = m_map2D.size.y;
		float ratio = width / height;
		m_camera.aspect = ratio;
		m_camera.orthographicSize = height / 2;
	}

	private static UnityEngine.AI.NavMeshTriangulation GetMeshInfo ()
	{
		return UnityEngine.AI.NavMesh.CalculateTriangulation ();
	}

	private static void GenerateDelegateDraws ()
	{
		Transform transDelegateDraw = GenerateDelegateDraw ("MatNavMeshMap");
		Vector3 pos = transDelegateDraw.position;
		pos.y -= 50;
		transDelegateDraw.position = pos;
		Transform transDelegateDraw1 = GenerateDelegateDraw ("MatNavMeshMap1");
		Vector3 pos1 = transDelegateDraw.position;
		pos1.y -= 100;
		pos1.z += RIM_WIDTH;
		transDelegateDraw1.position = pos1;
		Transform transDelegateDraw2 = GenerateDelegateDraw ("MatNavMeshMap1");
		Vector3 pos2 = transDelegateDraw.position;
		float offset = RIM_WIDTH * Mathf.Cos (45 * Mathf.Deg2Rad);
		pos2.y -= 100;
		pos2.x += offset;
		pos2.z += offset;
		transDelegateDraw2.position = pos2;
		Transform transDelegateDraw3 = GenerateDelegateDraw ("MatNavMeshMap1");
		Vector3 pos3 = transDelegateDraw.position;
		pos3.y -= 100;
		pos3.x += RIM_WIDTH;
		transDelegateDraw3.position = pos3;
		Transform transDelegateDraw4 = GenerateDelegateDraw ("MatNavMeshMap1");
		Vector3 pos4 = transDelegateDraw.position;
		pos4.y -= 100;
		pos4.x += offset;
		pos4.z -= offset;
		transDelegateDraw4.position = pos4;
		Transform transDelegateDraw5 = GenerateDelegateDraw ("MatNavMeshMap1");
		Vector3 pos5 = transDelegateDraw.position;
		pos5.y -= 100;
		pos5.z -= RIM_WIDTH;
		transDelegateDraw5.position = pos5;
		Transform transDelegateDraw6 = GenerateDelegateDraw ("MatNavMeshMap1");
		Vector3 pos6 = transDelegateDraw.position;
		pos6.y -= 100;
		pos6.x -= offset;
		pos6.z -= offset;
		transDelegateDraw6.position = pos6;
		Transform transDelegateDraw7 = GenerateDelegateDraw ("MatNavMeshMap1");
		Vector3 pos7 = transDelegateDraw.position;
		pos7.y -= 100;
		pos7.x -= RIM_WIDTH;
		transDelegateDraw7.position = pos7;
		Transform transDelegateDraw8 = GenerateDelegateDraw ("MatNavMeshMap1");
		Vector3 pos8 = transDelegateDraw.position;
		pos8.y -= 100;
		pos8.x -= offset;
		pos8.z += offset;
		transDelegateDraw8.position = pos8;
		m_listCachedDelegateDraw.Add (transDelegateDraw.gameObject);
		m_listCachedDelegateDraw.Add (transDelegateDraw1.gameObject);
		m_listCachedDelegateDraw.Add (transDelegateDraw2.gameObject);
		m_listCachedDelegateDraw.Add (transDelegateDraw3.gameObject);
		m_listCachedDelegateDraw.Add (transDelegateDraw4.gameObject);
		m_listCachedDelegateDraw.Add (transDelegateDraw5.gameObject);
		m_listCachedDelegateDraw.Add (transDelegateDraw6.gameObject);
		m_listCachedDelegateDraw.Add (transDelegateDraw7.gameObject);
		m_listCachedDelegateDraw.Add (transDelegateDraw8.gameObject);
	}

	private static Transform GenerateDelegateDraw (string mat_name)
	{
		UnityEngine.AI.NavMeshTriangulation nmt = GetMeshInfo ();
		Vector3[] vertices = nmt.vertices;
		int[] indices = nmt.indices;
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = indices;
		GameObject delegateDraw = new GameObject ("DelegateDraw");
		delegateDraw.layer = 17;
		MeshFilter mf = delegateDraw.AddComponent<MeshFilter> ();
		mf.mesh = mesh;
		MeshRenderer mr = delegateDraw.AddComponent<MeshRenderer> ();
		Material mat = ResourceManager.Me.SLoad<Material> ("Public/Material/"+mat_name);
		mr.material = mat;
		return delegateDraw.transform;
	}

	private static Texture2D GenerateTexture ()
	{
		RenderTexture rt = RenderTexture.GetTemporary (TEXTURE_WIDTH, TEXTURE_HEIGHT, 24, RenderTextureFormat.Default);
		RenderTexture.active = rt;
		RenderTexture oldRT = m_camera.targetTexture;
		m_camera.targetTexture = rt;
		m_camera.Render ();
		Texture2D texture = new Texture2D (TEXTURE_WIDTH, TEXTURE_HEIGHT, TextureFormat.ARGB32, false);
		texture.ReadPixels (new Rect (0, 0, TEXTURE_WIDTH, TEXTURE_HEIGHT), 0, 0);
		texture.Apply ();
		m_camera.targetTexture = oldRT;
		RenderTexture.active = null;
		Object.Destroy (rt);
		return texture;
	}

	public static void SavePNG ()
	{
		Texture2D texture = GenerateTexture ();
		byte[] bytes = texture.EncodeToPNG ();
		System.IO.File.WriteAllBytes (PNGPath, bytes);
	}

	public static Texture Get (Map2D map2D)
	{
		Ready (map2D);
		Texture texture = GenerateTexture ();
		Reset ();
		return texture;
	}

	public static void GetTexture (Map2D map2D, System.Action<Texture> callBack)
	{
		m_action = callBack;
		Ready (map2D);

		var oldWidth = ScreenShot.WIDTH;
		var oldHeight = ScreenShot.HEIGHT;
		var oldTextureFormat = ScreenShot.textureFormat;
		var oldTextureDepth = ScreenShot.textureDepth;
		var oldAntiAliasing = ScreenShot.antiAliasing;
		ScreenShot.WIDTH = 512;
		ScreenShot.HEIGHT = 512;
		ScreenShot.textureFormat = TextureFormat.ARGB32;
		ScreenShot.textureDepth = 16;
		ScreenShot.antiAliasing = ScreenShot.AntiAliasing.Samples8;
		var texture = ScreenShot.Get (new Camera[]{m_camera});
		ScreenShot.WIDTH = oldWidth;
		ScreenShot.HEIGHT = oldHeight;
		ScreenShot.textureFormat = oldTextureFormat;
		ScreenShot.textureDepth = oldTextureDepth;
		ScreenShot.antiAliasing = oldAntiAliasing;

		if (null != m_action) {
			m_action (texture);
		}
		Reset ();

//		ScreenShotHelper screenShotHelper = map2D.gameObject.AddComponent<ScreenShotHelper>();
//		screenShotHelper.captureHeight = 512;
//		screenShotHelper.GetScreenShot((x) => {
//			Component.Destroy(screenShotHelper);
//			if (m_action != null)
//			{
//				m_action(x);
//			}
//			Reset();
//		}, m_camera);
	}

	public static void Reset ()
	{
		Release ();
	}

	private static void Release ()
	{
		for (int i = 0; i < m_listCachedDelegateDraw.Count; i++) {
			GameObject.Destroy (m_listCachedDelegateDraw [i]);
		}
		m_listCachedDelegateDraw.Clear ();
		GameObject.Destroy (m_camera.gameObject);
	}
}
