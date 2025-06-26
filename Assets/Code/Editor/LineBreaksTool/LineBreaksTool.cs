using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class movingFile
    {
        static Dictionary<string, string> _MimeMapping = new Dictionary<string, string>()
        {
            {".meta", "text/plain"/*"application/unknown"*/},
            {".errors", "text/plain"/*"application/unknown"*/},
            //{".asset", "text/plain"/*"application/unknown"*/},
            //{".FBX", "application/unknown"},
            //{".png", "image/png"},
            //{".tga", "application/unknown"},
            //{".PNG", "image/png"},
            //{".mine", "application/unknown"},
            {".cs", "text/plain"},
            {".shader", "text/plain"/*"application/unknown"*/},
            //{".psd", "application/unknown"},
            {".flare", "text/plain"/*"application/unknown"*/},
            //{".bmp", "image/bmp"},
            {".sh", "text/plain"/*"application/unknown"*/},
            //{".TTF", "application/unknown"},
            {".fnt", "text/plain"/*"application/unknown"*/},
            //{".jpg", "image/jpeg"},
            //{".TGA", "application/unknown"},
            {".giparams", "text/plain"/*"application/unknown"*/},
            {".txt", "text/plain"},
            {".guiskin", "text/plain"/*"application/unknown"*/},
            //{".otf", "application/unknown"},
            //{"", "application/unknown"},
            {".mdown", "text/plain"/*"application/unknown"*/},
            {".projmods", "text/plain"/*"application/unknown"*/},
            {".sdkmods", "text/plain"/*"application/unknown"*/},
            {".h", "text/plain"},
            //{".a", "application/unknown"},
            //{".dll", "application/x-msdownload"},
            //{".dylib", "application/unknown"},
            //{".pdf", "application/pdf"},
            {".md", "text/plain"/*"application/unknown"*/},
            //{".exr", "application/unknown"},
            //{".wav", "audio/wav"},
            {".json", "text/plain"/*"application/unknown"*/},
            //{".fbx", "application/unknown"},
            {".js", "text/plain"},
            //{".tif", "image/tiff"},
            {".xml", "text/xml"},
            {".as", "text/plain"/*"application/unknown"*/},
            {".mask", "text/plain"/*"application/unknown"*/},
            {".cubemap", "text/plain"/*"application/unknown"*/},
            //{".jar", "application/unknown"},
            {".properties", "text/plain"/*"application/unknown"*/},
            //{".so", "application/unknown"},
            //{".zip", "application/x-zip-compressed"},
            {".plist", "text/plain"/*"application/unknown"*/},
            {".mm", "text/plain"/*"application/unknown"*/},
            {".m", "text/plain"/*"application/unknown"*/},
            //{".strings", "application/unknown"},
            {".renderTexture", "text/plain"/*"application/unknown"*/},
            //{".mp3", "audio/mpeg"},
            //{".ogg", "application/unknown"},
            {".cginc", "text/plain"/*"application/unknown"*/},
            {".proto", "text/plain"/*"application/unknown"*/},
            //{".mp4", "video/mp4"}
        };
        static HashSet<string> _MixMimeTypeHashSet = new HashSet<string>()
        {
            ".asset",
            ".plist",
            ".prefab",
            ".mat",
            ".unity",
            ".controller",
            ".anim",
            ".fbx"
        };
        const int CR = 0x0d;
        const int LF = 0x0a;

        public enum LineEndingStyle
        {
            None,
            Unix,
            Windows,
            MacOSX
        }
        public LineEndingStyle style;
        public string path;
        public string targetPath;
        public bool isText = false;
        public bool targetExist;
        public bool isEmptyDir = false;

        public movingFile(string _Path, string _TargetFloder, bool isEmptyDirectory = false)
        {
            path = _Path;
            targetPath = _TargetFloder + _Path;
            isEmptyDir = isEmptyDirectory;
            if(!isEmptyDir)
                Init ();
        }

        private void Init()
        {
            isText = IsTextMimeType (path);
            targetExist = File.Exists (targetPath);
            if(isText)
                style = ReadFirstLineEnding(targetExist ? targetPath : path);
        }

        private bool IsTextMimeType(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            if (_MixMimeTypeHashSet.Contains(ext))
                return TryReadContents(path);
            //string mimeType = "application/unknown";
            //#if UNITY_EDITOR_WIN 
            //  Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            //  if (regKey != null && regKey.GetValue("Content Type") != null)
            //      mimeType = regKey.GetValue("Content Type").ToString();
            //#else
            //  if(_MimeMapping.ContainsKey(ext.ToLower()))
            //      mimeType = _MimeMapping[ext.ToLower()];
            //#endif
            return _MimeMapping.ContainsKey(ext);
        }

        /// <summary>
        /// 尝试读取前1024个字符,如果有超过15%的字符不是ascii字符和中文字符则判断为二进制文件。
        /// 判断规则和svn等同
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool TryReadContents(string path)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("utf-8")))
            {
                int totalCount = 0;
                int charCount = 0;
                int n;
                while (!sr.EndOfStream && totalCount++ < 1024)
                {
                    n = sr.Read();
                    if (n == 0x09 || n == 0x0A || n == 0x0D || (n >= 0x20 && n <= 0x9FA5))
                        charCount++;
                }
                return charCount > totalCount * 0.85f;
            }
        }

        private LineEndingStyle ReadFirstLineEnding(string path)
        {
            LineEndingStyle _style = LineEndingStyle.None;
            using (FileStream fs = new FileStream (path, FileMode.Open, FileAccess.Read))
            {
                int n;
                while((n = fs.ReadByte()) > -1)
                {
                    switch(n)
                    {
                        case CR:// /r
                            if (fs.ReadByte () == LF)
                                _style = LineEndingStyle.Windows;
                            else
                                _style = LineEndingStyle.MacOSX;
                            break;
                        case LF:// /n
                            _style = LineEndingStyle.Unix;
                            break;
                        default:
                            break;
                    }
                    if (_style != LineEndingStyle.None)
                        return _style;
                }
            }
            return _style;
        }

        public void UnifyLineEndings()
        {
            if(isText)
                WriteFileWithEnds(path, targetPath, style);
        }

        public void Move()
        {
            if (isEmptyDir)
                _MoveEmptyDir();
            else
                _MoveFile();
        }

        private void _MoveEmptyDir()
        {
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
        }

        private void _MoveFile()
        {
            string targetFolder = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);
            if (isText)
                WriteFileWithEnds(path, targetPath, style);
            else
                File.Copy(path, targetPath, true);
        }

        private void WriteFileWithEnds(string srcPath, string targetPath, LineEndingStyle _Style)
        {
            byte[] srcContents = File.ReadAllBytes (srcPath);
            List<byte> targetContents = new List<byte> ();
            for(int i = 0; i < srcContents.Length; i++)
            {
                byte b = srcContents [i];
                switch(b)
                {
                    case CR:// r
                        if (srcContents.Length > i+1 && srcContents[i+1] == LF)
                            ++i;
                        AppendLineEnding(ref targetContents, _Style);
                        break;
                    case LF:// n
                        AppendLineEnding(ref targetContents, _Style);
                        break;
                    default:
                        targetContents.Add (b);
                        break;
                }
            }

            using (FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(targetContents.ToArray(), 0, targetContents.Count);
                fs.Close();
            }
        }

        private void AppendLineEnding(ref List<byte> contents, LineEndingStyle _Style)
        {
            switch (_Style) 
            {
                case LineEndingStyle.MacOSX:
                    contents.Add (CR);
                    break;
                case LineEndingStyle.Unix:
                    contents.Add (LF);
                    break;
                case LineEndingStyle.Windows:
                    contents.Add (CR);
                    contents.Add (LF);
                    break;
                case LineEndingStyle.None:
                    break;
            }
        }
    }

    public class LineBreaksTool
    {

        static List<string> _UnifyLineEndingFloders = new List<string>()
        {
            "Assets/Code",
            "Assets/DevelopTest",
            "Assets/Resources/Script"
        };
        static List<string> _MoveFloders = new List<string>()
        {
        };
        static List<string> _Errors = new List<string>();
        static List<movingFile> _files = new List<movingFile>();
        static int _FileIndex = 0;
        public static string _TargetFolder = string.Empty;
        [MenuItem("Assets/搬家工具/统一换行符")]
        public static void ExecuteUnify()
        {
            _FileIndex = 0;
            _Errors.Clear();
            CollectAllFolders(_UnifyLineEndingFloders);
            if(_files.Count > _FileIndex)
                EditorApplication.update = UpdateScanFiles;
        }

        [MenuItem("Assets/搬家工具/一键搬家")]
        public static void ExecuteMoveAll()
        {
            _TargetFolder = AppendSlash(EditorUtility.OpenFolderPanel("选择搬家目标路径, 选择Assets的父文件夹", Application.dataPath, ""));
            if (!string.IsNullOrEmpty(_TargetFolder))
            {
                _FileIndex = 0;
                _Errors.Clear();
                _files.Clear ();
                CollectAllSelection();
                CollectAllFolders(_MoveFloders);
                RemoveTargetFloders ();
                if(_files.Count > _FileIndex)
                    EditorApplication.update = UpdateMoveFiles;
            }
        }

        private static void UpdateMoveFiles()
        {
            movingFile file = _files[_FileIndex];
            bool isCancel = EditorUtility.DisplayCancelableProgressBar(string.Format("搬家到{0}", _TargetFolder), file.path, (float)_FileIndex / (float)_files.Count);
            file.Move();
            _FileIndex++;
            if (isCancel || _FileIndex >= _files.Count)
            {
                EditorUtility.ClearProgressBar();
                Debug.Log(string.Format("--扫描完成--    错误数量: {0}    总数量: {1}/{2}    错误信息↓:\n{3}\n----------输出完毕----------", _Errors.Count, _FileIndex, _files.Count, string.Join("\n", _Errors.ToArray())));
                Resources.UnloadUnusedAssets();
                GC.Collect();
                EditorApplication.update = null;
                _files.Clear();
                _FileIndex = 0;
            }
        }

        private static void UpdateScanFiles()
        {
            movingFile file = _files[_FileIndex];
            bool isCancel = EditorUtility.DisplayCancelableProgressBar("统一换行符, 文件扫描中", file.path, (float)_FileIndex / (float)_files.Count);
            file.UnifyLineEndings ();
            _FileIndex++;
            if (isCancel || _FileIndex >= _files.Count)
            {
                EditorUtility.ClearProgressBar();
                Debug.Log(string.Format("--扫描完成--    错误数量: {0}    总数量: {1}/{2}    错误信息↓:\n{3}\n----------输出完毕----------", _Errors.Count, _FileIndex, _files.Count, string.Join("\n", _Errors.ToArray())));
                Resources.UnloadUnusedAssets();
                GC.Collect();
                EditorApplication.update = null;
                _files.Clear ();
                _FileIndex = 0;
            }
        }

        static void _AddFile(string path)
        {
            movingFile f = new movingFile (path, _TargetFolder);
            _files.Add(f);
        }

        static void _AddEmptyDirectory(string path)
        {
            movingFile f = new movingFile(path, _TargetFolder, true);
            _files.Add(f);
        }

        private static void RemoveTargetFloders()
        {
            for(int i = 0; i < _MoveFloders.Count; i++)
            {
                string floder = _TargetFolder + _MoveFloders [i];
                if (Directory.Exists (floder))
                    Directory.Delete (floder, true);
            }
        }

        private static void CollectAllSelection()
        {
            List<string> _TopLvPath = GetTopLvSelected();
            string path = string.Empty;
            for(int i = 0; i < _TopLvPath.Count; i++)
            {
                path = _TopLvPath[i];
                if (File.Exists(path))
                {
                    _AddFile (path);
                }
                else
                {
                    if (!_MoveFloders.Contains(path))
                        _MoveFloders.Add(path);
                }
            }
        }

        private static List<string> GetTopLvSelected()
        {
            UnityEngine.Object[] objs = Selection.GetFiltered(typeof(object), SelectionMode.Assets);
            List<string> _SelectedPath = new List<string>();
            List<string> _TopLvPath = new List<string>();
            bool _IsTopLv = true;
            string path = string.Empty;
            for (int i = 0; i < objs.Length; i++)
            {
                _SelectedPath.Add(AssetDatabase.GetAssetPath(objs[i]));
            }
            for (int i = 0; i < _SelectedPath.Count; i++)
            {
                path = _SelectedPath[i];
                _IsTopLv = true;
                for (int k = 0; k < _SelectedPath.Count; k++)
                {
                    if (i != k && path.Contains(_SelectedPath[k]))
                    {
                        _IsTopLv = false;
                        break;
                    }
                }
                if (_IsTopLv)
                    _TopLvPath.Add(path);
            }
            return _TopLvPath;
        }

        private static void CollectAllFolders(List<string> folders)
        {
            for (int i = 0; i < folders.Count; i++)
            {
                if (Directory.Exists(folders[i]))
                    CollectFile(folders[i], true, folders[i]);
            }
        }

        private static void CollectFile(string folder, bool recursive = false, string ppath = "")
        {
            folder = AppendSlash(folder);
            ppath = AppendSlash(ppath);
            DirectoryInfo dir = new DirectoryInfo(folder);
            FileInfo[] files = dir.GetFiles();
            if (files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string fpath = ppath + files[i].Name;
                    _AddFile(fpath);
                }
            }
            else
                _AddEmptyDirectory(folder);

            if (recursive)
            {
                foreach (var sub in dir.GetDirectories())
                {
                    CollectFile(folder + sub.Name, recursive, ppath + sub.Name);
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
    }
}
