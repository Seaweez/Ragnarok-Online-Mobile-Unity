using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace EditorTool
{
    public class SelectedCell
	{
		public string error = string.Empty;

        public string atlasName;
        public List<string> resourcePathList = new List<string>();
        public string iconWidth;
        public string iconHeight;
        public string atlasPath;
        public string targetPfbPath;
        public string targetMatPath;
        public string targetTexPath;
        public int iosFormat;
        public int androidFormat;
        public int maxSize = 2048;
        public bool forceSquare = false;
        public bool splitPath = true;

        AtlasMakerConfigCell _Cell;
        public AtlasMakerConfigCell cell
        {
            get { return _Cell; }
            set
            {
                _Cell = value;
                Reset(_Cell);
            }
        }

        static string _PfbPath = "{0}/{1}.prefab";
        static string _MatPath = "{0}/{1}.mat";
        static string _TexPath = "{0}/{1}.png";
        static string _SplitPfbPath = "{0}/preferb/{1}.prefab";
        static string _SplitMatPath = "{0}/material/{1}.mat";
        static string _SplitTexPath = "{0}/picture/{1}.png";
        public SelectedCell()
        {
        }

        public void Select(AtlasMakerConfigCell c)
        {
            cell = c;
        }

        public void SetSrcPath(int idx, string path)
        {
            if (idx < resourcePathList.Count)
                resourcePathList[idx] = path;
            else
                resourcePathList.Add(path);
            if (idx == 0)
                ResetPath();
        }

        public void SetSplitPath(bool b)
        {
            splitPath = b;
            ResetPath();
        }

        public void ResetPath()
        {
            if (resourcePathList.Count > 0)
            {
                if(string.IsNullOrEmpty(atlasName))
                    atlasName = resourcePathList[0].Substring(resourcePathList[0].LastIndexOfAny(new char[] { '/', '\\' }) + 1);
                if (!string.IsNullOrEmpty(atlasPath))
                {
                    string format = splitPath ? _SplitPfbPath : _PfbPath;
                    targetPfbPath = string.Format(format, atlasPath, atlasName);
                    format = splitPath ? _SplitMatPath : _MatPath;
                    targetMatPath = string.Format(format, atlasPath, atlasName);
                    format = splitPath ? _SplitTexPath : _TexPath;
                    targetTexPath = string.Format(format, atlasPath, atlasName);
                }
            }
            else
            {
                atlasName = string.Empty;
                targetPfbPath = string.Empty;
                targetMatPath = string.Empty;
                targetTexPath = string.Empty;
            }
        }

        public void Reset(AtlasMakerConfigCell cell)
        {
			error = string.Empty;
            atlasName = cell.data.atlasName;
            resourcePathList.Clear();
            for (int i = 0; i < cell.data.resourcePathList.Count; i++)
                resourcePathList.Add(cell.data.resourcePathList[i]);
            iconWidth = cell.data.iconWidth;
            iconHeight = cell.data.iconHeight;
            atlasPath = cell.data.atlasPath;
            targetPfbPath = cell.data.targetPfbPath;
            targetMatPath = cell.data.targetMatPath;
            targetTexPath = cell.data.targetTexPath;
            iosFormat = cell.data.iosFormat;
            androidFormat = cell.data.androidFormat;
            maxSize = cell.data.maxSize;
            forceSquare = cell.data.forceSquare;
            splitPath = cell.data.splitPath;
        }

        public void Save()
        {
            cell.data.atlasName = atlasName;
            cell.data.resourcePathList.Clear();
            for (int i = 0; i < resourcePathList.Count; i++)
                cell.data.resourcePathList.Add(resourcePathList[i]);
            cell.data.iconWidth = iconWidth;
            cell.data.iconHeight = iconHeight;
            cell.data.atlasPath = atlasPath;
            cell.data.targetPfbPath = targetPfbPath;
            cell.data.targetMatPath = targetMatPath;
            cell.data.targetTexPath = targetTexPath;
            cell.data.iosFormat = iosFormat;
            cell.data.androidFormat = androidFormat;
            cell.data.maxSize = maxSize;
            cell.data.forceSquare = forceSquare;
            cell.data.splitPath = splitPath;
            AtlasMakerConfigManager.Instance.dataManager.Save();
        }
    }

    class AtlasInspectorWindow : EditorWindow
	{
		public static AtlasInspectorWindow window = null;
		public static SelectedCell selected = new SelectedCell();

        static int[] TexFormatEnums = new int[]
        {
            (int)TextureImporterFormat.AutomaticCompressed,
			(int)TextureImporterFormat.RGBA16,
			(int)TextureImporterFormat.RGBA32,
//            (int) TextureImporterCompression.Compressed,
//            (int)TextureImporterCompression.CompressedHQ,
//            (int)TextureImporterCompression.CompressedLQ,
//            (int)TextureImporterCompression.Uncompressed
        };
        static string[] TexFormatDesc = new string[] { "compressed", "16bit", "truecolor" };
        static int[] MaxSizeArr = new int[] { 1024, 2048 };
        static string[] MaxSizeDescArr = new string[] { "1024", "2048" };
        static GUIContent _NotificationGUIContent = new GUIContent();
		static Vector2 m_ScrollPos = Vector2.zero;
		public AtlasInspectorWindow()
		{
			window = this;
		}

		void Update()
		{
			Repaint ();
		}

		void OnGUI()
		{
			m_ScrollPos = EditorGUILayout.BeginScrollView (m_ScrollPos);
			if(selected.cell != null)
				DrawInspector ();
			EditorGUILayout.EndScrollView ();
			if (selected != null) 
			{
				EditorGUILayout.BeginHorizontal (EditorStyles.toolbar);
				if (GUILayout.Button ("制作图集", EditorStyles.toolbarButton))
					OnClickMakeAtlas ();
				if (GUILayout.Button ("删除该配置", EditorStyles.toolbarButton))
					DestroyCurrentCfg ();
				EditorGUILayout.EndHorizontal();
			}
		}

		public void ShowNotification(string contents)
		{
			_NotificationGUIContent.text = contents;
			ShowNotification (_NotificationGUIContent);
		}

		public void DestroyCurrentCfg()
		{
			if(selected != null && selected.cell != null)
			{
				if(EditorUtility.DisplayDialog("删除配置", string.Format("确定要删除图集{0}的配置？", selected.atlasName), "确定", "取消"))
					AtlasMakerConfigManager.Instance.DestroyCell(selected.cell);
			}
		}

		public void OnClickMakeAtlas()
        {
            if (selected != null)
            {
                selected.Save();
                AtlasMaker.MakeAtlas(selected.cell.data);
            }
		}


        public void DrawInspector()
        {
            GUILayout.Label("素材路径：");
            for (int i = 0; i < selected.resourcePathList.Count; i++)
                DrawPathCell(selected.resourcePathList[i], i);
            if (GUILayout.Button("新增路径", GUILayout.ExpandWidth(false)))
                OnClickAddSourcePath();
            GUILayout.Label("导出目录：");
            DrawFolderField("预设路径：", selected.targetPfbPath);
            DrawFolderField("材质路径：", selected.targetMatPath);
            DrawFolderField("纹理路径：", selected.targetTexPath);
            DrawExportOpBtn();
            GUILayout.Label("制作图集的参数：");
            selected.atlasName = EditorGUILayout.TextField("图集名称：", selected.atlasName, GUILayout.ExpandWidth(false));
            DrawSizeField();
			DrawTexFormat();
			DrawCheckBtn ();
            DrawTexture();
        }

        void DrawPathCell(string path, int idx)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.TextField(path, GUILayout.ExpandWidth(false));
            if (GUILayout.Button("GO", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
                PingObject(path);
            if (GUILayout.Button("改", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
                OnClickSelectSourcePath(idx);
            if (GUILayout.Button("删", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                DeletePath(idx);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFolderField(string desc, string path)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(desc, GUILayout.ExpandWidth(false));
            GUILayout.TextField(path, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSizeField()
        {
            EditorGUILayout.BeginHorizontal();
            string num = string.Empty;
            num = EditorGUILayout.TextField("图集宽度", selected.iconWidth, GUILayout.ExpandWidth(false));
            if (!string.Equals(num, selected.iconWidth))
                selected.iconWidth = num;
            num = EditorGUILayout.TextField("图集高度", selected.iconHeight, GUILayout.ExpandWidth(false));
            if (!string.Equals(num, selected.iconHeight))
                selected.iconHeight = num;
            EditorGUILayout.EndHorizontal();
        }

        void DrawExportOpBtn()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("GO", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
                PingObject(selected.targetTexPath != "" ? selected.targetTexPath : selected.atlasPath);
            if (GUILayout.Button("改", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
                OnClickSelectTargetPath();
            if (GUILayout.Button("删", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                selected.atlasPath = string.Empty;
            selected.SetSplitPath(EditorGUILayout.Toggle("是否路径分离", selected.splitPath));
            EditorGUILayout.EndHorizontal();
        }

        void DrawTexFormat()
        {
            EditorGUILayout.BeginHorizontal();
            selected.iosFormat = EditorGUILayout.IntPopup("iPhone纹理压缩格式", selected.iosFormat, TexFormatDesc, TexFormatEnums, GUILayout.ExpandWidth(false));
            selected.androidFormat = EditorGUILayout.IntPopup("Android纹理压缩格式", selected.androidFormat, TexFormatDesc, TexFormatEnums, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            selected.maxSize = EditorGUILayout.IntPopup("纹理最大尺寸", selected.maxSize, MaxSizeDescArr, MaxSizeArr, GUILayout.ExpandWidth(false));
            selected.forceSquare = EditorGUILayout.Toggle("是否强制为方形纹理", selected.forceSquare);
            EditorGUILayout.EndHorizontal();
        }

        void DrawTexture()
        {
            Texture tex = AssetDatabase.LoadAssetAtPath(selected.targetTexPath, typeof(Texture)) as Texture;
            Material mat = AssetDatabase.LoadAssetAtPath(selected.targetMatPath, typeof(Material)) as Material;
            if (tex != null && mat != null)
            {
                Rect r = EditorGUILayout.GetControlRect();
                r.width = tex.width;
				r.height = tex.height;
				GUILayoutUtility.GetRect (r.width, r.height);
				EditorGUI.DrawPreviewTexture(r, tex, mat);
				if (r.Contains (Event.current.mousePosition) && Event.current.type == EventType.MouseDown) 
				{
					UIAtlas atlas = AssetDatabase.LoadAssetAtPath<UIAtlas> (selected.targetPfbPath);
					if (atlas != null) 
					{
						NGUISettings.atlas = atlas;
						NGUISettings.selectedSprite = null;
						SpriteSelector.Show (null);
					}
				}
			}
        }

		void DrawCheckBtn()
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("检查素材",  GUILayout.ExpandWidth(false)))
				selected.error = CheckResources ();
			if (!string.IsNullOrEmpty (selected.error))
				GUILayout.Label (selected.error);
			EditorGUILayout.EndHorizontal();
		}

        private void OnClickSelectSourcePath(int idx)
        {
            string path = GetSourceFolderPath();
            if (!string.IsNullOrEmpty(path))
            {
                selected.SetSrcPath(idx, path);
            }
        }

        private void OnClickAddSourcePath()
        {
            string path = GetSourceFolderPath();
            if (!string.IsNullOrEmpty(path))
            {
                selected.resourcePathList.Add(path);
                if (selected.resourcePathList.Count == 1)
                    selected.ResetPath();
            }
        }

        private string OnClickSelectTargetPath()
        {
            string path = EditorUtility.SaveFolderPanel("选择图集路径", Path.Combine(Application.dataPath, "Resources/GUI/atlas"), string.Empty);
            if (!string.IsNullOrEmpty(path))
            {
//                if (!path.Contains("Resources"))
//                    window.ShowNotification("请把路径选在\"Resources\"文件夹下");
//                else
//                {
                    int strIdx = path.IndexOf("Assets");
                    path = path.Substring(strIdx);
                    selected.atlasPath = path;
                    selected.ResetPath();
//                }
            }
            return path;
        }

        private string GetSourceFolderPath()
        {
            string path = EditorUtility.OpenFolderPanel("选择素材路径", Path.Combine(Application.dataPath, "Art/Public/Texture/GUI/icon"), string.Empty);
            if (!string.IsNullOrEmpty(path))
            {
                int strIdx = path.IndexOf("Assets");
                path = path.Substring(strIdx);
            }
            else if (window != null)
                window.ShowNotification("选择文件夹失败！");
            return path;
        }

        private void PingObject(string path)
        {
            Object obj = AssetDatabase.LoadMainAssetAtPath(path);
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }

        private void DeletePath(int idx)
        {
            selected.resourcePathList.RemoveAt(idx);
            if (idx == 0)
                selected.ResetPath();
        }

		private string CheckResources()
		{
			List<string> handled = new List<string> ();
			List<UIAtlasMaker.SpriteEntry> sps = UIAtlasMaker.CreateSprites(AtlasMaker.CollectTexture(selected.cell.data));
			Texture2D[] ts = new Texture2D[sps.Count];
			for (int i = 0; i < sps.Count; ++i) ts[i] = sps[i].tex;
			if (!UITexturePacker.FitMaxSize (handled, ts, 32, 32, NGUISettings.atlasPadding, selected.maxSize)) {
				StringBuilder sb = new StringBuilder ();
				sb.AppendLine("下面这些图片，未能被打进图集:");
				for(int i=0;i<ts.Length;i++)
				{
					if(handled.Contains(ts[i].name)==false)
					{
						sb.AppendLine("\t").Append(ts[i].name);
					}
				}
				return sb.ToString();
			}
			return "图片都可以被打进图集中";
		}
    }
}

