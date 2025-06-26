using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
/// <summary>
///  by czj 5.3->2017  unity bug!!
/// </summary>
namespace RO
{
    public class UnloadBundleInfo
    {
        public AssetBundle m_Asset;
        public bool m_bVal;
        public int m_times;
       

        public UnloadBundleInfo(AssetBundle ab, bool bVal)
        {
            m_Asset = ab;
            m_bVal  = bVal;
            m_times = 2; 
        }
    }

    public class BUnloadMono : MonoBehaviour
    {
        protected Dictionary<string, UnloadBundleInfo> m_dicWaitUnload = new Dictionary<string, UnloadBundleInfo>();
        protected List<string> m_ClearList = new List<string>();
        private void Awake()
        {
            
        }
        public void Add(string id, AssetBundle ab, bool bVal)
        {
            if (m_dicWaitUnload.ContainsKey(id))
                return;
            m_dicWaitUnload.Add(id, new UnloadBundleInfo(ab, bVal));
        }
        public void Update()
        {
            foreach (var str in m_dicWaitUnload.Keys)
            {
                var info = m_dicWaitUnload[str];
                info.m_times = info.m_times - 1;
                if(info.m_times < 1)
                {
                    m_ClearList.Add(str);
                }
            }
            foreach(var str in m_ClearList)
            {
                var info = m_dicWaitUnload[str];
                //RO.ROLogger.LogError(info.m_Asset.name);
                info.m_Asset.Unload(info.m_bVal);
                m_dicWaitUnload.Remove(str);
            }
            m_ClearList.Clear();
        }
        
        public AssetBundle GetBundle(string id)
        {
            UnloadBundleInfo info;
            AssetBundle retVal = null;
            if(true == m_dicWaitUnload.TryGetValue(id, out info))
            {
                retVal = info.m_Asset;
                m_dicWaitUnload.Remove(id);
            }
            return retVal;
        }
        //public void LateUpdate()
        //{
        //    StartCoroutine(DoUnload());
        //}
        //private IEnumerator DoUnload()
        //{
        //    yield return new WaitForEndOfFrame();
        //    foreach(var a in m_dicWaitUnload.Values)
        //    {
        //        //RO.ROLogger.LogError(a.m_Asset.name);
        //        a.m_Asset.Unload(a.m_bVal);
        //    }
        //    m_dicWaitUnload.Clear();
        //}

    }

    public class BundleUnload : Singleton<BundleUnload>
    {
        GameObject m_UnloadGO;
        BUnloadMono m_Mono;

        public BundleUnload()
        {

        }

        public void Init()
        {
            m_UnloadGO = new GameObject();
            GameObject.DontDestroyOnLoad(m_UnloadGO);
            m_UnloadGO.name = "UnloadGO";
            m_Mono = m_UnloadGO.AddComponent<BUnloadMono>();
        }

        public void Add(string id, AssetBundle ab, bool bVal)
        {
            if (null == m_Mono)
                Init();
            m_Mono.Add(id, ab, bVal);
        }

        public AssetBundle GetBundle(string id)
        {
            if (null == m_Mono)
                Init();
            return m_Mono.GetBundle(id);
        }
    }
    #region NoUse
   /* public class UnloadBundleInfo
    {
        public AssetBundle m_Asset;
        public int m_Left;

        public UnloadBundleInfo(AssetBundle ab)
        {
            m_Asset = ab;
            m_Left = 150;
        }
    }

    public class BundleUnload : SingleTonGO<BundleUnload>
    {
        protected Dictionary<string, UnloadBundleInfo> m_dicWaitUnload = new Dictionary<string, UnloadBundleInfo>();
        protected List<string> m_Clear = new List<string>();
        void Update()
        {

            //¸üÐÂ
            foreach (string key in m_dicWaitUnload.Keys)
            {
                //UnloadBundleInfo info = 
            }


        }

        public void AddNeedRemoveBundle(string id, AssetBundle ab)
        {
            UnloadBundleInfo info = new UnloadBundleInfo(ab);
            if (true == m_dicWaitUnload.ContainsKey(id))
            {
                ROLogger.LogError("BundleUnload has Add : " + id);
                return;
            }
            m_dicWaitUnload.Add(id, info);
        }

        public AssetBundle TryGetBundle(string id)
        {
            UnloadBundleInfo info;
            if (true == m_dicWaitUnload.TryGetValue(id, out info))
            {
                m_dicWaitUnload.Remove(id);
                return info.m_Asset;
            }
            return null;
        }

    }*/
    #endregion
}
