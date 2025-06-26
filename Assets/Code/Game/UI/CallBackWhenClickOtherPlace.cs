using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CallBackWhenClickOtherPlace : MonoBehaviour
	{
		public List<Transform> targets = new List<Transform> ();
		public Action call = null;
		public bool excuteOnce = true;
		public bool mulityTouch = true;
		protected Dictionary<Transform, Bounds> targetSize = new Dictionary<Transform, Bounds> ();
		protected Dictionary<int, Vector3> catchPoses = new Dictionary<int, Vector3> ();
		protected Camera uiCamera;
		protected bool excute = false;

		void Start ()
		{
			uiCamera = NGUITools.FindCameraForLayer (gameObject.layer);
		}
		
		void OnEnable ()
		{
			ReInit ();
			excute = false;
		}

		virtual protected void ReInit ()
		{
			if (targets.Count == 0)
				targets.Add (transform);
			targetSize.Clear ();
			targets.RemoveAll (t => t == null);
			foreach (Transform t in targets) {
				targetSize [t] = (NGUIMath.CalculateRelativeWidgetBounds (t));
			}
		}

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				catchPoses [0] = Input.mousePosition;
			} else if (Input.GetMouseButtonUp (0)) {
				Vector3 pos;
				if (catchPoses.TryGetValue (0, out pos)) {
					if (!CheckPos (pos) && !excute) {
						excute = excuteOnce;
						catchPoses.Clear ();
						if (call != null) {
							call ();
						}
					}
				}
			}
			if (mulityTouch) {
				for (int i=1; i<Input.touches.Length; i++) {
					Touch touch = Input.touches [i];
					if (touch.phase == TouchPhase.Began) {
						catchPoses [touch.fingerId] = touch.position;
					} else if (touch.phase == TouchPhase.Ended) {
						Vector3 pos;
						if (catchPoses.TryGetValue (touch.fingerId, out pos)) {
							if (!CheckPos (pos) && !excute) {
								excute = excuteOnce;
								catchPoses.Clear ();
								if (call != null) {
									call ();
								}
							}
						}
					}
				}
			}
		}
		
		protected bool CheckPos (Vector3 screenPos)
		{
//			bool result = false;
			foreach (KeyValuePair<Transform, Bounds> kv in targetSize) {
				Transform parent = kv.Key;
				if (parent != null) {
					Vector3 localPos = parent.InverseTransformPoint (uiCamera.ScreenToWorldPoint (screenPos));
					if (kv.Value.Contains (localPos))
						return true;
				}
			}
			return false;
		}
	}
} // namespace RO
