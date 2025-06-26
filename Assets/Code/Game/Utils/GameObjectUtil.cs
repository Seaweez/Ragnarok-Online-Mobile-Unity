using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class GameObjectUtil : SingletonO<GameObjectUtil>
	{
		public static GameObjectUtil Instance {
			get {
				return instance;
			}
		}

		public static void SetBehaviourEnabled (Behaviour b, bool enabled)
		{
			b.enabled = enabled;
		}

		public void BringTOFront (GameObject obj)
		{
			int val = 0;
			var objs = this.GetAllChildren (obj);
			for (int i = 0; i < objs.Length; ++i) {
				var single = objs [i];
				val |= NGUITools.AdjustDepth (single, 1000);
			}

			if ((val & 1) != 0) {
				NGUITools.NormalizePanelDepths ();
			}
			if ((val & 2) != 0)
				NGUITools.NormalizeWidgetDepths ();
		}

		public void SetDepths (GameObject root, int depth, int index = 0)
		{
			var widget = root.transform.GetComponent<UIWidget> ();
			if (widget != null)
				widget.depth = depth + index;

			index += 1;
			foreach (Transform c in root.transform) {
				widget = c.GetComponent<UIWidget> ();
				if (widget != null)
					widget.depth = depth;
				this.SetDepths (c.gameObject, depth, index);
			}
		}

		public GameObject DeepFind (GameObject parent, string name)
		{
			Transform t = parent.transform.Find (name);
			if (t != null) {
				return t.gameObject;
			}
			Transform parent_t = parent.transform;
			GameObject resultGO = null;
			for (int i = 0; i < parent_t.childCount; i++) {
				resultGO = DeepFind (parent_t.GetChild(i).gameObject, name);
				if (resultGO != null) {
					return resultGO;
				}
			}
			return null;
		}

		public GameObject DeepFindChild (GameObject parent, string name)
		{
//			foreach (Transform c in parent.transform) {
//				if (c.name == name || c.name.StartsWith (name))
//					return c.gameObject;
//				else {
//					GameObject resultGo = DeepFindChild (c.gameObject, name);
//					if (resultGo != null)
//						return resultGo;
//				}
//			}
//			return null;
			return DeepFind (parent, name);
		}

		public GameObject[] DeepFindChildren (GameObject parent, string name)
		{
			List<GameObject> objs = new List<GameObject> ();
			if (parent.transform.childCount > 0) {
				foreach (Transform c in parent.transform) {
					if (c.name == name) {
						objs.Add (c.gameObject);
					}
					objs.AddRange (DeepFindChildren (c.gameObject, name));
				}
			}
			return objs.ToArray ();
		}

		public GameObject[] GetAllChildren (GameObject parent)
		{
			if (parent == null) {
				return null;
			}
			List<GameObject> results = new List<GameObject> ();
			foreach (Transform t in parent.transform) {
				if (t != null) {
					results.Add (t.gameObject);

					GameObject[] cs = GetAllChildren (t.gameObject);
					if (cs != null) {
						results.AddRange (cs);
					}
				}
			}
			return results.ToArray ();
		}

		public bool DestroyAllChildren (GameObject parent)
		{
			if (parent == null)
				return false;

			int count = parent.transform.childCount;
			for (int i = 0; i < count; i++) {
				Transform t = parent.transform.GetChild (0);
				if (t != null)
					GameObject.DestroyImmediate (t.gameObject);
			}
			return true;
		}

		/// <summary>
		/// get All Comps with noActive 
		/// </summary>
		public Component[] GetAllComponentsInChildren (GameObject p, System.Type t, bool containSelf = true)
		{
			List<Component> comps = new List<Component> ();
			Component sp = p.GetComponent (t);
			if (containSelf && sp != null)
				comps.Add (sp);

			foreach (Transform child in p.transform) {
				Component ct = child.GetComponent (t);
				if (ct != null) {
					comps.Add (ct);
				}
				comps.AddRange (GetAllComponentsInChildren (child.gameObject, t, false));
			}
			return comps.ToArray ();
		}

		public Material[] GetSharedMaterials (Renderer render)
		{
			return render.sharedMaterials;
		}

		public void ChangeLayersRecursively (GameObject parent, string LayerName)
		{
			if (parent.layer != LayerMask.NameToLayer (LayerName)) {
				parent.layer = LayerMask.NameToLayer (LayerName);
			}
			foreach (Transform c in parent.transform) {
				ChangeLayersRecursively (c.gameObject, LayerName);
			}
		}

		public void ChangeLayersRecursively (GameObject parent, int layer)
		{
			if (parent.layer != layer) {
				parent.layer = layer;
			}
			foreach (Transform c in parent.transform) {
				ChangeLayersRecursively (c.gameObject, layer);
			}
		}

		public GameObject CopyObjTo (GameObject obj, Transform parent)
		{
			if (parent == null) {
				parent = obj.transform.parent;
			}
			GameObject copy = GameObject.Instantiate (obj) as GameObject;
			if (!copy.activeInHierarchy) {
				copy.SetActive (true);
			}
			copy.transform.SetParent (parent, false);
			copy.name = obj.name;
			return copy;
		}

		public int ToHashCode (string ori)
		{
			return ori.GetHashCode ();
		}

		public bool ObjectIsNULL (UnityEngine.Object go)
		{
			return go == null;
		}

		public bool SystemObjectIsNULL (object obj)
		{
			return obj == null;
		}

		public float GetUIActiveHeight (GameObject go)
		{
			UIRoot ur = NGUITools.FindInParents<UIRoot> (go.transform);
			return ur.activeHeight;
		}

		public Component FindCompInParents (GameObject go, System.Type type)
		{
			if (go == null)
				return null;

			Component comp = go.GetComponent (type);
			if (comp == null) {
				Transform t = go.transform.parent;
				
				while (t != null && comp == null) {
					comp = t.gameObject.GetComponent (type);
					t = t.parent;
				}
			}
			return comp;
		}

		public void WrapLabel (UILabel uiLabel)
		{
			string strContent = uiLabel.processedText;
			string strOut = string.Empty;
			bool bWarp = uiLabel.Wrap (strContent, out strOut, uiLabel.height);
			if (!bWarp && strOut.Length > 0) {
				strOut = strOut.Substring (0, strOut.Length - 1);
				strOut += "...";
			}
			uiLabel.text = strOut;
		}

		public float GetScaleInBound (GameObject go, Vector2 boundSize, float defaultScale)
		{
			if (Vector2.Equals (Vector2.zero, boundSize)) {
				return defaultScale;
			}
			Vector3 localBoundSize;
			Transform modelTransform = null;
			var smr = go.GetComponentInChildren<SkinnedMeshRenderer> ();
			if (null != smr) {
				modelTransform = smr.transform;
				localBoundSize = smr.localBounds.size;
			} else {
				var r = go.GetComponentInChildren<Renderer> ();
				if (null != r) {
					modelTransform = r.transform;
					localBoundSize = r.bounds.size;
				} else {
					return defaultScale;
				}
			}
			if (null != modelTransform && ModelUtils.IsThreeDMaxImportedModel (modelTransform)) {
				localBoundSize = new Vector3 (localBoundSize.y, localBoundSize.x, localBoundSize.z);
			}
			
			return Mathf.Min (boundSize.x / localBoundSize.x, boundSize.y / localBoundSize.y);
		}

		public float GetScaleInBound (GameObject go, Vector2 boundSize)
		{
			return GetScaleInBound (go, boundSize, 1);
		}

		public void CopyAudioSourceSettings (AudioSource audioSource, AudioSource originAS)
		{
			audioSource.mute = originAS.mute;
			audioSource.bypassEffects = originAS.bypassEffects;
			audioSource.bypassListenerEffects = originAS.bypassListenerEffects;
			audioSource.bypassReverbZones = originAS.bypassReverbZones;
			audioSource.playOnAwake = originAS.playOnAwake;
			audioSource.loop = originAS.loop;
			audioSource.priority = originAS.priority;
			audioSource.volume = originAS.volume;
			audioSource.pitch = originAS.pitch;
			audioSource.panStereo = originAS.panStereo;
			audioSource.spatialize = originAS.spatialize;
			audioSource.spatialBlend = originAS.spatialBlend;
			audioSource.reverbZoneMix = originAS.reverbZoneMix;
			audioSource.dopplerLevel = originAS.dopplerLevel;
			audioSource.spread = originAS.spread;
			audioSource.rolloffMode = originAS.rolloffMode;
			audioSource.minDistance = originAS.minDistance;
			audioSource.maxDistance = originAS.maxDistance;
		}

	}
}
// namespace RO
