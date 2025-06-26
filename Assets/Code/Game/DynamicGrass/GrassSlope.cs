using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrassSlope : MonoBehaviour
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
		public int changeRate;
		public float time;

		public Force(int id, Vector2 direction, int value, int changeRate)
		{
			this._id = id;
			this.direction = direction;
			this.value = value;
			this.changeRate = changeRate;
			this.time = 0;
		}

		public bool isComplete
		{
			get
			{
				return time >= changeRate;
			}
		}
	}

	private Mesh mesh;
	private Vector3[] vertices;
	private Vector3[] originalVertices;
	private float[,] unitAmplitudesAndHeights;
	public float amplitude = 1;
	private float height;
	
	private List<Force> cachedForces;
	
	void Awake()
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
			// caculate unit amplitude base on height of vertice
			unitAmplitudesAndHeights = new float[vertices.Length,2];
			float[] zValues = new float[vertices.Length];
			for (int i = 0; i < vertices.Length; ++i)
			{
				zValues[i] = vertices[i].z;
			}
			AlgorithmForBubbleSort.BubbleSort(zValues);
			float low = zValues[0];
			float high = zValues[zValues.Length - 1];
			height = high - low;
			for (int i = 0; i < vertices.Length; ++i)
			{
				Vector3 vertice = vertices[i];
				float verticeHeight = vertice.z - low;
				float verticeUnitAmplitude = Mathf.Pow(verticeHeight, 2) * 1 / Mathf.Pow(height, 2);
				unitAmplitudesAndHeights[i, 0] = verticeHeight;
				unitAmplitudesAndHeights[i, 1] = verticeUnitAmplitude;
			}
			amplitude = Mathf.Clamp(amplitude, 0, height);
		}
		else
		{
			RO.LoggerUnused.LogError("GameObject has no MeshFilter Component.");
		}
	}
	
	void Start () {
		AddForce(new Vector2(1, 0), 50, 3);
	}
	
	void Update () {
		if (cachedForces != null && cachedForces.Count > 0)
		{
			for (int j = cachedForces.Count - 1; j >= 0; --j)
			{
				Force force = cachedForces[j];
				force.time += Time.deltaTime;
				if (force.isComplete)
				{
					cachedForces.Remove(force);
				}
			}

			if (cachedForces.Count > 0)
			{
				for (int i = 0; i < vertices.Length; ++i)
				{
					Vector3 vertice = vertices[i];
					float height = unitAmplitudesAndHeights[i, 0];
					float unitAmplitude = unitAmplitudesAndHeights[i, 1];
					Vector2 offset = Vector2.zero;
					float offsetDeltaZ = 0;
					for (int j = cachedForces.Count - 1; j >= 0; --j)
					{
						Force force = cachedForces[j];
						Vector2 forceDirection = force.direction;
						int forceValue = force.value;
						int forceChangeRate = force.changeRate;
						// linear
						// float tempForceValue = forceValue * force.time / forceChangeRate;
						// fast followed by slow
						float tempForceValue = Mathf.Sqrt(force.time / (forceChangeRate / Mathf.Pow(forceValue, 2)));
						float amplitudeForForce = (amplitude * tempForceValue / 100) * unitAmplitude;
						Vector2 offsetForForce = forceDirection.normalized * amplitudeForForce;
						float offsetZForForce = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(offsetForForce.magnitude, 2));
						offset += offsetForForce;
						offsetDeltaZ += offsetZForForce - vertice.z;
					}
					vertice.x = originalVertices[i].x + offset.x;
					vertice.y = originalVertices[i].y + offset.y;
					vertice.z += offsetDeltaZ;
					vertices[i] = vertice;
				}
				mesh.vertices = vertices;
				mesh.RecalculateBounds();
			}
		}
	}
	
	public int AddForce(Vector2 forceDirection, int forceValue, int forceChangeRage)
	{
		if (forceDirection != Vector2.zero && forceValue > 0)
		{
			if (cachedForces == null)
				cachedForces = new List<Force>();
			Force force = new Force(++forceIDGenerator, forceDirection, forceValue, forceChangeRage);
			cachedForces.Add(force);
			return force.id;
		}
		return 0;
	}

	public void MinusForce(int id)
	{

	}
	
	public void Reset()
	{
		ResetVertices();
	}

	public void ResetVertices()
	{
		if (mesh != null)
		{
			mesh.vertices = originalVertices;
			mesh.RecalculateBounds();
		}
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 100, 50), "force"))
		{
			int x = Random.Range(1, 10);
			int y = Random.Range(1, 10);
			int forceValue = Random.Range(1, 100);
			int forceChangeRate = Random.Range(1, 10);
			AddForce(new Vector2(x, y), forceValue, forceChangeRate);
		}
	}
}