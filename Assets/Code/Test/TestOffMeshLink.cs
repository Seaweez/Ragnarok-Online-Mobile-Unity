using UnityEngine;
using System.Collections.Generic;

namespace RO.Test
{
	public class TestOffMeshLink : MonoBehaviour 
	{
		public UnityEngine.AI.OffMeshLink offMeshLink = null;

		public bool occupied = false;

		void Reset()
		{
			if (null == offMeshLink)
			{
				offMeshLink = GetComponent<UnityEngine.AI.OffMeshLink>();
			}
		}

		void Start()
		{
			if (null == offMeshLink)
			{
				offMeshLink = GetComponent<UnityEngine.AI.OffMeshLink>();
			}
		}

		void Update()
		{
			if (null != offMeshLink)
			{
				occupied = offMeshLink.occupied;
			}
		}
	
	}
} // namespace RO.Test
