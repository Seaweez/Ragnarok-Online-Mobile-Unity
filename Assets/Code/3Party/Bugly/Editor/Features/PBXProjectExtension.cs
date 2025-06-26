using UnityEngine;
using System.Collections;

namespace UnityEditor.XCodeEditor
{
	public partial class PBXProject
	{
		protected string ATTRIBUTES_KEY = "attributes";
		protected string TargetAttributes_KEY = "TargetAttributes";
		const string ProvisioningStyle_KEY = "ProvisioningStyle";
		const string SystemCapabilities_KEY = "SystemCapabilities";
		const string ProvisioningStyle_AUTO = "Automatic";
		const string ProvisioningStyle_MANUAL = "Manual";

		protected string _ProjectGUID;

		public PBXDictionary attributes {
			get {
				return (PBXDictionary)_data [ATTRIBUTES_KEY];
			}
		}

		public PBXDictionary targetAttributes {
			get {
				return (PBXDictionary)attributes [TargetAttributes_KEY];
			}
		}

		public PBXDictionary targetGUIDAttributes {
			get {
				string guid = ProjectGUID;
				if (string.IsNullOrEmpty (guid)) {
					Debug.LogError ("pbxproj cant find target guid");
					return null;
				}
				PBXDictionary res;
				if (targetAttributes.ContainsKey (guid) == false) {
					res = new PBXDictionary ();
					targetAttributes.Add (guid, res);
				} else {
					res = (PBXDictionary)targetAttributes [guid];
				}
				return res;
			}
		}

		public string ProjectGUID {
			get { 
				if (string.IsNullOrEmpty (_ProjectGUID)) {
					PBXList targets = (PBXList)_data ["targets"];
					if (targets.Count > 0) {
						_ProjectGUID = (string)targets [0];
					}
				}
				return _ProjectGUID;
			}
		}

		public PBXProject SetCapability (string key, bool enable)
		{
			PBXDictionary capabilities = null;
			PBXDictionary guidTargetAttributes = targetGUIDAttributes;
			if (guidTargetAttributes.ContainsKey (SystemCapabilities_KEY)) {
				capabilities = (PBXDictionary)guidTargetAttributes [SystemCapabilities_KEY];
			} else {
				capabilities = new PBXDictionary ();
				guidTargetAttributes.Add (SystemCapabilities_KEY, capabilities);
			}
			var enabled = new PBXDictionary ();
			enabled.Add ("enabled", enable ? 1 : 0);
			if (capabilities.ContainsKey (key)) {
				capabilities [key] = enabled;
			} else {
				capabilities.Add (key, enabled);
			}
			return this;
		}

		public PBXProject ProvisioningStyle (bool auto)
		{
			PBXDictionary guidTargetAttributes = targetGUIDAttributes;
			if (guidTargetAttributes.ContainsKey (ProvisioningStyle_KEY)) {
				guidTargetAttributes [ProvisioningStyle_KEY] = auto ? ProvisioningStyle_AUTO : ProvisioningStyle_MANUAL;
			} else {
				guidTargetAttributes.Add (ProvisioningStyle_KEY, auto ? ProvisioningStyle_AUTO : ProvisioningStyle_MANUAL);
			}
			return this;
		}
	}
}