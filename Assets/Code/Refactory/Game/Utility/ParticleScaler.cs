using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	public class ParticleScaler : MonoBehaviour 
	{
		public new Renderer renderer;

		void Reset()
		{
			renderer = GetComponent<Renderer>();
		}

		void Start()
		{
			if (null == renderer)
			{
				renderer = GetComponent<Renderer>();
			}
			if (null == renderer)
			{
				Component.Destroy(this);
			}
		}

		void OnWillRenderObject()
		{
			var mat = renderer.material;
			mat.SetVector("_Position", Camera.current.worldToCameraMatrix.MultiplyPoint(transform.root.position));
			mat.SetVector("_Scale", transform.lossyScale);
//			mat.SetVector("_Scale", new Vector3(transform.localScale.x * (transform.lossyScale.x < 0 ? -1.0f : 1.0f), transform.localScale.y * (transform.lossyScale.y < 0 ? -1.0f : 1.0f), transform.localScale.z * (transform.lossyScale.z < 0 ? -1.0f : 1.0f)));
		}
	
	}
} // namespace RO
