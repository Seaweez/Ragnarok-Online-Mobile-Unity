using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

namespace RO.Config
{
	public class AndroidManifest{

		static AndroidManifest _Instance;
		static XmlDocument xmlDoc;
		static string path;

		public static AndroidManifest Instance {
			get {
				if (_Instance == null) {
					_Instance = new AndroidManifest ();

					path = Path.Combine (Application.dataPath, "Plugins/Android/AndroidManifest.xml");
					xmlDoc = new XmlDocument();
					xmlDoc.Load(path);
				}
				return _Instance;
			}
		}

		public void ChangePackage(string packageName)
		{
			if(!BuildParams.Instance.Get_JPush_Enable)
			{
				return;
			}

			if(packageName == null)
			{
				if(BuildBundleEnvInfo.Env == "Develop" || BuildBundleEnvInfo.Env == "Studio")
				{
					packageName = "com.xd.ro3";
				}
				else if(BuildBundleEnvInfo.Env == "Alpha" )
				{
					packageName = "com.xd.ro.xdapk";
				}
				else if(BuildBundleEnvInfo.Env == "Release")
				{
					packageName = "com.xd.ro.roapk";
				}
				else
					return;
			}

			XmlElement manifest = (XmlElement)xmlDoc.SelectSingleNode ("manifest");
			string orignPackage = manifest.GetAttribute ("package");

			if (orignPackage != packageName) {
				manifest.SetAttribute ("package", packageName);

				string androidName = "android:name";

				//activity
				XmlNodeList activity = xmlDoc.SelectNodes ("manifest//activity");
				foreach (XmlNode node in activity) {
					XmlElement ele = (XmlElement)node;
					string name = ele.GetAttribute (androidName);

					if (name.Equals ("cn.jpush.android.ui.PopWinActivity")) {
						SetFilterList (node, androidName, orignPackage, packageName);
					}
					else if (name.Equals ("cn.jpush.android.ui.PushActivity")) {
						SetFilterList (node, androidName, orignPackage, packageName);
					}
					else if (name.Equals ("cn.jpush.android.ui.PushActivity")) {
						SetFilterList (node, androidName, orignPackage, packageName);
					}
				}

				//service
				XmlNodeList service = xmlDoc.SelectNodes ("manifest//service");
				foreach (XmlNode node in service) {
					XmlElement ele = (XmlElement)node;
					string name = ele.GetAttribute (androidName);

					if (name.Equals ("cn.jpush.android.service.DaemonService")) {
						SetFilterList (node, androidName, orignPackage, packageName);
						break;
					}
				}

				//receiver
				XmlNodeList receiver = xmlDoc.SelectNodes ("manifest//receiver");
				foreach (XmlNode node in receiver) {
					XmlElement ele = (XmlElement)node;
					string name = ele.GetAttribute (androidName);

					if (name.Equals ("cn.jpush.android.service.PushReceiver") || name.Equals ("cn.jiguang.unity.push.JPushReceiver") || name.Equals("cn.jiguang.unity.push.JPushEventReceiver")) {
						SetFilterList (node, androidName, orignPackage, packageName);
					}
				}

				//activity
				XmlNodeList providers = xmlDoc.SelectNodes ("manifest//provider");
				foreach (XmlNode node in providers) {
					XmlElement ele = (XmlElement)node;
					string name = ele.GetAttribute ("android:authorities");

					if (name.Equals (orignPackage+".DataProvider")) {
						ele.SetAttribute("android:authorities",packageName+".DataProvider");
						break;
					}
				}

				//uses-permission
				XmlNodeList usesPermission = xmlDoc.SelectNodes ("manifest/uses-permission");
				foreach (XmlNode node in usesPermission) {
					XmlElement ele = (XmlElement)node;
					string name = ele.GetAttribute (androidName);

					if (name.Equals (orignPackage + ".permission.JPUSH_MESSAGE")) {
						ele.SetAttribute (androidName, packageName + ".permission.JPUSH_MESSAGE");
						break;
					}
				}

				//permission
				XmlNodeList permission = xmlDoc.SelectNodes ("manifest/permission");
				foreach (XmlNode node in permission) {
					XmlElement ele = (XmlElement)node;
					string name = ele.GetAttribute (androidName);

					if (name.Equals (orignPackage + ".permission.JPUSH_MESSAGE")) {
						ele.SetAttribute (androidName, packageName + ".permission.JPUSH_MESSAGE");
						break;
					}
				}

				//permission
				XmlNodeList metaData = xmlDoc.SelectNodes ("manifest//meta-data");
				foreach (XmlNode node in metaData) {
					XmlElement ele = (XmlElement)node;
					string name = ele.GetAttribute (androidName);

					if (name.Equals ("JPUSH_APPKEY")) {
						ele.SetAttribute ("android:value", BuildParams.Instance.Get_JPush_Appkey);
						break;
					}
				}
			}

			xmlDoc.Save (path);
		}

		public void SetFilterList(XmlNode parent,string attribute,string orignValue,string value)
		{
			XmlNodeList intentFilterList = parent.ChildNodes;
			foreach (XmlNode intentFilter in intentFilterList) {

				XmlNodeList categoryList = intentFilter.ChildNodes;
				foreach (XmlNode category in categoryList) {
					try{
						XmlElement categoryEle = (XmlElement)category;
						string categoryName = categoryEle.GetAttribute (attribute);

						if (categoryName.Equals (orignValue)) {
							categoryEle.SetAttribute (attribute, value);
						}
					}
					catch {

					}
				}
			}
		}
	}
}