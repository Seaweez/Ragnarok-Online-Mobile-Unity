using UnityEngine;
using System.Collections;
using System.IO;
using Ghost.Extensions;
using Ghost.Utils;

namespace Ghost.EditorTool
{
	public class RenderPictures : MonoBehaviour
	{
		public enum AntiAliasing
		{
			None = 0,
			Samples2 = 2,
			Samples4 = 4,
			Samples8 = 8,
		}

		public enum TextureType
		{
			JPG,
			PNG,
		}

		public Camera renderCamera;
		public float width = -1f;
		public float height = -1f;
		public TextureFormat textureFormat = TextureFormat.RGB24;
		public int textureDepth = 24;
		public AntiAliasing antiAliasing = AntiAliasing.None;
		public TextureType textureType = TextureType.JPG;
		public bool noOutline = false;
		public bool animationOn = true;

		public Transform node;
		public Object folder;
		public string suffix;

		private string path;
		private Coroutine renderProc = null;
		public bool running
		{
			get
			{
				return null != renderProc;
			}
		}

		public delegate string[] Func_GetGUIDs(Object folder, out string folderPath);
		public delegate GameObject Func_LoadObject(string guid, out string objPath);
		public delegate bool Func_SetAnimatorController(GameObject obj, string objPath);

		public delegate bool Func_DisplayCancelableProgressBar (string title, string info, float progress);
		public delegate void Func_ClearProgressBar();

		public void StartRender(
			string p, 
			Func_GetGUIDs GetGUIDs, 
			Func_LoadObject LoadObject, 
			Func_SetAnimatorController SetAnimatorController,
			Func_DisplayCancelableProgressBar DisplayCancelableProgressBar,
			Func_ClearProgressBar ClearProgressBar)
		{
			if (running)
			{
				return;
			}
			if (null == renderCamera 
				|| null == node
				|| null == folder
				|| string.IsNullOrEmpty(p))
			{
				return;
			}
			path = p;
			renderProc = StartCoroutine(DoRender(
				GetGUIDs, 
				LoadObject, 
				SetAnimatorController,
				DisplayCancelableProgressBar, 
				ClearProgressBar));
		}

		public void StopRender()
		{
			if (!running)
			{
				return;
			}
			StopCoroutine(renderProc);
			renderProc = null;
		}

		private IEnumerator DoRender(
			Func_GetGUIDs GetGUIDs, 
			Func_LoadObject LoadObject, 
			Func_SetAnimatorController SetAnimatorController,
			Func_DisplayCancelableProgressBar DisplayCancelableProgressBar,
			Func_ClearProgressBar ClearProgressBar)
		{
			GameObject objInstance = null;
			try
			{
				string folderPath;
				var guids = GetGUIDs(folder, out folderPath);
				if (!guids.IsNullOrEmpty())
				{
					System.Action<Texture2D, string> saveProc = null;
					switch (textureType)
					{
					case TextureType.JPG:
						saveProc = SaveJPG;
						break;
					case TextureType.PNG:
						saveProc = SavePNG;
						break;
					}

					var totalCount = guids.Length;

					var waitForEndOfFrame = new WaitForEndOfFrame();
					var waitForAnimation = new WaitForSeconds(0.1f);
					var rolePartShader = Shader.Find("RO/Role/Part");
					var rolePartOutLineShader = Shader.Find("RO/Role/PartOutline");

					int i = 0;
					foreach (var guid in guids)
					{
						if (null != objInstance)
						{
							GameObject.Destroy(objInstance);
							objInstance = null;
						}
						string objPath;
						var obj = LoadObject(guid, out objPath);

						objInstance = Object.Instantiate<GameObject>(obj);
						objInstance.transform.ResetParent(node);
				
						if (noOutline)
						{
							var renderers = objInstance.GetComponentsInChildren<Renderer>();
							if (!renderers.IsNullOrEmpty())
							{
								foreach (var r in renderers)
								{
									var mats = r.materials;
									if (!mats.IsNullOrEmpty())
									{
										foreach (var m in mats)
										{
											if (rolePartOutLineShader == m.shader)
											{
												m.shader = rolePartShader;
											}
										}
										r.materials = mats;
									}
								}
							}
						}

						if (animationOn && SetAnimatorController(objInstance, objPath))
						{
							yield return waitForAnimation;
						}

						yield return waitForEndOfFrame;

						if (null == renderCamera 
							|| null == node
							|| string.IsNullOrEmpty(path))
						{
							break;
						}
//						if (!Directory.Exists(path))
//						{
//							Directory.CreateDirectory(path);
//						}
						saveProc(Get(), PathUnity.Combine(path, obj.name+suffix));
						++i;
						if (DisplayCancelableProgressBar(
							string.Format("Proccessing Folder: {0}", folderPath), 
							string.Format("{0}/{1}, {2}", i, totalCount, obj.name), 
							(float)i/totalCount))
						{
							break;
						}
					}
				}
			}
			finally
			{
				if (null != objInstance)
				{
					GameObject.Destroy(objInstance);
				}
				renderProc = null;
				ClearProgressBar();
			}
		}

		private Texture2D Get()
		{
			Vector2 leftBottom = renderCamera.ViewportToScreenPoint(Vector2.zero);
			Vector2 rightTop = renderCamera.ViewportToScreenPoint(Vector2.one);
			Rect rect;
			if (0 < width)
			{
				if (0 < height)
				{
					rect = new Rect(0, 0, width, height);
				}
				else
				{
					var h = (rightTop.y-leftBottom.y)/(rightTop.x-leftBottom.x) * width;
					rect = new Rect(0, 0, width, h);
				}
			}
			else
			{
				if (0 < height)
				{
					var w = (rightTop.x-leftBottom.x)/(rightTop.y-leftBottom.y) * height;
					rect = new Rect(0, 0, w, height);
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

			var oldTexture = renderCamera.targetTexture;
			renderCamera.targetTexture = renderTexture;  
			renderCamera.Render();  
			renderCamera.targetTexture = oldTexture;

			var texture = new Texture2D((int)rect.width, (int)rect.height, textureFormat, false);
			texture.ReadPixels(rect, 0, 0);  
			texture.Apply();

			RenderTexture.active = oldActive; 
			Object.DestroyImmediate(renderTexture);

			return texture;
		}

		private static void SaveJPG(Texture2D texture, string path, int quality)
		{
			byte[] bytes = texture.EncodeToJPG(quality);
			path = Path.ChangeExtension(path, "jpg");
			DoSave(bytes, path);
		}

		private static void SaveJPG(Texture2D texture, string path)
		{
			SaveJPG(texture, path, 100);
		}

		private static void SavePNG(Texture2D texture, string path)
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
} // namespace Ghost.EditorTool
