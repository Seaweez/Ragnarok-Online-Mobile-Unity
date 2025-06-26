using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GrassSlopeV2 : MonoBehaviour
{
	private static int forceIDGenerator;
	private class Force
	{
		private int _id;
		public int id
		{
			get
			{
				return _id;
			}
		}
		public Vector2 direction;
		public int value;
		
		public Force(int id, Vector2 direction, int value)
		{
			this._id = id;
			this.direction = direction;
			this.value = value;
		}
	}
	private List<Force> cachedForces;

	private enum E_ShapeChangeType
	{
		ConstantDeceleration
	}
	private E_ShapeChangeType shapeChangeType;

	private Mesh mesh;
	private Vector3[] vertices;
	private Vector3[] originalVertices;
	// horizontal
	public int shapeChangePercent;
	private float amplitudeMax;
	// for shape change, deceleration
	private float[,] speedsAndHeights;
	private float height;
	private float lowestVerticeZ;
	private float highestVerticeZ;
	private float consumeTime;
	private float currentAmplitudeX;
	private float currentAmplitudeY;
	private float needTime;
	private float accelerationX;
	private float accelerationY;
	private bool isShapeChanging;
	private bool done;
	// for shape change, blink
	private float[,] amplitudesAndHeights;
	private float targetAmplitudeX;
	private float targetAmplitudeY;


	void Awake ()
	{
		MeshFilter mf = gameObject.GetComponent<MeshFilter>();
		if (mf != null)
		{
			mesh = mf.mesh;
			vertices = mesh.vertices;
			originalVertices = new Vector3[vertices.Length];
			for (int i = 0; i < vertices.Length; ++i)
			{
				originalVertices[i] = vertices[i];
			}

			// caculate height of grass
			float[] zValues = new float[vertices.Length];
			for (int i = 0; i < vertices.Length; ++i)
			{
				zValues[i] = vertices[i].z;
			}
			AlgorithmForBubbleSort.BubbleSort(zValues);
			lowestVerticeZ = zValues[0];
			highestVerticeZ = zValues[zValues.Length - 1];
			height = highestVerticeZ - lowestVerticeZ;

			shapeChangePercent = Mathf.Clamp(shapeChangePercent, 0, 100);
			amplitudeMax = height * shapeChangePercent / 100;

			currentAmplitudeX = 0;
			currentAmplitudeY = 0;

			amplitudesAndHeights = new float[vertices.Length, 3];
			speedsAndHeights = new float[vertices.Length, 5];
			for (int i = 0; i < vertices.Length; ++i)
			{
				Vector3 vertice = vertices[i];
				float verticeHeight = vertice.z - lowestVerticeZ;
				amplitudesAndHeights[i, 2] = speedsAndHeights[i, 4] = verticeHeight;
			}
		}
		else
		{
			RO.LoggerUnused.LogError("GameObject has no MeshFilter Component.");
		}
	}

	void Update ()
	{
		if (!done && isShapeChanging)
		{
			float deltaOffsetXMax = 0;
			float deltaOffsetYMax = 0;
			for (int i = 0; i < vertices.Length; ++i)
			{
				Vector3 vertice = vertices[i];
				Vector3 originVertice = originalVertices[i];
				float initializeSpeedXForVertice = speedsAndHeights[i, 0];
				float initializeSpeedYForVertice = speedsAndHeights[i, 1];
				float accelerationXForVertice = speedsAndHeights[i, 2];
				float accelerationYForVertice = speedsAndHeights[i, 3];
				float verticeHeight = speedsAndHeights[i, 4];
				if (verticeHeight == 0)
				{
					continue;
				}
				if (shapeChangeType == E_ShapeChangeType.ConstantDeceleration)
				{
					float velocityX = AlgorithmForSwing.SwingVelocityBaseTimeAndDeceleration(initializeSpeedXForVertice, accelerationXForVertice, consumeTime);
					float velocityY = AlgorithmForSwing.SwingVelocityBaseTimeAndDeceleration(initializeSpeedYForVertice, accelerationYForVertice, consumeTime);
					float deltaOffsetX = velocityX * Time.deltaTime;
					float deltaOffsetY = velocityY * Time.deltaTime;
					if (Mathf.Abs(deltaOffsetX) > Mathf.Abs(deltaOffsetXMax))
					{
						deltaOffsetXMax = deltaOffsetX;
					}
					if (Mathf.Abs(deltaOffsetY) > Mathf.Abs(deltaOffsetYMax))
					{
						deltaOffsetYMax = deltaOffsetY;
					}
					vertice.x += deltaOffsetX;
					vertice.y += deltaOffsetY;
					float offsetX = vertice.x - originVertice.x;
					float offsetY = vertice.y - originVertice.y;
					float offsetXY = Mathf.Sqrt(Mathf.Pow(offsetX, 2) + Mathf.Pow(offsetY, 2));
					float offsetZ = Mathf.Sqrt(Mathf.Pow(verticeHeight , 2) - Mathf.Pow(offsetXY, 2));
					vertice.z = offsetZ;
				}
				vertices[i] = vertice;
			}
			currentAmplitudeX += deltaOffsetXMax;
			currentAmplitudeY += deltaOffsetYMax;
			mesh.vertices = vertices;
			mesh.RecalculateBounds();

			consumeTime += Time.deltaTime;
			if (consumeTime >= needTime)
			{
				if (consumeTime - Time.deltaTime >= needTime)
				{
					done = true;
					float deltaAmplitudeX = targetAmplitudeX - currentAmplitudeX;
					float deltaAmplitudeY = targetAmplitudeY - currentAmplitudeY;
					ShapeChangeWithBlink(deltaAmplitudeX, deltaAmplitudeY);
					return;
				}
				else
				{
					consumeTime = needTime;
				}
			}
		}
	}

	void OnDestroy()
	{
		Release();
	}

//	void OnGUI()
//	{
//		if (GUI.Button(new Rect(0, 0, 100, 50), "sc"))
//		{
//			AddSumForce(new Vector2(50, 50), 100, 1);
//		}
//		if (GUI.Button(new Rect(0, 50, 100, 50), "sc"))
//		{
//			AddSumForce(new Vector2(-50, -50), 100, 1);
//		}
//		if (GUI.Button(new Rect(0, 100, 100, 50), "sc"))
//		{
//			AddSumForce(new Vector2(-50, 50), 100, 0);
//		}
//		if (GUI.Button(new Rect(0, 150, 100, 50), "sc"))
//		{
//			AddSumForce(new Vector2(50, 50), 100, 0);
//		}
//	}

	private void ShapeChange(int xValue, int yValue)
	{
		
	}

	private void SetSumForce(Vector2 forceDirection, int forceValue, float duration)
	{
		forceValue = Mathf.Clamp(forceValue, 0, 100);
		forceDirection = forceDirection.normalized;
		Vector2 force = forceDirection * (forceValue * amplitudeMax / 100);
		float amplitudeX = force.x;
		float amplitudeY = force.y;
		targetAmplitudeX = amplitudeX;
		targetAmplitudeY = amplitudeY;
		float deltaAmplitudeX = targetAmplitudeX - currentAmplitudeX;
		float deltaAmplitudeY = targetAmplitudeY - currentAmplitudeY;
		ShapeChangeWithConstantDeceleration(deltaAmplitudeX, deltaAmplitudeY, duration);
	}

	/// <param name="speed">initialize speed</param>
	private void ShapeChangeWithConstantDeceleration(float amplitudeX, float amplitudeY, float duration)
	{
		if (Mathf.Abs(amplitudeX) > 0 || Mathf.Abs(amplitudeY) > 0)
		{
			if (duration > 0)
			{
				ResetShapeChanging();
				ResumeShapeChange();
				done = false;

				float v0X = 2 * amplitudeX / duration;
				float v0Y = 2 * amplitudeY / duration;

				// caculate amplitude base on height of vertice
				for (int i = 0; i < vertices.Length; ++i)
				{
					float verticeHeight = speedsAndHeights[i, 4];
					float verticeSpeedX = v0X * Mathf.Pow(verticeHeight, 2) * 1 / Mathf.Pow(height, 2);
					float verticeSpeedY = v0Y * Mathf.Pow(verticeHeight, 2) * 1 / Mathf.Pow(height, 2);
					accelerationX = - verticeSpeedX / duration;
					accelerationY = - verticeSpeedY / duration;
					speedsAndHeights[i, 0] = verticeSpeedX;
					speedsAndHeights[i, 1] = verticeSpeedY;
					speedsAndHeights[i, 2] = accelerationX;
					speedsAndHeights[i, 3] = accelerationY;
				}

				shapeChangeType = E_ShapeChangeType.ConstantDeceleration;

				needTime = duration;

				Vector2 currentAmplitude = new Vector2(currentAmplitudeX, currentAmplitudeY);
				Vector2 newAmplitude = new Vector2(amplitudeX, amplitudeY);
				Mathf.Cos(Mathf.Deg2Rad * Vector2.Angle(currentAmplitude, newAmplitude));
			}
			else
			{
				ShapeChangeWithBlink(amplitudeX, amplitudeY);
			}
		}
	}

	private void ShapeChangeWithBlink(float amplitudeX, float amplitudeY)
	{
		PauseShapeChange();
		ResetShapeChanging();
		done = true;

		// caculate initialize speed base on height of vertice
		for (int i = 0; i < vertices.Length; ++i)
		{
			float verticeHeight = amplitudesAndHeights[i, 2];
			float verticeAmplitudeX = amplitudeX * Mathf.Pow(verticeHeight, 2) * 1 / Mathf.Pow(height, 2);
			float verticeAmplitudeY = amplitudeY * Mathf.Pow(verticeHeight, 2) * 1 / Mathf.Pow(height, 2);
			amplitudesAndHeights[i, 0] = verticeAmplitudeX;
			amplitudesAndHeights[i, 1] = verticeAmplitudeY;
		}

		for (int i = 0; i < vertices.Length; ++i)
		{
			Vector3 vertice = vertices[i];
			Vector3 originVertice = originalVertices[i];
			float verticeAmplitudeX = amplitudesAndHeights[i, 0];
			float verticeAmplitudeY = amplitudesAndHeights[i, 1];
			float verticeHeight = amplitudesAndHeights[i, 2];
			if (verticeHeight == 0)
			{
				continue;
			}
			float deltaOffsetX = verticeAmplitudeX;
			float deltaOffsetY = verticeAmplitudeY;
			vertice.x += deltaOffsetX;
			vertice.y += deltaOffsetY;
			float offsetX = vertice.x - originVertice.x;
			float offsetY = vertice.y - originVertice.y;
			float offsetXY = Mathf.Sqrt(Mathf.Pow(offsetX, 2) + Mathf.Pow(offsetY, 2));
 			float offsetZ = Mathf.Sqrt(Mathf.Pow(verticeHeight , 2) - Mathf.Pow(offsetXY, 2));
			vertice.z = offsetZ;
			vertices[i] = vertice;
		}
		currentAmplitudeX += amplitudeX;
		currentAmplitudeY += amplitudeY;
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	private void ResetShapeChanging()
	{
		consumeTime = 0;
	}

	private void Reset()
	{
		PauseShapeChange();
		ResetShapeChanging();
		accelerationX = 0;
		accelerationY = 0;
		needTime = 0;
		currentAmplitudeX = 0;
		currentAmplitudeY = 0;
		done = false;
		shapeChangeType = E_ShapeChangeType.ConstantDeceleration;
		if (mesh != null)
		{
			mesh.vertices = originalVertices;
			mesh.RecalculateBounds();
		}
	}

	private void PauseShapeChange()
	{
		isShapeChanging = false;
	}

	private void ResumeShapeChange()
	{
		isShapeChanging = true;
	}

	private Vector2 GetForceSum()
	{
		Vector2 ret = Vector2.zero;
		if (cachedForces != null && cachedForces.Count > 0)
		{
			foreach (Force force in cachedForces)
			{
				Vector2 vec2Force = force.direction * force.value;
				ret += vec2Force;
			}
		}
		return ret;
	}

	public int AddForce(Vector2 forceDirection, int forceValue, float duration)
	{
		int ret = 0;
		if (forceDirection != Vector2.zero)
		{
			forceValue = Mathf.Clamp(forceValue, 0, 100);
			if (forceValue > 0)
			{
				Force force = new Force(++forceIDGenerator, forceDirection.normalized, forceValue);
				ret = force.id;
				if (cachedForces == null)
					cachedForces = new List<Force>();
				cachedForces.Add(force);
				Vector2 vec2Force = GetForceSum();
				SetSumForce(vec2Force.normalized, Mathf.FloorToInt(vec2Force.magnitude), duration);
			}
		}
		return ret;
	}

	public void ChangeForce(int id, Vector2 forceDirection, int forceValue, float duration)
	{
		if (cachedForces != null && cachedForces.Count > 0)
		{
			Force force = cachedForces.Find(x => x.id == id);
			if (force != null)
			{
				force.direction = forceDirection.normalized;
				force.value = forceValue;
				Vector2 vec2Force = GetForceSum();
				SetSumForce(vec2Force.normalized, Mathf.FloorToInt(vec2Force.magnitude), duration);
			}
		}
	}

	public void MinusForce(int id)
	{
		if (cachedForces != null && cachedForces.Count > 0)
		{
			Force force = cachedForces.Find(x => x.id == id);
			if (force != null)
			{
				cachedForces.Remove(force);
				Vector2 vec2Force = GetForceSum();
				SetSumForce(vec2Force.normalized, Mathf.FloorToInt(vec2Force.magnitude), 1);
			}
		}
	}

	private void Release()
	{
		if (cachedForces != null)
		{
			cachedForces.Clear();
			cachedForces = null;
		}
		mesh = null;
	}
}
