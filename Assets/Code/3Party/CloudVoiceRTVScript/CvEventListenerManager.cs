
using System;
using UnityEngine;
using System.Collections.Generic;


namespace CloudVoiceVideoTroops
{
	public enum CvListenerEven :uint
	{
		SendRealTimeVoiceMessageErrorNotify,
		ReceiveRealTimeVoiceMessageNofify,
		ReceiveTextMessageNotify,
		RecorderMeteringPeakPowerNotify,
		PlayMeteringPeakPowerNotify,
        AudioToolsRecorderMeteringPeakPowerNotify,
        AudioToolsPlayMeteringPeakPowerNotify,
		MicStateNotify,
        onConnectFail,
        onReconnectSuccess,
        LoginNotify,
        LogoutNotify
	}

    public delegate void Callback<T>(T arg1);
    static public class CvEventListenerManager
    {
        private static Dictionary<CvListenerEven, Delegate> mProtocolEventTable = new Dictionary<CvListenerEven, Delegate>();

        public static void AddListener(CvListenerEven protocolEnum, Callback<System.Object> kHandler)
        {
            lock (mProtocolEventTable)
            {
                if (!mProtocolEventTable.ContainsKey(protocolEnum))
                {
                    mProtocolEventTable.Add(protocolEnum, null);
                }

                mProtocolEventTable[protocolEnum] = (Callback<System.Object>)mProtocolEventTable[protocolEnum] + kHandler;
            }
        }

        public static void RemoveListener(CvListenerEven protocolEnum, Callback<System.Object> kHandler)
        {
            lock (mProtocolEventTable)
            {
                if (mProtocolEventTable.ContainsKey(protocolEnum))
                {
                    mProtocolEventTable[protocolEnum] = (Callback<System.Object>)mProtocolEventTable[protocolEnum] - kHandler;

                    if (mProtocolEventTable[protocolEnum] == null)
                    {
                        mProtocolEventTable.Remove(protocolEnum);
                    }
                }
            }
        }

        public static void Invoke(CvListenerEven protocolEnum, System.Object arg1)
        {
            try
            {
                Delegate kDelegate;
                if (mProtocolEventTable.TryGetValue(protocolEnum, out kDelegate))
                {
                    Callback<System.Object> kHandler = (Callback<System.Object>)kDelegate;

                    if (kHandler != null)
                    {
                        kHandler(arg1);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static void UnInit()
        {
            mProtocolEventTable.Clear();
        }
			
    }


}
