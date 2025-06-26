using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[SLua.CustomLuaClassAttribute]
public class GetAppBundleVersion
{
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern string _GetCFBundleVersion ();
	#endif
	protected static string m_bundleVersion;

	public static string BundleVersion {
		get {
			if (m_bundleVersion == null) {
				GetVersionInfo ();
			}
			return m_bundleVersion;
		}
	}

	protected static void GetVersionInfo ()
	{
		#if UNITY_EDITOR
		m_bundleVersion = UnityEditor.PlayerSettings.GetApplicationIdentifier(UnityEditor.BuildTargetGroup.iOS);
		#elif UNITY_IPHONE
		m_bundleVersion = _GetCFBundleVersion();
		#elif UNITY_ANDROID
		m_bundleVersion = RO.ExternalInterfaces.GetPackageName();
		#else
		m_bundleVersion = "";
		#endif
	}

}
