using SLua;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorTool
{
    public class TableBase<T, U> : ITableBase
                                 where T : CfgCellBase<U>, new()
                                 where U : CfgBase, new()
    {
        #region 属性声明
        //private EFoldout _ChoosedFoldout;
        private TableTitleUI _TableTitleUI;
		public IDraw tableIDraw { get { return _TableTitleUI; }}
        public string tableName{ get; set; }
        /// <summary>
        /// 读取Table_XXX后生成<id, cfg>的字典, cfg为原始LuaTable对象, 表示一条Lua配置
        /// </summary>
        public Dictionary<int, LuaTable> luaTable = new Dictionary<int, LuaTable>();
        /// <summary>
        /// 解析luaTable后生成的数据
        /// </summary>
        public List<U> cfgList = new List<U>();
        /// <summary>
        /// 将数据和UI绑定后的对象
        /// </summary>
        public List<T> cellList = new List<T>();
        /// <summary>
        /// cellList的子集, 用于各种筛选
        /// </summary>
        public List<T> subCellList = new List<T>();
        #endregion

        /**
         * Table表创建流程：
         * 1. 初始化数据
         * 2. 创建各Table关联
         * 3. Table表检查
         * 4. 绑定UI和数据, 并初始化UI
         * **/
        public TableBase(string name)
        {
            _TableTitleUI = new TableTitleUI(name, this);
            tableName = name;
        }

        #region 1. 初始化数据
        public virtual void InitData()
        {
            LoadLuaTable();
            InitCfgList();
        }

        //1.1 初始化数据
        //加载LuaTable, 各子类分别重写
        public virtual void LoadLuaTable() { }

        //1.2 初始化数据
        //将LuaTable解析成CfgBase
        public virtual void InitCfgList()
        {
            cfgList = CreateCfgList(luaTable);
        }

        private List<U> CreateCfgList(Dictionary<int, LuaTable> infos)
        {
            List<U> list = new List<U>();
            foreach (var info in infos)
            {
                U cfg = new U();
                cfg.Init(info.Key, info.Value);
                //cell.SetOnChooseEvt(OnChooseEvt);
                list.Add(cfg);
            }
            list.Sort((a, b) => int.Parse(a.id).CompareTo(int.Parse(b.id)));
            return list;
        }
        #endregion

        #region 2. 创建各Table关联
        public virtual void CreateCfgBind()
        {
            for (int i = 0; i < cfgList.Count; i++)
            {
                cfgList[i].InitBind();
            }
        }
        #endregion

        #region 3. Table表检查
        public virtual void CfgCheck()
        {
            for (int i = 0; i < cfgList.Count; i++)
            {
                cfgList[i].Check();
            }
        }
        #endregion

        #region 4. 绑定UI和数据, 并初始化UI
        public virtual void InitUI()
        {
            InitCfgCellList();
            InitCellsUI();
        }

        //4.1 绑定UI和数据
        public virtual void InitCfgCellList()
        {
            cellList = CreateCfgCellList(cfgList);
            subCellList = cellList;
        }

        //4.2 初始化UI
        public virtual void InitCellsUI()
        {
            for (int i = 0; i < cellList.Count; i++)
            {
                cellList[i].InitCell();
            }
        }

        private List<T> CreateCfgCellList(List<U> cfglist)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < cfglist.Count; i++)
            {
                T cell = new T();
                cell.BindData(cfglist[i]);
                //cell.SetOnChooseEvt(OnChooseEvt);
                list.Add(cell);
            }
            return list;
        }
        #endregion

        public virtual void CfgDeepCheck()
        {
            for (int i = 0; i < cfgList.Count; i++)
            {
                cfgList[i].DeepCheck();
            }
            for(int i = 0; i < cellList.Count; i++)
            {
                cellList[i].ReInitUI();
            }
        }

        public int rownum
        {
            get { return _TableTitleUI.rownum; }
            set { _TableTitleUI.rownum = value; }
        }

        /// <summary>
        /// 设置每条配置对应的行号，供布局时使用
        /// 调用UI类的IDraw接口具体设置
        /// </summary>
        /// <param name="lastIdx">当前最后一行的行号</param>
        /// <returns>设置完成后最后一行的行号</returns>
        public int SetRowNum(int lastIdx)
        {
            return _TableTitleUI.SetRowNum(lastIdx);
        }

        public int SetChildRowNum(int lastIdx)
        {
            return _TableTitleUI.SetChildRowNum(lastIdx);
		}

		public IDraw GetChild(int row)
		{
			if (subCellList != null && subCellList.Count > row - 1)
				return subCellList [row - 1];
			return null; 
		}

        public virtual void DrawTitle(int firstRowIdx, int totalRowNum)
        {
            _TableTitleUI.DrawTitle(firstRowIdx, totalRowNum);
        }

        public virtual void DrawDetail()
        {
            _TableTitleUI.DrawDetail();
        }

        public virtual CfgBase FindCfgByKey(string key)
        {
            for (int i = 0; i < cfgList.Count; i++)
            {
                if (cfgList[i].key.ToString() == key)
                    return cfgList[i];
            }
            // Debug.LogError(string.Format("TableName = {0}, Cfg is not exist ! key = {1} !", tableName, key));
            return null;
        }

        public virtual CfgBase FindCfg(string key, string v)
        {
            for (int i = 0; i < cfgList.Count; i++)
            {
                if (cfgList[i].FindEntry(key).value == v)
                    return cfgList[i];
            }
            return null;
        }

        public virtual void Search(string searchStr)
		{
			List<T> filterList = new List<T> ();
            for (int i = 0; i < cellList.Count; i++)
            {
                if (cellList[i].Search(searchStr))
					filterList.Add(cellList[i]);
			}
			subCellList = filterList;
        }

        public virtual void ResetSearch()
        {
            subCellList = cellList;
        }

		public void OnFilterWarnsErrors(ConfigFilterType t)
        {
			int err = (int)(t & ConfigFilterType.Error);
			int warning = (int)(t & ConfigFilterType.Warning);
			if (err + warning < 1)
				return;
			List<T> filterList = new List<T> ();

			for (int i = 0; i < subCellList.Count; i++)
			{
				if (err > 0 && subCellList[i].cfgdata.errornum > 0)
				{
					filterList.Add(subCellList[i]);
					continue;
				}
				if(warning > 0 && subCellList[i].cfgdata.warnnum > 0)
				{
					filterList.Add(subCellList[i]);
					continue;
				}
            }
			subCellList = filterList;
        }

		public virtual void OnFilter (ConfigFilterType t, string searchContent)
		{
			if((t & ConfigFilterType.Search) > 0)
			{
				Search(searchContent);
			}
			else
			{
				ResetSearch();
				OnFilterWarnsErrors (t);
			}
		}

		public virtual void StatisticErrorNum(ref int errornum, ref int warnnum)
        {
			int e = 0, w = 0;
            for (int i = 0; i < cfgList.Count; i++)
            {
				e += cfgList[i].errornum;
				w += cfgList[i].warnnum;
            }
			errornum += e;
			warnnum += w;
			_TableTitleUI.RefreshErrorWarnLabel(e, w);
        }

        //private void OnChooseEvt(EComponentBase foldout, bool isChosse)
        //{
        //    if (_ChoosedFoldout != null)
        //        _ChoosedFoldout.SetChoose(false);
        //    _ChoosedFoldout = (EFoldout)foldout;
        //}

        #region Table的UI部分
        class TableTitleUI : IDraw
        {
			private ELabel _ErrorLabel = null;
			private ELabel _WarnLabel = null;
			public GUIContent _ErrorContent = new GUIContent(EditorGUIUtility.FindTexture("console.erroricon.sml"));
			public GUIContent _WarnContent = new GUIContent(EditorGUIUtility.FindTexture("console.warnicon.sml"));
            TableBase<T, U> _TableBase;
            ELabel _Label;
			static GUIStyle _TitleLabelStyle = new GUIStyle();
            static Texture _Icon = EditorGUIUtility.FindTexture("TextAsset Icon");
			public TableTitleUI(string tablename, TableBase<T, U> table)
            {
                GUIContent c = new GUIContent(tablename, _Icon);
				_Label = new ELabel(c, 100f, OnClickTitle);
				_TableBase = table;
				InitStyle();
			}

			void InitStyle()
			{
				_TitleLabelStyle.alignment = TextAnchor.MiddleLeft;
				_TitleLabelStyle.normal.textColor = Color.white;
				_TitleLabelStyle.fontSize = 12;
			}

            public int rownum { get; set; }

            public int SetRowNum(int lastIdx)
            {
                //设置自身
                rownum = ++lastIdx;
                return lastIdx;
            }

            public int SetChildRowNum(int lastIdx)
            {
                lastIdx = SetChildrenCellRowNum(lastIdx);
                return lastIdx;
			}

			public IDraw GetChild(int row)
			{
				if (_TableBase != null && _TableBase.subCellList != null && _TableBase.subCellList.Count > row - 1)
					return _TableBase.subCellList [row - 1];
				return null; 
			}

			public void RefreshErrorWarnLabel(int errorNum, int warnNum)
            {
				if (errorNum > 0) 
				{
					_ErrorContent.text = errorNum.ToString ();
					_ErrorLabel = new ELabel (_ErrorContent);
				}
				else
					_ErrorLabel = null;
				if(warnNum > 0)
				{
					_WarnContent.text = warnNum.ToString();
					_WarnLabel = new ELabel(_WarnContent);
				} 
				else
					_WarnLabel = null;
            }

            public void DrawTitle(int firstRowIdx, int totalRowNum)
            {
                if (rownum >= firstRowIdx || (rownum <= firstRowIdx + totalRowNum))
                {
                    Rect r = ConfigEditorWindow.GetTitleRect(rownum);
					if(ConfigEditorWindow.Instance.selectedUI == this)
						EditorGUI.DrawRect(r, selectedCol);
					_Label.DrawGUI(r, _TitleLabelStyle);

					r.x = r.x + r.width;
					if (_ErrorLabel != null)
					{
						r.x -= 50;
						_ErrorLabel.DrawGUI (r, _TitleLabelStyle);
					}
					if (_WarnLabel != null) 
					{
						r.x -= 50;
						_WarnLabel.DrawGUI (r, _TitleLabelStyle);
					}
                }
			}

			static Color32 selectedCol = new Color32(62, 85, 150, 255);
			public void OnClickTitle(EComponentBase ec)
			{
				ConfigEditorWindow.Instance.selectedUI = this;
			}

            public void DrawDetail()
            {
                DrawTableCellList(ConfigListWindow.firstRowIdx, ConfigListWindow.visibleRowNum);
            }

            void DrawTableCellList(int firstRowIdx, int totalRowNum)
            {
				for (int i = 0; i < _TableBase.subCellList.Count; i++)
                {
					_TableBase.subCellList[i].DrawTitle(firstRowIdx, totalRowNum);
                }
            }

            public int SetChildrenCellRowNum(int lastIdx)
            {
				for (int i = 0; i < _TableBase.subCellList.Count; i++)
                {
					lastIdx = _TableBase.subCellList[i].SetRowNum(lastIdx);
                }
                return lastIdx;
            }

        }
        #endregion
    }
    #region ITableBase接口, 主要给泛型调用
    public interface ITableBase : IDraw
    {
        #region 构造器接口，主要用于ConfigManager构造所有Table
        void InitData();
        void CreateCfgBind();
        void CfgCheck();
        void CfgDeepCheck();
        void InitUI();
        #endregion

        #region 搜索过滤接口
		void StatisticErrorNum(ref int errornum, ref int warnnum);
		void OnFilter (ConfigFilterType t, string searchContent);
        #endregion
        string tableName { get; }
        ///Table之间建立绑定关系时用以查询
        CfgBase FindCfgByKey(string key);
        CfgBase FindCfg(string key, string v);
		IDraw tableIDraw{ get; }
    }
    #endregion
}
