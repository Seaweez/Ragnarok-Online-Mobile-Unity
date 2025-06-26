using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
namespace RO
{
    public class SceneLightMapMono: MonoBehaviour
    {

        [System.Serializable]
        public struct RendererInfo
        {
            public Renderer renderer;
            public int lightmapIndex;
            public Vector4 lightmapOffsetScale;
        }


        public List<Texture2D>    m_LightmapColors;
        public List<RendererInfo> m_RendererInfo;
        public LightmapsMode      m_lightmapsMode;

        //贴图信息
        protected LightmapData[]  m_lightmapData;

        public void OnEnable()
        {
            SetUp();
        }

        public void OnDisable()
        {
            //Clear();
        }

        public void SetUp()
        {
            if (m_LightmapColors.Count < 1)
                return;

            if (m_lightmapData != null)
                return;

            //设置光照信息
            m_lightmapData = new LightmapData[m_LightmapColors.Count];
            for (int i = 0; i < m_lightmapData.Length; i++)
            {
                m_lightmapData[i] = new LightmapData();
                m_lightmapData[i].lightmapColor = m_LightmapColors[i];
            }
            LightmapSettings.lightmapsMode = m_lightmapsMode;
            LightmapSettings.lightmaps = m_lightmapData;
            LoadLightmap();
        }

        public void LoadLightmap()
        {
            if (m_RendererInfo.Count <= 0) return;
            foreach (var item in m_RendererInfo)
            {
                item.renderer.lightmapIndex = item.lightmapIndex;
                item.renderer.lightmapScaleOffset = item.lightmapOffsetScale;
            }
        }

        public void SaveLightInfo()
        {
            m_LightmapColors = new List<Texture2D>();
            for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
            {
                LightmapData data = LightmapSettings.lightmaps[i];
                //if(data.lightmapColor != null)
                m_LightmapColors.Add(data.lightmapColor);
            }
            m_RendererInfo = new List<RendererInfo>();
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer r in renderers)
            {
                if (r.lightmapIndex != -1)
                {
                    RendererInfo info = new RendererInfo();
                    info.renderer = r;
                    info.lightmapOffsetScale = r.lightmapScaleOffset;
                    info.lightmapIndex = r.lightmapIndex;
                    m_RendererInfo.Add(info);
                }
            }
            m_lightmapsMode = LightmapSettings.lightmapsMode;
        }

        public void Clear()
        {
            m_LightmapColors = null;
            m_RendererInfo = null;
            m_lightmapData = null;
        }
    }
}