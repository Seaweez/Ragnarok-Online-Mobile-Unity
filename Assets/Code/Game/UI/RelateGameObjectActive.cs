using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RelateGameObjectActive : MonoBehaviour
	{
		public GameObject[] enable_active;
		public GameObject[] enable_inactive;
		public GameObject[] disable_active;
		public GameObject[] disable_inactive;
		public Action enable_Call;
		public Action disable_Call;
		private List<GameObject> dynamicAddDisEnableActGos = new List<GameObject> ();
		private List<GameObject> dynamicAddDisEnableInActGos = new List<GameObject> ();

		//when quit SetActive(true)
		public void AddDisEnableAct (GameObject go)
		{
			go.SetActive (false);
			dynamicAddDisEnableActGos.Add (go);
		}
		//when quit SetActive(false)
		public void AddDisEnableInAct (GameObject go)
		{
			go.SetActive (true);
			dynamicAddDisEnableInActGos.Add (go);
		}

		void OnEnable ()
		{			
			foreach (var v in enable_active)
				setactive (v, true);
			foreach (var v in enable_inactive)
				setactive (v, false);

			if (enable_Call != null)
				enable_Call ();
		}

		void OnDisable ()
		{
			foreach (var v in disable_active)
				setactive (v, true);
			foreach (var v in disable_inactive)
				setactive (v, false);

			foreach (var v in dynamicAddDisEnableActGos)
				setactive (v, true);
			foreach (var v in dynamicAddDisEnableInActGos)
				setactive (v, false);

			if (disable_Call != null)
				disable_Call ();
		}

		void setactive (GameObject g, bool state)
		{
			if (g != null && g.activeSelf != state)
				g.SetActive (state);
		}
	
	}
} // namespace RO
