using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace RO
{
	public class AssetManageInfo
	{
		static AssetManageInfo _Default;
		[XmlAttribute("Id")] 
		public int
			id = -1;
		[XmlAttribute("Path")] 
		public string
			path;
		[XmlAttribute("ManageAssetMode")] 
		public AssetManageMode
			manageAssetMode;
		[XmlAttribute("ManageAssetLRUCount")] 
		public int
			manageAssetLRUCount;
		[XmlAttribute("ManageBundleMode")] 
		public AssetManageMode
			manageBundleMode;
		[XmlAttribute("ManageBundleLRUCount")] 
		public int
			manageBundleLRUCount;
		[XmlAttribute("Encryption")] 
		public AssetEncryptMode
			encryption;
		public List<AssetManageInfo>
			subs;

		public void ResetInfo (int id, string path, AssetManageMode assetMode, int assetLRUCount, AssetManageMode bundleMode, int bundleLRUCount, AssetEncryptMode encryption)
		{
			this.id = id;
			this.path = path;
			this.manageAssetMode = assetMode;
			this.manageAssetLRUCount = assetLRUCount;
			this.manageBundleMode = bundleMode;
			this.manageBundleLRUCount = bundleLRUCount;
			this.encryption = encryption;
		}

		public AssetManageInfo AddSubInfo (int id, string path, AssetManageMode assetMode, int assetLRUCount, AssetManageMode bundleMode, int bundleLRUCount, AssetEncryptMode encryption)
		{
			if (subs == null)
				subs = new List<AssetManageInfo> ();
			AssetManageInfo find = subs.Find ((info) => {
				return info.path == path;});
			if (find == null) {
				find = new AssetManageInfo ();
				subs.Add (find);
			}
			find.ResetInfo (id, path, assetMode, assetLRUCount, bundleMode, bundleLRUCount, encryption);
			return find;
		}
		
		public override string ToString ()
		{
			return string.Format ("[AssetManageInfo] id:{0} path:{1} manageAssetMode:{2} manageAssetLRUCount:{3} manageBundleMode:{2} manageBundleLRUCount:{3}", 
			                      id, path, manageAssetMode, manageAssetLRUCount, manageBundleMode, manageBundleLRUCount);
		}

		public bool NeedRecordAsDepends {
			get {
				return manageBundleMode == AssetManageMode.AutoUnloadNoDepends || manageBundleMode == AssetManageMode.Custom || manageBundleMode == AssetManageMode.ResourceAutoUnloadNoDepends || manageBundleMode == AssetManageMode.AutoUnloadNoDependsCachePool;
			}
		}

		public bool AutoUnloadNoDepend {
			get {
				return manageBundleMode == AssetManageMode.AutoUnloadNoDepends || manageBundleMode == AssetManageMode.AutoUnloadNoDependsCachePool;
			}
		}
	
		public static AssetManageInfo Default {
			get {
				if (_Default == null) {
					_Default = new AssetManageInfo ();
					_Default.ResetInfo (999999, "", AssetManageMode.Custom, 0, AssetManageMode.Custom, 0, AssetEncryptMode.None);
				}
				return _Default;
			}
		}
	}

	public enum AssetManageMode
	{
		[XmlEnum(Name="NeverUnLoad")]
		NeverUnLoad,
		[XmlEnum(Name="UnLoadImmediately")]
		UnLoadImmediately,
		[XmlEnum(Name="LRU")]
		LRU,
		[XmlEnum(Name="Custom")]
		Custom,
		[XmlEnum(Name="AutoUnloadNoDepends")]
		AutoUnloadNoDepends,
		[XmlEnum(Name="ResourceAutoUnloadNoDepends")]
		ResourceAutoUnloadNoDepends,
		[XmlEnum(Name="AutoUnloadNoDependsCachePool")]
		AutoUnloadNoDependsCachePool,
	}

	public enum AssetEncryptMode
	{
		[XmlEnum(Name="None")]
		None,
		[XmlEnum(Name="Encryption1")]
		Encryption1,
		[XmlEnum(Name="Encryption2")]
		Encryption2,
	}
} // namespace RO
