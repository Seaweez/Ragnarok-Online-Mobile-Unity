using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO.Test
{
	public class TestNavMesh : MonoBehaviour 
	{
		private UnityEngine.AI.NavMeshTriangulation info;

		void Reset()
		{
			info = UnityEngine.AI.NavMesh.CalculateTriangulation();
		}

		void Start()
		{
			info = UnityEngine.AI.NavMesh.CalculateTriangulation();
		}

		private void Draw()
		{
			if (info.vertices.IsNullOrEmpty())
			{
				return;
			}

//			var p1 = info.vertices[0];
//			var p2 = p1;
//			for (int i = 1; i < info.vertices.Length; ++i)
//			{
//				p2 = info.vertices[i];
//				Gizmos.DrawLine(p1, p2);
//				p1 = p2;
//			}
//			Gizmos.DrawLine(p2, p1);
			foreach (var p in info.vertices)
			{
				Gizmos.DrawSphere(p, 0.1f);
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Draw();
		}
		
		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Draw();
		}
	}
} // namespace RO.Test
