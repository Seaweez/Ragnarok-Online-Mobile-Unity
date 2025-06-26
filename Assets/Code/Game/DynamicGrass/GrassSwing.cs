using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrassSwing : MonoBehaviour
{
	private Mesh mesh;
	private Vector3[] vertices;
	private Vector3[] originalVertices;
	private float[,] amplitudesAndHeights;
	public float amplitude = 1;
	private float height;
	private float time;

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
			// caculate amplitude base on height of vertice
			amplitudesAndHeights = new float[vertices.Length,2];
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
				float verticeAmplitude = amplitude * Mathf.Pow(verticeHeight, 2) * 1 / Mathf.Pow(height, 2);
				amplitudesAndHeights[i, 0] = verticeHeight;
				amplitudesAndHeights[i, 1] = verticeAmplitude;
			}
			amplitude = Mathf.Clamp(amplitude, 0, height);
		}
		else
		{
			RO.LoggerUnused.LogError("GameObject has no MeshFilter Component.");
		}
	}

	void Start () {
		
	}

	void Update () {
		time += Time.deltaTime;
		float swingValue = AlgorithmForSwing.SwingValueBaseTime(time, 10);
		for (int i = 0; i < vertices.Length; ++i)
		{
			Vector3 vertice = vertices[i];
			float height = amplitudesAndHeights[i, 0];
			float amplitude = amplitudesAndHeights[i, 1];
			float offsetX = swingValue * amplitude;
			float offsetZ = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(offsetX, 2));
			vertice.x = originalVertices[i].x + offsetX;
			vertice.z = offsetZ;
			vertices[i] = vertice;
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	public void Reset()
	{
		if (mesh != null)
		{
			mesh.vertices = originalVertices;
			mesh.RecalculateBounds();
		}
	}
}
