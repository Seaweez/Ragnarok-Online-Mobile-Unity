using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Attribute;

namespace RO
{
	public class SpriteFade : MonoBehaviour 
	{
		public enum Phase
		{
			INVISIBLE,
			VISIBLE,
			FADE_IN,
			FADE_OUT
		}

		[System.Serializable]
		public class SpriteInfo
		{
			public Sprite sprite = null;
			public Rect drawRect;
		}
		public SpriteInfo[] spriteInfos;
		[SerializeField, SetProperty("fadeDuration")]
		private float fadeDuration_ = 1;
		public float fadeDuration
		{
			get
			{
				return fadeDuration_;
			}
			set
			{
				fadeDuration_ = value;
				fadeSpeed = 1f / fadeDuration;
			}
		}

		public bool visible = true;

		private Material fadeMaterial = null;
		public Phase phase{get;private set;}
		private float fadeSpeed = 1f;

		private Color fadeColor;
		public float alpha
		{
			get
			{
				return fadeColor.a;
			}
			private set
			{
				fadeColor.a = Mathf.Clamp01(value);
			}
		}

		public void FadeIn()
		{
			gameObject.SetActive(true);
			phase = Phase.FADE_IN;
		}

		public void FadeOut()
		{
			phase = Phase.FADE_OUT;
		}

		public void FullScreen()
		{
			if (!spriteInfos.IsNullOrEmpty())
			{
				var screenRect = new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height));
				foreach (var info in spriteInfos)
				{
					info.drawRect = screenRect;
				}
			}
		}

		private void KeepVisible()
		{
			phase = Phase.VISIBLE;
		}

		private void KeepInvisible()
		{
			gameObject.SetActive(false);
			phase = Phase.INVISIBLE;
		}

		void Awake()
		{
			fadeMaterial = new Material(Shader.Find("Sprites/Default"));
			fadeColor = fadeMaterial.color;
			alpha = 0;
			fadeSpeed = 1f / fadeDuration;
			phase = Phase.INVISIBLE;
		}

		void Update()
		{
			switch (phase)
			{
			case Phase.INVISIBLE:
				gameObject.SetActive(false);
				break;
			case Phase.FADE_IN:
				if (1f > alpha)
				{
					alpha += fadeSpeed * Time.deltaTime;
				}
				else
				{
					KeepVisible();
				}
				break;
			case Phase.VISIBLE:
				break;
			case Phase.FADE_OUT:
				if (0f < alpha)
				{
					alpha -= fadeSpeed * Time.deltaTime;
				}
				else
				{
					KeepInvisible();
				}
				break;
			}
		}

		void OnGUI()
		{
			if (visible && !spriteInfos.IsNullOrEmpty() && EventType.Repaint.Equals(Event.current.type))
			{
				foreach (var spriteInfo in spriteInfos)
				{
					if (null == spriteInfo || null == spriteInfo.sprite)
					{
						continue;
					}
					var sprite = spriteInfo.sprite;
					var texture = sprite.texture;
					var textureSize = new Vector2(texture.width, texture.height);
					var textureRect = sprite.textureRect;

					var sourceRect = new Rect(
						textureRect.x/textureSize.x, textureRect.y/textureSize.y, 
						textureRect.width/textureSize.x, textureRect.height/textureSize.y);
					Graphics.DrawTexture(
						spriteInfo.drawRect, 
						texture, 
						sourceRect, 
						(int)sprite.border.x,(int)sprite.border.y,(int)sprite.border.z,(int)sprite.border.w,
						fadeColor,
						fadeMaterial);
				}
			}
		}
	}
} // namespace RO
