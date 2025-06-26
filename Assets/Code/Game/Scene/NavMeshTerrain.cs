using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class NavMeshTerrain : MonoBehaviour 
	{
		public Mesh navMesh;
		public MeshCollider meshCollider;

		private Material clearMat_ = null;
		private Material clearMat
		{
			get
			{
				return clearMat_;
			}
			set
			{
				if (value == clearMat_)
				{
					return;
				}
				if (null != clearMat)
				{
					Material.Destroy(clearMat);
				}
				clearMat_ = value;
			}
		}

		public void CreateMesh()
		{
			navMesh = NavMeshUtils.NavMesh2Mesh();
		}

		void Start()
		{
			CreateMesh();
			if (null == navMesh)
			{
				GameObject.Destroy(gameObject);
				return;
			}
			if (null == meshCollider)
			{
				if (null != navMesh)
				{
					meshCollider = GetComponent<MeshCollider>();
					if (null == meshCollider)
					{
						meshCollider = gameObject.AddComponent<MeshCollider>();
					}
					meshCollider.sharedMesh = navMesh;
				}
			}
			else
			{
				meshCollider.sharedMesh = navMesh;
			}

			gameObject.layer = Config.Layer.TERRAIN.Value;

			transform.ResetParent(null);

//			clearMat = new Material(Shader.Find("RO/Clear"));
//			gameObject.AddComponent<MeshFilter>().sharedMesh = navMesh;
//			var renderer = gameObject.AddComponent<MeshRenderer>();
//			renderer.sharedMaterial = clearMat;
//			OptimizationUtils.OptiRenderer(renderer);
		}

		void OnDestroy()
		{
			if (null != navMesh)
			{
				Mesh.Destroy(navMesh);
			}
//			clearMat = null;
		}
	}
} // namespace RO
