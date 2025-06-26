using System;
using UnityEditor;
using UnityEngine;
using SLua;
using System.IO;
using System.Collections.Generic;

namespace EditorTool
{
	public class TableConfigMemoryToolWindow : EditorWindow
	{
		enum SortMode
		{
			Name,
			Memory,
            Size,
            Sub,
            Percent
		}

        class LabelCell
        {
            public string name;
            public int mem;
            public int size;
            public int sub;
            public int percent;

            static int _FontSize = 14;
            static GUIStyle _LeftAligment = null;
            public static GUIStyle leftAlignment
            {
                get
                {
                    if(_LeftAligment == null)
                    {
                        _LeftAligment = new GUIStyle();
                        _LeftAligment.normal.textColor = Color.white;
                        _LeftAligment.fontSize = _FontSize;
                        _LeftAligment.alignment = TextAnchor.MiddleLeft;
                    }
                    return _LeftAligment;
                }
            }

            static GUIStyle _RightAligment = null;
            public static GUIStyle rightAlignment
            {
                get
                {
                    if (_RightAligment == null)
                    {
                        _RightAligment = new GUIStyle();
                        _RightAligment.normal.textColor = Color.white;
                        _RightAligment.fontSize = _FontSize;
                        _RightAligment.alignment = TextAnchor.MiddleRight;
                    }
                    return _RightAligment;
                }
            }
            static Color32 _SelectedColor = new Color32(62, 85, 150, 255);
            public LabelCell(string name, int mem, int size)
            {
                this.name = name;
                this.mem = mem;
                this.size = size;
                this.sub = mem - size;
                this.percent = size == 0 ? 0 : (int)((mem / (float)size) * 100);
                //string color = percent < 110 ? "[00ffffff]" : percent < 120 ? "[ffff00ff]" : "[ff0000ff]";
            }

            public void Draw()
            {
                Rect r = EditorGUILayout.GetControlRect();
                if (r.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                    OnChooseLabel(this);
                if(_ChooseLabels.Contains(this))
                    EditorGUI.DrawRect(new Rect(r), _SelectedColor);
                EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField(name, GUILayout.MaxHeight(16), GUILayout.ExpandWidth(true));
                //EditorGUILayout.LabelField(mem.ToString(), GUILayout.MaxHeight(16), GUILayout.ExpandWidth(true));
                //EditorGUILayout.LabelField(size.ToString(), GUILayout.MaxHeight(16), GUILayout.ExpandWidth(true));
                //EditorGUILayout.LabelField(percent + "%", GUILayout.MaxHeight(16), GUILayout.ExpandWidth(true));

                GUI.Label(r, name, leftAlignment);
                //GUILayout.Label(name);
                //GUILayout.FlexibleSpace();
                float cellX = (r.width - 115) / 5;//115是检测按钮 + 隐藏小文件勾选框的总长度
                r.x += cellX + 40;
                r.width -= cellX + 40;//额外减去检测按钮的长度
                GUI.Label(r, mem.ToString(), leftAlignment);
                r.x += cellX;
                r.width -= cellX;
                GUI.Label(r, size.ToString(), leftAlignment);
                r.x += cellX;
                r.width -= cellX;
                GUI.Label(r, sub.ToString(), leftAlignment);
                r.x += cellX;
                r.width -= cellX;
                GUI.Label(r, percent + "%", leftAlignment);
                EditorGUILayout.EndHorizontal();
            }
        }
        public static TableConfigMemoryToolWindow window;

		static string _ResourcesFolder = "Assets/Resources/";
		List<string> _CheckFolderList = new List<string>(){
			"Script/Config",
			"Script/FrameWork/Config"
		};
		List<string> _InvaildFileList = new List<string>()
		{
			"Script/Config/Table",
			"Script/FrameWork/Config/CameraConfig",
			"Script/FrameWork/Config/ColorUtil",
			"Script/FrameWork/Config/LayerConfig",
			"Script/FrameWork/Config/LuaConfig",
			"Script/FrameWork/Config/PlotStoryView"
		};
        static int _ShowMemory = 0;
        static int _ShowSize = 0;
		static int _TotalMemory = 0;
        static int _TotalSize = 0;
		Vector2 m_v2ScrollPos = Vector2.zero;
        static List<LabelCell> _ChooseLabels = new List<LabelCell>();
		List<LabelCell> _ResultList = new List<LabelCell>();
		SortMode m_eSortMode = SortMode.Name;
		LuaFunction _LuaFunction;
        LuaFunction luaFunction
        {
            get
            {
                if(_LuaFunction == null)
                    _LuaFunction = LuaSvrForEditor.Me.DoString(m_sLuaRequire) as LuaFunction;
                return _LuaFunction;
            }
        }
		string m_sLuaRequire = @"
		return function(path)
			require(path)
		end
		";
//		public int getMemoryLuaFuncRef = 0;
//		string m_sGetMemoryLuaFunc = @"
//local gc = collectgarbage
//local mem = 0
//return function(path)
//	mem = gc('count')
//	require(path)
//	mem = gc('count') - mem
//	return mem
//end
//";
		LuaState luaState { get { return LuaSvrForEditor.Me.luaState; } }

        bool m_bHideSmallFile = false;

		[MenuItem("Window/配置编辑器/检测配置内存")]
		public static void OpenWindow()
		{
			window = GetWindow<TableConfigMemoryToolWindow> ();
			window.titleContent = new GUIContent ("检测配置内存");
			window.Init ();
		}

		void StartCheck()
		{
            ReStartLuaState();
            _TotalSize = _TotalMemory = 0;
			_ResultList.Clear ();
			CollectFolder (ref _CheckFolderList, "Script", "Config_", true, "Script");
			List<string> files = CollectAllFolders (_CheckFolderList);

           /* files = new List<string>();
            files.Add("Script/Config_Skill_JiNeng/Table_Skill@@");
            files.Add("Script/Config_Skill_JiNeng/Table_Buffer@@");
            files.Add("Script/Config_Item_DaoJu/Table_Item@@");
            files.Add("Script/Config_Npc_MoWu/Table_Npc@@");
            files.Add("Script/Config_Npc_MoWu/Table_Monster@@");
            files.Add("Script/Config_Item_DaoJu/Table_Reward@@");
            files.Add("Script/Config_Equip_ZhuangBei_KaPian/Table_Equip@@");*/
            
            for (int i = 0; i < files.Count; i++)
			{
				if(!_InvaildFileList.Contains(files[i]))
				{
					string fileName = Path.GetFileNameWithoutExtension (files [i]);
					int mem = GetLuaStateMemory();
					LuaRequire (files [i]);
					LuaDLL.lua_gc (luaState.L, LuaGCOptions.LUA_GCCOLLECT, 0);
					mem = GetLuaStateMemory() - mem;

                    string fullPath = Application.dataPath + "/Resources/" + files[i] + ".txt";
                    int size = File.Exists(fullPath) ? (int)(new FileInfo(fullPath).Length / 1024) : 0;

                    LabelCell entry = new LabelCell(fileName, mem, size);
					_ResultList.Add (entry);
					_TotalMemory += mem;
                    _TotalSize += size;
				}
			}
            _ShowMemory = _TotalMemory;
            _ShowSize = _TotalSize;
        }

        void ReStartLuaState()
        {
            LuaSvrForEditor.Me.Dispose();
            LuaSvrForEditor.Me.init();
            _LuaFunction = LuaSvrForEditor.Me.DoString(m_sLuaRequire) as LuaFunction;
        }

		void LuaRequire(string path)
		{
			luaFunction.call (path);
        }
        int GetLuaStateMemory()
        {
            if (luaState != null)
                return LuaDLL.lua_gc(luaState.L, LuaGCOptions.LUA_GCCOUNT, 0);
            return -1;
        }

        void Init()
		{
            _TotalSize = _TotalMemory = 0;
            _ShowSize = _ShowMemory = 0;
		}

		void Update()
		{
			Repaint ();
		}

		void OnEnable()
		{
			Init();
		}

		void OnGUI()
		{
			DrawToolbar ();
			if (_ResultList.Count > 0)
				DrawContents ();
		}

		void DrawToolbar()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            OnCheckBtn(GUILayout.Button("检测", EditorStyles.toolbarButton, GUILayout.MaxWidth(40)));

			OnToggle(SortMode.Name, GUILayout.Toggle (m_eSortMode == SortMode.Name, "名称", EditorStyles.toolbarButton));

			string str = "内存大小(kb)";
			if(_ShowMemory > 0)
				str += string.Format(" 总{0}MB", _ShowMemory / 1024);
			OnToggle(SortMode.Memory, GUILayout.Toggle (m_eSortMode == SortMode.Memory, str, EditorStyles.toolbarButton));

            str = "文件大小(kb)";
            if (_ShowSize > 0)
                str += string.Format(" 总{0}MB", _ShowSize / 1024);
            OnToggle(SortMode.Size, GUILayout.Toggle(m_eSortMode == SortMode.Size, str, EditorStyles.toolbarButton));

            str = "差值(kb)";
            if (_ShowMemory > 0)
                str += string.Format(" 总差值{0}MB", (_ShowMemory - _ShowSize) / 1024);
            OnToggle(SortMode.Sub, GUILayout.Toggle(m_eSortMode == SortMode.Sub, str, EditorStyles.toolbarButton));

            str = "比率(内存/文件大小)";
            if (_ShowMemory > 0 && _ShowSize > 0)
                str += string.Format(" 总比率{0}", (int)((_ShowMemory / (float)_ShowSize) * 100) + "%");
            OnToggle(SortMode.Percent, GUILayout.Toggle(m_eSortMode == SortMode.Percent, str, EditorStyles.toolbarButton));

            m_bHideSmallFile = EditorGUILayout.Toggle(m_bHideSmallFile, GUILayout.MaxWidth(15));
            EditorGUILayout.LabelField("隐藏小文件", GUILayout.MaxWidth(60));

			EditorGUILayout.EndHorizontal();
		}

		void DrawContents()
		{
			m_v2ScrollPos = GUILayout.BeginScrollView (m_v2ScrollPos);
            for (int i = 0, length = _ResultList.Count; i < length; ++i)
			{
                if (!m_bHideSmallFile || _ResultList[i].size > 200)
                    _ResultList[i].Draw();
            }
			GUILayout.EndScrollView ();
		}

        static void OnChooseLabel(LabelCell label)
        {
            if (!(Event.current.modifiers == EventModifiers.Control))
            {
                _ChooseLabels.Clear();
            }
            _ChooseLabels.Add(label);
            if (_ChooseLabels.Count > 1)
            {
                _ShowMemory = 0;
                for (int i = 0; i < _ChooseLabels.Count; i++)
                    _ShowMemory += _ChooseLabels[i].mem;
            }
            else
                _ShowMemory = _TotalMemory;
        }

		void OnCheckBtn(bool isOnBtn)
		{
			if (isOnBtn)
				StartCheck ();
		}

		void OnToggle(SortMode mode, bool isToggleOn)
		{
			if (isToggleOn)
			{
				m_eSortMode = mode;
				Sort (m_eSortMode);
			}
		}

		void Sort(SortMode mode)
		{
			if(_ResultList.Count > 0)
			{
				_ResultList.Sort ((x, y) => 
				{
					switch (mode) 
					{
						case SortMode.Memory:
							return y.mem.CompareTo(x.mem);
                        case SortMode.Size:
                            return y.size.CompareTo(x.size);
                        case SortMode.Sub:
                            return y.sub.CompareTo(x.sub);
                        case SortMode.Percent:
                            return y.percent.CompareTo(x.percent);
						case SortMode.Name:
						default:
							return x.name.CompareTo(y.name);
					}
				});
			}
		}

		private static List<string> CollectAllFolders(List<string> folders)
		{
			List<string> files = new List<string> ();
			for (int i = 0; i < folders.Count; i++)
			{
				if (Directory.Exists(_ResourcesFolder + folders [i]))
					CollectFile(ref files, folders [i], true, folders [i]);
			}
			return files;
		}

		private static void CollectFile(ref List<string> fileList, string folder, bool recursive = false, string ppath = "")
		{
			folder = AppendSlash(folder);
			ppath = AppendSlash(ppath);
			DirectoryInfo dir = new DirectoryInfo(_ResourcesFolder + folder);
			FileInfo[] files = dir.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				if (files [i].Extension == ".txt")
				{
					string fpath = ppath + Path.GetFileNameWithoutExtension(files [i].Name);
                    if(!string.IsNullOrEmpty(fpath))
					    fileList.Add (fpath);
				}
			}

			if (recursive)
			{
				foreach (var sub in dir.GetDirectories())
				{
					CollectFile(ref fileList, folder + sub.Name, recursive, ppath + sub.Name);
				}
			}
		}

		static void CollectFolder(ref List<string> floderList, string root, string pattern, bool recursive = false, string ppath = "")
		{
			root = AppendSlash(root);
			ppath = AppendSlash(ppath);
			DirectoryInfo dir = new DirectoryInfo(_ResourcesFolder + root);
			DirectoryInfo[] dirs = dir.GetDirectories ();
			for(int i = 0; i < dirs.Length; i++)
			{
				if(dirs[i].Name.StartsWith(pattern))
				{
					string dpath = ppath + dirs [i].Name;
					if (!string.IsNullOrEmpty (dpath) && !floderList.Contains (dpath))
						floderList.Add (dpath);
				}
				else if (recursive)
				{
					CollectFolder(ref floderList, root + dirs[i].Name, pattern, recursive, ppath + dirs[i].Name);
				}
			}
		}

		public static string AppendSlash(string path)
		{
			if (path == null || path == "")
				return "";
			int idx = path.LastIndexOf('/');
			if (idx == -1)
				return path + "/";
			if (idx == path.Length - 1)
				return path;
			return path + "/";
		}


#region 策划Lua导入       by czj
//        //按钮
//        [MenuItem("Window/策划脚本优化导入")]
//        public static void ImportConfigOpt()
//        {
//            ImportConfig();
//            ConfigOpt();
//        }
//        
//        //按钮
//        [MenuItem("Window/策划脚本无优化导入")]
//        public static void ImporttConfig()
//        {
//            ImportConfig();
//        }
		
		//按钮
		[MenuItem("Window/策划脚本纯优化")]
		public static void OnlyOptConfig()
		{
			//zhouxin
			SplitDialog.GetInstance().OptFile();
    		StringConfigOpt();
			ConfigOpt();
		}

        //相对路径
		public static string s_sConfigSource = "/../../../Cehua/Table/lua_client";
		public static string s_sConfigDir    = "/Resources/Script/";
        public static string s_sCompCopy     = "*.txt";
		static string reScriptSrcPath = "../../../client-export/tttt.txt";
		static string reScriptDestPath = "Resources/Script/";
        //导入
        public static void ImportConfig()
        {
            string src  = Application.dataPath + s_sConfigSource;
            string dest = Application.dataPath + s_sConfigDir;

            if (!Directory.Exists(src)) return;
			DirectoryInfo sourceInfo = new DirectoryInfo (src);
			CopyFiles (sourceInfo, Directory.CreateDirectory (dest));
        }

		static void copyRefactoryScript()
		{
			string srcPath = Path.Combine(Application.dataPath, reScriptSrcPath);
			string destPath = Path.Combine(Application.dataPath, reScriptDestPath);
			string destFileName = Path.Combine(destPath, "tttt.txt");
			FileInfo fileInfo = new FileInfo(srcPath);
			if (fileInfo != null)
			{
				fileInfo.CopyTo(destFileName, true);
			}
			AssetDatabase.Refresh();
		}

		static void delRefactoryScripty()
		{
			string destPath = Path.Combine(Application.dataPath, reScriptDestPath);
			string destFileName = Path.Combine(destPath, "tttt.txt");
			FileInfo fileInfo = new FileInfo(destFileName);
			if (fileInfo != null)
			{
				fileInfo.Delete();
			}
			AssetDatabase.Refresh();
		}

		static public void CopyFiles(DirectoryInfo sourceInfo, DirectoryInfo tarInfo)
		{
			FileInfo[] files = sourceInfo.GetFiles (s_sCompCopy, SearchOption.TopDirectoryOnly);
			for (int i = 0, max = files.Length; i < max; ++i) 
			{
				files [i].CopyTo (tarInfo.FullName + '/' + files [i].Name, true);
			}
			DirectoryInfo[] dirs = sourceInfo.GetDirectories ();
			for (int i = 0, max = dirs.Length; i < max; ++i) 
			{
				DirectoryInfo dirInfo = Directory.CreateDirectory (tarInfo.FullName + '/' + dirs[i].Name);
				CopyFiles (dirs [i], dirInfo);
			}
		}



		//String 优化

		#endregion

		public static void StringConfigOpt()
		{
			copyRefactoryScript();
			//优化列表初始化
			List<ConfigOptInfo> OptLists = new List<ConfigOptInfo>();
			OptLists.Add(new ConfigOptInfo("Config_Item_DaoJu", "Table_Item", ""));
			OptLists.Add(new ConfigOptInfo("Config_Npc_MoWu", "Table_Monster", ""));
			OptLists.Add(new ConfigOptInfo("Config_Npc_MoWu", "Table_Npc", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest1", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest2", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest3", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest4", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest5", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest6", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest7", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest10", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest11", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Quest61", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_NpcTalk", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_Left", ""));
			OptLists.Add(new ConfigOptInfo("MConfig/Dialog", "Table_Dialog_NpcDefault", ""));
			OptLists.Add(new ConfigOptInfo("Config_Equip_ZhuangBei_KaPian", "Table_EquipUpgrade", ""));
			OptLists.Add(new ConfigOptInfo("Config_Property_ZhiYe_ShuXing", "Table_AddPoint", ""));
            OptLists.Add(new ConfigOptInfo("Config_Skill_JiNeng", "Table_Buffer", ""));
			LuaSvrForEditor.Me.Dispose();
			LuaSvrForEditor.Me.init();
			
			foreach (ConfigOptInfo info in OptLists)
			{
				string filePathWithoutEx = Path.Combine("Script", info.m_sDirPath);
				string direct = Path.Combine("Script", "MConfig");;
				filePathWithoutEx = Path.Combine(filePathWithoutEx, info.m_sLuaTableName);
				string filePathWithEx = filePathWithoutEx + ".txt";
				string saveFile = Path.Combine(Application.dataPath, "Resources");
				direct = Path.Combine(saveFile, direct);
				
				saveFile = Path.Combine(saveFile, filePathWithEx);
				saveFile = saveFile.Replace("\\", "/");
				direct = direct.Replace("\\", "/");
				OptConfig.GetInstance().OptFileTableToString(filePathWithoutEx, saveFile, info.m_sLuaTableName,direct);
			}

			delRefactoryScripty();
		}
		
        //优化

		public static void ConfigOpt()
		{
			copyRefactoryScript();
			//Copy备用OptLua 去加载目录

			//优化列表初始化
			List<ConfigOptInfo> OptLists = new List<ConfigOptInfo>();
			OptLists.Add(new ConfigOptInfo("Config_Adventure_ChengJiu_MaoXian", "Table_AdventureAppend", "targetID"));
			OptLists.Add(new ConfigOptInfo("Config_Adventure_ChengJiu_MaoXian", "Table_AdventureLevel", "AdventureAttr"));
			OptLists.Add(new ConfigOptInfo("Config_Adventure_ChengJiu_MaoXian", "Table_Appellation", "GroupID"));

			OptLists.Add(new ConfigOptInfo("Config_Equip_ZhuangBei_KaPian", "Table_Card", "Position@@@CardType"));
			OptLists.Add(new ConfigOptInfo("Config_Equip_ZhuangBei_KaPian", "Table_Equip", "Type@@@SuitID@@@CanEquip"));
			OptLists.Add(new ConfigOptInfo("Config_Equip_ZhuangBei_KaPian", "Table_EquipEnchant", "EnchantType@@@CantEnchant"));
			OptLists.Add(new ConfigOptInfo("Config_Equip_ZhuangBei_KaPian", "Table_EquipRefine", "EuqipType"));
			OptLists.Add(new ConfigOptInfo("Config_Equip_ZhuangBei_KaPian", "Table_EquipSuit", "SuitOneAdd@@@SuitTwoAdd@@@SuitThreeAdd@@@SuitFourAdd@@@SuitFiveAdd@@@SuitSixAdd"));
			OptLists.Add(new ConfigOptInfo("Config_Equip_ZhuangBei_KaPian", "Table_Exchange", "FashionType"));
			OptLists.Add(new ConfigOptInfo("Config_Equip_ZhuangBei_KaPian", "Table_HighRefine", "Effect@@@PosType"));

			OptLists.Add(new ConfigOptInfo("Config_Event_ShiJian", "Table_RandomMonster", "MapGroupID"));
			OptLists.Add(new ConfigOptInfo("Config_Event_ShiJian", "Table_Viewspot", "MapName"));
			OptLists.Add(new ConfigOptInfo("Config_Event_ShiJian", "Table_WantedQuest", "Reward@@@Type@@@TeamSync"));

			OptLists.Add(new ConfigOptInfo("Config_Guild_GongHui", "Table_GuildFunction", "BuildingParam"));
			OptLists.Add(new ConfigOptInfo("Config_Guild_GongHui", "Table_GuildPVE_Monster", "LevelRange@@@GroupID@@@MonsterType"));
            
			OptLists.Add(new ConfigOptInfo("Config_Hint_TiShiZhiYin", "Table_Menu", "Tip"));
			OptLists.Add(new ConfigOptInfo("Config_Hint_TiShiZhiYin", "Table_RedTip", "Condition"));
			OptLists.Add(new ConfigOptInfo("Config_Hint_TiShiZhiYin", "Table_Sysmsg", "Type"));

			OptLists.Add(new ConfigOptInfo("Config_Skill_JiNeng", "Table_Skill", "NameZh@@@Icon@@@Class"));
//			OptLists.Add(new ConfigOptInfo("Config_Npc_MoWu", "Table_Npc", "Type@@@Race@@@Nature@@@MapIcon@@@Emoji"));
//			OptLists.Add(new ConfigOptInfo("Config_Npc_MoWu", "Table_Monster", "Type@@@Race@@@Zone@@@ShowName@@@Shape"));
//			OptLists.Add(new ConfigOptInfo("Config_Item_DaoJu", "Table_Item", "Quality@@@Type@@@MaxNum"));
			OptLists.Add(new ConfigOptInfo("Config_Skill_JiNeng", "Table_Buffer", "Condition@@@BuffType@@@BuffIcon"));
			OptLists.Add(new ConfigOptInfo("Config_Item_DaoJu", "Table_Reward", "team"));
            
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_1", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_10", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_11", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_12", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_2", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_3", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_4", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_5", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_6", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_7", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_8", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Augury_2_9", "Type@@@Title"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_MenuUnclock", "MenuID@@@MenuDes"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_amatsu", "StandPot@@@Dir"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_aldebaran", "StandPot@@@Dir"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_byalan", "StandPot@@@Dir"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_clocktower1", "StandPot@@@Dir"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_clocktower2", "StandPot@@@Dir"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_clocktower_dun1", "StandPot@@@Dir"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_FerrisWheel", "StandPot@@@Dir"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_flowersea", "StandPot@@@Dir"));
			OptLists.Add(new ConfigOptInfo("Config", "Table_Seat_gef_dun1", "StandPot@@@Dir"));
//			OptLists.Add(new ConfigOptInfo("Config_Property_ZhiYe_ShuXing", "Table_AddPoint","AddPointSolution_1"));
			
//	        List<String> dirList = new List<String>();
//	        dirList.Add(("Config*"));
			//创建新的LUA state
			
			foreach (ConfigOptInfo info in OptLists)
			{
				string filePathWithoutEx = Path.Combine("Script", info.m_sDirPath);
				filePathWithoutEx = Path.Combine(filePathWithoutEx, info.m_sLuaTableName);
				string filePathWithEx = filePathWithoutEx + ".txt";
				string saveFile = Path.Combine(Application.dataPath, "Resources");
				saveFile = Path.Combine(saveFile, filePathWithEx);
				saveFile = saveFile.Replace("\\", "/");
//				if (saveFile.Contains("@@.txt"))
//					continue;
//				saveFile = saveFile.Replace(".txt", "@@.txt");
				OptConfig.GetInstance().OptFile(filePathWithoutEx, saveFile, info.m_sLuaTableName, info.m_sOptKey);
			}
			delRefactoryScripty();
		}
	}
}

