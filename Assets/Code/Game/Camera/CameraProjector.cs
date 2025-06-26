using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraProjector : MonoBehaviour 
	{
		public Renderer targetRenderer = null;
		public Vector2 sampleSize = new Vector2(1024f, 1024f);
		
		private RenderTexture sampleTexture_ = null;
		private RenderTexture sampleTexture
		{
			get
			{
				return sampleTexture_;
			}
			set
			{
				if (value == sampleTexture)
				{
					return;
				}
				if (null != sampleTexture)
				{
					Object.DestroyImmediate(sampleTexture);
				}
				sampleTexture_ = value;
			}
		}
		private Vector2 currentSampleSize;
		private new Camera camera = null;

		private void Prepare()
		{
			if (null == sampleTexture || !Vector2.Equals(currentSampleSize, sampleSize))
			{
				sampleTexture = new RenderTexture((int)sampleSize.x, (int)sampleSize.y, 24);
				sampleTexture.useMipMap = false;
				sampleTexture.hideFlags = HideFlags.HideAndDontSave;
				sampleTexture.isPowerOfTwo = true;
				sampleTexture.name = "__CameraProjector" + GetInstanceID();

				currentSampleSize = sampleSize;
			}
			
			camera.targetTexture = sampleTexture;
		}

		void Reset()
		{
			camera = GetComponent<Camera>();
		}

		void Awake()
		{
			if (null == camera)
			{
				camera = GetComponent<Camera>();
			}
		}

		void Start()
		{
			if (null != camera)
			{
//				camera.enabled = false;
				camera.hideFlags = HideFlags.HideAndDontSave;
			}
		}

		void OnPreRender()
		{
			if (null != camera)
			{
				Prepare();
			}
		}

		void OnPostRender()
		{
			if (null != targetRenderer)
			{
				var mat = targetRenderer.material;
				if (null != mat)
				{
					mat.mainTexture = sampleTexture;
				}
			}
		}

		void OnDestroy()
		{
			Object.Destroy(sampleTexture);
		}
	}
} // namespace RO
