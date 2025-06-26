using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CircleDrawer : MonoBehaviour 
	{
		public new LineRenderer renderer;
		public int pieceCount = 30;

		public float radius;

		public void Draw()
		{
			if (null == renderer)
			{
				return;
			}
			if (0 < radius && 2 < pieceCount)
			{
				renderer.enabled = true;
				renderer.positionCount = pieceCount+1;

				Vector3 origin;
				Quaternion rotation;
				if (renderer.useWorldSpace)
				{
					origin = transform.position;
					rotation = transform.rotation;
				}
				else
				{
					origin = Vector3.zero;
					rotation = Quaternion.identity;
				}

				float pieceAngle = 360.0f / pieceCount;

				var p0 = origin + rotation * (Quaternion.identity * Vector3.forward * radius);
				renderer.SetPosition(0, p0);
				for (int i = 1; i < pieceCount; ++i)
				{
					var r = Quaternion.Euler(0, pieceAngle*i, 0);
					Vector3 p = origin + rotation * (r * Vector3.forward * radius);
					renderer.SetPosition(i, p);
				}
				renderer.SetPosition(pieceCount, p0);
			}
			else
			{
				renderer.positionCount = 0;
				renderer.enabled = false;
			}
		}
	}
} // namespace RO
