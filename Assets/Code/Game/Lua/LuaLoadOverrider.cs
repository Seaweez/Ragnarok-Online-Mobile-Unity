using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace RO.Test
{
	public class LuaLoadOverrider : SingleTonGO<LuaLoadOverrider>
	{
		public bool overrideMode;
		public bool printWithStack;
		public string newLuaRoot = "DevelopTest/sjb/LuaTest";
		SDictionary<string,string> _fullLuaPathMap;

		protected override void Awake ()
		{
			base.Awake ();
			Debuger.EnableLog = true;
			MyLuaSrv.Instance.luaState.loaderDelegate += loadLuaFile;
			MyLuaSrv.Instance.SetGlobalImportFunction (GetFullPath);
		}

		string GetFullPath (string arg)
		{
			string res = arg;
			string find = "";
			#if UNITY_EDITOR_OSX || UNITY_EDITOR
			if(overrideMode)
			{
				if(_fullLuaPathMap==null)
				{
					_fullLuaPathMap = new  SDictionary<string,string>();
					string root = Path.Combine( Application.dataPath ,newLuaRoot);
//					string prefix = Path.Combine( "Assets" ,newLuaRoot);
					string[] luas = Directory.GetFiles (root, "*.txt", SearchOption.AllDirectories);
					for (int i=0,Num = luas.Length; i<Num; i++) {
						luas [i] = luas [i].Replace (".txt", "").Replace (".lua", "").Replace(Application.dataPath,"");
						string file = Path.GetFileName(luas [i]);
						_fullLuaPathMap[file] = luas[i];
					}
				}
				find = _fullLuaPathMap[arg];
				if(string.IsNullOrEmpty(find)==false)
				{
					res = find;
					return res;
				}
			}
			#endif
			if (ResourceID.pathData != null) {
				ResourceID.pathData.fileToFull.TryGetValue (res, out find);
				if (string.IsNullOrEmpty (find) == false)
					res = find;
			}

			return res;
		}

		byte[] loadLuaFile (string fn)
		{
		    if (fn.EndsWith(".txt"))
		    {
		        fn = fn.Replace (".txt", "");
		    }
			fn = fn.Replace (".", "/");

#if UNITY_EDITOR_OSX || UNITY_EDITOR
			fn = fn.Replace(Path.Combine("Assets", newLuaRoot),"");
		    if (fn.StartsWith("/") || fn.StartsWith("\\"))
		    {
		        fn=fn.Remove(0,1);
		    }

			if(overrideMode)
			{
				string path = Path.Combine(Application.dataPath , fn) + ".txt";
				if(File.Exists(path))
				{
					FileStream fs = File.Open(path, FileMode.Open);
					long length = fs.Length;
					byte[] bytes = new byte[length];
					fs.Read(bytes, 0, bytes.Length);
					fs.Close();
					return bytes;
				}
				else
				{
					print(string.Format( "覆盖加载方式找不到lua->{0}，回滚到默认方式加载",path));
				}
			}
#endif

			TextAsset asset = null;

#if RESOURCE_LOAD

    #if LUA_READABLE || LUA_FASTPACKING || LUA_STREAMINGASSETS

    #else
			fn = fn.Replace("Script/", "Script2/");
    #endif
            if (null != ResourceManager.Me)
                asset = (TextAsset)ResourceManager.Me.SLoad(fn);
            else
                asset = (TextAsset)Resources.Load(fn);
#else
    #if LUA_STREAMINGASSETS && !(UNITY_ANDROID && !UNITY_EDITOR)
            //读取StreamingAssets目录下面的Script文件夹下面的lua脚本
            string luafilePath = Path.Combine(Application.streamingAssetsPath, fn) + ".txt";
		    if (File.Exists(luafilePath))
		    {
		        FileStream fs = File.Open(luafilePath, FileMode.Open);
		        long length = fs.Length;
		        byte[] bytes = new byte[length];
		        fs.Read(bytes, 0, bytes.Length);
		        fs.Close();
		        return bytes;
            }
    #endif

    #if LUA_FASTPACKING
            asset = (TextAsset)Resources.Load(fn);
    #else
		fn = fn.Replace("Script/", "Script2/");
		string bundleFile = "assets/resources/" + fn + ".bytes";
        string id = GetCacheID(fn);
        asset = ResourceManager.Me.SLoad<TextAsset> (id, bundleFile);
        m_HashSet.Add(id);
    #endif
#endif
            return asset == null ? null : asset.bytes;
		}

		SDictionary<string,string> _cachedID = new SDictionary<string, string>();
		StringBuilder builder = new StringBuilder ();
		public string GetCacheID(string fn)
		{
			string id = _cachedID [fn];
			if (id == null) {
				string[] splits = fn.Split(Path.DirectorySeparatorChar,'/');
				if(splits.Length>1)
				{
					builder.Length = 0;
					builder.Append(splits[0]);
					builder.Append("/");
					builder.Append(splits[1]);
					fn = builder.ToString();
				}
				id = fn;
				_cachedID [fn] = id;
			}
			return id;
		}

        protected HashSet<string> m_HashSet = new HashSet<string>();
        public void ClearLuaMapAsset()
        {
#if RESOURCE_LOAD
            return;
#endif

#if LUA_FASTPACKING
        return;
#endif
            ManagedBundleLoaderStrategy mgr = ResourceManager.LoaderStrategy as ManagedBundleLoaderStrategy;

            if (null == mgr)
                return;
            foreach (var str in m_HashSet)
            {
                mgr.UnLoadShareAB(str);
            }
        }
    }
} // namespace RO.Test
