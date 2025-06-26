
using SLua;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace EditorTool
{
    public class CfgCellBase<T> : EComponentBase, IDraw where T : CfgBase
    {
        public struct UIBindData
        {
            public CfgEntry data;
            public ELabel ui;
        }
		public GUIContent _WarnContent = new GUIContent(EditorGUIUtility.FindTexture("console.warnicon.sml"));
		public GUIContent _ErrorContent = new GUIContent(EditorGUIUtility.FindTexture("console.erroricon.sml"));
        //public EFoldout.OnChooseEvent OnChooseEvtPass2Parent;
		public T cfgdata;
		public ELabel _ItemLabel = new ELabel();
		public float inspectorLabelWidth = 200f;

		private static GUIStyle _DetailLabelStyle = new GUIStyle();
		private static GUIStyle _TitleLabelStyle = new GUIStyle();
		private static Color32 _Red = new Color32(200, 25, 25, 255);
		private List<UIBindData> _DetailUIDatas = new List<UIBindData>();
		private ELabel _ErrorLabel = new ELabel();
		private ELabel _WarnLabel = new ELabel();

        public CfgCellBase()
        {

        }

        public void BindData(T data)
        {
            cfgdata = data;
        }

        public virtual void InitCell()
		{
			ReInitUI();
			for (int i = 0; i < cfgdata.data.Count; i++)
			{
				UIBindData uidata = new UIBindData();
				uidata.data = cfgdata.data[i];
				uidata.ui = new ELabel(cfgdata.data[i].showString, inspectorLabelWidth, cfgdata.data[i].callback);
				_DetailUIDatas.Add(uidata);
				if (uidata.data.isError)
					uidata.ui.SetTooltip(uidata.data.GetErrorString());
			}
			InitStyle ();
        }

		void InitStyle()
		{
			_DetailLabelStyle.alignment = TextAnchor.MiddleLeft;
			_DetailLabelStyle.normal.textColor = Color.white;
			_DetailLabelStyle.fontSize = 12;
			_TitleLabelStyle.alignment = TextAnchor.MiddleLeft;
			_TitleLabelStyle.normal.textColor = Color.white;
			_TitleLabelStyle.fontSize = 12;
		}

        public virtual void ReInitUI()
		{
			_ItemLabel.AddOnClickListener (OnClickTitle);
			RefreshErrorWarnUI ();
			for(int i = 0; i < _DetailUIDatas.Count; i++)
			{
				_DetailUIDatas [i].ui.content.text = _DetailUIDatas [i].data.showString;
				if(_DetailUIDatas [i].data.isError)
					_DetailUIDatas [i].ui.SetTooltip(_DetailUIDatas [i].data.GetErrorString());
            }
            _ItemLabel.content.text = cfgdata.id + " " + cfgdata.name;
        }

		private void RefreshErrorWarnUI()
		{
			if (cfgdata.errornum > 0)
			{
				_ErrorContent.text = cfgdata.errornum.ToString();
				_ErrorLabel = new ELabel(_ErrorContent);
			}
			if (cfgdata.warnnum > 0)
			{
				_WarnContent.text = cfgdata.warnnum.ToString ();
				_WarnLabel = new ELabel (_WarnContent);
			}
		}

        //public void SetOnChooseEvt(EFoldout.OnChooseEvent evt)
        //{
        //    OnChooseEvtPass2Parent = evt;
        //}

        public int rownum { get; set; }

        public int SetRowNum(int lastIdx)
        {
            rownum = ++lastIdx;
            return rownum;
        }

        public int SetChildRowNum(int lastIdx)
        {
            return lastIdx;
        }

		//绘制左侧列表内的标题
        public virtual void DrawTitle(int firstRowIdx, int totalRowNum)
		{
			if (rownum >= firstRowIdx || (rownum <= firstRowIdx + totalRowNum))
			{
				Rect r = ConfigListWindow.GetTitleRect(rownum);
				if(ConfigListWindow.selectedUI == this)
					EditorGUI.DrawRect(r, selectedCol);
				if (_ItemLabel != null)
					_ItemLabel.DrawGUI(r, _TitleLabelStyle);

				r.x = r.x + r.width;
				if (cfgdata.errornum > 0)
				{
					r.x -= 30;
					_ErrorLabel.DrawGUI(r, _TitleLabelStyle);
				}
				if (cfgdata.warnnum > 0)
				{
					r.x -= 30;
					_WarnLabel.DrawGUI(r, _TitleLabelStyle);
				}
			}
        }

		//在详情面板ConfigInSpectorWindow上绘制配置详情
        public virtual void DrawDetail()
		{
			EditorGUILayout.BeginVertical();
			for (int i = 0; i < _DetailUIDatas.Count; i++)
			{
				if (_DetailUIDatas[i].data.isError)
					_DetailLabelStyle.normal.textColor = _Red;
				else if(_DetailUIDatas[i].data.isWarning)
					_DetailLabelStyle.normal.textColor = Color.yellow;
				else
					_DetailLabelStyle.normal.textColor = Color.white;
				_DetailUIDatas[i].ui.Draw(_DetailLabelStyle);
				if (_DetailUIDatas[i].data.isError)
					EditorGUILayout.HelpBox(_DetailUIDatas[i].data.GetErrorString(), MessageType.Error);
				if (_DetailUIDatas[i].data.isWarning)
					EditorGUILayout.HelpBox(_DetailUIDatas[i].data.GetWarningString(), MessageType.Warning);
			}
			EditorGUILayout.EndVertical();
        }

		public bool Search (string searchStr)
		{
			return cfgdata.Search (searchStr);
		}

		public IDraw GetChild(int row) { return null; }

		static Color32 selectedCol = new Color32(62, 85, 150, 255);
		public void OnClickTitle(EComponentBase ec)
		{
			ConfigListWindow.selectedUI = this;
		}
    }
}
