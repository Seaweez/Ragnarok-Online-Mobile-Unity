using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;
using Ghost.Attribute;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class Map2D : MonoBehaviour
	{
		[SerializeField, SetProperty("ID")]
		private int ID_ = 0;
		public int ID
		{
			get
			{
				return ID_;
			}
			set
			{
				ID_ = value;
				gameObject.name = string.Format("map2D_{0}", ID);
			}
		}

		public Vector2 size = new Vector2 (50, 50);
		private Vector3 min;
		private Vector3 max;

		private Texture m_cachedTexture;
		public Texture CachedTexture
		{
			get{return m_cachedTexture;}
		}

		public string textureSavePath{get;set;}

		private void Reset ()
		{
			var origin = transform.position;
			var offset = new Vector3 (size.x / 2.0f, 0, size.y / 2.0f);
			min = origin - offset;
			max = origin + offset;
		}

		public bool Contains(Vector2 p)
		{
			return new Rect(min.XZ(), size).Contains(p);
		}

		public Vector3 GetPosition (Vector2 coordinate01)
		{
			return NavMeshAdjustY.SamplePosition(new Vector3 (
				Mathf.Lerp (min.x, max.x, Mathf.Clamp01 (coordinate01.x)), 
				min.y, 
				Mathf.Lerp (min.z, max.z, Mathf.Clamp01 (coordinate01.y))));
		}

		public Vector2 GetCoordinate01 (Vector3 position)
		{
			return new Vector2 (
				(position.x - min.x) / (max.x - min.x),
				(position.z - min.z) / (max.z - min.z));
		}

		public void GetPositionByXY (float x, float y, out float px, out float py, out float pz)
		{
			var p =  NavMeshUtils.SamplePosition(new Vector3 (
				Mathf.Lerp (min.x, max.x, Mathf.Clamp01 (x)), 
				min.y, 
				Mathf.Lerp (min.z, max.z, Mathf.Clamp01 (y))));
			px = p.x;
			py = p.y;
			pz = p.z;
		}

		public void GetCoordinate01ByXZ (float x, float z, out float px, out float py)
		{
			var p = new Vector2 (
				(x - min.x) / (max.x - min.x),
				(z - min.z) / (max.z - min.z));
			px = p.x;
			py = p.y;
		}

		private List<System.Action> m_listCallbacksTextureIsGenerated;
		public void ListenTextureIsGenerated(System.Action action)
		{
			if (m_listCallbacksTextureIsGenerated == null)
			{
				m_listCallbacksTextureIsGenerated = new List<System.Action>();
			}
			if (action != null && !m_listCallbacksTextureIsGenerated.Contains(action))
			{
				m_listCallbacksTextureIsGenerated.Add(action);
			}
		}

		private void FireCallbacksTextureIsGenerated()
		{
			if (m_listCallbacksTextureIsGenerated != null && m_listCallbacksTextureIsGenerated.Count > 0)
			{
				for (int i = 0; i < m_listCallbacksTextureIsGenerated.Count; i++)
				{
					System.Action callback = m_listCallbacksTextureIsGenerated[i];
					if (callback != null)
					{
						callback();
						callback = null;
					}
				}
				m_listCallbacksTextureIsGenerated.Clear();
			}
		}

		private IEnumerator DoGetTexture()
		{
			yield return new WaitForEndOfFrame();
			NavMesh2Texture.GetTexture(this, (x) => {
				m_cachedTexture = x;
				FireCallbacksTextureIsGenerated();
			});
		}

		void Awake ()
		{
			transform.SetParent (null);
		}

		void Start ()
		{
			Reset ();

			if (0 == size.x || 0 == size.y
				|| !(null != Map2DManager.Me && Map2DManager.Me.Add(this))) 
			{
				GameObject.Destroy (gameObject);
			} 

			StartCoroutine(DoGetTexture());
		}

		void OnDestroy()
		{
			if (null != Map2DManager.Me)
			{
				Map2DManager.Me.Remove(this);
			}
		}

#if DEBUG_DRAW
		private void DrawSelf(Color color)
		{
			var p1 = min;
			var p2 = min;p2.x += size.x;
			var p3 = min;p3.z += size.y;
			var p4 = max;
			DebugUtils.DrawRect(p1, p2, p3, p4, color);

			Debug.DrawLine(p1, p4, color);
			Debug.DrawLine(p2, p3, color);
		}

		void OnDrawGizmos()
		{
			Reset();
			DrawSelf(Color.yellow);
		}
		
		void OnDrawGizmosSelected()
		{
			Reset();
			DrawSelf(Color.red);
		}
#endif // DEBUG_DRAW
	}
} // namespace RO