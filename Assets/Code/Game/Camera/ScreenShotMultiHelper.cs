using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ScreenShotMultiHelper : MonoBehaviour 
	{
		public float captureWidth = -1;
		public float captureHeight = 1080;
		public TextureFormat textureFormat = TextureFormat.RGB24;
		public int textureDepth = 24;
		public ScreenShot.AntiAliasing antiAliasing = ScreenShot.AntiAliasing.None;

		[System.Serializable]
		public class CameraList
		{
			public string suffix;
			public Camera[] cameras;
		}
		public CameraList[] cameraList;
		[HideInInspector]
		public string savePath;

		public void Setting(float width, float height, TextureFormat texFormat, int texDepth, ScreenShot.AntiAliasing aa)
		{
			captureWidth = width;
			captureHeight = height;
			textureFormat = texFormat;
			textureDepth = texDepth;
			antiAliasing = aa;
		}

		public void GetScreenShot(System.Action<Texture2D> callback, params object[] objs)
		{
			if (objs.IsNullOrEmpty())
			{
				return;
			}
			var cameras = new List<Camera>();
			foreach (var obj in objs)
			{
				cameras.Add(obj as Camera);
			}
			GetScreenShot (callback, cameras.ToArray());
		}

		[SLua.DoNotToLuaAttribute]
		public void GetScreenShot(System.Action<Texture2D> callback, Camera[] cameras)
		{
			StartCoroutine(DoGetScreenShot(callback, cameras));
		}

		private IEnumerator DoGetScreenShot(System.Action<Texture2D> callback, Camera[] cameras)
		{
			yield return new WaitForEndOfFrame();
			var oldWidth = ScreenShot.WIDTH;
			var oldHeight = ScreenShot.HEIGHT;
			var oldTextureFormat = ScreenShot.textureFormat;
			var oldTextureDepth = ScreenShot.textureDepth;
			var oldAntiAliasing = ScreenShot.antiAliasing;
			ScreenShot.WIDTH = captureWidth;
			ScreenShot.HEIGHT = captureHeight;
			ScreenShot.textureFormat = textureFormat;
			ScreenShot.textureDepth = textureDepth;
			ScreenShot.antiAliasing = antiAliasing;
			var texture = ScreenShot.Get(cameras);
			ScreenShot.WIDTH = oldWidth;
			ScreenShot.HEIGHT = oldHeight;
			ScreenShot.textureFormat = oldTextureFormat;
			ScreenShot.textureDepth = oldTextureDepth;
			ScreenShot.antiAliasing = oldAntiAliasing;
			callback(texture);
		}
	
	}
} // namespace RO
