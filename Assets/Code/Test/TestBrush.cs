using UnityEngine;
using System.Collections.Generic;

namespace RO.Test
{
	public class TestBrush : MonoBehaviour 
	{
		public string brushShader;
		public Texture brushShape;
		public float brushScale = 1f;
		public Color brushColor = Color.black;
		public int brushStepDistance = 1;

		public RenderTextureFormat canvasFormat = RenderTextureFormat.ARGB32;
		public int canvasDepth = 24;
		public Color canvasColor = Color.white;
		public RenderTexture canvas{get;private set;}
		private bool requestRebuildCavas = false;
		private Material mat;

		private bool drawing = false;
		private Vector2 prevPoint = Vector2.zero;

		private void StartDraw()
		{
			drawing = true;
			prevPoint = Input.mousePosition;
			DrawBrush(prevPoint);
		}

		private void EndDraw()
		{
			drawing = false;
		}

		private void DoDraw()
		{
			var point = Input.mousePosition;
			var dist = Vector2.Distance(prevPoint, point);

			if (1 > brushStepDistance)
			{
				brushStepDistance = 1;
			}
			if (brushStepDistance > dist)
			{
				return;
			}

			Vector2 currentPoint = prevPoint;
			var i_Dist = (int)dist;
			for (int i = brushStepDistance; i <= i_Dist; i += brushStepDistance)  
			{
				currentPoint = Vector2.Lerp(prevPoint, point, (float)i/i_Dist);
				DrawBrush(currentPoint);  
			}

			prevPoint = currentPoint;
		}

		private void DrawBrush(Vector2 p)
		{
			if (null == brushShape)
			{
				return;
			}
			var destRect = new Rect(p.x, p.y, brushShape.width, brushShape.height);
			var left = destRect.xMin - destRect.width * brushScale / 2.0f;  
			var right = destRect.xMin + destRect.width * brushScale / 2.0f;  
			var top = destRect.yMin - destRect.height * brushScale / 2.0f;  
			var bottom = destRect.yMin + destRect.height * brushScale / 2.0f;  
			
			Graphics.SetRenderTarget (canvas);
			
			GL.PushMatrix();  
			GL.LoadOrtho();  
			
			mat.SetTexture("_MainTex", brushShape);  
			mat.SetColor ("_Color", brushColor);
			mat.SetPass(0);
			
			GL.Begin(GL.QUADS);
			
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(left/Screen.width, top/Screen.height, 0);  
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(right/Screen.width, top/Screen.height, 0);      
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(right/Screen.width, bottom/Screen.height, 0);  
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(left/Screen.width, bottom/Screen.height, 0);  
			
			GL.End();
			GL.PopMatrix();
		} 

		private void DrawUpdate()
		{
			var mousePressed = Input.GetMouseButton(0);
			if (drawing)
			{
				if (mousePressed)
				{
					DoDraw();
				}
				else
				{
					EndDraw();
				}
			}
			else
			{
				if (mousePressed)
				{
					StartDraw();
				}
			}
		}

		public void Clear()  
		{
			Graphics.SetRenderTarget (canvas);       
			GL.PushMatrix();  
			GL.Clear(true, true, canvasColor);  
			GL.PopMatrix();
		} 

		public void RebuildCanvas(bool immediate = true)
		{
			if (immediate)
			{
				DoRebuildCanvas();
			}
			else
			{
				requestRebuildCavas = true;
			}
		}

		private void DoRebuildCanvas()
		{
			if (null != canvas)
			{
				RenderTexture.DestroyImmediate(canvas);
			}
			canvas = new RenderTexture (Screen.width, Screen.height, canvasDepth, canvasFormat);  
			Clear();
		}
		
		#region behaviours
		void Start () 
		{  
			RebuildCanvas();
			mat = new Material(Shader.Find(brushShader));
		}

		void OnDestroy()
		{
			if (null != canvas)
			{
				RenderTexture.DestroyImmediate(canvas);
			}
			if (null != mat)
			{
				Material.DestroyImmediate(mat);
			}
		}

		void Update()
		{
			if (requestRebuildCavas)
			{
				DoRebuildCanvas();
				requestRebuildCavas = false;
			}
			DrawUpdate();
		}

		void OnGUI()
		{
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), canvas, ScaleMode.StretchToFill);
		}
		#endregion behaviours
	
	}
} // namespace RO.Test
