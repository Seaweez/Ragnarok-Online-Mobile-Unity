using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ScreenShot 
	{
		public static float WIDTH = -1f;
		public static float HEIGHT = -1f;

		public static TextureFormat textureFormat = TextureFormat.RGB24;
		public static int textureDepth = 24;

		[SLua.CustomLuaClassAttribute]
		public enum AntiAliasing
		{
			None = 0,
			Samples2 = 2,
			Samples4 = 4,
			Samples8 = 8,
		}
		public static AntiAliasing antiAliasing = AntiAliasing.None;

		public static Texture2D Get(Camera[] cameras)
		{
			if (cameras.IsNullOrEmpty())
			{
				return null;
			}

			var mainCamera = cameras[0];
			Vector2 leftBottom = mainCamera.ViewportToScreenPoint(Vector2.zero);
			Vector2 rightTop = mainCamera.ViewportToScreenPoint(Vector2.one);
			Rect rect;
			if (0 < WIDTH)
			{
				if (0 < HEIGHT)
				{
					rect = new Rect(0, 0, WIDTH, HEIGHT);
				}
				else
				{
					var height = (rightTop.y-leftBottom.y)/(rightTop.x-leftBottom.x) * WIDTH;
					rect = new Rect(0, 0, WIDTH, height);
				}
			}
			else
			{
				if (0 < HEIGHT)
				{
					var width = (rightTop.x-leftBottom.x)/(rightTop.y-leftBottom.y) * HEIGHT;
					rect = new Rect(0, 0, width, HEIGHT);
				}
				else
				{
					rect = new Rect(0, 0, rightTop.x-leftBottom.x, rightTop.y-leftBottom.y);
				}
			}

			RenderTexture renderTexture = new RenderTexture((int)rect.width, (int)rect.height, textureDepth);
			renderTexture.useMipMap = false;
			if (AntiAliasing.None != antiAliasing)
			{
				renderTexture.antiAliasing = (int)antiAliasing;
			}
			var oldActive = RenderTexture.active;
			RenderTexture.active = renderTexture;  
			
			foreach (var camera in cameras)
			{
				if(camera == null)
					continue;
				var oldTexture = camera.targetTexture;
				camera.targetTexture = renderTexture;  
				camera.Render();  
				camera.targetTexture = oldTexture;
			}
			
			var texture = new Texture2D((int)rect.width, (int)rect.height, textureFormat, false);
			texture.ReadPixels(rect, 0, 0);  
			texture.Apply();
			
			RenderTexture.active = oldActive; 
			Object.DestroyImmediate(renderTexture);
			
			return texture;
		}
		
		public static void SaveJPG(Texture2D texture, string path, int quality)
		{
			byte[] bytes = texture.EncodeToJPG(quality);
			path = Path.ChangeExtension(path, "jpg");
			DoSave(bytes, path);
		}

		public static void SaveJPG(Texture2D texture, string path)
		{
			SaveJPG(texture, path, 100);
		}

		public static void SavePNG(Texture2D texture, string path)
		{
			byte[] bytes = texture.EncodeToPNG();
			path = Path.ChangeExtension(path, "png");
			DoSave(bytes, path);
		}

		private static void DoSave(byte[] bytes, string path)
		{
			var directory = Path.GetDirectoryName(path);
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			
			FileStream writer = null;
			try
			{
				FileInfo file = new FileInfo(path);
				writer = file.OpenWrite();
				writer.Write(bytes, 0, bytes.Length);
			}
			finally
			{
				if (null != writer)
				{
					writer.Close();
					writer.Dispose();
				}
			}
		}
	
	}
} // namespace RO
