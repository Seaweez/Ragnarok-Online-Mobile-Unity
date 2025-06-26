using System;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class LeftTimeInfo : ScriptableObject
    {
        public float leftTime = 0f;
    }

    public class CountDownWindow : EditorWindow
    {
        static float m_fEndTimeStamp = 0f;
        static float cd = 5f;//4小时
        static Action _Callback = null;
        static Vector2 _WindowSize = new Vector2(400f, 100f);
        const string _CDFormat = "{0:D2}小时{1:D2}分{2:D2}秒后自动更新项目...";
        bool m_bInited = false;
        
        SerializedObject _SerialObj;
        SerializedProperty seripLeftTime;

        private static CountDownWindow m_Instance = null;
        public static CountDownWindow Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindFirstInstance();
                    if (m_Instance == null)
						m_Instance = GetWindow<CountDownWindow>(true, "自动更新...");
                }
                return m_Instance;
            }
        }

        static GUIStyle _CenterAligment = null;
        public static GUIStyle centerAlignment
        {
            get
            {
                if (_CenterAligment == null)
                {
                    _CenterAligment = new GUIStyle();
                    _CenterAligment.normal.textColor = Color.white;
                    //_CenterAligment.fontSize = 16;
                    _CenterAligment.alignment = TextAnchor.MiddleCenter;
                }
                return _CenterAligment;
            }
        }

        public CountDownWindow() { }

        void Init()
        {
            InitPropety();
            var pos = Instance.position;
            pos = new Rect(Vector2.zero, _WindowSize);
            pos.center = new Vector2(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f);
            Instance.position = pos;
            m_bInited = true;
        }

        void Update()
        {
            if (!m_bInited)
                Init();
            if (m_fEndTimeStamp - EditorApplication.timeSinceStartup > 0)
                Repaint();
            else
                OnCallback();
        }

        void OnGUI()
        {
            if (!m_bInited)
                return;
            GUI.Label(new Rect(5f, 40f, 325f, 20f), GetLeftTimeStr(), centerAlignment);
            if(GUI.Button(new Rect(335f, 25f, 60f, 20f), "立刻更新"))
                ImmediateUpdate();
            if (GUI.Button(new Rect(335f, 55f, 60f, 20f), "取消"))
                CloseWindow();
        }

        void OnDisable()
        {
            m_Instance = null;
            m_bInited = false;
        }

        //void OnLostFocus()
        //{
        //    Focus();
        //}

        private void InitPropety()
        {
            ScriptableObject _ScripObj = ScriptableObject.FindObjectOfType<LeftTimeInfo>();
            if (_ScripObj == null)
                _ScripObj = ScriptableObject.CreateInstance<LeftTimeInfo>();
            _SerialObj = new SerializedObject(_ScripObj);
            seripLeftTime = _SerialObj.FindProperty("leftTime");
			if (seripLeftTime.floatValue > 0f && m_fEndTimeStamp == 0f)
                m_fEndTimeStamp = seripLeftTime.floatValue + (float)EditorApplication.timeSinceStartup;
        }

        private string GetLeftTimeStr()
        {
            float m_fLeftTime = m_fEndTimeStamp - (float)EditorApplication.timeSinceStartup;
            SetLeftTime(m_fLeftTime);
            int s = Mathf.FloorToInt(m_fLeftTime % 60f);
            int m = Mathf.FloorToInt((m_fLeftTime / 60f) % 60);
            int h = Mathf.FloorToInt(m_fLeftTime / 3600f);
            return string.Format(_CDFormat, h, m, s);
        }

        void OnCallback()
        {
            CloseWindow();
            if (_Callback != null)
                _Callback();
        }

        void ImmediateUpdate()
        {
            CloseWindow();
            AutoUpdate.ImmediateUpdate();
        }

        void SetLeftTime(float f)
        {
            seripLeftTime.floatValue = f;
            _SerialObj.ApplyModifiedProperties();
        }

        void CloseWindow()
        {
            SetLeftTime(0f);
            Close();
        }

        public static void Start(Action cb)
		{
			Instance.m_bInited = false;
            m_fEndTimeStamp = cd + (float)EditorApplication.timeSinceStartup;
            _Callback = cb;
            Instance.ShowUtility();
        }

        public static CountDownWindow FindFirstInstance()
        {
            var windows = (CountDownWindow[])Resources.FindObjectsOfTypeAll(typeof(CountDownWindow));
            if (windows.Length == 0)
                return null;
            return windows[0];
        }
    }
}
