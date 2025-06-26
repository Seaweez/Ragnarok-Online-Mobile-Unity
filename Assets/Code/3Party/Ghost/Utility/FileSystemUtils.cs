using System.Collections.Generic;
using System.IO;
using Ghost.Config;

namespace Ghost.Utils
{
	public class PathEnumerableBase : System.Collections.IEnumerable
	{
		
		public string path {get;set;}
		
		public PathEnumerableBase(string p)
		{
			path = p;
		}

		public System.Collections.IEnumerator GetEnumerator ()
		{
			return FileSystemUtils.GetPathEnumerator(path);
		}

		public System.Collections.IEnumerator GetReverseEnumerator ()
		{
			return FileSystemUtils.GetPathReverseEnumerator(path);
		}
	}

	public class PathEnumerable : PathEnumerableBase, IEnumerable<string>
	{
		public PathEnumerable(string p)
			: base(p)
		{
		}

		new public IEnumerator<string> GetEnumerator ()
		{
			return FileSystemUtils.GetPathEnumerator(path);
		}

		new public IEnumerator<string> GetReverseEnumerator ()
		{
			return FileSystemUtils.GetPathReverseEnumerator(path);
		}
	}

	public static class FileSystemUtils {
		
		public static IEnumerator<string> GetPathEnumerator(string path)
		{
			if (null != path)
			{
				var separators = new char[]{
					Path.AltDirectorySeparatorChar, 
					Path.DirectorySeparatorChar, 
					Path.PathSeparator, 
					Path.VolumeSeparatorChar
				};
				path = path.TrimStart(separators).TrimEnd(separators);
				while (0 < path.Length)
				{
					var i = path.IndexOfAny(separators);
					if (0 > i)
					{
						var part = path;
						path = string.Empty;
						yield return part;
						break;
					}
					else
					{
						var part = path.Substring(0, i);
						path = path.Substring(part.Length);
						path = path.TrimStart(separators);
						yield return part;
					}
				}
			}
		}

		public static IEnumerator<string> GetPathReverseEnumerator(string path)
		{
			if (null != path)
			{
				path = path.TrimStart(PathConfig.SEPARATORS).TrimEnd(PathConfig.SEPARATORS);
				while (0 < path.Length)
				{
					var i = path.LastIndexOfAny(PathConfig.SEPARATORS);
					if (0 > i)
					{
						var part = path;
						path = string.Empty;
						yield return part;
						break;
					}
					else
					{
						var part = path.Substring(i+1, path.Length-i-1);
						path = path.Substring(0, i);
						yield return part;
					}
				}
			}
		}

		public static void CopyFile(FileInfo file, string targetPath) 
		{ 
			File.Copy(file.FullName, targetPath, true); 
//			File.SetAccessControl(targetPath, files[i].GetAccessControl());
			File.SetAttributes(targetPath, file.Attributes);
		}

		public static bool CopyDirectory(string srcDir, string tgtDir, System.Predicate<FileInfo> filter = null, System.Action<FileInfo> callback = null) 
		{ 
			DirectoryInfo source = new DirectoryInfo(srcDir); 
			DirectoryInfo target = new DirectoryInfo(tgtDir); 
			
			if (target.FullName.StartsWith(source.FullName, System.StringComparison.CurrentCultureIgnoreCase)) 
			{ 
				return false;
			} 
			
			if (!source.Exists) 
			{ 
				return false; 
			} 
			
			if (!target.Exists) 
			{ 
				target.Create(); 
			} 
			
			FileInfo[] files = source.GetFiles(); 
			
			for (int i = 0; i < files.Length; i++) 
			{ 
				if (null == filter || filter(files[i]))
				{
					var targetPath = Path.Combine(target.FullName, files[i].Name);
					CopyFile(files[i], targetPath);
				}
				if (null != callback)
				{
					callback(files[i]);
				}
			} 
			
			DirectoryInfo[] dirs = source.GetDirectories(); 
			
			for (int j = 0; j < dirs.Length; j++) 
			{ 
				CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name), filter, callback); 
			} 
			return true;
		}
		
	}

	public static class PathUnity
	{
		public static string Combine(string path1, string path2)
		{
			var path = Path.Combine(path1, path2);
			path = path.Replace(Path.DirectorySeparatorChar, '/');
			return path;
		}
	}

} // namespace Ghost.Util
