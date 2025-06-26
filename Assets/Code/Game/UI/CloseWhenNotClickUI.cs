using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class CloseWhenNotClickUI : MonoBehaviour
	{
		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				if (!UICamera.isOverUI) {
					gameObject.SetActive (false);
				}
			}
		}
	
	}
} // namespace RO
