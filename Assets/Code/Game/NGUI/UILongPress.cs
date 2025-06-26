using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class UILongPress : MonoBehaviour
	{
		public Action<UILongPress,bool> pressEvent;
//		public Action<UILongPress,bool> pressEvent;
		public bool checkHoverSelf = true;
		public float pressTime = 0;
		bool isPressing;
		
		public virtual void OnPress (bool isPressed)
		{
			if (isPressed) {
				startCal ();
			} else
				stopCal ();
		}
		
		void startCal ()
		{
			if (pressTime <= 0)
				longPressed ();
			else {
//				StartTiming();
				StartCoroutine ("calculateLongPress");
			}
		}
		
		void stopCal ()
		{
			if (isPressing)
				NotifyPress (false);
			isPressing = false;
			StopCoroutine ("calculateLongPress");
		}
		
		void OnDisable ()
		{
			stopCal ();
		}
		
		void OnDestroy ()
		{
			stopCal ();
		}
		
		IEnumerator calculateLongPress ()
		{
			yield return new WaitForSeconds (pressTime);
			longPressed ();
		}
		
		void longPressed ()
		{
			EndTiming ();
			isPressing = true;
			if (this.gameObject == UICamera.hoveredObject)
				NotifyPress (true);
			else
				stopCal ();
		}

		void NotifyPress (bool val)
		{
			if (pressEvent != null)
				pressEvent (this, val);
		}

		void StartTiming ()
		{
		}

		void EndTiming ()
		{
		}

		void Update ()
		{
			if (checkHoverSelf && isPressing && UICamera.hoveredObject != null) {
				if (this.gameObject != UICamera.hoveredObject) {
					stopCal ();
				}
			}
		}
	}
} // namespace RO
