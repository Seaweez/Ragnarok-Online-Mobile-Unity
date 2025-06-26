using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class AppEnvConfig:XMLSerializedClass<AppEnvConfig>
	{
		[XmlAttribute("ChannelEnv")]
		public string channelEnv;
		[XmlAttribute("SDK")]
		public string sdk;
		static AppEnvConfig _Instance;

		public static AppEnvConfig Instance {
			get {
				if (_Instance == null) {
					TextAsset ta = Resources.Load ("ChannelEnv") as TextAsset;
					if (ta != null) {
						_Instance = AppEnvConfig.CreateByStr (ta.text);
					}
				}
				return _Instance;
			}
		}

		public static void ResetInstance()
		{
			_Instance = null;
		}

		public bool NeedSDK {
			get {
                #if _XDSDK_LINK_NATIVE_
                return true;
#endif

                if (string.IsNullOrEmpty (sdk) == false && sdk != "None")
					return true;
				return false;
			}
		}

        public static bool IsTestApp
        {
            get
            {
#if TestApp
            return true;
#else
                return false;
#endif
            }
        }
	}
} // namespace RO
