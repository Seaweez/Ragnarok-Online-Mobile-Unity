using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class SharedABRefCounter : MonoBehaviour
	{
		public List<string> dependsOnName;
		private List<SharedLoadedAB> _dependsSABs;

		public void AddDepends (string dependKey)
		{
			if (dependsOnName == null || dependsOnName.Count == 0) {
				dependsOnName = new List<string> ();
			}
			dependsOnName.Add (dependKey);
		}

		public void AddDepends (List<string> dependKeys)
		{
			if (dependsOnName == null || dependsOnName.Count == 0) {
				dependsOnName = new List<string> ();
			}
			dependsOnName.AddRange (dependKeys);
		}

		void Awake ()
		{
			if (dependsOnName != null && dependsOnName.Count > 0) {
				for (int i=0; i<dependsOnName.Count; i++) {
					SharedLoadedAB loaded = ResourceManager.Instance.GetSharedLoaded (dependsOnName [i]);
					if (loaded != null) {
						if (_dependsSABs == null) {
							_dependsSABs = new List<SharedLoadedAB> ();
						}
						_dependsSABs.Add (loaded.Share ());
					}
				}
				dependsOnName = null;
			}
		}

		void OnDestroy ()
		{
			if (_dependsSABs != null && _dependsSABs.Count > 0) {
				for (int i=0; i<_dependsSABs.Count; i++) {
					_dependsSABs [i].Dispose ();
					_dependsSABs [i].ManagerTryUnLoadThis ();
				}
				_dependsSABs = null;
			}
		}
	}
} // namespace RO
