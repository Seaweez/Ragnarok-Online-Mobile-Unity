using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditor.XCodeEditor
{
	public class RORequiredXCodePost : IROXCodePostProcess
	{
		const string IPHONEX_LAUNCHIMAGE_PATH = "Art/Public/Texture/General/LaunchImages";
		XCProject _project;
		XCPlist _plist;
		string _plistPath;
		Hashtable _hash = new Hashtable ();

		public RORequiredXCodePost (XCProject p)
		{
			_project = p;
			_plistPath = this._project.projectRootPath + "/Info.plist";
			_plist = new XCPlist (_plistPath);
		}

		#region IROXCodePostProcess implementation

		public void Process ()
		{
			//TODO disable the bitcode for iOS 9
			_project.overwriteBuildSetting ("ENABLE_BITCODE", "NO", "Release");
			_project.overwriteBuildSetting ("ENABLE_BITCODE", "NO", "Debug");
			//for xcode 8+ 改为手动选择provision
			_project.ProvisioningStyle (false);
			_Description ();
			if (_hash.Count > 0) {
				_plist.Process (_hash);
			}
			// keyboard修改，修复九宫格中文输入的bug
			ModifyKeyBoardClass keyboard = new ModifyKeyBoardClass (_project.projectRootPath + "/Classes/UI/Keyboard.mm");
			keyboard.FixJiugongge ();

			#if UNITY_2017 
				ModifyUnityViewControllerBaseiOSClass baseiOS = new ModifyUnityViewControllerBaseiOSClass (_project.projectRootPath + "/Classes/UI/UnityViewControllerBase+iOS.mm");
			#else
				//ModifyUnityViewControllerBaseiOSClass baseiOS = new ModifyUnityViewControllerBaseiOSClass (_project.projectRootPath + "/Classes/UI/UnityViewControllerBaseiOS.mm");
			#endif
			
			//baseiOS.WriteSwitchEdgeProtectCode ();

			// for iphoneX launchImage
			_AddLaunchImages ();
		}

		public void Release ()
		{
			_project = null;
			_plist = null;
		}

		#endregion

		/**
		<key>NSCalendarsUsageDescription</key>
		<string>访问日历</string>
		<key>NSCameraUsageDescription</key>
		<string>访问相机</string>
		<key>NSContactsUsageDescription</key>
		<string>访问通讯录</string>
		<key>NSLocationAlwaysUsageDescription</key>
		<string>访问地理位置</string>
		<key>NSLocationWhenInUseUsageDescription</key>
		<string></string>
		<key>NSMicrophoneUsageDescription</key>
		<string>访问麦克风</string>
		<key>NSPhotoLibraryUsageDescription</key>
		<string>访问相册</string>
		ios11新加的
		<key>NSPhotoLibraryAddUsageDescription</key>
		<string>访问相册</string>
		**/
		void _Description ()
		{
			_HashString ("NSCalendarsUsageDescription", "访问日历");
			_HashString ("NSCameraUsageDescription", "访问相机");
			_HashString ("NSContactsUsageDescription", "访问通讯录");
			_HashString ("NSLocationAlwaysUsageDescription", "访问地理位置");
			_HashString ("NSLocationWhenInUseUsageDescription", "...");
			_HashString ("NSMicrophoneUsageDescription", "访问麦克风");
			_HashString ("NSPhotoLibraryUsageDescription", "访问相册");
			_HashString ("NSPhotoLibraryAddUsageDescription", "保存到相册");
			_HashBoolean ("ITSAppUsesNonExemptEncryption", false);
		}

		void _HashString (string key, string v)
		{
			if (_hash.ContainsKey (key) == false) {
				_hash.Add (key, v);
			}
		}

		void _HashBoolean (string key, bool v)
		{
			if (_hash.ContainsKey (key) == false) {
				_hash.Add (key, v);
			}
		}

		void _AddLaunchImages ()
		{
			string appIconPath = Path.Combine (Application.dataPath, IPHONEX_LAUNCHIMAGE_PATH);  
			string[] iconflies = null;  
			if (Directory.Exists (appIconPath)) {  
				iconflies = Directory.GetFiles (appIconPath);      
			}  
			if (iconflies != null && iconflies.Length > 0) {  
				appIconPath = this._project.projectRootPath + "/Unity-iPhone/Images.xcassets/LaunchImage.launchimage/";  
				foreach (string file in iconflies) {  
					string fileName = Path.GetFileName (file);
					string extension = Path.GetExtension (fileName);
					if (extension.Equals (".png") || extension.Equals (".json")) {  
						Debug.Log ("Icon Name:" + fileName);   
						File.Copy (file, Path.Combine (appIconPath, fileName), true);  
					}  
				}  
			}  
		}
	}
}