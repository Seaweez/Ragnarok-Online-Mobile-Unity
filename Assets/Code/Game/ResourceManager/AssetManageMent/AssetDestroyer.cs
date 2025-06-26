using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class AssetDestroyer : MonoBehaviour
	{
		List<Object> _assets = new List<Object> ();

		public void AddToDestroy (Object asset)
		{
			if (_assets.Contains (asset) == false) {
				_assets.Add (asset);
				enabled = true;
			}
		}

		public void DestroyRightNow ()
		{
			if (_assets != null && _assets.Count > 0) {
				for (int i=0; i<_assets.Count; i++) {
					Object.DestroyImmediate (_assets [i], true);
				}
				_assets.Clear ();
			}
			enabled = false;
		}

		void Update ()
		{
			DestroyRightNow ();
		}

		void OnDestroy()
		{
			DestroyRightNow ();
		}
	}
} // namespace RO
