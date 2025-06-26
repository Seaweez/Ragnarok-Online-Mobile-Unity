using UnityEngine;
using System.Collections;

namespace UnityEditor.XCodeEditor
{
	public partial class XCProject
	{
		public PBXProject EnableCapability (string key, bool enabled)
		{
			return project.SetCapability (key, enabled);
		}

		public PBXProject ProvisioningStyle (bool auto)
		{
			return project.ProvisioningStyle (auto);
		}
	}
}
