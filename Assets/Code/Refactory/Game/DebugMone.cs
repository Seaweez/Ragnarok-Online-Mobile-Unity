using UnityEngine;
using System.Collections.Generic;


    public class DebugMono : MonoBehaviour
    {
        void Awake()
        {
        #if Internal
            DebugInit();
        #endif
        }
#region µ÷ÊÔ
#if Internal
            bool m_bShowError = false;
            List<string> m_ListCon = new List<string>();
            List<string> m_ListStack = new List<string>();
            int m_ErrorCount = 0;
            int m_CurShowCount = 0;

            public void LogCallback(string condition, string stackTrace, LogType type)
            {
                if(type == LogType.Exception)
                {
                    m_ListCon.Add(condition);
                    m_ListStack.Add(stackTrace);
                    m_ErrorCount++;
                }
            }
#endif

#if Internal
        private void DebugInit()
        {

                Application.logMessageReceived += LogCallback;

        }
#endif

#if Internal
    private void OnGUI()
        {

            GUIStyle fontStyle = new GUIStyle();
            fontStyle.normal.background = null;                 //ÉèÖÃ±³¾°Ìî³ä  
            fontStyle.normal.textColor = new Color(1, 0, 0);    //ÉèÖÃ×ÖÌåÑÕÉ«  
            fontStyle.fontSize = 20;
            if (GUI.Button(new Rect(0, 0, 80, 30), "ShowError" + m_ErrorCount, fontStyle))
            {
                m_bShowError = true;
            }

            if (GUI.Button(new Rect(150, 0, 80, 30), "HideError" + m_ErrorCount, fontStyle))
            {
                m_bShowError = false;
                //string a = m_ListCon[3];
            }

            if (GUI.Button(new Rect(300, 0, 80, 30), "NextError" + m_CurShowCount, fontStyle))
            {
                m_CurShowCount++;
                if (m_CurShowCount > m_ErrorCount)
                    m_CurShowCount = 0;
            }

            if (true == m_bShowError)
            {
                if (m_CurShowCount > m_ErrorCount)
                    m_CurShowCount = 0;
                if(m_ListCon.Count > m_CurShowCount)
                {
                    string val = "Error" + m_CurShowCount + "\n";
                    val += m_ListCon[m_CurShowCount] + "\n";
                    val += m_ListStack[m_CurShowCount] + "\n";
                    GUI.Button(new Rect(0, 50, 1800, 1000), val, fontStyle);
                }
            }

        }
#endif
#endregion

    }

