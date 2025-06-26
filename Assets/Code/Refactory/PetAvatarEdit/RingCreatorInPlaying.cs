using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCreatorInPlaying : MonoBehaviour
{
    public Color color;

    bool bInited = false;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (bInited) return;
        SetMesh(gameObject);
        GetComponent<Renderer>().material.color = color;
        if (!GetComponent<Collider>())
            gameObject.AddComponent<BoxCollider>();
        bInited = true;
    }

    public static void SetMesh(GameObject go)
    {
        if (null == go)
            return;
        //构建mesh
        Mesh myMesh = CreateMesh();
        myMesh.name = "Ring";
        myMesh.RecalculateBounds();
        myMesh.RecalculateNormals();
        myMesh.RecalculateTangents();
        //分配mesh
        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = myMesh;
        //分配材质
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        if (!mr)
            mr = go.AddComponent<MeshRenderer>();
        Material myMat = new Material(Shader.Find("RO/Color"));
        mr.sharedMaterial = myMat;
    }

    static Mesh CreateMesh()
    {
        float Radius = 1.0f;
        float InnerRadius = 0.95f;
        int Segments = 50;
        if (Radius <= 0)
            Radius = 1;

        if (InnerRadius <= 0)
            Radius = 1;

        if (Radius <= InnerRadius)
            InnerRadius = 0;

        float currAngle = Mathf.PI;
        int vertCount = 2 * (Segments * 1 + 1);
        float deltaAngle = 2 * currAngle / Segments;
        Vector3[] vertices = new Vector3[vertCount];
        for (int i = 0; i < vertCount; i += 2, currAngle -= deltaAngle)
        {
            float cosA = Mathf.Cos(currAngle);
            float sinA = Mathf.Sin(currAngle);
            vertices[i] = new Vector3(cosA * InnerRadius, sinA * InnerRadius, 0);
            vertices[i + 1] = new Vector3(cosA * Radius, sinA * Radius, 0);
        }

        int[] triangles = new int[3 * (vertCount - 2) * 2];
        for (int i = 0, j = 0; i < triangles.Length / 2; i += 6, j += 2)
        {
            triangles[i] = j + 1;
            triangles[i + 1] = j + 2;
            triangles[i + 2] = j + 0;
            triangles[i + 3] = j + 1;
            triangles[i + 4] = j + 3;
            triangles[i + 5] = j + 2;
        }
        for (int i = triangles.Length / 2, j = 0; i < triangles.Length; i += 6, j += 2)
        {
            triangles[i] = j + 1;
            triangles[i + 1] = j + 0;
            triangles[i + 2] = j + 2;
            triangles[i + 3] = j + 1;
            triangles[i + 4] = j + 2;
            triangles[i + 5] = j + 3;
        }

        Vector2[] uvs = new Vector2[vertCount];
        for (int i = 0; i < vertCount; ++i)
        {
            uvs[i] = new Vector2(vertices[i].x / Radius / 2 + 0.5f, vertices[i].y / Radius / 2 + 0.5f);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }
}
