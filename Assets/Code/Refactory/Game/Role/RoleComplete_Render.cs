using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public partial class RoleComplete
	{
		private RoleRender_Alpha renderAlpha = new RoleRender_Alpha();
		private RoleRender_ChangeColor renderChangeColor = new RoleRender_ChangeColor();

		#region interface
		public void AlphaTo(float to, float duration)
		{
			var oldOn = renderAlpha.on;
			renderAlpha.LerpTo(to, duration);
			StartAlpha(oldOn);
		}

		public void AlphaFromTo(float from, float to, float duration)
		{
			var oldOn = renderAlpha.on;
			renderAlpha.LerpTo(from, to, duration);
			StartAlpha(oldOn);
		}

		private void StartAlpha(bool oldOn)
		{
			if (renderAlpha.running || renderAlpha.on)
			{
				if (useInstanceMaterial)
				{
					if (!oldOn)
					{
						for (int i = 0; i < parts.Length; ++i)
						{
							var part = parts[i];
							if (null != part)
							{
								part.SetBlendMode(RolePart.BlendMode.Transparent);
								part.SetAlpha(renderAlpha.current);
							}
						}
					}
				}
				else
				{
					useInstanceMaterial = true;
				}
			}
		}

		private void EndAlpha(bool oldOn)
		{
//			if (oldOn && !renderAlpha.on)
//			{
//				if (useInstanceMaterial)
//				{
//					for (int i = 0; i < parts.Length; ++i)
//					{
//						var part = parts[i];
//						if (null != part)
//						{
//							part.SetBlendMode(RolePart.BlendMode.Opaque);
//						}
//					}
//				}
//			}
		}

		public void ChangeColorTo(Color to, float duration)
		{
			var oldOn = renderChangeColor.on;
			renderChangeColor.LerpTo(to, duration);
			StartChangeColor(oldOn);
		}

		public void ChangeColorFromTo(Color from, Color to, float duration)
		{
			var oldOn = renderChangeColor.on;
			renderChangeColor.LerpFromTo(from, to, duration);
			StartChangeColor(oldOn);
		}

		private void StartChangeColor(bool oldOn)
		{
			if (renderChangeColor.running || renderChangeColor.on)
			{
				if (useInstanceMaterial)
				{
					if (!oldOn)
					{
						for (int i = 0; i < parts.Length; ++i)
						{
							var part = parts[i];
							if (null != part)
							{
								part.SetChangeColorEnable(true);
								part.SetChangeColor(renderChangeColor.current);
							}
						}
					}
				}
				else
				{
					useInstanceMaterial = true;
				}
			}
		}

		private void EndChangeColor(bool oldOn)
		{
			if (oldOn && !renderChangeColor.on)
			{
				if (useInstanceMaterial)
				{
					for (int i = 0; i < parts.Length; ++i)
					{
						var part = parts[i];
						if (null != part)
						{
							part.SetChangeColorEnable(false);
						}
					}
				}
			}
		}

		public void ApplyRender()
		{
			var newUseInstanceMaterial = useInstanceMaterial;
			if (newUseInstanceMaterial)
			{
				for (int i = 0; i < parts.Length; ++i)
				{
					var part = parts[i];
					if (null != part)
					{
						InitPart_Render(part);
					}
				}
			}
			else
			{
				for (int i = 0; i < parts.Length; ++i)
				{
					var part = parts[i];
					if (null != part)
					{
						UninitPart_Render(part);
					}
				}
			}
		}

		private bool _renderEnable = false;
		public bool renderEnable
		{
			get
			{
				return _renderEnable;
			}
			set
			{
				if (value == renderEnable)
				{
					return;
				}
				_renderEnable = value;
				ApplyRender();
			}
		}

		private bool _useInstanceMaterial = false;
		public bool useInstanceMaterial
		{
			get
			{
				return _useInstanceMaterial && renderEnable;
			}
			set
			{
				var oldUseInstanceMaterial = useInstanceMaterial;
				_useInstanceMaterial = value;
				var newUseInstanceMaterial = useInstanceMaterial;
				if (oldUseInstanceMaterial != newUseInstanceMaterial)
				{
					ApplyRender();
				}
			}
		}

		public void InitPart_Render(RolePart part)
		{
			if (useInstanceMaterial)
			{
				part.UseInstanceMaterials();
				if (renderAlpha.on)
				{
					part.SetBlendMode(RolePart.BlendMode.Transparent);
					part.SetAlpha(renderAlpha.current);
				}
				if (renderChangeColor.on)
				{
					part.SetChangeColorEnable(true);
					part.SetChangeColor(renderChangeColor.current);
				}
			}
		}

		public void UninitPart_Render(RolePart part)
		{
			part.UseOriginMaterials();
		}
		#endregion interface

		#region behaviour
		private void Update_Render()
		{
			var oldAlphaOn = renderAlpha.on;
			var oldChangeColorOn = renderChangeColor.on;

			var setAlpha = renderAlpha.running;
			if (setAlpha)
			{
				renderAlpha.Update();
			}
			var setChangeColor = renderChangeColor.running;
			if (setChangeColor)
			{
				renderChangeColor.Update();
			}

			if (useInstanceMaterial)
			{
				for (int i = 0; i < parts.Length; ++i)
				{
					var part = parts[i];
					if (null != part)
					{
						if (setAlpha)
						{
							part.SetAlpha(renderAlpha.current);
						}
						if (setChangeColor)
						{
							part.SetChangeColor(renderChangeColor.current);
						}
					}
				}

				if (!renderAlpha.on
				    && !renderChangeColor.on)
				{
					useInstanceMaterial = false;
				}
				else
				{
					EndAlpha(oldAlphaOn);
					EndChangeColor(oldChangeColorOn);
				}
			}
		}
		#endregion behaviour
	
	}
} // namespace RO
