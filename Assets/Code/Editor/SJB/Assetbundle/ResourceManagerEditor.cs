using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(ResourceManager))]
	public class ResourceManagerEditor : Editor
	{
		ResourceManager _target;
		ManagedBundleLoaderStrategy _loaderStrategy;
		string searchInfo = "";

		public override void OnInspectorGUI ()
		{
			_target = target as ResourceManager;
			if (Application.isPlaying) {
				EditorGUILayout.BeginVertical ();
				EditorGUILayout.BeginHorizontal ();

				GUILayout.Label ("Debug Info", GUILayout.Width (100));
				_target.ShowDebug = GUILayout.Toggle (_target.ShowDebug, "");
				EditorGUILayout.EndHorizontal ();
				
				if (_target.ShowDebug) {
					if (_loaderStrategy == null) {
						_loaderStrategy = ResourceManager.LoaderStrategy as ManagedBundleLoaderStrategy;
					}

					if (_loaderStrategy != null) {
						foreach (KeyValuePair<AssetManageMode,IABManagement> kvp in _loaderStrategy.abManager.modeABManager) {
							EditorGUILayout.LabelField (string.Format ("{0} SAB 数量:{1} bundles:{2}", kvp.Key, kvp.Value.LoadedSAB, kvp.Value.CachedBundle));
						}
					}

					searchInfo = GUILayout.TextField (searchInfo);
					
					if (GUILayout.Button ("Search")) {
						if (string.IsNullOrEmpty (searchInfo) == false) {
							SharedLoadedAB sab = _loaderStrategy.GetSharedLoaded (searchInfo);
							Debug.Log (sab.ToString ());
						}
					}
				}

				if (GUILayout.Button ("手动GC")) {
					_target.GC ();
				}

				EditorGUILayout.EndVertical ();
			}
		}
	}
} // namespace EditorTool
