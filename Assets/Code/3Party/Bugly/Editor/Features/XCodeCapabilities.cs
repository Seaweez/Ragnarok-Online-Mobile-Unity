using UnityEngine;
using System.Collections;
using UnityEditor.XCodeEditor;

public class XCodeCapabilities
{
	XCProject _project;
	XCPlist _plist;
	private Hashtable _datastore = new Hashtable ();

	public Hashtable plistHash {
		get {
			return (Hashtable)_datastore;
		}
	}

	public XCodeCapabilities (XCProject project)
	{
		_project = project;
		string plistPath = this._project.projectRootPath + "/Info.plist";
		_plist = new XCPlist (plistPath);
	}

	public XCodeCapabilities AddPushNotifications ()
	{
		_project.EnableCapability (PBXCapabilitiesType.PushNotifications.Id, true);
		return this;
	}

	public XCodeCapabilities AddBackgroundModes (BackgroundModesOptions options = BackgroundModesOptions.None)
	{
		_project.EnableCapability (PBXCapabilitiesType.BackgroundModes.Id, true);

		if ((options & BackgroundModesOptions.ActsAsABluetoothLEAccessory) == BackgroundModesOptions.ActsAsABluetoothLEAccessory) {
			_AddToPlistAsArray (BGInfo.Key, BGInfo.ModeActsBluetoothValue, _datastore);
		}
		if ((options & BackgroundModesOptions.AudioAirplayPiP) == BackgroundModesOptions.AudioAirplayPiP) {
			_AddToPlistAsArray (BGInfo.Key, BGInfo.ModeAudioValue, _datastore);
		}
		if ((options & BackgroundModesOptions.BackgroundFetch) == BackgroundModesOptions.BackgroundFetch) {
			_AddToPlistAsArray (BGInfo.Key, BGInfo.ModeFetchValue, _datastore);
		}
		if ((options & BackgroundModesOptions.ExternalAccessoryCommunication) == BackgroundModesOptions.ExternalAccessoryCommunication) {
			_AddToPlistAsArray (BGInfo.Key, BGInfo.ModeExtAccessoryValue, _datastore);
		}
		if ((options & BackgroundModesOptions.LocationUpdates) == BackgroundModesOptions.LocationUpdates) {
			_AddToPlistAsArray (BGInfo.Key, BGInfo.ModeLocationValue, _datastore);
		}
		if ((options & BackgroundModesOptions.NewsstandDownloads) == BackgroundModesOptions.NewsstandDownloads) {
			_AddToPlistAsArray (BGInfo.Key, BGInfo.ModeNewsstandValue, _datastore);
		}
		if ((options & BackgroundModesOptions.RemoteNotifications) == BackgroundModesOptions.RemoteNotifications) {
			_AddToPlistAsArray (BGInfo.Key, BGInfo.ModePushValue, _datastore);
		}
		if ((options & BackgroundModesOptions.VoiceOverIp) == BackgroundModesOptions.VoiceOverIp) {
			_AddToPlistAsArray (BGInfo.Key, BGInfo.ModeVOIPValue, _datastore);
		}
		_Flush ();
		return this;
	}

	private void _AddToPlist (string key, string obj, Hashtable hash)
	{
		Debug.Log ("Adding plist items...");
		bool needProcess = false;
		if (hash == null) {
			needProcess = true;
			hash = new Hashtable ();
		}
		hash.Add (key, obj);
		if (needProcess) {
			_plist.Process (hash);
		}
	}

	private void _AddToPlistAsArray (string key, string obj, Hashtable hash)
	{
		Debug.Log ("Adding plist items...");
		bool needProcess = false;
		if (hash == null) {
			needProcess = true;
			hash = new Hashtable ();
		}
		PBXList array = (PBXList)hash [key];
		if (array == null) {
			array = new PBXList ();
			array.Add (obj);
			hash.Add (key, array);
		} else {
			if (array.Contains (obj) == false) {
				array.Add (obj);
			}
		}
		if (needProcess) {
			_plist.Process (hash);
		}
	}

	private void _Flush ()
	{
		_plist.Process (_datastore);
	}

	public void Release ()
	{
		_project = null;
	}
}


public enum BackgroundModesOptions
{
	None = 0,
	AudioAirplayPiP = 1 << 0,
	LocationUpdates = 1 << 1,
	VoiceOverIp = 1 << 2,
	NewsstandDownloads = 1 << 3,
	ExternalAccessoryCommunication = 1 << 4,
	UsesBluetoothLEAccessory = 1 << 5,
	ActsAsABluetoothLEAccessory = 1 << 6,
	BackgroundFetch = 1 << 7,
	RemoteNotifications = 1 << 8
}

internal class BGInfo
{
	internal static readonly string Key = "UIBackgroundModes";
	internal static readonly string ModeAudioValue = "audio";
	internal static readonly string ModeBluetoothValue = "bluetooth-central";
	internal static readonly string ModeActsBluetoothValue = "bluetooth-peripheral";
	internal static readonly string ModeExtAccessoryValue = "external-accessory";
	internal static readonly string ModeFetchValue = "fetch";
	internal static readonly string ModeLocationValue = "location";
	internal static readonly string ModeNewsstandValue = "newsstand-content";
	internal static readonly string ModePushValue = "remote-notification";
	internal static readonly string ModeVOIPValue = "voip";
}

/// <summary>
/// List of all the capabilities available.
/// </summary>
public sealed class PBXCapabilitiesType
{
	public static readonly PBXCapabilitiesType ICloud = new PBXCapabilitiesType ("com.apple.iCloud", true, "CloudKit.framework", true);
	public static readonly PBXCapabilitiesType PushNotifications = new PBXCapabilitiesType ("com.apple.Push", true);
	public static readonly PBXCapabilitiesType GameCenter = new PBXCapabilitiesType ("com.apple.GameCenter", false, "GameKit.framework");
	public static readonly PBXCapabilitiesType Wallet = new PBXCapabilitiesType ("com.apple.Wallet", true, "PassKit.framework");
	public static readonly PBXCapabilitiesType Siri = new PBXCapabilitiesType ("com.apple.Siri", true);
	public static readonly PBXCapabilitiesType ApplePay = new PBXCapabilitiesType ("com.apple.ApplePay", true);
	public static readonly PBXCapabilitiesType InAppPurchase = new PBXCapabilitiesType ("com.apple.InAppPurchase", false);
	public static readonly PBXCapabilitiesType Maps = new PBXCapabilitiesType ("com.apple.Maps.iOS", false, "MapKit.framework");
	public static readonly PBXCapabilitiesType PersonalVPN = new PBXCapabilitiesType ("com.apple.VPNLite", true, "NetworkExtension.framework");
	public static readonly PBXCapabilitiesType BackgroundModes = new PBXCapabilitiesType ("com.apple.BackgroundModes", false);
	public static readonly PBXCapabilitiesType KeychainSharing = new PBXCapabilitiesType ("com.apple.KeychainSharing", true);
	public static readonly PBXCapabilitiesType InterAppAudio = new PBXCapabilitiesType ("com.apple.InterAppAudio", true, "AudioToolbox.framework");
	public static readonly PBXCapabilitiesType AssociatedDomains = new PBXCapabilitiesType ("com.apple.SafariKeychain", true);
	public static readonly PBXCapabilitiesType AppGroups = new PBXCapabilitiesType ("com.apple.ApplicationGroups.iOS", true);
	public static readonly PBXCapabilitiesType HomeKit = new PBXCapabilitiesType ("com.apple.HomeKit", true, "HomeKit.framework");
	public static readonly PBXCapabilitiesType DataProtection = new PBXCapabilitiesType ("com.apple.DataProtection", true);
	public static readonly PBXCapabilitiesType HealthKit = new PBXCapabilitiesType ("com.apple.HealthKit", true, "HealthKit.framework");
	public static readonly PBXCapabilitiesType WirelessAccessoryConfiguration = new PBXCapabilitiesType ("com.apple.WAC", true, "ExternalAccessory.framework");
	
	
	private readonly string _id;
	private readonly bool _requiresEntitlements;
	private readonly string _framework;
	private readonly bool _optionalFramework;

	public bool OptionalFramework {
		get { return _optionalFramework; }
	}

	public string Framework {
		get { return _framework; }
	}

	public string Id {
		get { return _id; }
	}

	public bool RequiresEntitlements {
		get { return _requiresEntitlements; }
	}

	/// <summary>
	/// This private object represent what a capabity change in the PBXProject file
	/// </summary>
	/// <param name="id">The string used in the PBXProject file to identify the capability and mark it as enabled</param>
	/// <param name="requiresEntitlements">This capability change the entitlements file therefore we need to add this entitlements file to the code signing entitlement</param>
	/// <param name="framework">Specify which framework need to be added to the project for this capability, if "" no framework are added.</param>
	/// <param name="optionalFramework">Some capabilty (right now only iCloud) add framework, not all the time but just when some option are checked
	/// this parameter indicate if one of them is checked</param>
	private PBXCapabilitiesType (string id, bool requiresEntitlements, string framework = "", bool optionalFramework = false)
	{
		_id = id;
		_requiresEntitlements = requiresEntitlements;
		_framework = framework;
		_optionalFramework = optionalFramework;
	}
}