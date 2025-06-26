using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LuaGameObject : MonoBehaviour
	{
		public static bool ObjectIsNull (Object obj)
		{
			return null == obj;
		}

		public static new void DestroyObject (Object obj)
		{
			if (null != obj) {
				Object.Destroy (obj);
			}
		}

		public static void DestroyGameObject (Component obj)
		{
			if (null != obj && null != obj.gameObject) 
			{
				Object.Destroy (obj.gameObject);
			}
		}

		public static void RestoreBehaviours (GameObject go)
		{
			var behaviours = go.GetComponents<Behaviour>();
			if (!behaviours.IsNullOrEmpty())
			{
				for (int i = 0; i < behaviours.Length; ++i)
				{
					behaviours[i].enabled = true;
				}
			}
		}

		public static void SetRenderQueue (GameObject go, int renderQueue)
		{
			var renderers = go.GetComponentsInChildren<Renderer> ();
			if (renderers.IsNullOrEmpty ()) {
				return;
			}
			for (int i = 0; i < renderers.Length; ++i) {
				var mats = renderers [i].sharedMaterials;
				if (!mats.IsNullOrEmpty ()) {
					for (int j = 0; j < mats.Length; ++j) {
						var mat = mats [j];
						if (null != mat) {
							mat.renderQueue = renderQueue;
						}
					}
				}
			}
		}

		public static void SetUIDepth (GameObject go, int depth)
		{
			var uis = go.GetComponentsInChildren<UIWidget> ();
			if (uis.IsNullOrEmpty ()) {
				return;
			}
			for (int i = 0; i < uis.Length; ++i) {
				uis [i].depth = depth;
			}
		}

		public static void FollowTransform (Transform src, Transform target, Vector3 offset)
		{
			src.position = target.position + offset;
		}

		public static void FollowTransformWithRotation (Transform src, Transform target, Vector3 offset)
		{
			src.position = target.position + offset;
			src.rotation = target.rotation;
		}

		public static void GetForwardPosition (Transform t, float distance, out float x, out float y, out float z)
		{
			var p = t.position;
			p += t.forward * distance;
			x = p.x;
			y = p.y;
			z = p.z;
		}

		public static void GetPosition (Transform t, out float x, out float y, out float z)
		{
			var p = t.position;
			x = p.x;
			y = p.y;
			z = p.z;
		}

		public static void GetLocalPosition (Transform t, out float x, out float y, out float z)
		{
			var p = t.localPosition;
			x = p.x;
			y = p.y;
			z = p.z;
		}

		public static void GetRotation (Transform t, out float x, out float y, out float z, out float w)
		{
			var p = t.rotation;
			x = p.x;
			y = p.y;
			z = p.z;
			w = p.w;
		}

		public static void GetLocalRotation (Transform t, out float x, out float y, out float z, out float w)
		{
			var p = t.localRotation;
			x = p.x;
			y = p.y;
			z = p.z;
			w = p.w;
		}

		public static void GetEulerAngles (Transform t, out float x, out float y, out float z)
		{
			var p = t.eulerAngles;
			x = p.x;
			y = p.y;
			z = p.z;
		}

		public static void GetLocalEulerAngles (Transform t, out float x, out float y, out float z)
		{
			var p = t.localEulerAngles;
			x = p.x;
			y = p.y;
			z = p.z;
		}

		public static void GetScale (Transform t, out float x, out float y, out float z)
		{
			var p = t.lossyScale;
			x = p.x;
			y = p.y;
			z = p.z;
		}

		public static void GetLocalScale (Transform t, out float x, out float y, out float z)
		{
			var p = t.localScale;
			x = p.x;
			y = p.y;
			z = p.z;
		}

		public static void SetLocalEulerAngleY (Transform t, float angleY)
		{
			var p = t.localEulerAngles;
			p.y = angleY;
			t.localEulerAngles = p;
		}

		public static void LocalRotateToByAxisY (Transform t, Vector3 p)
		{
			var angleY = VectorHelper.GetAngleByAxisY (t.position, p);
			var angles = t.localEulerAngles;
			angles.y = angleY;
			t.localEulerAngles = angles;
		}

		public static void LocalRotateDeltaByAxisY (Transform t, float delta)
		{
			var angles = t.localEulerAngles;
			angles.y += delta;
			t.localEulerAngles = angles;
		}

		public static void InverseTransformPointByVector3 (Transform t, Vector3 p, out float x, out float y, out float z)
		{
			var tp = t.InverseTransformPoint (p);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		public static void InverseTransformPointByTransform (Transform t, Transform p, Space s, out float x, out float y, out float z)
		{
			var tp = t.InverseTransformPoint (s == Space.Self ? p.localPosition : p.position);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		public static void InverseTransformVector (Transform t, Vector3 p, out float x, out float y, out float z)
		{
			var tp = t.InverseTransformVector (p);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		public static void TransformPoint (Transform t,Vector3 p,out float x, out float y, out float z)
		{
			var tp = t.TransformPoint (p);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		public static void WorldToViewportPointByVector3 (Camera c, Vector3 p, out float x, out float y, out float z)
		{
			var tp = c.WorldToViewportPoint (p);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		public static void WorldToViewportPointByTransform (Camera c, Transform p, Space s,out float x, out float y, out float z)
		{
			var tp = c.WorldToViewportPoint (s == Space.Self ? p.localPosition : p.position);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		public static void ScreenToWorldPointByVector3 (Camera c, Vector3 p, out float x, out float y, out float z)
		{
			var tp = c.ScreenToWorldPoint (p);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}
		
		public static void ScreenToWorldPointByTransform (Camera c, Transform p, Space s,out float x, out float y, out float z)
		{
			var tp = c.ScreenToWorldPoint (s == Space.Self ? p.localPosition : p.position);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		public static void WorldToScreenPointByVector3 (Camera c, Vector3 p, out float x, out float y, out float z)
		{
			var tp = c.WorldToScreenPoint (p);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		public static void WorldToScreenPointByTransform (Camera c, Transform p, Space s, out float x, out float y, out float z)
		{
			var tp = c.WorldToScreenPoint (s == Space.Self ? p.localPosition : p.position);
			x = tp.x;
			y = tp.y;
			z = tp.z;
		}

		#region Input.Touch and mouse

		public static void GetTouchPosition(int index,bool isRaw ,out float x, out float y)
		{
			Touch touch = Input.GetTouch (index);
			var pos = isRaw ? touch.rawPosition : touch.position;
			x = pos.x;
			y = pos.y;
		}

		public static void GetTouchDeltaPosition(int index,out float x, out float y)
		{
			Touch touch = Input.GetTouch (index);
			var pos = touch.deltaPosition;
			x = pos.x;
			y = pos.y;
		}

		public static void GetMousePosition(out float x, out float y,out float z)
		{
			var pos = Input.mousePosition;
			x = pos.x;
			y = pos.y;
			z = pos.z;
		}

		#endregion

		const string CloneStr = "(Clone)";
		public static void TrimNameClone(GameObject go)
		{
			go.name = go.name.Replace (CloneStr, "");
		}

		public int type = 0;
		public int ID = 0;
		public string[] properties;
		public Component[] components;

		public bool registered{ get; private set; }

		public string GetProperty(int i)
		{
			if (null == properties || !properties.CheckIndex(i))
			{
				return null;
			}
			return properties[i];
		}

		public Component GetComponentProperty(int i)
		{
			if (null == components || !components.CheckIndex(i))
			{
				return null;
			}
			return components[i];
		}

		private IEnumerator TryRegister ()
		{
			while (!registered) {
				if (null != LuaLuancher.Me) {
					registered = System.Convert.ToBoolean (LuaLuancher.Me.Call ("RegisterGameObject", this));
				}
				yield return null;
			}
		}

		public void Register ()
		{
			if (registered) {
				return;
			}
			StartCoroutine (TryRegister ());
		}

		public void Unregister ()
		{
			if (!registered) {
				return;
			}
			if (null != LuaLuancher.Me) {
				System.Convert.ToBoolean (LuaLuancher.Me.Call ("UnregisterGameObject", this));
			}
			registered = false;
		}

		#region behaviour
		protected virtual void Awake ()
		{
			Register ();
		}

		protected virtual void OnDestroy ()
		{
			Unregister ();
		}
#if DEBUG_DRAW
		[SLua.DoNotToLuaAttribute]
		public float ddSphereRange = 0;
		void OnDrawGizmos()
		{
			if (!Application.isPlaying && 0 < ddSphereRange)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(transform.position, ddSphereRange);
			}

			if (7 == type)
			{
				if (null != properties && 1 < properties.Length 
					&& null != components && 0 < components.Length)
				{
					try
					{
						var triggerCount = System.Convert.ToInt32(properties[1]);
						if (0 < triggerCount)
						{
							var tCount = Mathf.Min(triggerCount, components.Length);
							for (int i = 0; i < tCount; ++i)
							{
								var t = components[i] as Transform;
								if (null != t)
								{
									DebugUtils.DrawCircle(t.position, t.localScale.x, 50, Color.green);
								}
							}
						}
					}
					catch(System.Exception)
					{
						
					}
				}
			}
		}
#endif // DEBUG_DRAW
		#endregion behaviour
	}
} // namespace RO
